///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;

[RequireComponent( typeof(EnemyInfo), typeof(DamagedFeedback))]
public class EnemyHealth : TriggeringObject
{
	[SerializeField]
	private float hitPoints = 20f;
	[SerializeField]
	private float dmgTakenOnShipHit = 50f;
	[SerializeField]
	private float dmgTakenOnMapHit = 2f;
	[SerializeField]
	Timer doomTimer = null;

	public delegate void DelegateEnemyType(Enemies e);
	static public event DelegateEnemyType OnKilledStatic;
	public event System.Action OnKilledInstance;

	private DamagedFeedback damagedFeedback;
	private EnemyInfo info;
	Spawner mySpawner;


	protected override void DoOnAwake()
	{
		info = GetComponent<EnemyInfo>();
		damagedFeedback = GetComponent<DamagedFeedback>();
	}


	void OnEnable()
	{
		if (doomTimer != null)
			doomTimer.OnTimesUp += HandleOnTimesUp;
	}


	void OnDisable()
	{
		if (doomTimer != null)
			doomTimer.OnTimesUp -= HandleOnTimesUp;
	}


	private void HandleOnTimesUp()
	{
		//unregister from event
		doomTimer.OnTimesUp -= HandleOnTimesUp;

		TakeDamage(Mathf.Infinity, false);
	}


	public void Register(Spawner spawner)
	{
		mySpawner = spawner;
	}


	public float GetHps()
	{
		return hitPoints;
	}


	//Called by EnemyCollider when a player ship's bullet hits that enemy
	public void GotHitByWeapon(float dmg)
	{
		//We damage the ship only if its EnemyHealth component is active
		if (!enabled)
			return;

		TakeDamage(dmg);
	}


	public void GotHitByShip()
	{
		//We damage the ship only if its EnemyHealth component is active
		if (!enabled)
			return;

		TakeDamage(dmgTakenOnShipHit);
	}


	public void GotMapHit()
	{
		if (dmgTakenOnMapHit > 0)
			TakeDamage(dmgTakenOnMapHit * Time.fixedDeltaTime, false);
	}


	void TakeDamage(float dmg, bool mayBeAKill = true)
	{
		hitPoints -= dmg;

		if (hitPoints <= 0)
		{
			Farewell();
			if (mayBeAKill)
			{
				OnKilledStatic?.Invoke(info.type);

				OnKilledInstance?.Invoke();
			}

			if (mySpawner != null)
				//Whether the enemy is killed by the ship or by the environment, it's killed and we inform mySpawner accordingly
				mySpawner.DoOnEnemyDestroyed(true, transform.position);
		}
		else if (damagedFeedback != null)
		{
			//visual feedback of enemy hit
			damagedFeedback.GotDamaged();
		}
	}


	void Farewell()
	{
		TriggerOthersAndFinish();

		Destroy(gameObject);
	}
}

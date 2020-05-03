///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;
using System;

public class PlayerShipHealth : MonoBehaviour {

	[SerializeField]
	Transform explosionVfxRef = null;
	[SerializeField]
	float maxHitPoints = 100f;
	[SerializeField]
	float dmgTakenOnMapHit = 2f;
	[SerializeField]
	float hpGainedOnPickup = 20f;

	float hp;

	static public event Action OnShipDestroyed;
	static public event Action<float,float> OnHpChange;


	void Awake()
	{
		hp = maxHitPoints;
	}


	void OnEnable()
	{
		EnemyCollider.OnHitPlayerShip += handleOnHitByEnemy;
		Bullet.OnHitPlayerShip += handleOnHitByBullet;
		DamagingArea.OnHitPlayerShip += handleOnHitByBullet;
		GameManager.OnVictory += handleOnVictory;
		Pickup.OnPickup += HandleOnPickup;

	}


	void OnDisable()
	{
		EnemyCollider.OnHitPlayerShip -= handleOnHitByEnemy;
		Bullet.OnHitPlayerShip -= handleOnHitByBullet;
		DamagingArea.OnHitPlayerShip -= handleOnHitByBullet;
		GameManager.OnVictory -= handleOnVictory;
		Pickup.OnPickup -= HandleOnPickup;

	}


	private void handleOnHitByEnemy(Enemies e, float dmg)
	{
		takeDamage(dmg);
	}

	private void handleOnHitByBullet(float dmg)
	{
		takeDamage(dmg);
	}


	private void handleOnVictory()
	{
		enabled = false;
	}


	void HandleOnPickup(Pickups type)
	{
		if (type == Pickups.Health)
		{
			if (hp == maxHitPoints)
				return;

			hp += hpGainedOnPickup;
			if (hp > maxHitPoints)
				hp = maxHitPoints;

			OnHpChange?.Invoke(hp, maxHitPoints);
		}
	}


	public void gotMapHit()
	{
		takeDamage(dmgTakenOnMapHit * Time.fixedDeltaTime);
	}


	void takeDamage(float dmg)
	{
		if (dmg == 0)
			return;

		hp -= dmg;

		if (OnHpChange != null)
			OnHpChange(hp, maxHitPoints);

		if (hp <= 0)
		{
			if (OnShipDestroyed != null)
				OnShipDestroyed();
			explode();
		}
	}


	void explode()
	{
		//instantiate an explosion vfx object
		Instantiate(explosionVfxRef, transform.position, Quaternion.identity);

		//destroy this whole gameobject (including the particle system component)
		Destroy(gameObject);
	}

}

///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using System;
using UnityEngine;

public class Bullet : TriggeringObject
{
	static public event Action<float> OnHitPlayerShip;

	[SerializeField]
	protected float defaultDamage = 5f;
	[SerializeField]
	protected float defaultSpeed = 12f;
	[SerializeField]
	protected int maxBounce = 0;
	//[SerializeField]
	//protected float sineAmp = 0f;
	//[SerializeField]
	//protected float sineSpeed = 1f;
	[SerializeField]
	protected Timer doomTimer;
	[SerializeField]
	protected Transform toInstantiateOnMapHit;
	[SerializeField]
	protected bool keepAsChild = false;
	//float startTime;
	//float currTime;
	//float initialAngle;
	//Vector2 initialDirVector;

	protected Rigidbody2D rb;
	protected int bounces;
	bool isEnemyProjectile;


	//public void Init(Vector3 pos, float rot)
	//{
	//	transform.eulerAngles = new Vector3(0f, 0f, rot);
	//	rb.rotation = rot;
	//	rb.position = transform.position = pos;
	//}


	override protected void DoOnAwake()
	{
		rb = GetComponent<Rigidbody2D>();

		if (!keepAsChild)
			transform.parent = null;

		base.DoOnAwake();
	}


	void Start()
	{
		//registering to event
		if (doomTimer != null)
			doomTimer.OnTimesUp += HandleOnTimesUp;

		//Apply flag according to physics layer
		if (gameObject.layer == Alias.LAYER_ENEMY_PROJECTILES)
		{
			isEnemyProjectile = true;
		}
		else if (gameObject.layer == Alias.LAYER_SHIP_PROJECTILES)
		{
			isEnemyProjectile = false;
		}
		else
		{
			Debug.LogError("Projectile " + gameObject.name + " must be either in Layer Ship Projectiles or Enemy Projectiles"); //DEBUG
		}

		//startTime = Time.time;
		//currTime = 0f;
		//initialAngle = transform.eulerAngles.z;
		//initialDirVector = transform.TransformVector(Vector2.right);

		bounces = 0;

		Launch();
	}


	void Launch()
	{
		Vector2 localForce = transform.TransformVector(Vector3.right) * defaultSpeed;
		if (rb.isKinematic)
			rb.velocity = localForce;
		else
			rb.AddForce(localForce, ForceMode2D.Impulse);
	}


	private void HandleOnTimesUp()
	{
		DestroyMyself();
	}


	public void OnHitByExplosion()
	{
		DestroyMyself();
	}


	//void Update()
	//{
	//	//Calculate forward bullet movement at this frame
	//	Vector2 move = new Vector2(speed * Time.deltaTime, 0f);

	//	//Add local y offset due to sine movement, if needed
	//	//if (sineAmp != 0f)
	//	//{
	//	//	currTime = Time.time;
	//	//	float a = currTime * sineSpeed;
	//	//	move.y = Mathf.Sin(a) * sineAmp;
	//	//}

	//	//Local space to global space
	//	Vector2 newPos = transform.TransformPoint(move);

	//	//Update the bullet's position
	//	transform.position = rb.position = newPos;
	//}


	void OnBecameInvisible()
	{
		DestroyNClean();
	}


	protected void DestroyMyself()
	{
		TriggerOthersAndFinish();

		DestroyNClean();
	}


	void DestroyNClean()
	{
		//destroy this whole gameobject
		Destroy(gameObject);
		//unregistering from event
		if (doomTimer != null)
			doomTimer.OnTimesUp -= HandleOnTimesUp;
	}


	protected void HitMap(bool isSolid)
	{
		if (/*bounces <= maxBounce &&*/ toInstantiateOnMapHit != null)
		{
			Instantiate(toInstantiateOnMapHit, transform.position, Quaternion.identity);
		}

		bounces++;

		if (!isSolid || bounces > maxBounce)
		{
			DestroyMyself();
		}
	}


	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.layer == Alias.LAYER_MAP)
		{
			HitMap(true);
		}
		else if (!isEnemyProjectile && col.gameObject.layer == Alias.LAYER_ENEMIES)
		{
			col.gameObject.GetComponent<EnemyCollider>().OnHitByBullet(defaultDamage);
			DestroyMyself();
		}
		else if (isEnemyProjectile && col.gameObject.layer == Alias.LAYER_SHIP)
		{
			OnHitPlayerShip?.Invoke(defaultDamage);
			DestroyMyself();
		}
	}


	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.layer == Alias.LAYER_MAP)
		{
			HitMap(false);
		}
		if (!isEnemyProjectile && col.gameObject.layer == Alias.LAYER_ENEMIES)
		{
			col.gameObject.GetComponent<EnemyCollider>().OnHitByBullet(defaultDamage);
			DestroyMyself();
		}
		else if (isEnemyProjectile && col.gameObject.layer == Alias.LAYER_SHIP)
		{
			OnHitPlayerShip?.Invoke(defaultDamage);
			DestroyMyself();
		}
	}
}

///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D), typeof(EnemyInfo))]
public class EnemyCollider : MonoBehaviour
{
	[SerializeField]
	float damageToShip = 25f;

	EnemyInfo info;
	EnemyHealth healthManager;
	bool isMapCollidable;

	public delegate void DelegateEnemyDmg(Enemies e, float dmg);
	static public event DelegateEnemyDmg OnHitPlayerShip;


	void Start()
	{
		info = GetComponent<EnemyInfo>();
		healthManager = GetComponent<EnemyHealth>();

		isMapCollidable = false;
		StartCoroutine(CoDelay());
	}


	IEnumerator CoDelay()
	{
		yield return new WaitForSeconds(0.8f);
		isMapCollidable = true;
	}


	//Called by ShipBullet when it hits that enemy
	public void OnHitByBullet(float dmg)
	{
		if (healthManager != null)
			healthManager.GotHitByWeapon(dmg);
	}


	//Called by ShipLaser when it hits that enemy
	public void OnHitByLaser(float dmg)
	{
		if (healthManager != null)
			healthManager.GotHitByWeapon(dmg);
	}


	void OnCollisionEnter2D(Collision2D otherColl)
	{
		//Check if collide with player ship 
		if (otherColl.gameObject.layer == Alias.LAYER_SHIP)
		{
			OnHitPlayerShip?.Invoke(info.type, damageToShip);
			if (healthManager != null)
				healthManager.GotHitByShip();

		}
		//check if collide with map
		else if (otherColl.gameObject.layer == Alias.LAYER_MAP && isMapCollidable)
		{
			if (otherColl.gameObject.layer == Alias.LAYER_MAP && isMapCollidable)
				if (healthManager != null)
					healthManager.GotMapHit();
		}
	}


	void OnCollisionStay2D(Collision2D otherColl)
	{
		//Check if collide with player ship 
		if (otherColl.gameObject.layer == Alias.LAYER_SHIP)
		{
			OnHitPlayerShip?.Invoke(info.type, damageToShip * Time.fixedDeltaTime);
			if (healthManager != null)
				healthManager.GotHitByShip();
		}
		//check if collide with map
		else if (otherColl.gameObject.layer == Alias.LAYER_MAP && isMapCollidable)
			if (healthManager != null)
				healthManager.GotMapHit();
	}


	void OnTriggerEnter2D(Collider2D otherColl)
	{
		//Check if collide with player ship 
		if (otherColl.gameObject.layer == Alias.LAYER_SHIP)
		{
			OnHitPlayerShip?.Invoke(info.type, damageToShip);
			if (healthManager != null)
				healthManager.GotHitByShip();

		}
		//check if collide with map
		else if (otherColl.gameObject.layer == Alias.LAYER_MAP && isMapCollidable)
		{
			if (otherColl.gameObject.layer == Alias.LAYER_MAP && isMapCollidable)
				if (healthManager != null)
					healthManager.GotMapHit();
		}
	}


	void OnTriggerStay2D(Collider2D otherColl)
	{
		//Check if collide with player ship 
		if (otherColl.gameObject.layer == Alias.LAYER_SHIP)
		{
			OnHitPlayerShip?.Invoke(info.type, damageToShip * Time.fixedDeltaTime);
			if (healthManager != null)
				healthManager.GotHitByShip();
		}
		//check if collide with map
		else if (otherColl.gameObject.layer == Alias.LAYER_MAP && isMapCollidable)
			if (healthManager != null)
				healthManager.GotMapHit();
	}

}

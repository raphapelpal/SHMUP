///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;
using System;

public class DamagingArea : TriggeringObject
{
	[SerializeField]
	bool isDamagingShip = false;
	[SerializeField]
	bool isDamagingEnemies = true;
	[SerializeField]
	bool isDestroyingEnemyBullets = true;
	[SerializeField]
	bool isDestroyingShipBullets = false;
	//	[SerializeField]
	//	float radius = 5f;
	[SerializeField]
	float damage = 50f;
	[SerializeField]
	///Time while the AoE lasts. If 0 or negative, it is instant (lasts one tick)
	float duration = 0.0f;
	[SerializeField]
	GameObject objToActivate = null;

	static public event Action<float> OnHitPlayerShip;

	//int layerMask;
	Collider2D aoeCollider;
	float endTime;


	void Awake()
	{
		//bitwise operations to construct the layer mask to define what collision layers will be checked by the explosion
		//layerMask = 0;
		//if (isDamagingShip)
		//	layerMask = 1 << Alias.LAYER_SHIP;
		//if (isDamagingEnemies)
		//	layerMask |= 1 << Alias.LAYER_ENEMIES;
		//if (isDestroyingEnemyBullets)
		//	layerMask |= 1 << Alias.LAYER_ENEMY_PROJECTILES;
		//if (isDestroyingShipBullets)
		//	layerMask |= 1 << Alias.LAYER_SHIP_PROJECTILES;

		aoeCollider = GetComponent<Collider2D>();
		//Affiche messages au cas où oubli/erreurs sur le collider servant d'AoE
		if (aoeCollider == null)
		{
			Debug.LogError("A 2D collider component must be attached to the same GameObject as " + this.name);
		}
		else if (!aoeCollider.isTrigger)
		{
			Debug.LogWarning("The collider on the GameObject of " + this.name + " must be 'trigger'!");
			aoeCollider.isTrigger = true;
		}
	}


	void OnEnable()
	{
		Activate();
	}


	private void FixedUpdate()
	{
		if (Time.fixedTime > endTime)
			Deactivate();
	}


	private void Activate()
	{
		aoeCollider.enabled = true;
		if (objToActivate!= null) objToActivate.SetActive(true);

		float l = Mathf.Max(Time.fixedTime + duration, Time.fixedDeltaTime /*0.0167f*/);
		endTime = Time.time + l;

		//Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
	}


	private void Deactivate()
	{
		aoeCollider.enabled = false;
		enabled = false;
		TriggerOthersAndFinish();
	}


	private void OnTriggerEnter2D(Collider2D c)
	{
		Hit(c);
		if (duration <= 0f) //if instant damage only
			Deactivate();
	}


	private void OnTriggerStay2D(Collider2D c)
	{
		Hit(c);
	}


	void Hit(Collider2D c)
	{
		if (isDamagingEnemies && c.gameObject.layer == Alias.LAYER_ENEMIES)
		{
			c.gameObject.GetComponent<EnemyCollider>().OnHitByBullet(damage);
		}
		if (isDamagingShip && c.gameObject.layer == Alias.LAYER_SHIP)
		{
			OnHitPlayerShip?.Invoke(damage);
		}
		if ((isDestroyingEnemyBullets && c.gameObject.layer == Alias.LAYER_ENEMY_PROJECTILES)
			|| (isDestroyingShipBullets && c.gameObject.layer == Alias.LAYER_SHIP_PROJECTILES))
		{
			c.gameObject.GetComponent<Bullet>().OnHitByExplosion();
		}
	}

}

///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;

public class EnemyShoot : TriggerableObject
{
	[SerializeField]
	private Animator animatorIfFireAnim = null;
	[SerializeField]
	private Bullet bulletPrefab = null;
	[SerializeField]
	private float delayBtwFire = 1f;
	public float shootingAngle = 180f;

	bool isActive = false;


	void OnEnable()
	{
		isActive = true;
		StartCoroutine(CoShooting());
	}


	void OnDisable()
	{
		isActive = false;
	}


	IEnumerator CoShooting()
	{
		while (isActive)
		{
			//We start the enemy animation to prepare shooting,
			//but it'll be an animation event that will actually trigger the shoot by calling actuallyFire()
			if (animatorIfFireAnim != null)
			{
				animatorIfFireAnim.SetTrigger("goShoot");
			}
			//If no animator is specified, we shoot directly
			else
			{
				actuallyFire();
			}
			yield return new WaitForSeconds(delayBtwFire);
		}
	}


	//called by animation event when the visual is ready to fire
	private void actuallyFire()
	{
		Bullet bullet = Instantiate<Bullet>(bulletPrefab, transform.position,
			Quaternion.Euler(0f, 0f, transform.eulerAngles.z + shootingAngle), transform);
	}
}

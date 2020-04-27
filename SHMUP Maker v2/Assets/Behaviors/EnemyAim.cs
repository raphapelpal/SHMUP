///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;

public class EnemyAim : TriggerableObject
{
	[SerializeField]
	EnemyShoot shootManager = null; //to update shootManager.shootingAngle
	[SerializeField]
	Transform target = null;
	[SerializeField]
	float maxDelta = 60f; //max target angle from the initialAngle

	float initialAngle;
	float ccwAngleMax, cwAngleMax;


	void Awake()
	{
		//define min & max angles
		initialAngle = shootManager.shootingAngle;

		ccwAngleMax = initialAngle + maxDelta;
		ccwAngleMax %= 360f;
		if (ccwAngleMax < 0)
			ccwAngleMax += 360f;
		cwAngleMax = initialAngle - maxDelta;
		cwAngleMax %= 360f;
		if (cwAngleMax < 0)
			cwAngleMax += 360f;
	}


	private void Start()
	{
		if (target == null)
		{
			//target = FindObjectOfType<PlayerShipMove>().transform;
			PlayerShipMove psm = PlayerShipMove.Me;
			if (psm != null)
				target = psm.transform;
			else
			{
				Debug.LogError("No component PlayerShipMove in the scene!"); //TEST
			}
		}
	}


	void Update()
	{
		if (target != null)
		{
			//if the aim if for the shoot angle
			if (shootManager != null)
			{
				Vector2 v = target.position - transform.position;
				float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
				shootManager.shootingAngle = clampAngle(angle);
			}
		}
	}


	/// <summary>
	/// clamp angle to remain within authorized values; the angle must always be smaller than ccwAngleMax and larger than cwAngleMax
	/// </summary>
	float clampAngle(float angle)
	{
		//Make sure angle is between 0 and 360 degrees
		angle %= 360f;
		if (angle < 0)
			angle += 360f;

		//clamp angle if needed
		if (ccwAngleMax > cwAngleMax)
		{
			if (angle > ccwAngleMax)
				angle = ccwAngleMax;
			else if (angle < cwAngleMax)
				angle = cwAngleMax;
		}
		else
		{
			if (angle > ccwAngleMax && angle < cwAngleMax)
			{
				if (Mathf.DeltaAngle(angle, ccwAngleMax) < Mathf.DeltaAngle(angle, cwAngleMax))
					angle = ccwAngleMax;
				else
					angle = cwAngleMax;
			}
		}

		return angle;
	}
}

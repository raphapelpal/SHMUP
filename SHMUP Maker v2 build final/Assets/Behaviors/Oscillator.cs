///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
	[SerializeField]
	float amplitude = 30f;
	[SerializeField]
	float angularSpeed = 60f;
	[SerializeField]
	bool isPlayerShip = true;

	bool mustOscillate = false;
	float defaultAngle;
	float currDelta;


	void Awake()
	{
		defaultAngle = transform.eulerAngles.z;
		currDelta = 0f;
	}


	void OnEnable()
	{
		if (isPlayerShip)
		{
			PlayerShipShooter.OnStartFiring += HandleOnStartFiring;
			PlayerShipShooter.OnStopFiring += HandleOnStopFiring;
			//PlayerShipShooter.OnStartLaser += HandleOnStartFiring;
			//PlayerShipShooter.OnStopLaser += HandleOnStopFiring;
		}
		else
		{
			mustOscillate = true;
			StartCoroutine(CoOscillate());
		}
	}


	void OnDisable()
	{
		if (isPlayerShip)
		{
			PlayerShipShooter.OnStartFiring -= HandleOnStartFiring;
			PlayerShipShooter.OnStopFiring -= HandleOnStopFiring;
			//PlayerShipShooter.OnStartLaser -= HandleOnStartFiring;
			//PlayerShipShooter.OnStopLaser -= HandleOnStopFiring;
		}
		else
		{
			StopAllCoroutines();
		}
	}


	void HandleOnStartFiring(GunData gun)
	{
		//if the cannon (Transform) that starts firing is on the same GameObject which this oscillator is attached to, it starts too
		if (gun.cannon == transform)
		{
			mustOscillate = true;
			StopAllCoroutines();
			StartCoroutine(CoOscillate());
		}
	}


	void HandleOnStopFiring(GunData gun)
	{
		//if the cannon (Transform) that stopped firing is on the same GameObject which this oscillator is attached to, it stops too
		if (gun.cannon == transform)
		{
			mustOscillate = false;
			ResetAngle();
		}
	}


	IEnumerator CoOscillate()
	{
		do
		{
			currDelta += angularSpeed * Time.deltaTime;
			if (currDelta > amplitude / 2f)
			{
				currDelta = amplitude / 2f;
				angularSpeed *= -1;
			}
			else if (currDelta < -amplitude / 2f)
			{
				currDelta = -amplitude / 2f;
				angularSpeed *= -1;
			}
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, defaultAngle + currDelta);
			yield return null;
		} while (mustOscillate);

		ResetAngle();
	}


	void ResetAngle()
	{
		currDelta = 0;
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, defaultAngle);
	}
}

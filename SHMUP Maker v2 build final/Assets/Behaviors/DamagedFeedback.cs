///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
internal class DamagedFeedback : MonoBehaviour
{
	[SerializeField]
	DamagedBlinkDataSO data = null;

	SpriteRenderer spriteRenderer;
	int remainingPhases;
	bool isPhaseStrong;
	float phaseEndTime;
	Color regColor;


	//Appelée par EnemyHealth quand damaged
	internal void GotDamaged()
	{
		remainingPhases = data.blinksTotal * 2;
	}


	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		regColor = spriteRenderer.color;
		isPhaseStrong = false;
	}


	private void Update()
	{
		if (data == null)
			throw new Exception("Houston... we got a problem: the field 'Data' of DamagedFeedback in " + gameObject.name + " is empty!");

		if (remainingPhases > 0)
		{
			if (Time.time >= phaseEndTime) //on change de phase
			{
				isPhaseStrong = !isPhaseStrong;
				spriteRenderer.color = isPhaseStrong ? data.colorPhaseStrong : data.colorPhaseWeak;
				phaseEndTime = Time.time + data.phaseDuration;
				remainingPhases--;
			}
		}
		else
		{
			if (spriteRenderer.color != regColor && Time.time >= phaseEndTime)
				ResetVisual();
		}
	}


	private void OnDisable()
	{
		ResetVisual();
	}


	private void ResetVisual()
	{
		spriteRenderer.color = regColor;
	}
}
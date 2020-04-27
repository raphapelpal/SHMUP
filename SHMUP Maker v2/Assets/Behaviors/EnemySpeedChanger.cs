///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;

public class EnemySpeedChanger : TriggerableObject
{
	[SerializeField]
	EnemyMovePath moveManager = null;
	[SerializeField]
	float newSpeed = 5f;


	void OnEnable()
	{
		moveManager.speed = newSpeed;

		//disable this script right after having done its duty
		enabled = false;
	}
}

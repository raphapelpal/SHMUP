///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;
using System;

public class Timer : TriggeringObject
{
	[SerializeField]
	float duration = 1.5f;
//	[SerializeField]
//	bool mustSendEvent = true;
//	[SerializeField]
//	bool mustSelfDestruct = false;

	public event System.Action OnTimesUp;

	float startTime;


	void OnEnable()
	{
		startTime = Time.time;
	}


	void Update()
	{
		if (Time.time > startTime + duration)
		{
			TriggerOthersAndFinish();

			if (/*mustSendEvent &&*/ OnTimesUp != null)
				OnTimesUp();

//			if (mustSelfDestruct)
//				Destroy(gameObject);
		}
	}

}

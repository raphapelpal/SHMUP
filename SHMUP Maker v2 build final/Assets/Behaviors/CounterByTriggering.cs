///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;

/// <summary>
/// When this component has been triggered 'counter' times by other components, it activates all components in its own list 'componentsToTrigger'.
/// NOTE: Must be deactivated in Editor to work properly.
/// </summary>
public class CounterByTriggering : TriggeringObject
{
	[SerializeField]
	int counter = 1;

	int currCount = 0;


	void OnEnable()
	{
		currCount++;

		if (currCount >= counter)
		{
			TriggerOthersAndFinish();
		}
		else
		{
			enabled = false;
		}
	}
}

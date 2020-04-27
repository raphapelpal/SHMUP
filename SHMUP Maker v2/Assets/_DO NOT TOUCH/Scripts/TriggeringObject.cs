///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;

public abstract class TriggeringObject : TriggerableObject
{
	[SerializeField]
	protected TriggerableObject[] componentsToTrigger = new TriggerableObject[0];
	[SerializeField]
	bool dontAutoDisableComponents = false;

	bool mustDisableEventually = true;


	void Awake()
	{
		DoOnAwake();
	}


	protected virtual void DoOnAwake()
	{
		if (!dontAutoDisableComponents)
		{
			//disable all listed objects at level init
			foreach (TriggerableObject obj in componentsToTrigger)
			{
				if (obj != null)
					obj.enabled = false;
			}
		}
	}


	protected void TriggerOthersAndFinish()
	{
		if (mustDisableEventually)
			enabled = false;

		foreach (TriggerableObject obj in componentsToTrigger)
		{
			if (obj != null)
				obj.TriggerMe();
		}
	}


	//protected void MayDisable()
	//{
	//	if (mustDisableEventually)
	//		enabled = false;
	//}
}

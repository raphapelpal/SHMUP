///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBase : TriggeringObject {

	[SerializeField]
	protected Color colorLink2Targets = new Color(0.6f, 0.6f, 0.6f);

	void OnDrawGizmos()
	{
		//Needed coz when just added in a GameObject, error when array is empty
		if (componentsToTrigger.Length == 0)
			return;

		//drawing the line from this object to triggerableObjects referenced if they belong to other GameObjects
		foreach (TriggerableObject trigObj in componentsToTrigger)
		{
			if (trigObj != null && trigObj.gameObject != gameObject)
			{
			Gizmos.color = colorLink2Targets;
			Gizmos.DrawLine(transform.position, trigObj.transform.position);
			}
		}
	}

}

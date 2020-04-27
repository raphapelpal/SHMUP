///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOnCam : TriggerBase
{
	[SerializeField]
	Color colorTrigger = new Color(0f, 0.6f, 0f);


	void Update()
	{
		if (CamScroller.Me.screenBoundaries.xMax > transform.position.x)
		{
			TriggerOthersAndFinish();
		}
	}


	void OnDrawGizmos()
	{
		//drawing the vertical line of the trigger
		Gizmos.color = colorTrigger;
		Gizmos.DrawLine(new Vector2(transform.position.x, transform.position.y - 6f),
			new Vector2(transform.position.x, transform.position.y + 6f));

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

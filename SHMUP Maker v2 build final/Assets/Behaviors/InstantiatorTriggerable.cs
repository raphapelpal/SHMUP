///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;

public class InstantiatorTriggerable : TriggerableObject
{
	[SerializeField]
	GameObject objToSpawn = null;

	void OnEnable()
	{
		Instantiate(objToSpawn, transform.position, transform.rotation);

		enabled = false;
	}
}

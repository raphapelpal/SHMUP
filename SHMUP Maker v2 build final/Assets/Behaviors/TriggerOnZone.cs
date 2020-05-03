///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerOnZone : TriggerBase
{
	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == Alias.LAYER_SHIP)
		{
			TriggerOthersAndFinish();
		}
	}
}

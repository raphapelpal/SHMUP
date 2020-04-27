///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollSpeedChanger : TriggerableObject
{
	[SerializeField]
	float scrollingSpeed = 2f;

	void OnEnable()
	{
		CamScroller.Me.setScrollSpeed(scrollingSpeed); 
	}
}

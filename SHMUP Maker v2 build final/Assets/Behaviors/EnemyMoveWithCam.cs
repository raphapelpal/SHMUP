///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;
using System;

public class EnemyMoveWithCam : EnemyMove
{
	Vector2 initialDeltaPos;
	Vector2 totalMove = Vector2.zero;


	protected override void DoOnEnable()
	{
		initialDeltaPos = initialPos - (Vector2)Camera.main.transform.position;
	}


	protected override void DoOnUpdate()
	{
		moveRelative();
	}


	override protected void CheckDistanceCondition()
	{
		if (totalMove.x > value)
		{
			TriggerOthersAndFinish();
		}
	}


	private void moveRelative()
	{
		totalMove.x += speed * Time.deltaTime;

		Vector2 newPos = (Vector2)Camera.main.transform.position + initialDeltaPos + totalMove;
		transform.position = rb.position = newPos;
	}
}

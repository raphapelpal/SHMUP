///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;

public class EnemyMoveTarget : EnemyMove
{
	[SerializeField]
	protected Transform target;


	override protected void DoOnStart()
	{
		base.DoOnStart();
		//if no target specified, it is the player's ship
		if (target == null)
		{
			PlayerShipMove psm = FindObjectOfType<PlayerShipMove>();
			if (psm != null)
				target = FindObjectOfType<PlayerShipMove>().transform;
		}
	}


	//USE THAT METHOD INSTEAD OF Update() FOR CLASSES THAT DERIVE FROM EnemyMove!
	override protected void DoOnUpdate()
	{
		MoveToTarget();
	}


	protected override void CheckDistanceCondition()
	{
		if (target == null)
			return;

		float d = Vector2.SqrMagnitude(transform.position - target.position);
		if (d < value * value)
		{
			TriggerOthersAndFinish();
		}
	}


	void MoveToTarget()
	{
		if (target != null)
		{
			Vector2 newPos = Vector2.MoveTowards(rb.position, target.position, speed * Time.deltaTime);
			transform.position = rb.position = newPos;
		}
	}

}

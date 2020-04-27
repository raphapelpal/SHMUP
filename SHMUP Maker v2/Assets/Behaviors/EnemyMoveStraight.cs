///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;


public class EnemyMoveStraight : EnemyMove
{
	[SerializeField]
	private Vector2 freeMoveDir = Vector2.left;


	//USE THAT METHOD INSTEAD OF Update() FOR CLASSES THAT DERIVE FROM EnemyMove!
	override protected void DoOnUpdate()
	{
		MoveStraight();
	}


	override protected void CheckDistanceCondition()
	{
		float d = Vector2.SqrMagnitude((Vector2)transform.position - initialPos);
		if (d > value * value)
		{
			TriggerOthersAndFinish();
		}
	}


	void MoveStraight()
	{
		Vector2 move = freeMoveDir * (speed * Time.deltaTime);
		Vector2 newPos = new Vector2(transform.position.x + move.x, transform.position.y + move.y);
		transform.position = rb.position = newPos;
	}

}

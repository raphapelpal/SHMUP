///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;
using System;

public class EnemyMovePath : EnemyMove
{
	[NonSerialized]
	public Spawner mySpawner;
	[SerializeField]
	TriggerableObject[] componentsToTriggerOnPhase2 = null;

	int currTgtWpIndex;
	bool isPhase2Reached = false;
	Vector2 moveDir;
	bool isPostPath;


	public void Init(Spawner spawner)
	{
		endCondition = BehaviorChangeConditions.NA;
		mySpawner = spawner;
		currTgtWpIndex = 1;
		isPostPath = false;

		//Positionning enemy at the first waypoint's coordinates
		if (mySpawner != null)
			rb.position = transform.position = mySpawner.GetPath()[0];

		//Orienting enemy in the direction of the path, if there are more than one waypoints
		//if (path.Length > 1)
		//{
		//	transform.LookAt(path[currTargetIndex]);
		//	rb.rotation = transform.eulerAngles.z;
		//}
	}


	override protected void DoOnUpdate()
	{
		if (!isPostPath)
		{
			if (mySpawner != null)
			{
				MoveOnPath();
				CheckPhase2();
			}
			else
			{
				GoPostPath();
			}
		}
		else
		{
			MovePostPath();
		}
	}


	void MoveOnPath()
	{
		float move = speed * Time.deltaTime;

		//If there is still a waypoint as target, we check if we are close enough to it to consider we have reached it, and target the next index
		if (currTgtWpIndex < mySpawner.GetPath().Length)
		{
			Vector2 v = rb.position - (Vector2)mySpawner.GetPath()[currTgtWpIndex];
			if (v.sqrMagnitude < move * move)
			{
				currTgtWpIndex++;
			}
		}

		Vector2 newPos;
		//Once the wp index has been potentially updated, we check if the target index is valid (i.e. if we are still in the path or have finished it)
		if (currTgtWpIndex < mySpawner.GetPath().Length)
		{
			//If we are in the path we move towards the target wp
			newPos = Vector2.MoveTowards(rb.position, mySpawner.GetPath()[currTgtWpIndex], move);
			moveDir = newPos - rb.position;
			transform.position = rb.position = newPos;
		}
		else
		{
			//If the path is finished we activate the next EnemyMove behavior
			GoPostPath();
		}
	}


	void CheckPhase2()
	{
		//If we are beyond the waypoint to start a potential phase 2 of the enemy, we inform it (but only once)
		if (!isPhase2Reached && currTgtWpIndex > mySpawner.GetPhase2PathIndex()) {
			isPhase2Reached = true;
			TriggerPhase2();
		}
	}


	private void TriggerPhase2()
	{
		foreach (TriggerableObject obj in componentsToTriggerOnPhase2)
		{
			obj.TriggerMe();
		}
	}


	void GoPostPath()
	{
		isPostPath = true;

		//if no EnemyMove behavior has been specified in the trigger list, we keep the current script enabled and will move straight in the current direction and speed
		if (!hasNextMoveBehavior)
		{
			moveDir.Normalize(); //we normalize moveDir so that we can use it as a pure direction (vector length of 1) henceforth
		}

		//We still move the enemy during this frame
		MovePostPath();

		//And we trigger the other objects
		TriggerOthersAndFinish();
	}


	void MovePostPath()
	{
		Vector2 move = moveDir * (speed * Time.deltaTime);
		transform.position = rb.position = (Vector2)transform.position + move;
	}

}

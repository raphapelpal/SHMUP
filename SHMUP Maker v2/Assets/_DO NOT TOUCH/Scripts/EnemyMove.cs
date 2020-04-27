///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using System;
using UnityEngine;

abstract public class EnemyMove : TriggeringObject
{
	protected Rigidbody2D rb; //rigidbody2d reference
	public float speed = 5f; //speed of enemy
	[SerializeField]
	protected BehaviorChangeConditions endCondition;
	[SerializeField]
	protected float value;
	[SerializeField]
	protected EnemyHealth healthManager;

	protected Vector2 initialPos;
	private float maxTimeOffscreen = 3f;
	float offscreenTime;
	float startTime;
	protected bool hasNextMoveBehavior = false;


	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();

		int enemyMoveScripts = 0;
		foreach (TriggerableObject obj in componentsToTrigger)
		{
			if (obj != null && obj.GetComponent<EnemyMove>() != null)
			{
				enemyMoveScripts++;
			}
			if (enemyMoveScripts > 1)
			{
				Debug.LogWarningFormat("<color=brown>Component " + this + " has " + enemyMoveScripts + " EnemyMove scripts in its list of components to trigger. (Should have only one.)</color>");//DEBUG
				hasNextMoveBehavior = true;
			}
		}
	}


	void Start()
	{
		DoOnStart();
	}


	//use that method instead of start() for classes that will derive from enemymove
	virtual protected void DoOnStart() { }


	void OnEnable()
	{
		initialPos = transform.position;
		startTime = Time.time;

		DoOnEnable();
	}


	//use that method instead of onenable() for classes that will derive from enemymove
	virtual protected void DoOnEnable() { }


	void Update()
	{
		DoOnUpdate();

		CheckChangeBehaviorCondition();

		CheckOffscreen();
	}


	//use that method instead of update() for classes that will derive from enemymove
	virtual protected void DoOnUpdate() { }

	private void CheckChangeBehaviorCondition()
	{
		switch (endCondition)
		{
			case BehaviorChangeConditions.delay:
				CheckDelayCondition();
				break;
			case BehaviorChangeConditions.healthRemaining:
				CheckHealthCondition();
				break;
			case BehaviorChangeConditions.distance:
				CheckDistanceCondition();
				break;
		}
	}


	virtual protected void CheckHealthCondition()
	{
		//if hitpoints are lower (in proportion) than a certain value (btw 1 and 0), we change behavior
		if (healthManager.GetHps() <= value)
		{
			TriggerOthersAndFinish();
		}
	}


	virtual protected void CheckDistanceCondition() { }


	virtual protected void CheckDelayCondition()
	{
		if (Time.time >= startTime + value)
		{
			TriggerOthersAndFinish();
		}
	}
	//Checks whether this enemy is on- or offscreen, as after some time offscreen the enemy is destroyed
	protected void CheckOffscreen()
	{
		if (rb.position.x < CamScroller.Me.screenBoundaries.xMin
			|| rb.position.y > CamScroller.Me.screenBoundaries.yMax || rb.position.y < CamScroller.Me.screenBoundaries.yMin)
		{
			offscreenTime += Time.fixedDeltaTime;

			if (offscreenTime > maxTimeOffscreen)
			{
				JustDestroy();
			}
		}
		else
		{
			offscreenTime = 0f;
		}
	}


	private void JustDestroy()
	{
		Destroy(gameObject);
	}

}


public enum BehaviorChangeConditions
{
	NA,
	distance,
	healthRemaining,
	delay
}
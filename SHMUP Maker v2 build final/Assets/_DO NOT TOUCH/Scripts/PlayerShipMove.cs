///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;
using System;

public class PlayerShipMove : MonoBehaviour
{
	public static PlayerShipMove Me;

	[SerializeField]
	private float speedX = 10f;
	[SerializeField]
	private float speedY = 10f;
	[SerializeField]
	private Animator animator = null;
	[SerializeField]
	private Rigidbody2D rb = null;

	ShipStates state;
	float accelVictory = 20f;


	void Awake()
	{
		Me = this;

		state = ShipStates.init;
	}

	
	void OnEnable()
	{
		GameManager.OnPlayStart += handleOnPlayStart;
		GameManager.OnVictory += handleOnVictory;
		PlayerShipHealth.OnShipDestroyed += handleOnShipDestroyed;
	}


	void OnDisable()
	{
		GameManager.OnPlayStart -= handleOnPlayStart;
		GameManager.OnVictory -= handleOnVictory;
		PlayerShipHealth.OnShipDestroyed -= handleOnShipDestroyed;
	}


	private void handleOnPlayStart()
	{
		//unregister from event that don't happen again 
		GameManager.OnPlayStart -= handleOnPlayStart;

		state = ShipStates.controllable;
	}


	private void handleOnVictory()
	{
		//unregister from event that don't happen again 
		GameManager.OnVictory -= handleOnVictory;

		state = ShipStates.victorious;
		
		//Anim
		animator.SetFloat("Vertical Speed", 0f);
	}


	private void handleOnShipDestroyed()
	{
		//unregister from event that don't happen again 
		PlayerShipHealth.OnShipDestroyed -= handleOnShipDestroyed;

		state = ShipStates.dying;
	}


	void FixedUpdate()
	{
		if (state == ShipStates.victorious)
		{
			doOnVictory();
			return;
		}
		//We don't allow control of the ship if we are not in play
		else if (state != ShipStates.controllable)
			return;

		//Taking move input from the user
		float moveHorizontal = Input.GetAxisRaw("Horizontal");
		float moveVertical = Input.GetAxisRaw("Vertical");

		//Movement according to the input of the player
		Vector2 movement = new Vector2(moveHorizontal * speedX * Time.fixedDeltaTime, moveVertical * speedY * Time.fixedDeltaTime);

		//Adding cam scrolling
		movement.x += CamScroller.Me.getScrollSpeed() * Time.fixedDeltaTime;

		//Calculating the new position of the ship, clamp it
		Vector2 pos = (Vector2)transform.position + movement;
		//Clamp the new position to screen's edges
		pos.x = Mathf.Clamp(pos.x, CamScroller.Me.screenBoundaries.xMin, CamScroller.Me.screenBoundaries.xMax);
		pos.y = Mathf.Clamp(pos.y, CamScroller.Me.screenBoundaries.yMin, CamScroller.Me.screenBoundaries.yMax);

		//positioning the ship
		rb.position = pos;

		//Anim
		animator.SetFloat("Vertical Speed", movement.y);
	}


	private void doOnVictory()
	{
		speedX += accelVictory * Time.fixedDeltaTime;
		//Calculating the new position of the ship
		Vector2 pos = (Vector2)transform.position + new Vector2(speedX * Time.fixedDeltaTime, 0f);
		//positioning the ship
		transform.position = rb.position = pos;
	}
}


public enum ShipStates
{
	init,
	controllable,
	dying,
	victorious
}


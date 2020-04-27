///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;

public class PlayerShipCollider : MonoBehaviour
{
	[SerializeField]
	PlayerShipHealth healthManager = null;

	void OnEnable()
	{
		GameManager.OnVictory += handleOnVictory;
	}


	void OnDisable()
	{
		GameManager.OnVictory -= handleOnVictory;
	}


	private void handleOnVictory()
	{
		enabled = false;

		GetComponent<Collider2D>().enabled = false;
	}


	void OnCollisionEnter2D(Collision2D otherColl)
	{
		//Check collision with level
		if (otherColl.gameObject.layer == Alias.LAYER_MAP)
			healthManager.gotMapHit();
	}


	void OnCollisionStay2D(Collision2D otherColl)
	{
		//Check collision with level
		if (otherColl.gameObject.layer == Alias.LAYER_MAP)
			healthManager.gotMapHit();
	}
}

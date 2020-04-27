///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivatorUniversal : TriggerableObject
{
	[SerializeField]
	SpriteRenderer spriteRenderer = null;
	[SerializeField]
	Animator animator = null;
	[SerializeField]
	Rigidbody2D rigidbody2d = null;
	[SerializeField]
	Collider2D collider2d = null;
	[SerializeField]
	MonoBehaviour[] otherComponentsToDeactivate = null;


	void OnEnable()
	{
		foreach (MonoBehaviour comp in otherComponentsToDeactivate)
		{
			comp.enabled = false;
		}

		if (spriteRenderer != null)
			spriteRenderer.enabled = false;
		if (animator != null)
			animator.enabled = false;
		if (rigidbody2d != null)
			rigidbody2d.simulated = false;
		if (collider2d != null)
			collider2d.enabled = false;

		enabled = false;
	}

}

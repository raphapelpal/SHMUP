///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorGeneric : TriggerableObject
{
	[SerializeField]
	SpriteRenderer spriteRenderer = null;
	[SerializeField]
	Animator animator = null;
	[SerializeField]
	Rigidbody2D rigidbody2d = null;
	[SerializeField]
	Collider2D collider2d = null;


	void Awake()
	{
		if (spriteRenderer != null)
			spriteRenderer.enabled = false;
		if (animator != null)
			animator.enabled = false;
		if (rigidbody2d != null)
			rigidbody2d.simulated = false;
		if (collider2d != null)
			collider2d.enabled = false;
	}


	void OnEnable()
	{
		if (spriteRenderer != null)
			spriteRenderer.enabled = true;
		if (animator != null)
			animator.enabled = true;
		if (rigidbody2d != null)
			rigidbody2d.simulated = true;
		if (collider2d != null)
			collider2d.enabled = true;
	}
}

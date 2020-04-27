///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody2D))]
public class Pickup : TriggeringObject
{
	//[SerializeField]
	Rigidbody2D rb = null;
	//[SerializeField]
	Animator animator = null;
	[SerializeField] Pickups[] types = { Pickups.BulletA };
	[SerializeField] float cyclingDelay = 2.5f;
	//[SerializeField] Pickups[] cycleList; // = { Pickups.BulletA, Pickups.BulletB, Pickups.BulletC, Pickups.LaserA, Pickups.LaserB };
	[SerializeField]
	bool isSwinging = true;
	//[SerializeField]
	Vector2 swingRadii = new Vector2(0.06f, 0.12f);
	//[SerializeField]
	float swingSpeed = 3f;

	public delegate void DelegatePickup(Pickups type);
	static public event DelegatePickup OnPickup;

	bool isPicked;
	bool isCycling = false;
	int currIndex = 0;


	void OnBecameVisible()
	{
		enabled = true;
	}


	void Start()
	{
		isPicked = false;
		enabled = false;

		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		if (types.Length > 1 && types[0] != Pickups.Bomb /*&& types[0] != Pickups.Health*/)
		{
			isCycling = true;
			StartCoroutine(CoCycle());
		}

		UpdateVisual();

		if (isSwinging)
			StartCoroutine(coSwing());

	}


	private IEnumerator CoCycle()
	{
		//init current index
		currIndex = 0;

		do
		{
			yield return new WaitForSeconds(cyclingDelay);
			if (++currIndex == types.Length)
				currIndex = 0;
			UpdateVisual();
		} while (!isPicked);
	}


	IEnumerator coSwing()
	{
		Vector2 basePos = transform.position;
		float a = 0f;

		do
		{
			a += Time.deltaTime * swingSpeed;
			float xOffset = Mathf.Sin(a * 2) * swingRadii.x;
			float yOffset = Mathf.Sin(a) * swingRadii.y;
			rb.position = transform.position = new Vector3(basePos.x + xOffset, basePos.y + yOffset, 0f);
			yield return null;
		} while (!isPicked);
	}


	void OnTriggerEnter2D(Collider2D otherColl)
	{
		if (otherColl.gameObject.layer == Alias.LAYER_SHIP)
		{
			GotPicked();
		}
	}


	private void GotPicked()
	{
		isPicked = true;

		OnPickup?.Invoke(types[currIndex]);
		//if (OnPickupInstance != null)	OnPickupInstance();

		TriggerOthersAndFinish();
	}


	void UpdateVisual()
	{
		if (animator != null)
		{
			animator.SetInteger("Type", (int)types[currIndex]);
			animator.SetBool("isCycle", isCycling);
		}
	}


	/// <summary>
	/// Display special visual for each type of Pickup in the scene view (editor only)
	/// </summary>
	void OnDrawGizmos()
	{
		Color color;
		string label;
		int textSize;

		if (types[0] == Pickups.Bomb)
		{
			//Gizmos.color = Color.red;
			//Gizmos.DrawWireSphere(rb.position, 0.7f);
			color = new Color(1f, 0f, 0f, 0.4f);
			label = "B";
			textSize = 22;
		}
		else if (types[0] == Pickups.Health)
		{
			color = isCycling ? new Color(0.5f, 0.9f, 0f, 0.4f) : new Color(0f, 1f, 0.3f, 0.4f);
			label = "+";
			textSize = 22;
		}
		else
		{
			color = isCycling ? new Color(1f, 1f, 0f, 0.4f) : new Color(1f, 1f, 1f, 0.4f);
			label = types[0].ToString();
			label = (label[0]).ToString() + label[label.Length - 1];
			textSize = 20;
		}

#if UNITY_EDITOR
		Handles.color = color;
		Handles.DrawSolidDisc(transform.position, Vector3.back, 0.6f);

		//Display label
		GUIContent content = new GUIContent(label);

		GUIStyle style = new GUIStyle();
		style.fontSize = textSize;
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = Color.black;
		Vector2 contentSize = style.CalcSize(content);
		style.contentOffset = new Vector2(-contentSize.x * 0.45f, -contentSize.y * 0.6f);
		Handles.Label(transform.position, content, style);
#endif
	}
}


public enum Pickups
{
	BulletA = 0,
	BulletB = 1,
	BulletC = 2,
	BulletD = 3,
	BulletE = 4,
	LaserA = 6,
	LaserB = 7,
	LaserC = 8,
	Bomb = 10,
	Health = 15,
}

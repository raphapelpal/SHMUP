///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;
using System;

public class ShipLaser : MonoBehaviour
{
	static int CollLayerMask = (1 << Alias.LAYER_ENEMIES) | (1 << Alias.LAYER_MAP);

	[SerializeField]
	float dmgPerSec = 50f;
	[SerializeField]
	bool isStoppedByMap = true;
	[SerializeField]
	bool isStoppedByEnemies = false;
	[SerializeField]
	Transform hitObject = null;

	Transform beamOrigin;
	Vector2 endPoint;
	Vector2 defaultScale;
	float maxLength;
	float spriteLength;


	void Awake()
	{
		defaultScale = transform.localScale;
		spriteLength = GetComponent<SpriteRenderer>().bounds.extents.x;
	}


	public void Init(Transform origin)
	{

		beamOrigin = origin;

		InitHitObject();
	}


	void InitHitObject()
	{
		if (hitObject != null)
		{
			hitObject.parent = null;
			hitObject.localScale = Vector3.one;
		}
	}


	void OnEnable()
	{
		PlayerShipShooter.OnStopFiring += HandleOnStopLaser;
	}


	void OnDisable()
	{
		PlayerShipShooter.OnStopFiring -= HandleOnStopLaser;
	}


	void Update()
	{
		Move();
		CheckColl();
		UpdateVisual();
	}


	private void HandleOnStopLaser(GunData gun)
	{
		//if the cannon (Transform) that stopped firing is on the same GameObject which this Laser is attached to, it stops too
		if (gun.cannon == beamOrigin)
		{
			EndMyself();
		}
	}


	private void Move()
	{
		transform.position = beamOrigin.position;
		transform.rotation = beamOrigin.rotation;
	}


	private void CheckColl()
	{
		//Find intersection bw laser point and screen edge
		Vector2 endPtScrnEdge;
		float x, y;

		Vector2 p = beamOrigin.position; //known pt on the line
		Vector2 fv = beamOrigin.right; //slope in parametric format

		//finding x edge
		if (fv.x == 0)
			fv.x = 0.0001f;

		//Special case: horizontal line
		if (fv.y == 0f)
		{
			x = fv.x > 0f ? CamScroller.Me.screenBoundaries.xMax : CamScroller.Me.screenBoundaries.xMin;
			endPtScrnEdge = new Vector2(x, p.y);
		}
		//Special case: vertical line
		else if (fv.x == 0f)
		{
			y = fv.y > 0f ? CamScroller.Me.screenBoundaries.yMax : CamScroller.Me.screenBoundaries.yMin;
			endPtScrnEdge = new Vector2(p.x, y);
		}
		//Other straight line, requires to find line's equation
		else
		{
			float a = fv.y / fv.x; //slope
			float b = p.y - a * p.x; //offset

			x = fv.x > 0f ? CamScroller.Me.screenBoundaries.xMax : CamScroller.Me.screenBoundaries.xMin;
			y = a * x + b;
			//if the line exits the screen from top or bottom edge instead of left or right, we use yMax or yMin to find x
			if (y > CamScroller.Me.screenBoundaries.yMax || y < CamScroller.Me.screenBoundaries.yMin)
			{
				y = fv.y > 0f ? CamScroller.Me.screenBoundaries.yMax : CamScroller.Me.screenBoundaries.yMin;
				x = (y - b) / a;
			}
			endPtScrnEdge = new Vector2(x, y);
		}

		//Cast the ray
		RaycastHit2D[] hits = Physics2D.LinecastAll(beamOrigin.position, endPtScrnEdge, CollLayerMask);
		//Check for pts of contact with enemy ships or map
		for (int i = 0; i < hits.Length; i++)
		{
			//laser on contact with enemy ships
			if (hits[i].collider.gameObject.layer == Alias.LAYER_ENEMIES)
			{
				hits[i].transform.GetComponent<EnemyCollider>().OnHitByLaser(dmgPerSec * Time.deltaTime);
				//laser stops if it doesn't go through enemies
				if (isStoppedByEnemies)
				{
					endPoint = hits[i].point;
					UpdateHitObject(true);
					return;
				}
			}
			//laser on contact with map
			else if (hits[i].collider.gameObject.layer == Alias.LAYER_MAP && isStoppedByMap)
			{
				endPoint = hits[i].point;
				UpdateHitObject(true);
				return;
			}
		}
		//executed only if nothing collided or laser wasn't stopped
		endPoint = endPtScrnEdge;
		UpdateHitObject(false);
	}


	void UpdateVisual()
	{
		//Scale beam according to endPoint
		float length = (endPoint - (Vector2)beamOrigin.position).magnitude;
		float mul = length / spriteLength * .5f;
		transform.localScale = new Vector3(defaultScale.x * mul, defaultScale.y, 1f);
	}


	void UpdateHitObject(bool isDisplayed)
	{
		if (hitObject != null)
		{
			if (isDisplayed)
			{
				if (!hitObject.gameObject.activeSelf)
					hitObject.gameObject.SetActive(true);
				hitObject.position = endPoint;
			}
			else
			{
				if (hitObject.gameObject.activeSelf)
					hitObject.gameObject.SetActive(false);
			}
		}
	}


	private void EndMyself()
	{
		EndHitObject();
		Destroy(gameObject);
	}


	void EndHitObject()
	{
		if (hitObject != null)
		{
			Destroy(hitObject.gameObject);
		}
	}

}

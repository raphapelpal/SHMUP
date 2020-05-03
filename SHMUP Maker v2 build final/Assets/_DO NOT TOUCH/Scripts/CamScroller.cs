///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;
using System;

public class CamScroller : MonoBehaviour
{
	static public CamScroller Me;

	[SerializeField]
	private Camera cam = null;
	[SerializeField]
	private float scrollSpeed = 2f;

	public Rect screenBoundaries { get; private set; }

	bool isInPlay;


	public float getScrollSpeed()
	{
		return scrollSpeed;
	}


	public void setScrollSpeed(float val)
	{
		scrollSpeed = val;
	}


	void Awake()
	{
		Me = this;
		isInPlay = false;
	}


	void OnEnable()
	{
		GameManager.OnPlayStart += handleOnPlayStart;
		GameManager.OnGameOver += handleOnGameOver;
	}

	private void handleOnGameOver()
	{
		isInPlay = false;
	}

	void OnDisable()
	{
		GameManager.OnPlayStart -= handleOnPlayStart;
	}


	private void handleOnPlayStart()
	{
		isInPlay = true;
	}


	// Use this for initialization
	void Start()
	{
		updateBoundaries();
	}


    void Update()
	{
		if (!isInPlay)
			return;

		updateBoundaries();
	}


	void LateUpdate()
	{
		if (!isInPlay)
			return;

		scroll();
	}


	private void scroll()
	{
		transform.Translate(new Vector3(scrollSpeed * Time.deltaTime, 0f, 0f));
	}


	void updateBoundaries()
	{
		float halfX = cam.orthographicSize * cam.aspect;
		float halfY = cam.orthographicSize;
        screenBoundaries = new Rect(transform.position.x - halfX, transform.position.y - halfY, halfX * 2, halfY * 2);
	}
}

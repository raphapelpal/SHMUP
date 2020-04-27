// This is free and unencumbered software released into the public domain.
// For more information, please refer to <http://unlicense.org/>
//Mod by IT

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScreenFader : MonoBehaviour
{
	public enum Transitions
	{
		intro,
		gameOver,
		victory,
		menuToLevel
	}

	static public ScreenFader Me;

	//public bool fadeIn = true;
	public float fadeInTime = 0.6f;
	public float menuToLevelFadeTime = 0.7f;
	public float gameOverFadeTime = 0.9f;
	public float victoryFadeTime = 1.2f;
	public Color fadeColorDefault = Color.black;
	public Color fadeColorVictory = Color.white;
	public Material fadeMaterial = null;

	private List<ScreenFadeControl> fadeControls = new List<ScreenFadeControl>();


	void Awake()
	{
		Me = this;
	}


	public IEnumerator CoFade(Transitions transition)
	{
		// Clean up from last fade
		foreach (ScreenFadeControl fadeControl in fadeControls)
		{
			Destroy(fadeControl);
		}
		fadeControls.Clear();

		// Find all cameras and add fade material to them (initially disabled)
		foreach (Camera c in Camera.allCameras)
		{
			var fadeControl = c.gameObject.AddComponent<ScreenFadeControl>();
			fadeControl.fadeMaterial = fadeMaterial;
			fadeControls.Add(fadeControl);
		}

		// Do fade
		if (transition == Transitions.intro)
			yield return StartCoroutine(CoFadeIn());
		else
			yield return StartCoroutine(CoFadeOut(transition));
	}


	IEnumerator CoFadeOut(Transitions moment)
	{
		// Derived from OVRScreenFade
		float elapsedTime = 0.0f;
		Color color;
		float duration;
        if (moment == Transitions.victory)
		{
			color = fadeColorVictory;
			duration = victoryFadeTime;
        }
		else if (moment == Transitions.gameOver)
		{
			color = fadeColorDefault;
			duration = gameOverFadeTime;
        }
		else
		{
			print("here we go");//TEST
			color = fadeColorDefault;
			duration = menuToLevelFadeTime;
        }
		color.a = 0.0f;
		fadeMaterial.color = color;
        while (elapsedTime < duration)
		{
			yield return new WaitForEndOfFrame();
			elapsedTime += Time.deltaTime;
			color.a = Mathf.Clamp01(elapsedTime / duration);
			fadeMaterial.color = color;
		}
	}


	IEnumerator CoFadeIn()
	{
		float elapsedTime = 0.0f;
		Color color = fadeMaterial.color = fadeColorDefault;
		while (elapsedTime < fadeInTime)
		{
			yield return new WaitForEndOfFrame();
			elapsedTime += Time.deltaTime;
			color.a = 1.0f - Mathf.Clamp01(elapsedTime / fadeInTime);
			fadeMaterial.color = color;
		}
		SetFadersEnabled(false);
	}


	void SetFadersEnabled(bool value)
	{
		foreach (ScreenFadeControl fadeControl in fadeControls)
			fadeControl.enabled = value;
	}

}

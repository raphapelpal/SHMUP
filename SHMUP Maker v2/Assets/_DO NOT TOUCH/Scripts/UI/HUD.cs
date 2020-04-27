///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour
{
	[SerializeField]
	Text scoreDisplay = null;
	[SerializeField]
	Slider hpSlider = null;


	void OnEnable()
	{
		ScoreManager.OnScoreUpdated += HandleOnScoreUpdated;
		PlayerShipHealth.OnHpChange += HandleOnShipHpChange;
	}


	void OnDisable()
	{
		ScoreManager.OnScoreUpdated -= HandleOnScoreUpdated;
		PlayerShipHealth.OnHpChange -= HandleOnShipHpChange;
	}


	private void HandleOnShipHpChange(float hp, float max)
	{
		hpSlider.value = Mathf.InverseLerp(0f, max, hp);
	}


	private void HandleOnScoreUpdated(int i)
	{
		UpdateScoreDisplay(i);
	}


	void UpdateScoreDisplay(int newVal)
	{
		scoreDisplay.text = newVal.ToString();
	}

}

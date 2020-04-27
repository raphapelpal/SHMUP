///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using System;
using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class ScoreManager : ScriptableObject
{
	[Serializable]
	public struct EnemyValue
	{
		public Enemies enemy;
		public int value;
	}

	[SerializeField]
	EnemyValue[] enemyValues = null;

	int score;

	static public event Action<int> OnScoreUpdated;


	void OnEnable()
	{
		score = 0;
		SetScore(score);
		EnemyHealth.OnKilledStatic += HandleOnEnemyKilled;
	}


	void OnDisable()
	{
		EnemyHealth.OnKilledStatic -= HandleOnEnemyKilled;
	}


	private void HandleOnEnemyKilled(Enemies e)
	{
		int value = -1;

		foreach (EnemyValue ev in enemyValues)
		{
			if (ev.enemy == e)
			{
				value = ev.value;
				break;
			}
		}

		if (value > -1)
		{
			AddScore(value);
		}
		else
			Debug.Log("BUG in Scorer: enemy type not referenced");//DEBUG
	}


	void AddScore(int addedVal)
	{
		SetScore(score + addedVal);
	}


	void SetScore(int newVal)
	{
		score = newVal;

		OnScoreUpdated?.Invoke(score);
	}

}

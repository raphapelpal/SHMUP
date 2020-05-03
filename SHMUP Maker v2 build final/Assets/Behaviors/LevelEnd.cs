///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;

public class LevelEnd : TriggerableObject
{
	[SerializeField]
	private int nextLevelIndex = 0;


	private void OnEnable()
	{
		GameManager.GoVictory(nextLevelIndex);
	}
}

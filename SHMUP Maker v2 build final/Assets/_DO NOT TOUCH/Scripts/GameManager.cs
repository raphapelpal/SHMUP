///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
	static GameManager Me;

	//[SerializeField]
	//float initDuration = 1f;
	[SerializeField]
	float gameOverDuration = 2f;
	[SerializeField]
	float victoryDuration = 0f;

	GameStates state;
	int nextSceneIndex = 0;


	public static event System.Action OnPlayStart;
	public static event System.Action OnGameOver;
	public static event System.Action OnVictory;


	static public void GoVictory(int nextBuildIndex)
	{
		Me.nextSceneIndex = nextBuildIndex;
		Me.state = GameStates.victory;
	}


	void Awake()
	{
		Me = this;
		StartCoroutine(CoInit());
	}


	void OnEnable()
	{
		PlayerShipHealth.OnShipDestroyed += HandleOnShipDestroyed;
		//LevelEnd.OnReachedExit += handleOnReachedExit;
	}


	void OnDisable()
	{
		PlayerShipHealth.OnShipDestroyed -= HandleOnShipDestroyed;
		//LevelEnd.OnReachedExit -= handleOnReachedExit;
	}


	private void HandleOnShipDestroyed()
	{
		state = GameStates.over;
	}


	IEnumerator CoInit()
	{
		state = GameStates.init;

		//yield return new WaitForSeconds(initDuration);
		yield return StartCoroutine(ScreenFader.Me.CoFade(ScreenFader.Transitions.intro));

		OnPlayStart?.Invoke();

		StartCoroutine(CoPlay());
	}


	IEnumerator CoPlay()
	{
		state = GameStates.play;

		do
		{
			yield return null;
		} while (state == GameStates.play);

		if (state == GameStates.over)
		{
			StartCoroutine(CoGameOver());
		}
		else if (state == GameStates.victory)
		{
			StartCoroutine(CoVictory());
		}
	}


	IEnumerator CoGameOver()
	{
		OnGameOver?.Invoke();

		yield return new WaitForSeconds(gameOverDuration);

		yield return StartCoroutine(ScreenFader.Me.CoFade(ScreenFader.Transitions.gameOver));

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //reload current scene
	}


	IEnumerator CoVictory()
	{
		OnVictory?.Invoke();

		yield return new WaitForSeconds(victoryDuration);

		yield return StartCoroutine(ScreenFader.Me.CoFade(ScreenFader.Transitions.victory));

		if (nextSceneIndex < 0)
		{
			SceneManager.LoadScene("Scene Level Select");
		}
		else if (nextSceneIndex < 9)
		{
			SceneManager.LoadScene(nextSceneIndex);
		}
		else
		{
			SceneManager.LoadScene("Scene End Game");
		}
	}
}


public enum GameStates
{
	init,
	play,
	over,
	victory
}


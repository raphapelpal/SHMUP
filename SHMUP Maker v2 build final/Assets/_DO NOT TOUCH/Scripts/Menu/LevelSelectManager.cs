///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelSelectManager : MonoBehaviour
{
	//public static string TeamLetter = "A";

	//[SerializeField] Text titleField;


	void Start()
	{
		//titleField.text = "EPISODE " + TeamLetter;
	}


	public void LaunchLevel(int levelIndex)
	{
		//string sceneName = "Scene Level " + levelIndex; //"T" + TeamLetter + " Scene Level " + levelIndex;

		StartCoroutine(CoLaunchLevel(levelIndex));
	}


	public void GoBackToMainMenu()
	{
		SceneManager.LoadScene("Scene Level Select");
	}


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}


	IEnumerator CoLaunchLevel(string sceneName)
	{
		//yield return StartCoroutine(ScreenFader.Me.coFade(ScreenFader.Transitions.menuToLevel));
		yield return null;
		SceneManager.LoadScene(sceneName);
	}


	IEnumerator CoLaunchLevel(int buildIndex)
	{
		//yield return StartCoroutine(ScreenFader.Me.coFade(ScreenFader.Transitions.menuToLevel));
		yield return null;
		SceneManager.LoadScene(buildIndex);
	}
}

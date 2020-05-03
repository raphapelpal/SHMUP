///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{

	void Start()
	{

	}


	public void launchLevelSelectScene(string teamLetter)
	{
		//LevelSelectManager.TeamLetter = teamLetter;

		SceneManager.LoadScene("Scene Level Select");
	}


	public void exit()
	{
		Application.Quit();
	}
}

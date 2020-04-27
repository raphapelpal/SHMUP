///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MiscInputsManager : MonoBehaviour
{
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SceneManager.LoadScene("Scene Level Select");
		}
	}
}

///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponSpawnSystem : MonoBehaviour
{
	[SerializeField]
	private Weapons weapon; //only for information in the inspector

	[SerializeField]
	private WeaponSpawnPoints[] ptsAtLvl = new WeaponSpawnPoints[1];

	public Transform[] getSpawnPoints(int lvl)
	{
		if (lvl >= ptsAtLvl.Length)
			lvl = ptsAtLvl.Length - 1;

		return ptsAtLvl[lvl].points;
	}
}


[System.Serializable]
public class WeaponSpawnPoints
{
	public Transform[] points = new Transform[1];
}

///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SpawnerPathUpdater : MonoBehaviour
{
	private CatmullRom_IT spline;
	Spawner spawner;
	Vector2[] waypointsPos;
	bool isSmoothState;
	int phase2WpIndexValue;


	void OnEnable()
	{
		Init();
	}


	void Init()
	{
		spawner = GetComponent<Spawner>();

		if (spawner == null)
			return;

		phase2WpIndexValue = spawner.GetPhase2WpIndex();
		isSmoothState = spawner.GetIsSmooth();

		UpdateSpawnerPath();
	}


	void Update()
	{
		if (spawner == null)
		{
			Init();
			if (spawner == null)
				throw new System.NullReferenceException("Spawner component doesn't exist!");

			return;
		}

		if (CheckForChanges())
		{
			UpdateSpawnerPath();
		}
	}


	void UpdateSpawnerPath()
	{
		UpdateWaypointsPosList();

		if (spawner.GetIsSmooth() && waypointsPos.Length > 2) //Below 3 points the spline is impossible
		{
			spline = new CatmullRom_IT(waypointsPos);
			spawner.SetPath(spline.splinePoints);
			spawner.UpdatePhase2PathIndex(spline.resolution);
		}
		else
		{
			spawner.SetPath(waypointsPos);
			spawner.UpdatePhase2PathIndex(1);
		}
	}


	void UpdateWaypointsPosList()
	{
		waypointsPos = new Vector2[spawner.GetWaypoints().Length];
		for (int i = 0; i < waypointsPos.Length; i++)
		{
			if (spawner.GetWaypoints()[i] != null)
			{
				waypointsPos[i] = spawner.GetWaypoints()[i].position;
			}
			//Prise en compte du cas o� un des wp aurait �t� destroyed : on garde les coordonn�es stock�es
			else if (waypointsPos[i] == null)
			{
				waypointsPos[i] = this.transform.position;
				throw new System.Exception("Houston... we got a problem: no waypoint at index " + i + " for spawner " + spawner.name);
			}
		}
	}


	bool CheckForChanges()
	{
		if (spawner.GetPhase2WpIndex() != phase2WpIndexValue)
		{
			phase2WpIndexValue = spawner.GetPhase2WpIndex();
			return true;
		}

		if (spawner.GetIsSmooth() != isSmoothState)
		{
			isSmoothState = spawner.GetIsSmooth();
			return true;
		}

		if (spawner.GetWaypoints().Length != waypointsPos.Length)
			return true;

		for (int i = 0; i < spawner.GetWaypoints().Length; i++)
		{
			if (spawner.GetWaypoints()[i] != null && (Vector2)spawner.GetWaypoints()[i].position != waypointsPos[i])
				return true;
		}

		return false;
	}

}

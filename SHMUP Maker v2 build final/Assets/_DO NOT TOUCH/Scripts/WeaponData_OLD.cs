///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;


[System.Serializable]
public class WeaponData_OLD
{
	public Weapons type;
	public WeaponSpawnSystem mySpawnSystem;
	public float[] damageAtLvl = { 5f, 5f, 5f, 6f, 6f };
	public float[] speedAtLvl = { 5f, 5f, 5f, 6f, 6f };
	public float[] fireDelayAtLvl = { 0.15f, 0.15f, 0.12f, 0.12f, 0.1f };
	public Transform[] toSpawnAtLevel = new Transform[1];
	public ShipLaser[] laserToSpawnAtLevel = new ShipLaser[1];


	public float getSpeed(int lvl)
	{
		if (lvl >= speedAtLvl.Length)
			lvl = speedAtLvl.Length - 1;

		return speedAtLvl[lvl];
	}


	public float getDamage(int lvl)
	{
		if (lvl >= damageAtLvl.Length)
			lvl = damageAtLvl.Length - 1;

		return damageAtLvl[lvl];
	}


	public float getFireDelay(int lvl)
	{
		if (lvl >= fireDelayAtLvl.Length)
			lvl = fireDelayAtLvl.Length - 1;

		return fireDelayAtLvl[lvl];
	}


	public Transform getBulletPrefabToSpawn(int lvl)
	{
		if (lvl >= toSpawnAtLevel.Length)
			lvl = toSpawnAtLevel.Length - 1;

		return toSpawnAtLevel[lvl];
	}


	public ShipLaser getLaserPrefabToSpawn(int lvl)
	{
		if (lvl >= laserToSpawnAtLevel.Length)
			lvl = laserToSpawnAtLevel.Length - 1;

		return laserToSpawnAtLevel[lvl];
	}
}


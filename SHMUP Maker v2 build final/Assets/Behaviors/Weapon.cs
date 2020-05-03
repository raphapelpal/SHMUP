///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;


public class Weapon : MonoBehaviour
{
	public string _comment = "Notes...";
	public Weapons type;
	//public bool isResetOnBtnUp = false;
	public WeaponLeveData[] lvls;
}


[System.Serializable]
public class WeaponLeveData
{
	public GunData[] guns;
}


[System.Serializable]
public class GunData
{
	public enum GunTypes
	{
		Bullet = 0,
		Laser = 1
	}

	public Transform cannon;
	public GunTypes type = GunTypes.Bullet;
	public bool mustResetDelayOnBtnUp = false;
	public float initialDelay = 0f;
	public float fireDelay = 0.2f;
	public float fireDuration = 9999f;
	public Bullet bulletPrefab;
	public ShipLaser laserPrefab;

	public float FireStartTime { get; set; }
	public float NextFireTime { get; set; }
}


public enum Weapons
{
	Weapon1 = 0,
	Weapon2 = 1,
	Weapon3 = 2,
	Weapon4 = 3,
	Weapon5 = 4,
	Weapon6 = 6,
	Weapon7 = 7,
	Weapon8 = 8
}


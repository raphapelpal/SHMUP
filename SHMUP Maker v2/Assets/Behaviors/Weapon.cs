///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;


public class Weapon : MonoBehaviour
{
	public string _comment = "Notes...";
	public Weapons type;
	public bool isResetOnBtnUp = false;
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
	public Transform cannon;
	public float initialDelay;
	public float fireDelay;
	public float fireDuration = 9999f;
	public Bullet bulletPrefab;
	public ShipLaser laserPrefab;

	public float FireStartTime { get; set; }
	public float NextFireTime { get; set; }
}


public enum Weapons
{
	BulletA = 0,
	BulletB = 1,
	BulletC = 2,
	BulletD = 3,
	BulletE = 4,
	LaserA = 6,
	LaserB = 7,
	LaserC = 8
}


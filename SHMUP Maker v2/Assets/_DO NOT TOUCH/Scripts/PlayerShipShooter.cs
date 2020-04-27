///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerShipShooter : MonoBehaviour
{
	public Weapon[] weapons;

	static public event Action<GunData> OnStartFiring;
	static public event Action<GunData> OnStopFiring;

	Weapon currWeapon;
	int currLvl;
	GunData[] currGuns;
	ShipStates state;
	bool isFiring;


	// Use this for initialization
	void Awake()
	{
		state = ShipStates.init;
		isFiring = false;

		currWeapon = weapons[0];
		currLvl = 0;
		currGuns = currWeapon.lvls[currLvl].guns;

		ResetFireTimes(false);
	}


	void OnEnable()
	{
		GameManager.OnPlayStart += HandleOnPlayStart;
		GameManager.OnVictory += HandleOnVictory;
		PlayerShipHealth.OnShipDestroyed += HandleOnShipDestroyed;
		Pickup.OnPickup += HandleOnPickup;
	}


	void OnDisable()
	{
		GameManager.OnPlayStart -= HandleOnPlayStart;
		GameManager.OnVictory -= HandleOnVictory;
		PlayerShipHealth.OnShipDestroyed -= HandleOnShipDestroyed;
		Pickup.OnPickup -= HandleOnPickup;
	}


	private void HandleOnPlayStart()
	{
		state = ShipStates.controllable;
	}


	private void HandleOnVictory()
	{
		state = ShipStates.victorious;
		StopFiring();
	}


	private void HandleOnShipDestroyed()
	{
		state = ShipStates.dying;
		StopFiring();
	}


	private void HandleOnPickup(Pickups pkpType)
	{
		int intVal = (int)pkpType;

		//Si le pickup ramassé n'est pas une weapon, on l'ignore.
		if (intVal > (int)Weapons.LaserC)
			return;

		Weapons correspondingWeaponType = (Weapons)intVal;
		Weapon newWeapon = GetWeapon(correspondingWeaponType);

		//If pickup's weapon isn't an available weapon, we exit
		if (newWeapon == null)
			return;

		//Si c'est un pickup de l'arme equipée...
		if (currWeapon.type == correspondingWeaponType)
		{
			//if we can upgrade this weapon
			if (currLvl < currWeapon.lvls.Length - 1)
			{
				StopFiring();
				IncreaseLvl();
				//ResetFireTimes();
			}
		}
		//Si c'est un pickup d'une arme differente...
		else
		{
			StopFiring();
			ChangeWeapon(newWeapon);
			//ResetFireTimes();
		}
	}


	// Update is called once per frame
	void Update()
	{
		//We don't allow control of the ship if we are not in play
		if (state != ShipStates.controllable)
			return;

		//fire when Fire1 is pressed
		if (Input.GetButtonDown("Fire1") || Input.GetButton("Fire1"))
		{
			if (!isFiring)
			{
				if (currWeapon.isResetOnBtnUp)
				{
					ResetFireTimes(false);
				}
				else if ((int)currWeapon.type >= (int)Weapons.LaserA)
				{
					ResetFireTimes(false);
					//FixFireTimesForNonResettableLaser();
				}
				else
				{
					ResetFireTimes(true);
				}

				StartFiring();
			}
		}
		else if (Input.GetButtonUp("Fire1"))
		{
			StopFiring();
		}
	}


	void StartFiring()
	{
		isFiring = true;
		for (int i = 0; i < currGuns.Length; i++)
		{
			OnStartFiring?.Invoke(currGuns[i]);
		}

		StartCoroutine(CoFire());
	}


	IEnumerator CoFire()
	{
		while (isFiring && state == ShipStates.controllable)
		{
			//we scan the last firing times of all current level's current weapon's guns
			for (int i = 0; i < currGuns.Length; i++)
			{
				//if it's not yet time to stop firing...
				if (Time.time <= currGuns[i].FireStartTime + currGuns[i].fireDuration)
				{
					//...we check if it's time to fire a bullet – or to start a laser
					if (Time.time >= currGuns[i].NextFireTime)
					{
						if ((int)currWeapon.type < (int)Weapons.LaserA) //If the weapon is of Bullet type
						{
							//fire a new bullet from this gun and, by default, make it child of the Ship (for those that must remain children of their ships)
							Bullet bullet = Instantiate(currGuns[i].bulletPrefab, currGuns[i].cannon.position,
								currGuns[i].cannon.rotation, transform);

							//update time for this gun
							currGuns[i].NextFireTime = Time.time + currGuns[i].fireDelay;
						}
						else
						{
							//start a new laser from this gun
							ShipLaser laser = Instantiate<ShipLaser>(currGuns[i].laserPrefab, currGuns[i].cannon.position,
								//currGuns[i].cannon.rotation);
								Quaternion.identity);
							laser.Init(currGuns[i].cannon);

							//update next fire time for this gun
							currGuns[i].NextFireTime = Time.time + 9999f; //so that we won't instantiate another laser for this gun during this fire session
						}
					}
				}
				//if it's time to stop firing, check to avoid going through this block more than once per firing session
				else if (currGuns[i].FireStartTime > 0f)
				{
					//negative val for above reasons, and equal to abs. same val so that we can reinit Laser gun correctly when StartFire
					currGuns[i].FireStartTime = -currGuns[i].FireStartTime;
					OnStopFiring?.Invoke(currGuns[i]); //for oscillator & laser

					//currGuns[i].NextFireTime = Time.time + 9999f; //normally useless
				}
			}
			yield return null;
		}
	}


	void StopFiring()
	{
		for (int i = 0; i < currGuns.Length; i++)
		{
			OnStopFiring?.Invoke(currGuns[i]);
		}

		isFiring = false;
	}


	void IncreaseLvl()
	{
		currLvl += 1;
		currGuns = currWeapon.lvls[GetCurrWeaponLvlClamped()].guns;
	}


	private void ChangeWeapon(Weapon newWeapon)
	{
		currWeapon = newWeapon;
		currGuns = currWeapon.lvls[GetCurrWeaponLvlClamped()].guns;
	}


	Weapon GetWeapon(Weapons weaponType)
	{
		for (int i = 0; i < weapons.Length; i++)
		{
			if (weapons[i] != null && weapons[i].type == weaponType)
			{
				return weapons[i];
			}
		}
		//if the weapon type did not match any availabe weapon, we return the currently equipped weapon's index
		return null;
	}


	void ResetFireTimes(bool onlyFireStartTimes)
	{
		for (int i = 0; i < currGuns.Length; i++)
		{
			if (onlyFireStartTimes)
			{
				currGuns[i].FireStartTime = Mathf.Max(Time.time, currGuns[i].NextFireTime + currGuns[i].fireDelay);
			}
			else
			{
				currGuns[i].NextFireTime = currGuns[i].FireStartTime = Time.time + currGuns[i].initialDelay;
			}
		}
	}


	//void FixFireTimesForNonResettableLaser()
	//{
	//	for (int i = 0; i < currGuns.Length; i++)
	//	{
	//		if (currGuns[i].FireStartTime < 0)
	//		{
	//			currGuns[i].NextFireTime = -currGuns[i].FireStartTime + currGuns[i].fireDelay;
	//			currGuns[i].FireStartTime = Time.time;
	//		}
	//	}
	//}


	int GetCurrWeaponLvlClamped()
	{
		return (currWeapon.lvls.Length > currLvl) ? currLvl : currWeapon.lvls.Length - 1;
	}

}

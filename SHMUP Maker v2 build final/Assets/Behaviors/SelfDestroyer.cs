///Copyright 2017-2020 Ian Thecleric; all rights reserved.

//The only thing that this script does, is destroying its GameObject
//To self-destruct an Enemy or a Bullet after a certain time, use their doomTimer property instead.
using UnityEngine;

public class SelfDestroyer : TriggerableObject
{
	void Start()
	{
		Destroy(gameObject);
	}
}

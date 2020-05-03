///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;

/// <summary>
/// Spawn source, may be animated. Must trigger a Spawner that will actually spawn a wave of enemies.
/// </summary>
public class SpawnerProp : TriggeringObject
{
	public EnemyHealth HealthManager { get; private set; }
	private Animator animator;

	//public event System.Action OnOpened;


	void OnEnable()
	{
		HealthManager = GetComponent<EnemyHealth>();
		animator = GetComponent<Animator>();

		foreach (TriggerableObject trigObj in componentsToTrigger)
		{
			if (trigObj is Spawner)
			{
				(trigObj as Spawner).InitFromSpawnProp(this);
			}
		}

		if (animator != null)
		{
			animator.SetTrigger("goOpen");
		}
		else
		{
			OnDoneOpening();
		}
	}


	//Appelée par Unity Animation Event
	private void OnDoneOpening()
	{
		TriggerOthersAndFinish();

		//if (OnOpened != null)
		//	OnOpened();
	}
}

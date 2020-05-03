///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;

public abstract class TriggerableObject : MonoBehaviour
{
	[SerializeField]
	protected string _comment = "Can be added to a COMPONENTS TO TRIGGER list";

	public void TriggerMe()
	{
		enabled = true;
	}
}

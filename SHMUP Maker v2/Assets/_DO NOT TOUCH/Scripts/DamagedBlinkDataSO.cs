///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DamagedBlinkDataSO : ScriptableObject
{
	public int blinksTotal = 1;
	public float phaseDuration = 0.066f;
	public Color colorPhaseStrong = new Color(0.85f, 0f, 0f, 0.8f);
	public Color colorPhaseWeak = new Color(1f, 1f, 1f, 0.9f);
}

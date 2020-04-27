///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEditorUtils
{
	public static Color[] Colors = new Color[] {
		Color.white, //in the editor it's actually the default background color of input fields 
		new Color(1f, 0.1f, 0.1f, 0.6f), //pink
		new Color(1f, 0.5f, 0f, 0.6f), //orange
		new Color(1f, 1f, 0f, 0.6f), //yellow
		new Color(0.5f, 1f, 0f, 0.6f), //lime green
		new Color(0f, 1f, 0.5f, 0.6f), //glass green
		new Color(0f, 0.7f, 1f, 0.6f), //cyan
		new Color(0f, 0.2f, 1f, 0.6f), //blue
		new Color(0.5f, 0f, 1f, 0.6f), //violet
		new Color(1f, 0f, 0.9f, 0.6f), //purple
	};


	static public Color ResetColor()
	{
		return Colors[0];
	}

}


[System.Serializable]
public struct CoupleOfInts
{
	public int uid;
	public int colorIndex;

	public CoupleOfInts(int uniqueId, int colorIdx)
	{
		uid = uniqueId;
		colorIndex = colorIdx;
	}
}

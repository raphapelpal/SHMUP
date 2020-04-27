///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using UnityEditor;
using System.Collections;

public class SnapToGrid : ScriptableObject
{

	//[MenuItem("Edit/Snap to Grid &s")]
	static void MenuSnapToGrid()
	{
		string output = "--> snap";
		Transform[] sel = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
        for (int i = 0; i < sel.Length; i++)
		{
			Transform t = sel[i];
			if (i > 0)
				output += ",";
			output += " " + t.name;
			t.position = new Vector3(
				Mathf.Round(t.position.x / EditorPrefs.GetFloat("MoveSnapX")) * EditorPrefs.GetFloat("MoveSnapX"),
				Mathf.Round(t.position.y / EditorPrefs.GetFloat("MoveSnapY")) * EditorPrefs.GetFloat("MoveSnapY"),
				Mathf.Round(t.position.z / EditorPrefs.GetFloat("MoveSnapZ")) * EditorPrefs.GetFloat("MoveSnapZ")
			);
		}
		if (sel.Length == 0)
			output += "... nothing.";
		else
			output += "!";
		Debug.Log(output);
	}

}
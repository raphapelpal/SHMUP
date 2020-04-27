///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerShipShooter))]
public class PlayerShipShooterInspector : Editor
{
	GUIContent weaponDataNameContent = new GUIContent("Weapons");


	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		DisplayWeapons(serializedObject.FindProperty("weapons"));

		serializedObject.ApplyModifiedProperties();
	}


	private void DisplayWeapons(SerializedProperty weaponsList)
	{
		//Display the number of entries in 'weapons', preceded by a custom title as a label of the property
		EditorGUILayout.PropertyField(weaponsList.FindPropertyRelative("Array.size"), weaponDataNameContent);
		EditorGUI.indentLevel += 1;
		if (weaponsList.isExpanded) //enabling folding
		{
			//Display each Weapon entry
			for (int i = 0; i < weaponsList.arraySize; i++)
			{
				SerializedProperty weaponProp = weaponsList.GetArrayElementAtIndex(i);

				DisplayWeaponField(weaponProp, i);
			}
			EditorGUI.indentLevel -= 1;
		}
	}


	/// <summary>
	/// Displays the Weapon field with the color of the component referenced
	/// </summary>
	void DisplayWeaponField(SerializedProperty weaponProp, int index)
	{
		PlayerShipShooter tgtObject = (PlayerShipShooter)target;
		//Needed or out-of-bound when expanding the size of the array in the inspector
		if (index >= tgtObject.weapons.Length) return;

		int referencedComponentId = weaponProp.objectReferenceInstanceIDValue;
		if (referencedComponentId != 0)
		{
			int cIdx = WeaponInspector.GetComponentData(referencedComponentId).colorIndex;
			GUI.backgroundColor = CustomEditorUtils.Colors[cIdx];
		}

		//Display the header/title of that weapon using the 'type' field value
		string title = tgtObject.weapons[index] != null ? tgtObject.weapons[index].type.ToString() : "No weapon";
		EditorGUILayout.PropertyField(weaponProp, new GUIContent(title));

		GUI.backgroundColor = CustomEditorUtils.ResetColor();
	}

}

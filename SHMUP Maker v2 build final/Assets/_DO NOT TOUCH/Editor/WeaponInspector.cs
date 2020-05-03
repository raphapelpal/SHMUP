///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Weapon))]
public class WeaponInspector : Editor
{
	static int CurrColorIndex = 0;

	static List<CoupleOfInts> ComponentsData;

	readonly GUIContent lvlsNameContent = new GUIContent("Weapon upgrade lvls");
	readonly GUIContent isResetOnBtnUpNameContent = new GUIContent("Reset delay on Btn Up");
	readonly GUIContent numOfLevelsContent = new GUIContent("Number of lvls");
	readonly GUIContent numOfGunsContent = new GUIContent("Guns at this lvl");

	/// <summary>
	/// Returns the instance id and color index of the component which instance id is passed as parameter
	/// Note: returning instance id is useless as it must be passed as parameter, but it is part of CoupleOfInts returned struct
	/// </summary>
	static public CoupleOfInts GetComponentData(int id)
	{
		if (ComponentsData != null)
		{
			for (int j = 0; j < ComponentsData.Count; j++)
			{
				if (id == ComponentsData[j].uid)
				{
					return ComponentsData[j];
				}
			}
		}
		else
		{
			ComponentsData = new List<CoupleOfInts>();
		}

		//if this component's uid wasn't found in ComponentsData or if ComponentsData wasn't existing, we add it along with a new color
		CoupleOfInts data = new CoupleOfInts(id, GetNewColorIndex());
		ComponentsData.Add(data);

		return data;
	}


	static int GetNewColorIndex()
	{
		CurrColorIndex++;
		if (CurrColorIndex >= CustomEditorUtils.Colors.Length)
		{
			CurrColorIndex = 1;
		}

		return CurrColorIndex;
	}


	//Called each time the target gameobject's inspector becomes displayed
	void Awake()
	{
		if (ComponentsData == null)
		{
			ComponentsData = new List<CoupleOfInts>();
		}
		Object obj = serializedObject.targetObject;
		int iid = obj.GetInstanceID();
		GetComponentData(iid);
	}


	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		DisplayColoredComment();

		Weapon tgtObject = (Weapon)target;

		EditorGUILayout.PropertyField(serializedObject.FindProperty("type"));

		//if ((int)tgtObject.type < (int)Weapons.Weapon6) //display only for Bullet weapons
		//{
		//	EditorGUILayout.PropertyField(serializedObject.FindProperty("isResetOnBtnUp"), isResetOnBtnUpNameContent);
		//}

		SerializedProperty lvlListProp = serializedObject.FindProperty("lvls");
		
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(lvlListProp.FindPropertyRelative("Array.size"), numOfLevelsContent);
		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}

		for (int i = 0; i < lvlListProp.arraySize; i++)
		{
			DisplayHorizontalLine();
			
			//Affichage name for one entry of the array 'lvlListProp'; it's an object of type WeaponLevelData
			SerializedProperty lvlProp = lvlListProp.GetArrayElementAtIndex(i);
			GUIContent lvlLabel = new GUIContent("Lvl " + (i + 1) + " of " + tgtObject.type);
			EditorGUILayout.PropertyField(lvlProp, lvlLabel, false);

			EditorGUI.indentLevel += 1;
			if (lvlProp.isExpanded)
			{
				//Affichage array 'guns' of the WeaponLevelData property (sans afficher son nom
				SerializedProperty gunsProp = lvlProp.FindPropertyRelative("guns");
				
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(gunsProp.FindPropertyRelative("Array.size"), numOfGunsContent);
				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
				}

				for (int j = 0; j < gunsProp.arraySize; j++)
				{
					SerializedProperty gProp = gunsProp.GetArrayElementAtIndex(j);
					EditorGUILayout.PropertyField(gProp, new GUIContent("Gun " + (j + 1)), false);

					EditorGUI.indentLevel += 1;
					if (gProp.isExpanded)
					{
						EditorGUILayout.PropertyField(gProp.FindPropertyRelative("cannon"));
						EditorGUILayout.PropertyField(gProp.FindPropertyRelative("type"));
						EditorGUILayout.PropertyField(gProp.FindPropertyRelative("initialDelay"));
						EditorGUILayout.PropertyField(gProp.FindPropertyRelative("fireDuration"));

						//Display bullet or laser prefab field according to gun type
						if (tgtObject.lvls[i].guns[j].type == GunData.GunTypes.Bullet)
						{
							EditorGUILayout.PropertyField(gProp.FindPropertyRelative("fireDelay"));
							EditorGUILayout.PropertyField(gProp.FindPropertyRelative("mustResetDelayOnBtnUp"), isResetOnBtnUpNameContent);
							EditorGUILayout.PropertyField(gProp.FindPropertyRelative("bulletPrefab"));
						}
						else
						{
							EditorGUILayout.PropertyField(gProp.FindPropertyRelative("laserPrefab"));
						}
					}
					EditorGUI.indentLevel -= 1;
				}
			}
			EditorGUI.indentLevel -= 1;
		}

		serializedObject.ApplyModifiedProperties();
	}


	private void DisplayColoredComment()
	{
		SerializedProperty commentProp = serializedObject.FindProperty("_comment");
		GUIStyle commentStyle = new GUIStyle(GUI.skin.textArea)
		{
			fontSize = 10
		};

		GUI.backgroundColor = CustomEditorUtils.Colors[GetComponentData(serializedObject.targetObject.GetInstanceID()).colorIndex];
		EditorGUIUtility.labelWidth = 1;
		commentProp.stringValue = GUILayout.TextArea(commentProp.stringValue, commentStyle);
		EditorGUIUtility.labelWidth = 0;
		GUI.backgroundColor = CustomEditorUtils.ResetColor();
	}


	private void DisplayHorizontalLine()
	{
		Rect lastRect = GUILayoutUtility.GetLastRect();
		Handles.BeginGUI();
		Handles.color = Color.grey;
		Handles.DrawLine(new Vector3(lastRect.xMin, lastRect.yMax + 2), new Vector3(lastRect.xMax, lastRect.yMax + 2));
		Handles.EndGUI();
	}

}

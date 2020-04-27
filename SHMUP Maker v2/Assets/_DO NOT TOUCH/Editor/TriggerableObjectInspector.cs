///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TriggerableObject), true)]
public class TriggerableObjectInspector : Editor
{
	protected static int CurrColorIndex = 0;

	protected static List<CoupleOfInts> ComponentsData;
	protected string[] excludedProps = new string[] { "m_Script", "_comment" };
	private GUIContent commentGUIContent = new GUIContent("Note");

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

		DoOnInspectorGUI();

		serializedObject.ApplyModifiedProperties();
	}


	protected virtual void DoOnInspectorGUI()
	{
		DrawPropertiesExcluding(serializedObject, excludedProps);

		DisplayColoredFrame();
	}


	protected void DisplayColoredComment()
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


	protected void DisplayColoredFrame()
	{
		//affichage rectangle
		//Handles.color = Color.white;
		//Handles.DrawLine(new Vector3(rect.xMin, rect.yMax + 2), new Vector3(rect.xMax, rect.yMax + 2));
		//rect.yMax += 2f;
		Rect lastRect = GUILayoutUtility.GetLastRect();
		Handles.BeginGUI();
		Color col = CustomEditorUtils.Colors[GetComponentData(serializedObject.targetObject.GetInstanceID()).colorIndex];
		Handles.DrawSolidRectangleWithOutline(new Rect(3, -20, lastRect.xMax, lastRect.yMax + 20 + 3), new Color(0, 0, 0, 0), col);
		Handles.EndGUI();
		//EditorGUI.DrawRect(new Rect(0, 0, lastRect.xMax, lastRect.yMax), Color.green);
	}

}

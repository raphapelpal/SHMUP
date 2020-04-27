///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TriggeringObject), true)]
public class TriggeringObjectInspector : TriggerableObjectInspector
{

	GUIContent componentsArraySizeContent = new GUIContent("Count");


	protected override void DoOnInspectorGUI()
	{
		SerializedProperty prop = serializedObject.GetIterator();
		while (prop.NextVisible(true))
		{
			//Debug.Log(prop.type + " " + prop.name + " depth = " + prop.depth); //TEST

			if (prop.name == "_comment")
			{
				//nothing
			}
			else if (prop.depth == 0)
			{
				if (prop.isArray)
				{
					if (prop.name.StartsWith("componentsToT"))
					{
						string str;
						str = System.Text.RegularExpressions.Regex.Replace(prop.name, "([A-Z])", " $1").Trim();
						str = str.ToUpperInvariant();
						DisplayComponentsToTrigger(prop, str);
					}
					else if (this.GetType() != typeof(SpawnerInspector))
					{
						EditorGUILayout.PropertyField(prop, true);
					}
				}
				else if (prop.type == "PPtr<$TriggerableObject>" && this.GetType() != typeof(SpawnerInspector))
				{
					DisplayOneComponentField(prop);
				}
				else if (prop.name == "dontAutoDisableComponents")
				{
					EditorGUIUtility.labelWidth = 230;
					EditorGUILayout.PropertyField(prop, new GUIContent("Don't disable components at initialization"));
					EditorGUIUtility.labelWidth = 0;
					//ajout espace
					//EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
					EditorGUILayout.Space();
				}
				else if (prop.name != "m_Script" && this.GetType() != typeof(SpawnerInspector))
				{
					EditorGUILayout.PropertyField(prop, true);
				}
			}
		}

		if (this.GetType() != typeof(SpawnerInspector))
			DisplayColoredFrame();
	}


	/// <summary>
	/// Displays componentsToTrigger array
	/// </summary>
	protected void DisplayComponentsToTrigger(SerializedProperty componentsProp, string label)
	{
		EditorGUILayout.PropertyField(componentsProp, new GUIContent(label), false);
		if (componentsProp.isExpanded)
		{
			EditorGUI.indentLevel += 1;
			EditorGUILayout.PropertyField(componentsProp.FindPropertyRelative("Array.size"), componentsArraySizeContent);
			for (int i = 0; i < componentsProp.arraySize; i++)
			{
				DisplayOneComponentField(componentsProp.GetArrayElementAtIndex(i), i);
			}
			EditorGUI.indentLevel -= 1;
		}
	}


	/// <summary>
	/// Displays each field of componentsToTrigger array with the color of the component referenced
	/// </summary>
	protected void DisplayOneComponentField(SerializedProperty compFieldProp, int index = -1)
	{
		int referencedComponentId = compFieldProp.objectReferenceInstanceIDValue;
		if (referencedComponentId != 0)
		{
			int cIdx = TriggerableObjectInspector.GetComponentData(referencedComponentId).colorIndex;
			GUI.backgroundColor = CustomEditorUtils.Colors[cIdx];
		}

		if (index > -1)
		{
			string str = "#" + index.ToString();
			EditorGUIUtility.labelWidth = 35;
			EditorGUILayout.PropertyField(compFieldProp, new GUIContent(str));
			EditorGUIUtility.labelWidth = 0;
		}
		else
		{
			EditorGUILayout.PropertyField(compFieldProp);
		}

		GUI.backgroundColor = CustomEditorUtils.ResetColor();
	}

}

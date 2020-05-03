///Copyright 2017-2020 Ian Thecleric; all rights reserved.


using UnityEngine;
using UnityEditor;
using System.Reflection;

//[CustomEditor(typeof(Spawner))]
public class SpawnerInspector : TriggeringObjectInspector
{
	GUIContent pathNameContent = new GUIContent("Waypoints");
	GUIContent enemyPrefabContent = new GUIContent("Enemy Prefab");
	GUIContent pickupContent = new GUIContent("Pickup on wave killed", "Optional: Pickup prefab to instantiate if the whole wave has been killed");
	GUIContent spawnCountContent = new GUIContent("Spawn count");
	GUIContent spawnDelayContent = new GUIContent("Spawn delay");
	GUIContent phase2WpIdxContent = new GUIContent("Wpt index Phase 2");
	GUIContent colorPathContent = new GUIContent("Path default");
	GUIContent colorPhase2Content = new GUIContent("Path phase 2");

	bool isFoldout = false;


	protected override void DoOnInspectorGUI()
	{
		base.DoOnInspectorGUI(); //IMPORTANT NOTE: TriggeringObjectInspector prend en compte le cas particulier de SpawnerInspector

		EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyToSpawnRef"), enemyPrefabContent);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnCount"), spawnCountContent);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnDelay"), spawnDelayContent);

		EditorGUILayout.Space();

		//Display the number of entries in 'wayPoints', preceded by a custom title as a label of the property
		SerializedProperty pathProp = serializedObject.FindProperty("wayPoints");
		EditorGUILayout.PropertyField(pathProp.FindPropertyRelative("Array.size"), pathNameContent);

		EditorGUI.indentLevel += 1;
		for (int i = 0; i < pathProp.arraySize; i++)
		{
			string str = i == 0 ? "Spawn point" : "Waypoint " + i;
			EditorGUILayout.PropertyField(pathProp.GetArrayElementAtIndex(i), new GUIContent(str));
		}
		EditorGUI.indentLevel -= 1;

		EditorGUILayout.Space();

		EditorGUILayout.PropertyField(serializedObject.FindProperty("phase2WpIndex"), phase2WpIdxContent);

		EditorGUILayout.Space();

		EditorGUILayout.PropertyField(serializedObject.FindProperty("pickupRef"), pickupContent);

		EditorGUILayout.Space();

		isFoldout = EditorGUILayout.Foldout(isFoldout, "Colors");
		if (isFoldout)
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("colorPath"), colorPathContent);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("colorPathPhase2"), colorPhase2Content);
		}

		serializedObject.ApplyModifiedProperties();

		DisplayColoredFrame();
	}

}


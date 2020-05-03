///Copyright 2017-2020 Ian Thecleric; all rights reserved.

using UnityEngine;

[ExecuteInEditMode]
public class CamHelperInEdtor : MonoBehaviour
{
	[SerializeField] Color color = Color.cyan;
	[SerializeField] float length = 1000f;

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		float halfY = Camera.main.orthographicSize;

		Gizmos.color = color;
		Gizmos.DrawRay(new Vector3(0f, transform.position.y + halfY), new Vector3(length, 0f));
		Gizmos.DrawRay(new Vector3(0f, transform.position.y - halfY), new Vector3(length, 0f));
	}
#endif
}

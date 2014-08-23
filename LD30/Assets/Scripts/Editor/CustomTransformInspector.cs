using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
//[CustomEditor(typeof(Transform), true)]
public class CustomTransformInspector : Editor
{
	static public CustomTransformInspector instance;

	SerializedProperty m_pos;
	SerializedProperty m_rot;
	SerializedProperty m_scale;

	void OnEnable ()
	{
		instance = this;

		m_pos = serializedObject.FindProperty("m_LocalPosition");
		m_rot = serializedObject.FindProperty("m_LocalRotation");
		m_scale = serializedObject.FindProperty("m_LocalScale");
	}

	void OnDestroy () { instance = null; }

	public override void OnInspectorGUI ()
	{
		serializedObject.Update();
		DrawDefaultInspector();
		DrawResetButtons();
	}

	void DrawResetButtons ()
	{
		GUILayout.BeginVertical();
		{
			bool resetPos = GUILayout.Button("Reset Postion");
			if (resetPos) m_pos.vector3Value = Vector3.zero;

			bool resetScale = GUILayout.Button("Reset Scale");
			if (resetScale) m_scale.vector3Value = Vector3.one;

			bool resetRot = GUILayout.Button("Reset Rotation");
			if (resetRot) m_rot.quaternionValue = Quaternion.identity;
		}
		GUILayout.EndVertical();
	}
}

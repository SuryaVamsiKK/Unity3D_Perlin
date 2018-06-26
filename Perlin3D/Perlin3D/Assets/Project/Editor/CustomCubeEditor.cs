using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Visualizer))]
public class CustomCubeEditor : Editor {

	Visualizer db;
	bool custom = true;

	void OnEnable()
	{
		db = (Visualizer)target;
	}

	public override void OnInspectorGUI()
	{
		custom = GUILayout.Toggle(custom, "Custom Mode");

		GUILayout.Space(5);

		if (custom == true)
		{
			if (GUILayout.Button("Reset"))
			{
				resetCube();
			}

			GUILayout.Space(10);

			for (int i = 0; i < db.v.Length; i++)
			{
				GUILayout.BeginHorizontal();
				db.v[i].vert = GUILayout.Toggle(db.v[i].vert, " : V " + i);
				GUILayout.Label("ID : " + db.v[i].ID);
				GUILayout.EndHorizontal();
			}

			GUILayout.Space(10);

			GUILayout.BeginHorizontal();
			db.debugCube = GUILayout.Toggle(db.debugCube, "Debug Cube");
			db.debugMesh = GUILayout.Toggle(db.debugMesh, "Debug Mesh");
			db.test = GUILayout.Toggle(db.test, "Testing Type");
			db.edgeLog = GUILayout.Toggle(db.edgeLog, "Log Edges");
			GUILayout.EndHorizontal();
		}
		if (custom == false)
		{
			base.OnInspectorGUI();
		}
	}

	void resetCube()
	{
		for (int i = 0; i < db.v.Length; i++)
		{
			db.v[i].vert = false;
		}
	}

}

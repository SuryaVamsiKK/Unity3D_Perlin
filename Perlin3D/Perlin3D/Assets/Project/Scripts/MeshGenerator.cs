using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

	Mesh mesh;

	[HideInInspector]
	public Vector3[] vertices;
	int[] triangles;

	private void Awake()
	{
		mesh = GetComponent<MeshFilter>().mesh;
	}

	private void Start()
	{
		CreateMesh();
	}

	public void CreateMesh()
	{
		triangles = new int[vertices.Length];
		for (int i = 0; i < vertices.Length; i++)
		{
			triangles[i] = i;
		}
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
	}
}

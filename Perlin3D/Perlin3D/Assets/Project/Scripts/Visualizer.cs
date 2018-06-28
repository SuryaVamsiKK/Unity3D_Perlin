using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour {

	bool[] e = new bool[12];
	public bool[] v = new bool[8];

	List<int> drawable = new List<int>();
	public bool debugCube = false;
	public bool debugMesh = false;
	public bool edgeLog = false;
	public float xOffset;
	public float yOffset;
	public float zOffset;

	[HideInInspector]
	public GameObject[] vertices;
	[HideInInspector]
	public List<GameObject[]> totalVerts;
	[HideInInspector]
	public GameObject[] edgeNodes;
	[HideInInspector]
	public List<GameObject[]> totalEdgs = new List<GameObject[]>();
	int xs, ys, zs;


	private void OnDrawGizmos()
	{
		if (this.transform.childCount >= 2)
		{
			#region Debugging

			GameObjectsAsVerts();

			if (debugCube == true)
			{
				#region Setting the Vert Positions
				transform.GetChild(0).GetChild(0).position = new Vector3(0, 0, 0);
				transform.GetChild(0).GetChild(1).position = new Vector3(xOffset, 0, 0);
				transform.GetChild(0).GetChild(2).position = new Vector3(xOffset, 0, zOffset);
				transform.GetChild(0).GetChild(3).position = new Vector3(0, 0, zOffset);
				transform.GetChild(0).GetChild(4).position = new Vector3(0, yOffset, 0);
				transform.GetChild(0).GetChild(5).position = new Vector3(xOffset, yOffset, 0);
				transform.GetChild(0).GetChild(6).position = new Vector3(xOffset, yOffset, zOffset);
				transform.GetChild(0).GetChild(7).position = new Vector3(0, yOffset, zOffset);
				#endregion

				#region Setting the Edge Median Positions
				for (int i = 0; i < edgeNodes.Length; i++)
				{
					edgeNodes[i].transform.position = Tables.VertexInterpolate(vertices[Tables.JoiningVerts[i, 0]].transform.position, vertices[Tables.JoiningVerts[i, 1]].transform.position);
				}
				#endregion

				#region Debugging Edges
				for (int i = 0; i < 12; i++)
				{
					Debugger.DebugLine(
						transform.GetChild(0).GetChild(Tables.JoiningVerts[i, 0]).position, 
						transform.GetChild(0).GetChild(Tables.JoiningVerts[i, 1]).position, 
						this.gameObject.GetComponent<ColorCode>().linecolor);
				}
				#endregion

				#region Debugging Edge Points
				for (int i = 0; i < edgeNodes.Length; i++)
				{
					Debugger.DebugPoint(
						edgeNodes[i].transform.position, 
						this.gameObject.GetComponent<ColorCode>().edgeSize, e[i], 
						this.gameObject.GetComponent<ColorCode>().edgeOffColor, 
						this.gameObject.GetComponent<ColorCode>().edgeOnColor);
				}
				#endregion

				#region Debugging Vertices
				for (int i = 0; i < vertices.Length; i++)
				{
					Debugger.DebugPoint(
						vertices[i].transform.position,
						this.gameObject.GetComponent<ColorCode>().vertSize, v[i],
						this.gameObject.GetComponent<ColorCode>().vertOffColor,
						this.gameObject.GetComponent<ColorCode>().vertOnColor);
				}
				#endregion
			}

			if (debugMesh == true)
			{
				Evaluation();
			}

			#endregion
		}
	}

	public void Evaluation()
	{
		int cubeindex = 0;
		float isoLevel = 1f;

		#region  Debugging the enabled Vertices from the choosne edges.
		for (int i = 0; i < 8; i++)
		{
			Evaluate_Boolean(i, Tables.edgeNeighbours[i, 0], Tables.edgeNeighbours[i, 1], Tables.edgeNeighbours[i, 2]);
		}
		#endregion

		#region permutational Calculations.

		for (int i = 0; i < 8; i++)
		{
			if ((v[i] ? 1 : 0) < isoLevel) cubeindex |= (int)Mathf.Pow(2, i);
		}

		int[] vertlist = new int[12];

		for (int i = 0; i < 12; i++)
		{
			if (Tables.isVertEnabled(Tables.edgeTable[cubeindex], (int)Mathf.Pow(2, i)))
				vertlist[i] = i;
		}
		#endregion

		#region  Drawing the triangles based on the choosne vertices and Debugging the vertice names.
		for (int i = 0; Tables.triTable[cubeindex, i] != -1; i += 3)
		{
			if (edgeLog == true)
			{
				Debug.Log(vertlist[Tables.triTable[cubeindex, i]] + " : " + vertlist[Tables.triTable[cubeindex, i + 1]] + " : " + vertlist[Tables.triTable[cubeindex, i + 2]]);
			}

			Debugger.DebugTri(
				edgeNodes[vertlist[Tables.triTable[cubeindex, i]]].transform.position, 
				edgeNodes[vertlist[Tables.triTable[cubeindex, i + 1]]].transform.position, 
				edgeNodes[vertlist[Tables.triTable[cubeindex, i + 2]]].transform.position, 
				this.gameObject.GetComponent<ColorCode>().MeshColor);
		}
		#endregion
	}

	void Evaluate_Boolean(int vert, int e1, int e2, int e3)
	{
		#region Choosing the vert to be on or off by first checking its current status
		if (v[vert] == true)
		{
			// Firstly making all the linked edges true.
			e[e1] = true;
			e[e2] = true;
			e[e3] = true;

			#region  Checking what neighbours are ON and disabling the edges based on the Neighbours that are ON.
			for (int i = 0; i < 8; i++)
			{
				if (vert == i)
				{
					if (v[Tables.vertNeighbours[i, 0]] == true)               // if the neighbour is true then siwtch the link betwwen them off
					{
						e[Tables.edgeNeighbours[i, 0]] = false;
					}
					if (v[Tables.vertNeighbours[i, 1]] == true)
					{
						e[Tables.edgeNeighbours[i, 1]] = false;
					}
					if (v[Tables.vertNeighbours[i, 2]] == true)
					{
						e[Tables.edgeNeighbours[i, 2]] = false;
					}
				}
			}
			#endregion

			//checking the neighbours based on the integer table above just to reduce the some many if statments.
		}

		if (v[vert] == false)
		{
			// Firstly making all the linked edges false.
			e[e1] = false;
			e[e2] = false;
			e[e3] = false;

			#region Checking what neighbours are ON and leaving the linked edges that connect to the neighbours ON.
			for (int i = 0; i < 8; i++)
			{
				if (vert == i)
				{
					if (v[Tables.vertNeighbours[i, 0]] == true)        // if the neighbour is true then siwtch the link betwwen them back on
					{
						e[Tables.edgeNeighbours[i, 0]] = true;
					}
					if (v[Tables.vertNeighbours[i, 1]] == true)
					{
						e[Tables.edgeNeighbours[i, 1]] = true;
					}
					if (v[Tables.vertNeighbours[i, 2]] == true)
					{
						e[Tables.edgeNeighbours[i, 2]] = true;
					}
				}
			}
			#endregion

			//again checking the neighbours based on the integer table above just to reduce the some many if statments.
		}
		#endregion
	}

	void GameObjectsAsVerts()
	{
		edgeNodes = new GameObject[transform.GetChild(1).childCount];
		vertices = new GameObject[transform.GetChild(0).childCount];
		for (int i = 0; i < transform.GetChild(1).childCount; i++)
		{
			edgeNodes[i] = transform.GetChild(1).GetChild(i).gameObject;
		}

		for (int i = 0; i < transform.GetChild(0).childCount; i++)
		{
			vertices[i] = transform.GetChild(0).GetChild(i).gameObject;
		}
	}

}

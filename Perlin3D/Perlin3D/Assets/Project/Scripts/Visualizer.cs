using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour {



	public Color vertOffColor;
	public Color vertOnColor;
	public Color edgeOffColor;
	public Color edgeOnColor;
	public Color linecolor;
	public float vertSize;
	public float edgeSize;
	bool[] e;
	public bool[] v;

	//                 v7_______e6_____________v6
	//                  /|                    /|
	//                 / |                   / |
	//              e7/  |                e5/  |
	//               /___|______e4_________/   |
	//            v4|    |                 |v5 |e10
	//              |    |                 |   |
	//              |    |e11              |e9 |
	//            e8|    |                 |   |
	//              |    |_________________|___|
	//              |   / v3      e2       |   /v2
	//              |  /                   |  /
	//              | /e3                  | /e1
	//              |/_____________________|/
	//              v0         e0          v1


	//				All the neightbouring edges
	//				v0  -----  e0		e3		e8
	//				v1  -----  e0		e1		e9
	//				v2  -----  e1		e10		e2
	//				v3  -----  e2		e3		e11
	//				v4  -----  e8		e7		e4
	//				v5  -----  e4		e5		e9
	//				v6  -----  e5		e6		e10
	//				v7  -----  e6		e7		e11

	//				All the neightbouring Verts
	//				v0  -----  v1		v3		v4
	//				v1  -----  v0		v2		v5
	//				v2  -----  v1		v6		v3
	//				v3  -----  v2		v0		v7
	//				v4  -----  v0		v7		v5
	//				v5  -----  v4		v6		v1
	//				v6  -----  v5		v7		v2
	//				v7  -----  v6		v4		v3


	#region Neighbour Tables

	int[,] edgeNeighbours =
		{ { 0, 3, 8},
		  { 0, 1, 9},
		  { 1, 10, 2},
		  { 2, 3, 11},
		  { 8, 7, 4},
		  { 4, 5, 9},
		  { 5, 6, 10},
		  { 6, 7, 11},
		};

	int[,] vertNeighbours =
		{ { 1, 3, 4},
		  { 0, 2, 5},
		  { 1, 6, 3},
		  { 2, 0, 7},
		  { 0, 7, 5},
		  { 4, 6, 1},
		  { 5, 7, 2},
		  { 6, 4, 3},
		};

	#endregion

	void Start () {
		
	}
	
	void Update () {
		
	}

	private void OnValidate()
	{
		
	}

	private void OnDrawGizmos()
	{
		Evaluation();
		if (this.transform.childCount >= 2)
		{
			#region Verts and Mids

			for (int i = 0; i < transform.GetChild(0).childCount; i++)
			{
				if (v[i] == true)
				{
					Gizmos.color = vertOnColor;
				}
				if (v[i] == false)
				{
					Gizmos.color = vertOffColor;
				}

				Gizmos.DrawSphere(transform.GetChild(0).GetChild(i).position, vertSize);
			}

			for (int i = 0; i < transform.GetChild(1).childCount; i++)
			{
				if (e[i] == true)
				{
					Gizmos.color = edgeOnColor;
				}
				if (e[i] == false)
				{
					Gizmos.color = edgeOffColor;
				}
				Gizmos.DrawSphere(transform.GetChild(1).GetChild(i).position, edgeSize);
			}

			#endregion

			#region Edges
			drawline(transform.GetChild(0).GetChild(0), transform.GetChild(0).GetChild(1));
			drawline(transform.GetChild(0).GetChild(0), transform.GetChild(0).GetChild(4));
			drawline(transform.GetChild(0).GetChild(0), transform.GetChild(0).GetChild(3));
			drawline(transform.GetChild(0).GetChild(1), transform.GetChild(0).GetChild(2));
			drawline(transform.GetChild(0).GetChild(1), transform.GetChild(0).GetChild(5));
			drawline(transform.GetChild(0).GetChild(2), transform.GetChild(0).GetChild(6));
			drawline(transform.GetChild(0).GetChild(2), transform.GetChild(0).GetChild(3));
			drawline(transform.GetChild(0).GetChild(3), transform.GetChild(0).GetChild(7));
			drawline(transform.GetChild(0).GetChild(4), transform.GetChild(0).GetChild(5));
			drawline(transform.GetChild(0).GetChild(4), transform.GetChild(0).GetChild(7));
			drawline(transform.GetChild(0).GetChild(6), transform.GetChild(0).GetChild(7));
			drawline(transform.GetChild(0).GetChild(6), transform.GetChild(0).GetChild(5));
			#endregion
		}
	}

	public void drawline(Transform p1, Transform p2)
	{
		Gizmos.color = linecolor;
		Gizmos.DrawLine(p1.transform.position, p2.transform.position);
	}

	void Evaluation()
	{
		for (int i = 0; i < 8; i++)
		{
			Evaluate_Boolean(i, edgeNeighbours[i, 0], edgeNeighbours[i, 1], edgeNeighbours[i, 2]);
		}
	}

	void Evaluate_Boolean(int vert, int e1, int e2, int e3)
	{
		if (v[vert] == true)
		{
			// Firstly making all the linked edges true.
			e[e1] = true;
			e[e2] = true;
			e[e3] = true;

			//Checking what neighbours are ON and disabling the edges based on the Neighbours that are ON.
			for (int i = 0; i < 8; i++)
			{
				if (vert == i)
				{
					if (v[vertNeighbours[i, 0]] == true)				// if the neighbour is true then siwtch the link betwwen them off
					{
						e[edgeNeighbours[i, 0]] = false;
					}
					if (v[vertNeighbours[i, 1]] == true)
					{
						e[edgeNeighbours[i, 1]] = false;
					}
					if (v[vertNeighbours[i, 2]] == true)
					{
						e[edgeNeighbours[i, 2]] = false;
					}
				}
			}
			//checking the neighbours based on the integer table above just to reduce the some many if statments.
		}

		if (v[vert] == false)
		{
			// Firstly making all the linked edges false.
			e[e1] = false;
			e[e2] = false;
			e[e3] = false;

			//Checking what neighbours are ON and leaving the linked edges that connect to the neighbours ON.
			for (int i = 0; i < 8; i++)
			{
				if (vert == i)
				{
					if (v[vertNeighbours[i, 0]] == true)        // if the neighbour is true then siwtch the link betwwen them back on
					{
						e[edgeNeighbours[i, 0]] = true;
					}
					if (v[vertNeighbours[i, 1]] == true)
					{
						e[edgeNeighbours[i, 1]] = true;
					}
					if (v[vertNeighbours[i, 2]] == true)
					{
						e[edgeNeighbours[i, 2]] = true;
					}
				}
			}
			//again checking the neighbours based on the integer table above just to reduce the some many if statments.
		}
	}
}

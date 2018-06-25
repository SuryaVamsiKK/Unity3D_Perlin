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
	public bool[] verts;
	public bool[] e;
	public bool[] v;

	int[,] combs =
		{ { 0, 3, 8},
		  { 0, 1, 9},
		  { 1, 10, 2},
		  { 2, 3, 11},
		  { 8, 7, 4},
		  { 4, 5, 9},
		  { 5, 6, 10},
		  { 6, 7, 11},
		};

	int[,] neighbours =
		{ { 1, 0, 3, 3, 4, 8},
		  { 0, 0, 2, 1, 5, 9},
		  { 1, 1, 6, 10, 3, 2},
		  { 0, 3, 2, 2, 7, 11},
		  { 0, 8, 5, 4, 7, 7},
		  { 1, 9, 6, 5, 4, 4},
		  { 5, 5, 7, 6, 2, 10},
		  { 6, 7, 3, 11, 6, 6},
		};

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnValidate()
	{
		
	}

	private void OnDrawGizmos()
	{
		combinations();
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

	void combinations()
	{

		//v0  -----  e0		e3		e8
		//v1  -----  e0		e1		e9
		//v2  -----  e1		e10		e2
		//v3  -----  e2		e3		e11
		//v4  -----  e8		e7		e4
		//v5  -----  e4		e5		e9
		//v6  -----  e5		e6		e10
		//v7  -----  e6		e7		e11

		//enabler(0, 0, 3, 8);
		//enabler(1, 0, 1, 9);
		//enabler(2, 1, 10, 2);
		//enabler(3, 2, 3, 11);
		//enabler(4, 8, 7, 4);
		//enabler(5, 4, 5, 9);
		//enabler(6, 5, 6, 10);
		//enabler(7, 6, 7, 11);
		#region trial one
		for (int i = 0; i < 8; i++)
		{
			if (verts[i] == true)
			{
				v[i] = !v[i];
				enabler(i, combs[i, 0], combs[i, 1], combs[i, 2]);
				verts[i] = false;
			}
		}
		#endregion
	}

	void enabler(int vert, int e1, int e2, int e3)
	{
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

		if (v[vert] == true)
		{
			e[e1] = !e[e1];
			e[e2] = !e[e2];
			e[e3] = !e[e3];
		}
		if (v[vert] == false)
		{
			e[e1] = false;
			e[e2] = false;
			e[e3] = false;

			for (int i = 0; i < 8; i++)
			{
				if (vert == i)
				{
					if (v[neighbours[i, 0]] == true)
					{
						e[neighbours[i, 1]] = true;
					}
					if (v[neighbours[i, 2]] == true)
					{
						e[neighbours[i, 3]] = true;
					}
					if (v[neighbours[i, 4]] == true)
					{
						e[neighbours[i, 5]] = true;
					}
				}
			}
		}
	}
}

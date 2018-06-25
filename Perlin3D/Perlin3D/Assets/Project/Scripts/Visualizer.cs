using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour {


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


	public Color vertColor;
	public Color edgeColor;
	public Color linecolor;
	public float vertSize;
	public float edgeSize;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnDrawGizmos()
	{

		Gizmos.color = vertColor;
		for (int i = 0; i < transform.GetChild(0).childCount; i++)
		{
			Gizmos.DrawSphere(transform.GetChild(0).GetChild(i).position, vertSize);
		}

		Gizmos.color = edgeColor;
		for (int i = 0; i < transform.GetChild(1).childCount; i++)
		{
			Gizmos.DrawSphere(transform.GetChild(1).GetChild(i).position, edgeSize);
		}

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
	}

	public void drawline(Transform p1, Transform p2)
	{
		Gizmos.color = linecolor;
		Gizmos.DrawLine(p1.transform.position, p2.transform.position);
	}
}

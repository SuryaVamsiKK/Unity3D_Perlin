using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marching_cubes : MonoBehaviour {

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

	public bool[] v;
	public bool[] e;

	void Start () {
		
	}
	
	void Update () {
		
	}

	void MarchingCube()
	{
		for (int i = 0; i < this.transform.childCount; i++)
		{
			v[i] = this.transform.GetChild(i).GetComponent<CubeButtons>().button;
		}

	}

	void onepoint(int vert, int e1,int e2, int e3)
	{
		if (v[vert] == true)
		{
			for (int i = 0; i < e.Length; i++)
			{
				e[i] = false;
			}
			e[e1] = true;
			e[e2] = true;
			e[e3] = true;
		}
	}

	void twopoint_typeA(int vert1, int vert2, int e1, int e2, int e3, int e4)
	{

	}

	void twopoint_typeB(int vert1, int vert2, int e1, int e2, int e3, int e4)
	{

	}

	void twopoint_typeC(int vert1, int vert2, int e1, int e2, int e3, int e4, int e5, int e6)
	{

	}
}

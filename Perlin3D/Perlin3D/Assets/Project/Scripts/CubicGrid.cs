using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubicGrid : MonoBehaviour {

	[HideInInspector]
	public GameObject[,,] grid;

	public int xSize = 3, ySize = 3, zSize = 3;
	public GameObject emt;
	public GameObject emt1;
	bool destroyed = false;
	// Use this for initialization
	void Start ()
	{
		drawstart();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDrawGizmos()
	{

	}

	void OnValidate()
	{

	}

	void drawstart()
	{
		for (int i =0;i < this.transform.GetChild(2).childCount; i++)
		{
			DestroyImmediate(this.transform.GetChild(2).GetChild(i).gameObject);
		}

		grid = new GameObject[xSize, ySize, zSize];

		GameObject vs = Instantiate(emt1, this.transform.GetChild(2)) as GameObject;
		GameObject es = Instantiate(emt1, this.transform.GetChild(2)) as GameObject;

		for (int x = 0; x < xSize; x++)
		{
			for (int y = 0; y < ySize; y++)
			{
				for (int z = 0; z < zSize; z++)
				{
					GameObject obj = Instantiate(emt, this.transform.GetChild(2).GetChild(0)) as GameObject;
					obj.transform.position = new Vector3(x, y, z);
					grid[x, y, z] = obj;
				}
			}
		}
	}
}

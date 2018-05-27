using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gen : MonoBehaviour {

	public int size;
	[SerializeField]
	private GameObject blocks;
	private GameObject[,,] grids;

	// Use this for initialization
	void Start () {

		grids = new GameObject[size, size, size];

		for (int x = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
			{
				for (int z = 0; z < size; z++)
				{
					GameObject cell = Instantiate(blocks, new Vector3(x, y, z), Quaternion.identity, this.transform) as GameObject;
					grids[x, y, z] = cell;
				}
			}
		}

		for (int x = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
			{
				for (int z = 0; z < size; z++)
				{
					if (x == 0 || y == 0 || z == 0 || x == (size - 1) || y == (size - 1) || z == (size - 1))
					{
						goto ot;
					}

					grids[x, y, z].GetComponent<checker>().neighbours.Add(grids[x + 1, y, z]);
					grids[x, y, z].GetComponent<checker>().neighbours.Add(grids[x - 1, y, z]);

					grids[x, y, z].GetComponent<checker>().neighbours.Add(grids[x, y + 1, z]);
					grids[x, y, z].GetComponent<checker>().neighbours.Add(grids[x, y - 1, z]);

					grids[x, y, z].GetComponent<checker>().neighbours.Add(grids[x, y, z + 1]);
					grids[x, y, z].GetComponent<checker>().neighbours.Add(grids[x, y, z - 1]);
					ot:;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

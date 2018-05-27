using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spwancheker : MonoBehaviour {

	public List<GameObject> neighbours;
	int i = 0;
	public bool check = false;

	// Use this for initialization
	void Start () {

		foreach (GameObject g in neighbours)
		{
			if (g.activeInHierarchy)
			{
				i++;
			}
		}

		if (i == 6)
		{
			this.gameObject.GetComponent<MeshRenderer>().enabled = false;
		}
	}

	// Update is called once per frame
	void Update () {

		

	}
}

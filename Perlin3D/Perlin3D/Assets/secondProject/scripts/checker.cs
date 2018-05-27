using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checker : MonoBehaviour {

	public List<GameObject> neighbours;
	private bool check = false;
	private bool neithernull = false;
	int i = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		foreach (GameObject g in neighbours)
		{
			i++;
		}

		if (i >= 6)
		{
			this.gameObject.SetActive(false);
		}
	}
}

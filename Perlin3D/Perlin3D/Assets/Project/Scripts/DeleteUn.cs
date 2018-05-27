using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteUn : MonoBehaviour {

	bool ready = false;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update () {

		if (ready == false)
		{
			LateStart();
			ready = true;
		}

		/*
			if (this.gameObject.GetComponent<MeshRenderer>().enabled == false)
			{
				this.gameObject.GetComponent<MeshRenderer>().enabled = true;
				this.gameObject.SetActive(false);
			}
		 */
	}

	void LateStart()
	{
		for (int i = 0; i < this.transform.childCount; i++)
		{
			if (!this.transform.GetChild(i).gameObject.activeInHierarchy || this.transform.GetChild(i).GetComponent<MeshRenderer>().enabled == false)
			{
				Destroy(this.transform.GetChild(i).gameObject);
			}
		}
	}
}

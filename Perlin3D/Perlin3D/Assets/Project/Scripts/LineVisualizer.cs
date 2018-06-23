using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineVisualizer : MonoBehaviour {

	public Color linesColor;
	public Color vertsColor;
	public float radius;
	public Transform[] verts;

	void Start () {
		
	}
	
	void Update () {

	}

	private void OnDrawGizmos()
	{
		Gizmos.color = vertsColor;
		Gizmos.DrawWireSphere(this.transform.position, radius);
		for (int i = 0; i < verts.Length; i++)
		{ 
			Gizmos.color = linesColor;
			Gizmos.DrawLine(this.transform.position, verts[i].transform.position);
		}
	}
}

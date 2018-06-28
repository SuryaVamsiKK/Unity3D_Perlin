using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirdWork : MonoBehaviour {

	public bool GridDebug = false;
	public bool MeshDebug = true;
	public bool RenderMesh = true;
	public bool sphere = true;
	public bool flipNormals = true;
	public float radius = 20f;
	public Material mat;

	public Vector3 noisePosition;
	public float noiseScale = 0.9f;
	public float readableValue = 0.5f;

	public int xSize, ySize, zSize;
	[Header("")]
	public float xOffset;
	public float yOffset;
	public float zOffset;


	public Vertx[] v = new Vertx[8];
	[HideInInspector]
	public List<Vector3> FullVertList;
	bool meshgen = false;

	void OnDrawGizmos()
	{
		GizmoDraw();
	}

	private void Awake()
	{
		GameObject terrian = new GameObject();
		terrian.name = "Terrian";
		terrian.transform.parent = this.transform;

		terrian.AddComponent<MeshFilter>();
		terrian.AddComponent<MeshRenderer>();
		terrian.AddComponent<MeshGenerator>();
		terrian.GetComponent<MeshRenderer>().sharedMaterial = mat;

		meshgen = true;
		Draw();
		transform.GetChild(0).GetComponent<MeshGenerator>().vertices = FullVertList.ToArray();
	}

	public void Draw()
	{
		FullVertList = new List<Vector3>();

		if (noiseScale < 0)
		{
			noiseScale *= -1;
		}

		Vertx[,,] verts = new Vertx[xSize, ySize, zSize];

		#region  Vert and edge assignment 
		for (int x = 0; x < xSize; x++)
		{
			for (int y = 0; y < ySize; y++)
			{
				for (int z = 0; z < zSize; z++)
				{
					verts[x, y, z] = new Vertx();
					verts[x, y, z].pos = new Vector3(x * xOffset, y * yOffset, z * zOffset) + (transform.position - new Vector3(xOffset*xSize/2, yOffset*ySize/2, zOffset*zSize/2));
				}
			}
		}
		#endregion

		#region Applying Perlin Noise into the Algorithm
		for (int x = 0; x < xSize; x++)
		{
			for (int y = 0; y < ySize; y++)
			{
				for (int z = 0; z < zSize; z++)
				{

					float gen = Perlin3D(x * noiseScale + noisePosition.x, y * noiseScale + noisePosition.y, z * noiseScale + noisePosition.z);
					Mathf.Round(gen);
					if (gen >= readableValue)
					{
						if (sphere == true)
						{
							radius = (xSize / 2) - 1;
							if (Vector3.Distance(new Vector3(x, y, z), Vector3.one * radius) < radius)
							{
								verts[x, y, z].button = true;
							}
							if(Vector3.Distance(new Vector3(x, y, z), Vector3.one * radius) >= radius)
							{								
								verts[x, y, z].button = false;
							}
						}
						if (sphere == false)
						{
							verts[x, y, z].button = true;
						}
					}

					if (GridDebug == true)
					{
						Debugger.DebugPoint(verts[x, y, z].pos, GetComponent<ColorCode>().vertSize, verts[x, y, z].button, this.GetComponent<ColorCode>().vertOffColor, GetComponent <ColorCode>().vertOnColor);
						if (x != xSize - 1)
						{
							Debugger.DebugLine(verts[x, y, z].pos, verts[x + 1, y, z].pos, GetComponent<ColorCode>().linecolor);
						}
						if (y != ySize - 1)
						{
							Debugger.DebugLine(verts[x, y, z].pos, verts[x, y + 1, z].pos, GetComponent<ColorCode>().linecolor);
						}
						if (z != zSize - 1)
						{
							Debugger.DebugLine(verts[x, y, z].pos, verts[x, y, z + 1].pos, GetComponent<ColorCode>().linecolor);
						}
					}
				}
			}
		}
		#endregion

		#region Drawing Mesh
		for (int x = 0; x < xSize - 1; x++)
		{
			for (int y = 0; y < ySize - 1; y++)
			{
				for (int z = 0; z < zSize - 1; z++)
				{
					v[0] = verts[x, y, z];
					v[1] = verts[x + 1, y, z];
					v[2] = verts[x + 1, y, z + 1];
					v[3] = verts[x, y, z + 1];
					v[4] = verts[x, y + 1, z];
					v[5] = verts[x + 1, y + 1, z];
					v[6] = verts[x + 1, y + 1, z + 1];
					v[7] = verts[x, y + 1, z + 1];
					Evaluate(v);
				}
			}
		}
		#endregion
	}

	public void GizmoDraw()
	{
		FullVertList = new List<Vector3>();

		if (noiseScale < 0)
		{
			noiseScale *= -1;
		}

		Vertx[,,] verts = new Vertx[xSize, ySize, zSize];

		#region  Vert and edge assignment 
		for (int x = 0; x < xSize; x++)
		{
			for (int y = 0; y < ySize; y++)
			{
				for (int z = 0; z < zSize; z++)
				{
					verts[x, y, z] = new Vertx();
					verts[x, y, z].pos = new Vector3(x * xOffset, y * yOffset, z * zOffset) + (transform.position - new Vector3(xOffset * xSize / 2, yOffset * ySize / 2, zOffset * zSize / 2));
				}
			}
		}
		#endregion

		#region Applying Perlin Noise into the Algorithm
		for (int x = 0; x < xSize; x++)
		{
			for (int y = 0; y < ySize; y++)
			{
				for (int z = 0; z < zSize; z++)
				{

					float gen = Perlin3D(x * noiseScale + noisePosition.x, y * noiseScale + noisePosition.y, z * noiseScale + noisePosition.z);
					Mathf.Round(gen);
					if (gen >= readableValue)
					{
						if (sphere == true)
						{
							radius = (xSize / 2) - 1;
							if (Vector3.Distance(new Vector3(x, y, z), Vector3.one * radius) < radius)
							{
								verts[x, y, z].button = true;
							}
							if (Vector3.Distance(new Vector3(x, y, z), Vector3.one * radius) >= radius)
							{
								verts[x, y, z].button = false;
							}
						}
						if (sphere == false)
						{
							verts[x, y, z].button = true;
						}
					}

					if (GridDebug == true)
					{
						Debugger.DebugPoint(verts[x, y, z].pos, GetComponent<ColorCode>().vertSize, verts[x, y, z].button, this.GetComponent<ColorCode>().vertOffColor, GetComponent<ColorCode>().vertOnColor);
						if (x != xSize - 1)
						{
							Debugger.DebugLine(verts[x, y, z].pos, verts[x + 1, y, z].pos, GetComponent<ColorCode>().linecolor);
						}
						if (y != ySize - 1)
						{
							Debugger.DebugLine(verts[x, y, z].pos, verts[x, y + 1, z].pos, GetComponent<ColorCode>().linecolor);
						}
						if (z != zSize - 1)
						{
							Debugger.DebugLine(verts[x, y, z].pos, verts[x, y, z + 1].pos, GetComponent<ColorCode>().linecolor);
						}
					}
				}
			}
		}
		#endregion

		#region Drawing Mesh
		for (int x = 0; x < xSize - 1; x++)
		{
			for (int y = 0; y < ySize - 1; y++)
			{
				for (int z = 0; z < zSize - 1; z++)
				{
					v[0] = verts[x, y, z];
					v[1] = verts[x + 1, y, z];
					v[2] = verts[x + 1, y, z + 1];
					v[3] = verts[x, y, z + 1];
					v[4] = verts[x, y + 1, z];
					v[5] = verts[x + 1, y + 1, z];
					v[6] = verts[x + 1, y + 1, z + 1];
					v[7] = verts[x, y + 1, z + 1];
					GizmoEvaluate(v);
				}
			}
		}
		#endregion
	}

	void Evaluate(Vertx[] ver)
	{
		#region permutational Calculations.

		int cubeindex = 0;
		float isoLevel = 1f;

		for (int i = 0; i < 8; i++)
		{
			if ((ver[i].button ? 1 : 0) < isoLevel) cubeindex |= (int)Mathf.Pow(2, i);
		}

		Vector3[] vertlist = new Vector3[12];

		for (int i = 0; i < 12; i++)
		{
			if (Tables.isVertEnabled(Tables.edgeTable[cubeindex], (int)Mathf.Pow(2, i)))
			{
				vertlist[i] = Tables.VertexInterpolate(ver[Tables.JoiningVerts[i, 0]].pos, ver[Tables.JoiningVerts[i, 1]].pos);
			}
		}

		for (int i = 0; Tables.triTable[cubeindex, i] != -1; i += 3)
		{
			if (meshgen == true)
			{
				if (flipNormals == true)
				{
					FullVertList.Add(vertlist[Tables.triTable[cubeindex, i]]);
					FullVertList.Add(vertlist[Tables.triTable[cubeindex, i + 1]]);
					FullVertList.Add(vertlist[Tables.triTable[cubeindex, i + 2]]);
				}
				if (flipNormals == false)
				{
					FullVertList.Add(vertlist[Tables.triTable[cubeindex, i + 2]]);
					FullVertList.Add(vertlist[Tables.triTable[cubeindex, i + 1]]);
					FullVertList.Add(vertlist[Tables.triTable[cubeindex, i]]);
				}
			}
		}
		#endregion
	}

	void GizmoEvaluate(Vertx[] ver)
	{
		#region permutational Calculations.

		int cubeindex = 0;
		float isoLevel = 1f;

		for (int i = 0; i < 8; i++)
		{
			if ((ver[i].button ? 1 : 0) < isoLevel) cubeindex |= (int)Mathf.Pow(2, i);
		}

		Vector3[] vertlist = new Vector3[12];

		for (int i = 0; i < 12; i++)
		{
			if (Tables.isVertEnabled(Tables.edgeTable[cubeindex], (int)Mathf.Pow(2, i)))
			{
				vertlist[i] = Tables.VertexInterpolate(ver[Tables.JoiningVerts[i, 0]].pos, ver[Tables.JoiningVerts[i, 1]].pos);
				if (GridDebug == true)
				{
					Debugger.DebugPoint(vertlist[i], GetComponent<ColorCode>().edgeSize, true, GetComponent<ColorCode>().edgeOffColor, GetComponent<ColorCode>().edgeOnColor);
				}
			}
		}

		for (int i = 0; Tables.triTable[cubeindex, i] != -1; i += 3)
		{
			if (MeshDebug == true)
			{
				Debugger.DebugTri(
					vertlist[Tables.triTable[cubeindex, i]],
					vertlist[Tables.triTable[cubeindex, i + 1]],
					vertlist[Tables.triTable[cubeindex, i + 2]],
					this.gameObject.GetComponent<ColorCode>().MeshColor);
			}
		}
		#endregion
	}

	public static float Perlin3D(float x, float y, float z)
	{
		float ab = Mathf.PerlinNoise(x, y);
		float bc = Mathf.PerlinNoise(y, z);
		float ac = Mathf.PerlinNoise(z, x);

		float ba = Mathf.PerlinNoise(y, x);
		float cb = Mathf.PerlinNoise(z, y);
		float ca = Mathf.PerlinNoise(z, x);

		float abc = ab + bc + ac + ba + cb + ca;

		return abc / 6f;
	}

}

public class MeshData
{
	public Vector3[] vertices;
	public int[] triangles;
	public Vector2[] uvs;

	public MeshData(int verts)
	{
		vertices = new Vector3[verts];
		uvs = new Vector2[verts];
		triangles = new int[verts];
	}

	public Mesh CreateMesh()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals();

		return mesh;
	}
}

public class Vertx
{
	public Vector3 pos;
	public bool button;
}

public class Edgs
{
	public Vector3 pos;
	public bool button;
}
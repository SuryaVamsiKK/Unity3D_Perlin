using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrianGenerator : MonoBehaviour
{
	[Header ("Look of the Terrian")]
	[SerializeField]
	private GameObject blocks;
	public Material material;
	public bool voxlate = false;
	public bool visualize = true;

	[Header ("Noise Values")]
	[SerializeField]
	private float noiseScale = 50;
	[Range(0, 1)]
	public float readableValue = 0.5f;
	public Vector3 noisePosition;

	[Header ("Terrian Size Values")]
	public int chunkSize = 30;
	public float range;


	public bool sphere = false;
	private Cell[,,] ListofGameobjects;

	private List<Cell>[,,] Listofneighbours;
	private List<Cell>[,,] Listofallaround;

	void Awake()
	{
		noiseScale = noiseScale * 0.001f;
		ListofGameobjects = new Cell[chunkSize, chunkSize, chunkSize];
		Listofneighbours = new List<Cell>[chunkSize, chunkSize, chunkSize];
		Listofallaround = new List<Cell>[chunkSize, chunkSize, chunkSize];
		generator();
	}

	void generator()
	{
		for (int i = 0; i < this.transform.childCount; i++)
		{
			Destroy(this.transform.GetChild(i).gameObject);
		}

		List<CombineInstance> combine = new List<CombineInstance>();

		MeshFilter blockMesh = Instantiate(blocks, Vector3.zero, Quaternion.identity).GetComponent<MeshFilter>();
		blockMesh.transform.parent = this.gameObject.transform;

		#region Perline Noise Application on the Gird of Cubes
		for (int x = 0; x < chunkSize; x++)
		{
			for (int y = 0; y < chunkSize; y++)
			{
				for (int z = 0; z < chunkSize; z++)
				{
					ListofGameobjects[x, y, z] = new Cell(false, true, new Vector3(x,y,z), true, "" + x + ":" + y + ":" + z);
					Listofneighbours[x, y, z] = new List<Cell>();
					Listofallaround[x, y, z] = new List<Cell>();

					float gen = Perlin3D(x * noiseScale + noisePosition.x, y * noiseScale + noisePosition.y, z * noiseScale + noisePosition.z);	// pussing the generated perlin value by the method written below.
					Mathf.Round(gen);

					if (gen >= readableValue)
					{
						float radius = chunkSize / 2;
						if (sphere == true && Vector3.Distance(new Vector3(x, y, z), Vector3.one * radius) > radius)
							continue;

						ListofGameobjects[x, y, z].inrange = true;		// enebling all the possible positions that are under the given range in the generated Perlin
					}

					#region For The Outer Shell if a planet
					if (gen <= readableValue)
					{
						float radius = chunkSize / 2;

						if (sphere == true && Vector3.Distance(new Vector3(x, y, z), Vector3.one * radius) >= radius && sphere == true && Vector3.Distance(new Vector3(x, y, z), Vector3.one * radius) < (radius + 1))
						{
							ListofGameobjects[x, y, z].inrange = true;  // enabeling the possible positions for the planetry formation
						}
					}
					#endregion
				}
			}
		}
		#endregion

		#region Neighbour Assignment
		for (int x = 0; x < chunkSize; x++)
		{
			for (int y = 0; y < chunkSize; y++)
			{
				for (int z = 0; z < chunkSize; z++)
				{
					if (x == 0 || y == 0 || z == 0 || x == (chunkSize - 1) || y == (chunkSize - 1) || z == (chunkSize - 1))
					{
						goto ot;
					}

					Listofneighbours[x, y, z].Add(ListofGameobjects[x + 1, y, z]); // Neighbour to the left of the Current cell
					Listofneighbours[x, y, z].Add(ListofGameobjects[x - 1, y, z]); // Neighbour to the right of the Current cell 

					Listofneighbours[x, y, z].Add(ListofGameobjects[x, y + 1, z]); // Neighbour Above the current Cell
					Listofneighbours[x, y, z].Add(ListofGameobjects[x, y - 1, z]); // Neighbour Below the current Cell

					Listofneighbours[x, y, z].Add(ListofGameobjects[x, y, z + 1]); // Neighbour Ahead of the Current Cell
					Listofneighbours[x, y, z].Add(ListofGameobjects[x, y, z - 1]); // Neighbour Behind the Current Cell
					ot:;
				}
			}
		}
		#endregion

		#region Lines

		#region Neighbour Assignment with corner Assignment

		for (int x = 0; x < chunkSize; x++)
		{
			for (int y = 0; y < chunkSize; y++)
			{
				for (int z = 0; z < chunkSize; z++)
				{					
					if (x < (chunkSize - 1)) Listofallaround[x, y, z].Add(ListofGameobjects[x + 1, y, z]);
					if (x > 0)Listofallaround[x, y, z].Add(ListofGameobjects[x - 1, y, z]);

					if (y < (chunkSize - 1)) Listofallaround[x, y, z].Add(ListofGameobjects[x, y + 1, z]);
					if (y > 0) Listofallaround[x, y, z].Add(ListofGameobjects[x, y - 1, z]);

					if (z < (chunkSize - 1)) Listofallaround[x, y, z].Add(ListofGameobjects[x, y, z + 1]);
					if (z > 0) Listofallaround[x, y, z].Add(ListofGameobjects[x, y, z - 1]);


					if (x < (chunkSize - 1) && y < (chunkSize - 1))						    { Listofallaround[x, y, z].Add(ListofGameobjects[x + 1, y + 1, z]); }
					if (x < (chunkSize - 1) && y > 0)									    { Listofallaround[x, y, z].Add(ListofGameobjects[x + 1, y - 1, z]); }
					if (x < (chunkSize - 1) && y < (chunkSize - 1) && z < (chunkSize - 1))  { Listofallaround[x, y, z].Add(ListofGameobjects[x + 1, y + 1, z + 1]); }
					if (x < (chunkSize - 1) && y < (chunkSize - 1) && z > 0)				{ Listofallaround[x, y, z].Add(ListofGameobjects[x + 1, y + 1, z - 1]); }
					if (x < (chunkSize - 1) && y > 0 && z < (chunkSize - 1))				{ Listofallaround[x, y, z].Add(ListofGameobjects[x + 1, y - 1, z + 1]); }
					if (x < (chunkSize - 1) && y > 0 && z > 0)								{ Listofallaround[x, y, z].Add(ListofGameobjects[x + 1, y - 1, z - 1]); }
					if (x < (chunkSize - 1) && z < (chunkSize - 1))							{ Listofallaround[x, y, z].Add(ListofGameobjects[x + 1, y, z + 1]); }
					if (x < (chunkSize - 1) && z > 0)										{ Listofallaround[x, y, z].Add(ListofGameobjects[x + 1, y, z - 1]); }

					if (x > 0 && y < (chunkSize - 1))										{ Listofallaround[x, y, z].Add(ListofGameobjects[x - 1, y + 1, z]); }
					if (x > 0 && y > 0)														{ Listofallaround[x, y, z].Add(ListofGameobjects[x - 1, y - 1, z]); }
					if (x > 0 && y < (chunkSize - 1) && z < (chunkSize - 1))				{ Listofallaround[x, y, z].Add(ListofGameobjects[x - 1, y + 1, z + 1]); }
					if (x > 0 && y < (chunkSize - 1) && z > 0)								{ Listofallaround[x, y, z].Add(ListofGameobjects[x - 1, y + 1, z - 1]); }
					if (x > 0 && y > 0 && z < (chunkSize - 1))								{ Listofallaround[x, y, z].Add(ListofGameobjects[x - 1, y - 1, z + 1]); }
					if (x > 0 && y > 0 && z > 0)											{ Listofallaround[x, y, z].Add(ListofGameobjects[x - 1, y - 1, z - 1]); }
					if (x > 0 && z < (chunkSize - 1))										{ Listofallaround[x, y, z].Add(ListofGameobjects[x - 1, y, z + 1]); }
					if (x > 0 && z > 0)														{ Listofallaround[x, y, z].Add(ListofGameobjects[x - 1, y, z - 1]); }

					if (y < (chunkSize - 1) && z < (chunkSize - 1))							{ Listofallaround[x, y, z].Add(ListofGameobjects[x, y + 1, z + 1]); }
					if (y < (chunkSize - 1) && z > 0)										{ Listofallaround[x, y, z].Add(ListofGameobjects[x, y + 1, z - 1]); }

					if (y > 0 && z < (chunkSize - 1))										{ Listofallaround[x, y, z].Add(ListofGameobjects[x, y - 1, z + 1]); }
					if (y > 0 && z > 0)														{ Listofallaround[x, y, z].Add(ListofGameobjects[x, y - 1, z - 1]); }

				//	ot:;
					

				}
			}
		}

		#endregion

		#endregion

		#region Switching off the inner cubes
		for (int x = 0; x < chunkSize; x++)
		{
			for (int y = 0; y < chunkSize; y++)
			{
				for (int z = 0; z < chunkSize; z++)
				{

					List<Cell> neighbours;
					neighbours = Listofneighbours[x, y, z];
					int i = 0;

					foreach (Cell g in neighbours)
					{
						if (g.inrange == true)
						{
							i++;
						}
					}

					if (i == 6)
					{
						ListofGameobjects[x, y, z].needed = false;          // these are the possible cube positions where the cubes are totally covered and not needed
					}

					if (x == 0 || y == 0 || z == 0 || x == (chunkSize - 1) || y == (chunkSize - 1) || z == (chunkSize - 1))
					{
						ListofGameobjects[x, y, z].needed = false;      // negating the outercube 
					}
				}
			}
		}
		#endregion
		

		if (voxlate == true)
		{
			#region Voxel Mesh Generation

			#region Vertices Feeding
			for (int x = 0; x < chunkSize; x++)
			{
				for (int y = 0; y < chunkSize; y++)
				{
					for (int z = 0; z < chunkSize; z++)
					{
						if (ListofGameobjects[x, y, z].inrange == true && ListofGameobjects[x, y, z].needed == true) // Printing The Cube at the correct positions for the Mesh.
						{
							#region Mesh Creation
							blockMesh.transform.position = new Vector3(x, y, z);
							combine.Add(new CombineInstance { mesh = blockMesh.sharedMesh, transform = blockMesh.transform.localToWorldMatrix });
							#endregion
						}
					}
				}
			}
			#endregion

			#region Mesh Generation
			List<List<CombineInstance>> combineLists = new List<List<CombineInstance>>();
			int vertexCount = 0;
			combineLists.Add(new List<CombineInstance>());

			for (int i = 0; i < combine.Count; i++)
			{
				vertexCount += combine[i].mesh.vertexCount;
				if (vertexCount > 65536)
				{
					vertexCount = 0;
					combineLists.Add(new List<CombineInstance>());  // Breaking the mesh to new mesh if the vertex count increases than the limit.
					i--;
				}
				else
				{
					combineLists.Last().Add(combine[i]);  // if it doesnt exceed the mesh count putting the mesh in the last to continue added verts.
				}
			}

			int numberChunk = 1;
			Transform meshys = new GameObject("Terrian").transform;
			meshys.transform.parent = this.gameObject.transform;

			foreach (List<CombineInstance> list in combineLists) // Creating as many chunks required for the entire Terrian
			{
				numberChunk++;
				GameObject g = new GameObject("Part " + numberChunk);
				g.transform.parent = meshys;
				MeshFilter mf = g.AddComponent<MeshFilter>();
				MeshRenderer mr = g.AddComponent<MeshRenderer>();
				mr.material = material;
				mf.mesh.CombineMeshes(list.ToArray());
			}
			#endregion

			#endregion
		}
		
		#region Part of Older Version
		/*
		for (int i = 0; i < this.transform.childCount; i++)
		{
			if (this.transform.GetChild(i).gameObject.tag == "TestSubjects")
			{
				Destroy(this.transform.GetChild(i).gameObject);
			}
		}*/
		#endregion
	}

	void Update()
	{
		if (visualize == true)
		{
			#region Vertices Feeding for lines

			for (int x = 0; x < chunkSize; x++)
			{
				for (int y = 0; y < chunkSize; y++)
				{
					for (int z = 0; z < chunkSize; z++)
					{
						if (ListofGameobjects[x, y, z].inrange == true && ListofGameobjects[x, y, z].needed == true) // Printing The Cube at the correct positions for the Mesh.
						{
							for (int i = 0; i < Listofallaround[x, y, z].Count; i++)
							{
								if (Listofallaround[x, y, z][i].inrange == true && Listofallaround[x, y, z][i].needed == true && Listofallaround[x, y, z][i].neighbours == true)
								{
									Debug.DrawLine(ListofGameobjects[x, y, z].pos, Listofallaround[x, y, z][i].pos, Color.red);
								}
							}
							
						}
					}
				}
			}
			#endregion
		}

		if (Input.GetKeyDown(KeyCode.G))
		{
			generator();
		}

		if (Input.GetKey(KeyCode.X))
		{
			if (Input.GetKeyDown(KeyCode.KeypadPlus))
			{
				noisePosition.x += 0.1f;
				generator();
			}

			if (Input.GetKeyDown(KeyCode.KeypadMinus))
			{
				noisePosition.x -= 0.1f;
				generator();
			}
		}

		if (Input.GetKey(KeyCode.Y))
		{
			if (Input.GetKeyDown(KeyCode.KeypadPlus))
			{
				noisePosition.y += 0.1f;
				generator();
			}

			if (Input.GetKeyDown(KeyCode.KeypadMinus))
			{
				noisePosition.y -= 0.1f;
				generator();
			}
		}

		if (Input.GetKey(KeyCode.Z))
		{
			if (Input.GetKeyDown(KeyCode.KeypadPlus))
			{
				noisePosition.z += 0.1f;
				generator();
			}

			if (Input.GetKeyDown(KeyCode.KeypadMinus))
			{
				noisePosition.z -= 0.1f;
				generator();
			}
		}

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

public class Cell
{
	public bool inrange;
	public bool needed;
	public bool toface;
	public Vector3 pos;
	public bool neighbours;
	public string ID;

	public Cell(bool a, bool b, Vector3 postion, bool neigh, string id)
	{
		ID = id;
		neighbours = neigh;
		pos = postion;
		toface = false;
		inrange = a;
		needed = b;
	}
}

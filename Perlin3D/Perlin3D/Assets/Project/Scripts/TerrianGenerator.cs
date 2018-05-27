using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrianGenerator : MonoBehaviour
{

	[SerializeField]
	private GameObject blocks;
	[SerializeField]
	private float noiseScale = 0.05f;
	public int chunkSize = 30; //numberof cubes
	public float limitsL = 0.5f;
	public float limitsH = 0.5f;
	public float limitsLr = 0.5f;
	public float limitsHr = 0.5f;
	public float xp;
	public float yp;
	public float zp;
	public float range;
	public bool sphere = false;
	public Material material;
	private GameObject[,,] ListofGameobjects;

	private List<GameObject>[,,] Listofneighbours;

	void Awake()
	{
		ListofGameobjects = new GameObject[chunkSize, chunkSize, chunkSize];
		Listofneighbours = new List<GameObject>[chunkSize, chunkSize, chunkSize];
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

		for (int i = 0; i < this.transform.childCount; i++)
		{
			Destroy(this.transform.GetChild(i).gameObject);
		}

		#region Perline Noise Application on the Gird of Cubes
		for (int x = 0; x < chunkSize; x++)
		{
			for (int y = 0; y < chunkSize; y++)
			{
				for (int z = 0; z < chunkSize; z++)
				{
					GameObject nth = Instantiate(blocks, new Vector3(x, y, z), Quaternion.identity, this.transform) as GameObject;
					ListofGameobjects[x, y, z] = nth;
					Listofneighbours[x, y, z] = new List<GameObject>();

					float gen = Perlin3D(x * noiseScale + xp, y * noiseScale + yp, z * noiseScale + zp);
					Mathf.Round(gen);

					if (gen >= limitsL)
					{
						float radius = chunkSize / 2;
						if (sphere == true && Vector3.Distance(new Vector3(x, y, z), Vector3.one * radius) > radius)
							continue;

						ListofGameobjects[x, y, z].SetActive(true);
					}

					#region For The Outer Shell if a planet
					if (gen <= limitsL)
					{
						float radius = chunkSize / 2;

						if (sphere == true && Vector3.Distance(new Vector3(x, y, z), Vector3.one * radius) >= radius && sphere == true && Vector3.Distance(new Vector3(x, y, z), Vector3.one * radius) < (radius + 1))
						{
							ListofGameobjects[x, y, z].SetActive(true);
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

					Listofneighbours[x, y, z].Add(ListofGameobjects[x + 1, y, z]);
					Listofneighbours[x, y, z].Add(ListofGameobjects[x - 1, y, z]);

					Listofneighbours[x, y, z].Add(ListofGameobjects[x, y + 1, z]);
					Listofneighbours[x, y, z].Add(ListofGameobjects[x, y - 1, z]);

					Listofneighbours[x, y, z].Add(ListofGameobjects[x, y, z + 1]);
					Listofneighbours[x, y, z].Add(ListofGameobjects[x, y, z - 1]);
					ot:;
				}
			}
		}
		#endregion


		for (int x = 0; x < chunkSize; x++)
		{
			for (int y = 0; y < chunkSize; y++)
			{
				for (int z = 0; z < chunkSize; z++)
				{

					List<GameObject> neighbours;
					neighbours = Listofneighbours[x, y, z];
					int i = 0;

					foreach (GameObject g in neighbours)
					{
						if (g.activeInHierarchy)
						{
							i++;
						}
					}

					if (i == 6)
					{
						ListofGameobjects[x,y,z].GetComponent<MeshRenderer>().enabled = false;
					}
				}
			}
		}

		#region Vertices Feeding
		for (int x = 0; x < chunkSize; x++)
		{
			for (int y = 0; y < chunkSize; y++)
			{
				for (int z = 0; z < chunkSize; z++)
				{
					if (ListofGameobjects[x, y, z].activeInHierarchy && ListofGameobjects[x, y, z].GetComponent<MeshRenderer>().enabled == true)
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
				combineLists.Add(new List<CombineInstance>());
				i--;
			}
			else
			{
				combineLists.Last().Add(combine[i]);
			}
		}

		Transform meshys = new GameObject("Meshys").transform;
		meshys.transform.parent = this.gameObject.transform;
		foreach (List<CombineInstance> list in combineLists)
		{
			GameObject g = new GameObject("Meshy");
			g.transform.parent = meshys;
			MeshFilter mf = g.AddComponent<MeshFilter>();
			MeshRenderer mr = g.AddComponent<MeshRenderer>();
			mr.material = material;
			mf.mesh.CombineMeshes(list.ToArray());
		}
		#endregion

		#region Destroying The Active Cubes
		for (int i = 0; i < this.transform.childCount; i++)
		{
			if (this.transform.GetChild(i).gameObject.tag == "TestSubjects")
			{
				Destroy(this.transform.GetChild(i).gameObject);
			}
		}
		#endregion
	}

	void Update()
	{

		if (Input.GetKeyDown(KeyCode.G))
		{
			generator();
		}

		if (Input.GetKey(KeyCode.X))
		{
			if (Input.GetKeyDown(KeyCode.KeypadPlus))
			{
				xp += 0.1f;
				generator();
			}

			if (Input.GetKeyDown(KeyCode.KeypadMinus))
			{
				xp -= 0.1f;
				generator();
			}
		}

		if (Input.GetKey(KeyCode.Y))
		{
			if (Input.GetKeyDown(KeyCode.KeypadPlus))
			{
				yp += 0.1f;
				generator();
			}

			if (Input.GetKeyDown(KeyCode.KeypadMinus))
			{
				yp -= 0.1f;
				generator();
			}
		}

		if (Input.GetKey(KeyCode.Z))
		{
			if (Input.GetKeyDown(KeyCode.KeypadPlus))
			{
				zp += 0.1f;
				generator();
			}

			if (Input.GetKeyDown(KeyCode.KeypadMinus))
			{
				zp -= 0.1f;
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

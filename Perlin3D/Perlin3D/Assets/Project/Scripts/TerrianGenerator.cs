using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrianGenerator : MonoBehaviour {

	[SerializeField]
	private GameObject blocks;
	[SerializeField]
	private float noiseScale = 0.05f;
	public float chunkSize = 0.05f;
	public float limitsL = 0.5f;
	public float limitsH = 0.5f;
	public float limitsLr = 0.5f;
	public float limitsHr = 0.5f;
	public float xp;
	public float yp;
	public float zp;
	public float range;
	public bool sphere = false;

	void Start()
	{
		generator();
	}

	void generator ()
	{
		for (int i = 0; i < this.transform.childCount; i++)
		{
			Destroy(this.transform.GetChild(i).gameObject);
		}

		for (int x = 0; x < chunkSize; x++)
		{
			for (int y = 0; y < chunkSize; y++)
			{
				for (int z = 0; z < chunkSize; z++)
				{
					float gen = Perlin3D(x * noiseScale + xp, y * noiseScale + yp, z * noiseScale + zp);

					if (gen >= limitsL && gen < limitsH)
					{
						float radius = chunkSize / 2;

						if (sphere == true && Vector3.Distance(new Vector3(x, y, z), Vector3.one * radius) > radius)
							continue;
						Instantiate(blocks, new Vector3(x, y, z), Quaternion.identity, this.transform);
					}
					if (gen <= limitsL)
					{
						float radius = chunkSize / 2;

						if (sphere == true && Vector3.Distance(new Vector3(x, y, z), Vector3.one * radius) >= radius && sphere == true && Vector3.Distance(new Vector3(x, y, z), Vector3.one * radius) < (radius+1))
						{
							Instantiate(blocks, new Vector3(x, y, z), Quaternion.identity, this.transform);
						}
					}
				}
			}
		}


		
	}

	void Update () {

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

		return abc/6f;
	}
} 

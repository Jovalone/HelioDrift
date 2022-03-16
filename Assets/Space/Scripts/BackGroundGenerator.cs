using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundGenerator : MonoBehaviour
{

	//private bool BL, BM, BR, ML, MM, MR, TL, TM, TR;
	private bool Nessecary;

	public Transform SpaceGenerator;
	public Transform Player;
	public float size;
	private float X, Y;
	private int Size, Length;

	private bool[] Spots;

	public GameObject Chunk;
	public List<GameObject> Chunks;

	public bool follow;
	public float followRatio;

	void Start()
    {
		Player = GameObject.FindWithTag("Player").transform;

		Size = (int)(4 * Mathf.Pow(size, 2) - 4 * size + 1);
		Length = (int)Mathf.Sqrt(Size);

		Spots = new bool[Size];
		Chunks = new List<GameObject>();

		for (int i = 0; i < Spots.Length; i++)
		{
			Spots[i] = false;
			GameObject chunk = (GameObject)Instantiate(Chunk, new Vector3((float)((i % Length) * 40) - 80, (float)((int)(i / Length)) * 40 - 80, 0), SpaceGenerator.rotation, SpaceGenerator);
			Chunks.Add(chunk);
		}
	}

	void Update()
	{

		//GameObject[] allChunks = GameObject.FindGameObjectsWithTag("Chunk");

		X = Mathf.Round(Player.position.x / 80);
		Y = Mathf.Round(Player.position.y / 80);
		
		for (int i = 0; i < Spots.Length; i++)
        {
			Spots[i] = false;
		}
		
		foreach (GameObject Chunk in Chunks)
        {
			bool n = false;
			if (Chunk != null)
            {
				for (int j = 0; j < Spots.Length; j++)
				{
					if ((Chunk.transform.position == new Vector3((j % Length) * 40 - 80 + X * 40, (float)((int)(j / Length)) * 40 - 80 + Y * 40, 0)) && !Spots[j])
					{
						Spots[j] = true;
						n = true;
					}
				}
				if (!n)
				{
					Destroy(Chunk);
				}
			}
		}

		for (int k = 0; k < Spots.Length; k++)
		{
			if (!Spots[k] && Chunks[k] == null)
            {
				GameObject chunk = (GameObject)Instantiate(Chunk, new Vector3((float)((k % Length) * 40) - 80, (float)((int)(k / Length)) * 40 - 80, 0), SpaceGenerator.rotation, SpaceGenerator);
				Chunks.Add(chunk);
			}
        }
	}
}

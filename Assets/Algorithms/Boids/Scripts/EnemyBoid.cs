using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoid : MonoBehaviour
{

	public Vector3 position;
	//public Level level;
	public float dist;

	void Start()
    {
		Level.levelInstance.enemies.Add(this);
		position = transform.position;
	}

	void Update()
    {
		position = transform.position;
	}
}

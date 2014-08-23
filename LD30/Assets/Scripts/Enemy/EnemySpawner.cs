using UnityEngine;
using System.Collections;
using System;

public class EnemySpawner : MonoBehaviour {

	[SerializeField]
	GameObject m_prefab;

	[SerializeField]
	float m_spawnTimeoutMS  = 5000;

	DateTime m_lastSpawn = DateTime.MinValue;

	void Update()
	{
		DateTime curr = DateTime.UtcNow;

		if((curr - m_lastSpawn).TotalMilliseconds > m_spawnTimeoutMS)
		{
			Spawn();
			m_lastSpawn = curr;
		}
	}

	void Spawn()
	{
		GameObject go = Instantiate(m_prefab, transform.position, Quaternion.identity) as GameObject;
	}
}

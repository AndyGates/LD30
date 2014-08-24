using UnityEngine;
using System.Collections;
using System;

public class EnemySpawner : MonoBehaviour {

	[SerializeField]
	GameObject m_prefab;

	[SerializeField]
	float m_spawnTimeoutMS  = 5000;

	[SerializeField]
	AudioClip m_dieSound; 

	DateTime m_lastSpawn;
	Animator m_anim;

	void Awake()
	{
		m_lastSpawn = DateTime.UtcNow;
		m_anim = GetComponent<Animator>();
	}

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

	public void Remove()
	{
		AudioSource.PlayClipAtPoint(m_dieSound, transform.position);
		m_anim.SetTrigger("Despawn");
		Destroy(gameObject, 0.6f);
	}
}

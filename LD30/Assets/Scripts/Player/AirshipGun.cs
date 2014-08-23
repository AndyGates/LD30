using UnityEngine;
using System.Collections;
using System;

public class AirshipGun : MonoBehaviour {
		
	[SerializeField]
	GameObject m_bulletPrefab;

	[SerializeField]
	AudioClip m_fireSound; 

	[SerializeField]
	Transform[] m_gunTransforms;

	[SerializeField]
	float m_cooldownMS = 200f;

	[SerializeField]
	float m_bulletForce = 100.0f; 
	
	int m_currentGun = 0;
	DateTime m_lastFire;

	void Awake()
	{
		m_lastFire = DateTime.MinValue;
	}

	public void Fire()
	{
		if((DateTime.UtcNow - m_lastFire).TotalMilliseconds > m_cooldownMS)
		{
			SpawnProjectile(transform.up);
			m_lastFire = DateTime.UtcNow;
			m_currentGun++;
			if(m_currentGun > m_gunTransforms.Length - 1) m_currentGun = 0;
		}
	}

	void SpawnProjectile(Vector3 direction)
	{
		GameObject go = Instantiate(m_bulletPrefab) as GameObject;
		go.transform.position = m_gunTransforms[m_currentGun].position;
		go.transform.rotation = transform.rotation;

		go.rigidbody2D.AddForce(transform.up * m_bulletForce);

		AudioSource.PlayClipAtPoint(m_fireSound, transform.position, 1.0f);
		Destroy(go, 2.0f);
	}
}

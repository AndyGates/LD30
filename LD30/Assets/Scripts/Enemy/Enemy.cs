using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour 
{
	[SerializeField]
	int m_health = 2;

	[SerializeField]
	AudioClip m_hitSound;

	[SerializeField]
	AudioClip m_deathSound;

	[SerializeField]
	HealthBar m_healthBar;

	int m_initialHealth; 

	void Awake()
	{
		m_initialHealth = m_health;
	}

	void OnCollisionEnter2D(Collision2D col) 
	{
		PlayerProjectile m_projectile = col.gameObject.GetComponent<PlayerProjectile>();
		if(m_projectile != null)
		{
			m_health -= m_projectile.Damage;
			Destroy(col.gameObject);
			AudioSource.PlayClipAtPoint(m_hitSound, transform.position);
			m_healthBar.SetHealth((float) m_health / (float) m_initialHealth);
		}
		else
		{
			m_health = 0;
		}

		if(m_health <= 0)
		{
			OnDie();
		}
	}

	void OnDie()
	{
		AudioSource.PlayClipAtPoint(m_deathSound, transform.position);
		Destroy(gameObject);
	}
}

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
	GameObject m_deathPrefab;

	[SerializeField]
	HealthBar m_healthBar;

	[SerializeField]
	float m_speed = 0.01f;

	int m_initialHealth; 

	GameObject m_target;

	void Awake()
	{
		//m_target = GameObject.FindObjectOfType<AirshipController>();
		m_target = TryGetTarget();
		m_initialHealth = m_health;
	}

	GameObject TryGetTarget()
	{
		GameObject target = null;

		if(NPCManager.Instance.Builder != null)
		{
			target = NPCManager.Instance.Builder.gameObject;
		}
		else
		{
			Debug.Log("Could not find a builder");
			target = FindObjectOfType<AirshipController>().gameObject;
		}

		return target;
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
		GameObject go = Instantiate(m_deathPrefab, transform.position, Quaternion.identity) as GameObject;
		AudioSource.PlayClipAtPoint(m_deathSound, transform.position);
		Destroy(gameObject);
		Destroy(go, 0.3f);
	}

	void Update()
	{
		if(m_target != null)
		{
			rigidbody2D.MovePosition(Vector2.MoveTowards(transform.localPosition, m_target.transform.position, m_speed));
		}
		else
		{
			m_target = TryGetTarget();
		}
	}
}

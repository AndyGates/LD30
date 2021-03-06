﻿using UnityEngine;
using System.Collections;

public class AirshipController : MonoBehaviour {
		
	[SerializeField]
	float m_speed = 1.0f;

	[SerializeField]
	float m_health = 100f;

	AirshipGun m_gun;
	
	void Awake()
	{
		m_gun = GetComponent<AirshipGun>();
	}

	void Update()
	{
		float velocity = m_speed * Time.smoothDeltaTime;
		Vector3 newPosition = transform.localPosition;

		if(Input.GetKey(KeyCode.W)) newPosition.y += velocity;
		else if(Input.GetKey(KeyCode.S)) newPosition.y -= velocity;

		if(Input.GetKey(KeyCode.D)) newPosition.x += velocity;
		else if(Input.GetKey(KeyCode.A)) newPosition.x -= velocity;

		Vector3 move = (newPosition - transform.localPosition);
		float mag = move.magnitude;

		if(mag != 0)
		{
			Vector3 dir = -move.normalized;
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 0.1f);

			newPosition = transform.localPosition += (transform.up * velocity);
			transform.localPosition = newPosition;
		}

		if(Input.GetKey(KeyCode.Space)) m_gun.Fire();
	}
}

using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class NPCBuilder : NPCBase {
	
	public Bridge CurrentBridge {get;set;}
	public Action OnDeath {get;set;}

	GameObject m_hat; 

	protected override void Awake ()
	{
		base.Awake ();
	
		foreach(Transform child in transform)
		{
			if(child.gameObject.name == "Hat") m_hat = child.gameObject;
		}
		if(m_hat != null) m_hat.SetActive(true);

		Bridge bridge = m_currentIsland.CreateBridgeToNext();
		Debug.Log(bridge.LastPosition);
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		Debug.Log("Collision " + other.gameObject.name);
		Enemy e = other.gameObject.GetComponent<Enemy>();
		Debug.Log(e);
		if(e != null)
		{
			m_anim.SetTrigger("Die");
			if(OnDeath != null) OnDeath();
		}
	}	
}

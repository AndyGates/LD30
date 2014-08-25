using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class NPCBuilder : NPCBase {
	
	public Bridge CurrentBridge {get;set;}
	public Action OnDeath {get;set;}
	public Action OnReachedEnd {get;set;}
	public Action OnReachedIsland {get;set;}

	GameObject m_hat; 
	bool m_building = false;

	protected override void Awake ()
	{
		base.Awake ();
	
		foreach(Transform child in transform)
		{
			if(child.gameObject.name == "Hat") m_hat = child.gameObject;
		}
		if(m_hat != null) m_hat.SetActive(true);

		if(NPCManager.Instance.CurrentBridge == null) GetNextBridge();
		else
		{
			CurrentBridge = NPCManager.Instance.CurrentBridge;
			m_target = CurrentBridge.LastPosition;
			m_building = false;
		}
	}
	
	void GetNextBridge()
	{
		CurrentBridge = m_currentIsland.CreateBridgeToNext();
		CurrentBridge.PlaceBridgePart();
		m_target = CurrentBridge.StartPosition + m_offset;
		CurrentBridge.OnBridgeComplete += OnBridgeComplete;
		NPCManager.Instance.CurrentIsland = m_currentIsland; 
		NPCManager.Instance.CurrentBridge = CurrentBridge;
	}

	public void OnBridgeComplete()
	{
		CurrentBridge.OnBridgeComplete -= OnBridgeComplete;
		CurrentBridge = null;
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

	IEnumerator PlaceBridgePart()
	{
		yield return new WaitForSeconds(2);

		CurrentBridge.PlaceBridgePart();
		m_building = false;

		if(CurrentBridge != null) m_target = CurrentBridge.LastPosition + m_offset;
		else
		{
			GetClosestIsland();
			NPCManager.Instance.CurrentIsland = m_currentIsland;
			m_target = m_currentIsland.transform.position;
		}
	}
	
	protected override void Update ()
	{
		base.Update ();
		if(AtTarget)
		{
			if(CurrentBridge != null)
			{
				if(!m_building)
				{
					m_building = true;
					StartCoroutine(PlaceBridgePart());
				}
			}
			else
			{
				GetClosestIsland();
				Debug.Log(m_currentIsland + " " + m_currentIsland.IsFinal);
				if(!m_currentIsland.IsFinal)
				{
					if(OnReachedIsland != null) OnReachedIsland();
					GetNextBridge();
				}
				else
				{
					if(OnReachedEnd != null) OnReachedEnd();
				}
			}
		}
	}
}

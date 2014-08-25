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
	bool m_followingBridge = false;
	int m_currentBridgeWaypoint = 0; 

	protected override void Awake ()
	{
		base.Awake ();
	
		foreach(Transform child in transform)
		{
			if(child.gameObject.name == "Hat") m_hat = child.gameObject;
		}
		if(m_hat != null) m_hat.SetActive(true);

		if(NPCManager.Instance.CurrentIsland == null || NPCManager.Instance.CurrentIsland.ExitBridge == null)
		{
			GetNextBridge();
			//Debug.Log ("Builder got next bridge! " + CurrentBridge);
		}
		else
		{
			m_followingBridge = true;
			m_currentBridgeWaypoint = 0;
			CurrentBridge = NPCManager.Instance.CurrentIsland.ExitBridge;
			GetNextWaypoint();

			/*
			if(NPCManager.Instance.CurrentIsland.ExitBridge.IsFinished)
			{
				Debug.Log("Bridge finished, heading directly to island");
				m_target = NPCManager.Instance.CurrentIsland.ExitBridge.EndIsland.transform.position;
			}
			else
			{
				CurrentBridge = NPCManager.Instance.CurrentIsland.ExitBridge;
				Debug.Log ("Builder got existing bridge! " + CurrentBridge);

				m_target = CurrentBridge.LastPosition + m_offset;
				Debug.Log(CurrentBridge.LastPosition);
			}
			*/
		}
	}

	void GetNextWaypoint()
	{
		Vector2 waypoint = CurrentBridge.GetWaypoint(m_currentBridgeWaypoint);
		if(waypoint != Vector2.zero)
		{
			m_target = waypoint + m_offset;
			m_currentBridgeWaypoint++;
		}
		else
		{
			//m_target = CurrentBridge.EndIsland.transform.position;
			m_followingBridge = false;
		}
	
	}

	void GetNextBridge()
	{
		CurrentBridge = m_currentIsland.CreateBridgeToNext();
		CurrentBridge.PlaceBridgePart();
		m_target = CurrentBridge.StartPosition + m_offset;
		CurrentBridge.OnBridgeComplete += OnBridgeComplete;
		NPCManager.Instance.CurrentIsland = m_currentIsland; 
	}

	public void OnBridgeComplete()
	{
		CurrentBridge.OnBridgeComplete -= OnBridgeComplete;
		//Debug.Log("BridgeComplete");
		//CurrentBridge = null;
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		Enemy e = other.gameObject.GetComponent<Enemy>();
		if(e != null)
		{
			m_anim.SetTrigger("Die");
			if(OnDeath != null) OnDeath();

			RaycastHit2D[] hits = Physics2D.CircleCastAll(e.transform.position, 3.0f, Vector2.one);
			foreach(RaycastHit2D h in hits)
			{
				Enemy he = h.collider.gameObject.GetComponent<Enemy>();
				if(he != null) he.TakeDamage(2);
			}
		
			e.TakeDamage(2);
		}
	}	

	IEnumerator PlaceBridgePart()
	{
		yield return new WaitForSeconds(2);

		CurrentBridge.PlaceBridgePart();
		m_building = false;

		if(!CurrentBridge.IsFinished) m_target = CurrentBridge.LastPosition + m_offset;
		else
		{
			//Debug.Log ("Placed last part");
			m_target = m_currentIsland.ExitBridge.EndIsland.transform.position;
		}
	}
	
	protected override void Update ()
	{
		base.Update ();
		if(AtTarget)
		{
			if(m_followingBridge)
			{
				GetNextWaypoint();
			}
			//If heading to somewhere on a bridge
			else if(!CurrentBridge.IsFinished)
			{
				if(!m_building)
				{
					m_building = true;
					StartCoroutine(PlaceBridgePart());
				}
			}
			else
			{
				//Debug.Log("Got to end of finished bridge");
				GetClosestIsland();
				NPCManager.Instance.CurrentIsland = m_currentIsland;
				if(OnReachedIsland != null) OnReachedIsland();

				if(!m_currentIsland.IsFinal)
				{
					GetNextBridge();
					//Debug.Log("Builder now getting next bridge " + m_currentIsland + " " + CurrentBridge);

				}
				else
				{
					if(OnReachedEnd != null) OnReachedEnd();
				}
			}
		}
	}
}

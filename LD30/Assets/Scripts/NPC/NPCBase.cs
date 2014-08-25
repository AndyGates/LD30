using UnityEngine;
using System.Collections;

public class NPCBase : MonoBehaviour 
{
	protected Island m_currentIsland;
	protected Collider2D m_currentIslandCollider;
 
	protected Vector2 m_target;
	protected Animator m_anim;
	protected Vector2 m_offset = new Vector2(0, 0.4f);

	protected bool AtTarget
	{
		get
		{
			return (Mathf.Approximately(transform.position.x, m_target.x) && Mathf.Approximately(transform.position.y, m_target.y));
		}
	}

	protected virtual void Awake()
	{
		m_anim = GetComponent<Animator>();
		Vector2 pos = transform.position;
		m_target = pos + m_offset;
		GetClosestIsland();
	}

	public void MoveToCurrentIsland()
	{
		if(NPCManager.Instance.CurrentIsland != null)
		{
			transform.position = NPCManager.Instance.CurrentIsland.transform.position;
			GetClosestIsland();
			transform.position = m_currentIsland.transform.position;
			Vector2 pos = GetRandomPosition();
			m_target = pos + m_offset;
		}
	}
	
	protected void GetClosestIsland()
	{
		Island[] islands = GameObject.FindObjectsOfType<Island>();
		Island closest = null;
		float closestDst = 1000;
		
		foreach(Island i in islands)
		{
			float dst = Vector2.Distance(i.transform.position, transform.position);
			if(dst < closestDst)
			{
				closestDst = dst;
				closest = i;
			}
		}
		if(closest != null)
		{
			m_currentIsland = closest;
			m_currentIslandCollider = closest.GetComponentInChildren<PolygonCollider2D>();
		}
		else Debug.Log("Couldnt find an island :(");
	}
	
	protected virtual void Update()
	{
		if(!AtTarget) 
		{
			m_anim.SetBool("Walking", true);
			transform.position = Vector2.MoveTowards(transform.position, m_target, 0.02f);
		}
		else
		{
			m_anim.SetBool("Walking", false);
		}
	}

	public Vector2 GetRandomPosition()
	{
		return RandomPositionInCollider(m_currentIslandCollider);
	}
	
	protected Vector2 RandomPositionInCollider(Collider2D col)
	{
		return new Vector2(Random.Range(col.bounds.min.x, col.bounds.max.x), Random.Range(col.bounds.min.y, col.bounds.max.y));
	}
}

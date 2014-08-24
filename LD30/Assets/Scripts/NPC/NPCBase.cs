using UnityEngine;
using System.Collections;

public class NPCBase : MonoBehaviour 
{
	protected Island m_currentIsland;
	protected Collider2D m_currentIslandCollider;
 
	protected Vector2 m_target;
	protected Animator m_anim;

	protected virtual void Awake()
	{
		m_anim = GetComponent<Animator>();
		m_target = transform.position;
		GetClosestIsland();
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
		if((!Mathf.Approximately(transform.position.x, m_target.x) || !Mathf.Approximately(transform.position.y, m_target.y))) 
		{
			m_anim.SetBool("Walking", true);
			transform.position = Vector2.MoveTowards(transform.position, m_target, 0.02f);
		}
		else
		{
			m_anim.SetBool("Walking", false);
		}
	}
}

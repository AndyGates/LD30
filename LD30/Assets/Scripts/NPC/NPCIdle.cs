using UnityEngine;
using System.Collections;

public class NPCIdle : MonoBehaviour 
{
	[SerializeField]
	Collider2D m_currentIsland;

	Vector2 m_target;
	Animator m_anim;

	public bool ShouldIdleWalk {get;set;}

	void Awake()
	{
		m_anim = GetComponent<Animator>();
		m_target = transform.position;
		ShouldIdleWalk = true;
	}

	void GetClosestIsland()
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
			m_currentIsland = closest.GetComponentInChildren<PolygonCollider2D>();
		}
		else Debug.Log("Couldnt find an island :(");
	}

	void Update()
	{
		if(Random.Range(0, 400) == 1)
		{
			MoveToRandomPosition();
		}

		if((!Mathf.Approximately(transform.position.x, m_target.x) || !Mathf.Approximately(transform.position.y, m_target.y)) && ShouldIdleWalk) 
		{
			m_anim.SetBool("Walking", true);
			transform.position = Vector2.MoveTowards(transform.position, m_target, 0.02f);
		}
		else
		{
			m_anim.SetBool("Walking", false);
		}
	}

	void MoveToRandomPosition()
	{
		m_target = Vector2.zero;

		while(m_target == Vector2.zero || !m_currentIsland.OverlapPoint(m_target - new Vector2(0, collider2D.bounds.size.y / 2.0f)))
		{
			m_target = RandomPositionInCollider(m_currentIsland);
		}
	}

	Vector2 RandomPositionInCollider(Collider2D col)
	{
		return new Vector2(Random.Range(col.bounds.min.x, col.bounds.max.x), Random.Range(col.bounds.min.y, col.bounds.max.y));
	}
}

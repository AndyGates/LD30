using UnityEngine;
using System.Collections;

public class NPCIdle : NPCBase 
{
	public bool ShouldIdleWalk {get;set;}

	protected override void Awake ()
	{
		base.Awake ();
		ShouldIdleWalk = true;
	}

	protected override void Update ()
	{
		if(!ShouldIdleWalk) return;
		
		if(Random.Range(0, 400) == 1)
		{
			MoveToRandomPosition();
		}

		base.Update ();
	}

	void MoveToRandomPosition()
	{
		m_target = Vector2.zero;
		
		while(m_target == Vector2.zero || !m_currentIslandCollider.OverlapPoint(m_target - new Vector2(0, collider2D.bounds.size.y / 2.0f)))
		{
			m_target = RandomPositionInCollider(m_currentIslandCollider);
		}
	}
	
	Vector2 RandomPositionInCollider(Collider2D col)
	{
		return new Vector2(Random.Range(col.bounds.min.x, col.bounds.max.x), Random.Range(col.bounds.min.y, col.bounds.max.y));
	}
}

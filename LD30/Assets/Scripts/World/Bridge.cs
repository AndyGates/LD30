using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Bridge : MonoBehaviour {

	[SerializeField]
	GameObject m_bridgePart;

	[SerializeField]
	Island m_start;

	[SerializeField]
	Island m_end;

	public Island StartIsland {
		get { return m_start; }
		set { m_start = value; }
	}

	public Island EndIsland {
		get { return m_end; }
		set { m_end = value; }
	}

	Dictionary<Vector2, BridgePart> m_placedParts = new Dictionary<Vector2, BridgePart>();

	Vector2 m_lastPosition; 
	BridgePart m_lastPart;

	Vector2 m_preEnd;

	Vector2 m_startPosition;
	Vector2 m_endPosition;

	Vector2 m_startDirection; 
	Vector2 m_endDirection;

	public Vector2 LastPosition {
		get { return m_lastPosition; }
		set { m_lastPosition = value; }
	}

	public Vector2 StartPosition {
		get { return m_startPosition; }
		set { m_startPosition = value; }
	}

	public Action OnBridgeComplete {get;set;}

	// Use this for initialization
	void Start () {
		//FindClosestPath();
		//StartCoroutine(BuildBridge());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FindClosestPath()
	{
		int startIndex = 0; 
		int endIndex = 0;

		float prevBest = 1000;

		for(int i1 = 0; i1 < m_start.BridgePoints.Length; i1++)
		{
			for(int i2 = 0; i2 < m_end.BridgePoints.Length; i2++)
			{
				float dst = Vector2.Distance(m_start.BridgePoints[i1].position, m_end.BridgePoints[i2].position);
				if(dst < prevBest) 
				{
					prevBest = dst;
					startIndex = i1; 
					endIndex = i2;
				}
			}
		}

		Transform start = m_start.BridgePoints[startIndex];
		Transform end = m_end.BridgePoints[endIndex];

		m_startPosition = start.position;
		m_startDirection = start.right;

		m_endPosition = end.position;
		m_endDirection = end.right;

		m_preEnd = (m_endPosition + m_endDirection);
	}

	public void PlaceBridgePart()
	{
		if(m_placedParts.Count == 0)
		{
			AddBridgePart(m_startPosition);
		}
		else if(m_placedParts.Count == 1)
		{
			Vector2 startOffset = m_startDirection;
			AddBridgePart(m_lastPosition + startOffset); 
		}
		else if(NearEnough(m_lastPosition, m_preEnd))
		{		
			AddLastBridgePart();
		}
		else
		{
			Vector2 nextPosition = GetNextBuildPosition();
			AddBridgePart(nextPosition);
		}
	}

	IEnumerator BuildBridge()
	{
		//Debug.Log(string.Format("{0} -> {1}", m_startPosition, m_endPosition));

		AddBridgePart(m_startPosition);
		Vector2 startOffset = m_startDirection;
		AddBridgePart(m_lastPosition + startOffset); 

		m_preEnd = (m_endPosition + m_endDirection);
		while(!NearEnough(m_lastPosition, m_preEnd))
		{
			//Debug.Log(string.Format("Last: {0} End: {1} Equals: {2}", m_lastPosition, m_preEnd, NearEnough(m_lastPosition, m_preEnd)));

			Vector2 nextPosition = GetNextBuildPosition();
			yield return new WaitForSeconds(1.0f);
			AddBridgePart(nextPosition);
		}

		yield return new WaitForSeconds(1.0f);
		AddLastBridgePart();
	}

	void AddLastBridgePart()
	{
		AddBridgePart(GetNextBuildPosition());
		m_end.Capture();
		if(OnBridgeComplete != null) OnBridgeComplete();
	}

	Vector2 GetNextBuildPosition()
	{
		bool last = NearEnough(m_lastPosition, m_preEnd);
		Vector2 diff = (last ? m_endPosition : m_preEnd) - m_lastPosition;
		if(Mathf.Abs(diff.x) >= Mathf.Abs(diff.y))
		{
			return m_lastPosition + new Vector2(diff.x > 0.1f ? 1 : -1, 0);
		}
		else 
		{
			return m_lastPosition + new Vector2(0, diff.y > 0.1f ? 1 : -1);
		}

	}

	void AddBridgePart(Vector2 position, float scale = 1.0f)
	{
		GameObject go = Instantiate(m_bridgePart, position, Quaternion.identity) as GameObject;
		go.transform.parent = transform;
		BridgePart b = go.GetComponent<BridgePart>();

		Vector2 diff = position - m_lastPosition;
		if(m_lastPart != null)
		{
			//Added to left
			if(diff.x < -0.1f)
			{
				b.Right = m_lastPart;
				m_lastPart.Left = b;
			}
			//Added to right
			else if(diff.x > 0.1f)
			{
				b.Left = m_lastPart;
				m_lastPart.Right = b;
			}
			//Added to top
			else if(diff.y > 0.1f)
			{
				b.Down = m_lastPart;
				m_lastPart.Up = b;
			}
			//Added to bottom
			else if(diff.y < -0.1f)
			{
				b.Up = m_lastPart;
				m_lastPart.Down = b;
			}

			b.UpdateType();
			m_lastPart.UpdateType();
		}

		if(!m_placedParts.ContainsKey(position))m_placedParts.Add(position, b);
		m_lastPart = b;
		m_lastPosition = position;

	}

	bool NearEnough(Vector2 a, Vector2 b)
	{
		Vector2 diff = a - b;
		return (Mathf.Abs(diff.x) < 1.0f && Mathf.Abs(diff.y) < 1.0f);
	}
}

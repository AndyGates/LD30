using UnityEngine;
using System.Collections;

public class Island : MonoBehaviour {

	public enum AccessoryType
	{
		None,
		Cave, 
		House
	}

	[System.Serializable]
	public class AccessoryPoint
	{
		public Transform Position; 
		public AccessoryType StartingAccessory;
		public GameObject Accessory {get;set;}
	}

	[SerializeField]
	Transform[] m_bridgePoints;

	[SerializeField]
	GameObject m_housePrefab;

	[SerializeField]
	GameObject m_cavePrefab;

	[SerializeField]
	GameObject m_bridgePrefab;

	[SerializeField]
	bool m_isFinal; 

	[SerializeField]
	bool m_isFirst; 

	bool m_captured = false;
	
	public bool IsFinal { get { return m_isFinal; } }
	public Bridge ExitBridge {get;set;}

	public Transform[] BridgePoints { get { return m_bridgePoints; } }

	[SerializeField]
	AccessoryPoint[] m_accessoryPoints;

	[SerializeField]
	Island m_nextIsland; 

	[SerializeField]
	Island[] m_islandsToActivate;
	
	void Start()
	{
		if(m_isFirst) Capture();
	}

	void Activate () 
	{
		foreach(AccessoryPoint ap in m_accessoryPoints)
		{
			if(ap.StartingAccessory != AccessoryType.None)
			{
				SpawnAccessory(ap, ap.StartingAccessory);
			}
		}
	}

	public Bridge CreateBridgeToNext()
	{
		GameObject brGO = Instantiate(m_bridgePrefab) as GameObject;
		brGO.name = gameObject.name + "Bridge"; 
		Bridge br = brGO.GetComponent<Bridge>();
		br.StartIsland = this;
		br.EndIsland = m_nextIsland;
		br.FindClosestPath();
		ExitBridge = br;
		return br;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Capture()
	{
		if(!m_captured)
		{
			m_captured = true;
			StartCoroutine(CaptureIsland());
		}
	}

	IEnumerator CaptureIsland()
	{
		foreach(Island i in m_islandsToActivate)
		{
			i.Activate();
		}

		foreach(EnemySpawner e in GetComponentsInChildren<EnemySpawner>())
		{
			e.Remove();
		}

		yield return new WaitForSeconds(0.8f);

		foreach(AccessoryPoint p in m_accessoryPoints)
		{
			if(p.Accessory == null) SpawnAccessory(p, AccessoryType.House);
		}
	}

	public void SpawnAccessory(AccessoryPoint point, AccessoryType accessory)
	{
		GameObject pref = GetAccessoryPrefab(accessory);

		GameObject go = Instantiate(pref, Vector2.zero, Quaternion.identity) as GameObject;
		go.transform.parent = point.Position;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;

		point.Accessory = go;
	}

	GameObject GetAccessoryPrefab(AccessoryType accessory)
	{
		return (accessory == AccessoryType.Cave ? m_cavePrefab : m_housePrefab);
	}

	public void SpawnHouse()
	{

	}

	public void SpawnCave()
	{

	}

}

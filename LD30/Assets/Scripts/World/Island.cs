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

	public bool IsFinal { get { return m_isFinal; } }

	public Transform[] BridgePoints { get { return m_bridgePoints; } }

	[SerializeField]
	AccessoryPoint[] m_accessoryPoints;

	[SerializeField]
	Island m_nextIsland; 

	// Use this for initialization
	void Start () 
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

		Bridge br = brGO.GetComponent<Bridge>();
		br.StartIsland = this;
		br.EndIsland = m_nextIsland;
		br.FindClosestPath();
		return br;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Capture()
	{
		StartCoroutine(CaptureIsland());
	}

	IEnumerator CaptureIsland()
	{
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

using UnityEngine;
using System.Collections;

public class Builder : MonoBehaviour {

	NPCIdle m_idle;

	public Bridge CurrentBridge {get;set;}

	// Use this for initialization
	void Start () {
		m_idle = GetComponent<NPCIdle>();
		m_idle.ShouldIdleWalk = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

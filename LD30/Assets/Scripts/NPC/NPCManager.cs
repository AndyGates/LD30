using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class NPCManager : SingletonBehaviour<NPCManager> 
{
	[SerializeField]
	Text m_text;

	public int NPCs { get;private set; }
	public int MaxNPCs { get; private set; }

	List<NPCIdle> m_npcs = new List<NPCIdle>(); 
	public NPCBuilder Builder {get;set;}
	public Bridge CurrentBridge {get;set;}

	// Use this for initialization
	void Start () 
	{
		m_npcs = GetComponentsInChildren<NPCIdle>().ToList();
		MaxNPCs = m_npcs.Count;
		NPCs = MaxNPCs;
		SpawnBuilder();
	}

	void UpdateText()
	{
		m_text.text = string.Format("PEOPLE REMAINING: {0}/{1}", MaxNPCs, NPCs); 
	}
		
	public void SpawnBuilder()
	{
		NPCIdle idle = m_npcs.Last();
		m_npcs.Remove(idle);

		GameObject go = idle.gameObject;
		Destroy(idle);

		Builder = go.AddComponent<NPCBuilder>();
		Builder.OnDeath += OnBuilderDeath;
	}

	public void OnBuilderDeath()
	{
		Builder.OnDeath -= OnBuilderDeath;
		Destroy(Builder.gameObject, 0.6f);

		NPCs--;
		UpdateText();
		SpawnBuilder();
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;

public class NPCManager : SingletonBehaviour<NPCManager> 
{
	[SerializeField]
	Text m_text;

	[SerializeField]
	GameObject m_win; 

	[SerializeField]
	Text m_winText;

	[SerializeField]
	GameObject m_initialText;

	[SerializeField]
	GameObject m_lose;

	public int NPCs { get;private set; }
	public int MaxNPCs { get; private set; }

	List<NPCIdle> m_npcs = new List<NPCIdle>(); 
	public NPCBuilder Builder {get;set;}

	public Island CurrentIsland {get;set;}

	public bool StartScreen { get { return m_startScreen; } }

	bool m_startScreen = true;
	bool m_gameOver = false;
	DateTime m_gameOverTime; 

	// Use this for initialization
	void Start () 
	{
		Time.timeScale = 0;

		//Had to cut :(
		m_npcs = GetComponentsInChildren<NPCIdle>().ToList();

		MaxNPCs = m_npcs.Count;
		//MaxNPCs = 1;
		NPCs = MaxNPCs;

		UpdateText();
	}

	void StartGame()
	{
		m_startScreen = false;
		m_initialText.SetActive(false);
		Time.timeScale = 1;
		SpawnBuilder();
	}

	void Restart()
	{
		Application.LoadLevel("Main");
	}

	void UpdateText()
	{
		m_text.text = string.Format("PEOPLE REMAINING: {0}/{1}", NPCs, MaxNPCs); 
	}

	void Update()
	{
		if(m_startScreen && Input.anyKey)
		{
			StartGame();
		}
		else if(m_gameOver && (DateTime.UtcNow - m_gameOverTime).TotalSeconds > 3 && Input.anyKey)
		{
			Restart();
		}
	}
		
	public void SpawnBuilder()
	{
		NPCIdle idle = m_npcs.Last();
		m_npcs.Remove(idle);

		GameObject go = idle.gameObject;
		Destroy(idle);

		Builder = go.AddComponent<NPCBuilder>();
		Builder.OnDeath += OnBuilderDeath;
		Builder.OnReachedEnd += OnReachedEnd;
		Builder.OnReachedIsland += OnReachedIsland;
	}

	public void OnBuilderDeath()
	{
		Builder.OnDeath -= OnBuilderDeath;
		Builder.OnReachedEnd -= OnReachedEnd;
		Builder.OnReachedIsland -= OnReachedIsland;

		Destroy(Builder.gameObject, 0.6f);

		NPCs--;
		UpdateText();

		if(NPCs > 0)
		{
			SpawnBuilder();
		}
		else
		{
			OnDied();
		}
	}

	public void OnDied()
	{
		Destroy(Builder.gameObject);
		Time.timeScale = 0;
		m_lose.SetActive(true);
		m_gameOver = true;
		m_gameOverTime = DateTime.UtcNow;
	}

	public void OnReachedEnd()
	{
		Builder.enabled = false;
		//Time.timeScale = 0;

		Island[] allIslands = FindObjectsOfType<Island>();
		foreach(Island i in allIslands)
			i.Capture();

		Enemy[] allEnemies = FindObjectsOfType<Enemy>();
		foreach(Enemy e in allEnemies)
			e.TakeDamage(2);

		m_winText.text = string.Format("You saved {0}/{1} people", NPCs, MaxNPCs);
		m_win.SetActive(true);
		m_gameOver = true;
		m_gameOverTime = DateTime.UtcNow;
	}

	public void OnReachedIsland()
	{
		foreach(NPCIdle i in m_npcs)
		{
			i.MoveToCurrentIsland();
		}
	}
}

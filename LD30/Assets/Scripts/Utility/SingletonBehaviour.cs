using UnityEngine;
using System.Collections;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T m_instance = null;
	public static T Instance
	{ 
		get
		{
			if (m_instance == null)
			{
				m_instance = FindObjectOfType(typeof(T)) as T;
				if( m_instance == null )
				{
					GameObject go = new GameObject( typeof(T).Name + " (Singleton)", typeof( T ) );
					m_instance = go.GetComponent<T>();
				}
			}
			return m_instance; 
		} 
	}
}

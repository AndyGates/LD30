using UnityEngine;
using System.Collections;

public class PlayerProjectile : MonoBehaviour {

	[SerializeField]
	int m_damage;
		
	public int Damage {get { return m_damage; }}
}

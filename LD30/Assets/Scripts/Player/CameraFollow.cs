using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	[SerializeField]
	Transform m_follow;
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = Vector3.Slerp(transform.localPosition, new Vector3(m_follow.localPosition.x, m_follow.localPosition.y, transform.localPosition.z), 0.1f); 	
	}
}

using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	[SerializeField]
	Transform m_follow;

	[SerializeField]
	float m_followHeight = 10;

	Vector3 FollowPosition
	{
		get { return new Vector3(m_follow.localPosition.x, m_follow.localPosition.y, m_follow.localPosition.z - m_followHeight); }
	}

	void Start()
	{
		transform.localPosition = FollowPosition;
	}

	// Update is called once per frame
	void Update () {
		transform.localPosition = Vector3.Slerp(transform.localPosition, FollowPosition, 0.1f); 	
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	[SerializeField]
	Image m_image;

	[SerializeField]
	Color m_fullColour;

	[SerializeField]
	Color m_emptyColour; 

	void Awake()
	{
		m_image.color = m_fullColour;
	}
	
	public void SetHealth(float health)
	{
		Vector3 newScale = m_image.rectTransform.localScale;
		newScale.x = Mathf.Clamp01(health);
		m_image.rectTransform.localScale = newScale;

		m_image.color = Color.Lerp(m_emptyColour, m_fullColour, Mathf.Clamp01(health - 0.25f));
	}
}

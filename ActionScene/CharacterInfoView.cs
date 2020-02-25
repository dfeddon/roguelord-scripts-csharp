using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfoView : MonoBehaviour
{
	private GameObject panel;

	void Awake()
	{
		panel = transform.Find("Panel").gameObject;
	}
	void Start()
	{

	}

	public void UpdateCharacterInfo(CharacterVO vo, int state)
	{
		// state: 1=enter, 0=exit

		if (vo == null) return;
		
		// update stats
		panel.transform.Find("Handle").gameObject.GetComponent<TextMeshProUGUI>().text = vo.fullname;

		// if mouse exit, default to active character
		if (state == 0)
		{

		}
	}
}
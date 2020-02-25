using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityHistoryView : MonoBehaviour
{
	private GameObject attackerHistory;
	private GameObject defenderHistory;

	void Awake()
	{
		attackerHistory = GameObject.FindWithTag("AttackerHistory");
		defenderHistory = GameObject.FindWithTag("DefenderHistory");
	}
	void Start()
	{

	}

	public void UpdateHistoryPanel(CharacterVO vo, int state)
	{
		// state: 1=enter, 0=exit

		if (vo == null) return;
		// if (state == 1 && vo == null) return;
		// if (state == 0 && turnCharacter != null)
		// 	vo = turnCharacter;

		Image img;
		AbilityVO a;
		float opacity = 0f;
		for (int i = 0; i < 5; i++)
		{
			if (i < vo.abilityHistory.Count)// - 1)
			{
				switch (i + 1)
				{
					case 1: opacity = 1.00f; break;
					case 2: opacity = 0.80f; break;
					case 3: opacity = 0.60f; break;
					case 4: opacity = 0.40f; break;
					case 5: opacity = 0.15f; break;
				}
				img = transform.GetChild(i).gameObject.GetComponent<Image>();
				img.sprite = GameManager.instance.assetBundleCombat.LoadAsset<Sprite>(vo.abilityHistory[i]);
				img.color = new Color(img.color.r, img.color.g, img.color.b, opacity);
			}
			else
			{
				img = transform.GetChild(i).gameObject.GetComponent<Image>();
				img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
				img.sprite = null;
			}
		}

		// if mouse exit, default to active character
		if (state == 0)
		{

		}
	}
}
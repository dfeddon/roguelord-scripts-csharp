using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardSODisplay: MonoBehaviour
{
	public CardSO card;

	public TextMeshProUGUI nameText;
	public TextMeshProUGUI descriptionText;

	public Image imageImage;

	public TextMeshProUGUI costText;
	public TextMeshProUGUI attackText;
	public TextMeshProUGUI defenseText;

	void Start()
	{
		nameText.text = card.name;
		descriptionText.text = card.description;

		imageImage.sprite = card.image;

		costText.text = card.cost.ToString();
		attackText.text = card.attack.ToString();
		defenseText.text = card.defense.ToString();
	}
}
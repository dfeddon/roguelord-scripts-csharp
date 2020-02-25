using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IncidentCard : MonoBehaviour
{

	public Button buttonComponent;
	public Text descriptionLabel;
	public Image iconImage;
	public Text rewardText;


	private Item item;
	private IncidentScrollList scrollList;

	// Use this for initialization
	void Start()
	{
		buttonComponent.onClick.AddListener(HandleClick);
	}

	public void Setup(Item currentItem, IncidentScrollList currentScrollList)
	{
		item = currentItem;
		descriptionLabel.text = item.itemName;
		if (item.icon)
			iconImage.sprite = item.icon;
		rewardText.text = item.price.ToString();
		scrollList = currentScrollList;

	}

	public void HandleClick()
	{
		EventParam eventParam = new EventParam();
		eventParam.data = "card1";
		Debug.Log("* incident card clicked " + descriptionLabel.text);
		EventManager.TriggerEvent("incidentClickEvent", eventParam);
		// scrollList.TryTransferItemToOtherShop(item);
	}
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Item
{
	public string itemName;
	public Sprite icon;
	public float price = 1;
}

public class IncidentScrollList : MonoBehaviour
{

	public List<Item> itemList;
	public Transform contentPanel;
	public IncidentScrollList otherShop;
	public Text myGoldDisplay;
	public SimpleObjectPool incidentObjectPool;
	public GameObject gameManager;

	public float gold = 20f;

	void Awake()
	{
		// if (GameManager.instance == null)
		// 	Instantiate(gameManager);
		// GameManager.instance.incidentScrollList = this;

	}
	// Use this for initialization
	void Start()
	{
		GameManager.instance.incidentScrollList = this;
		// Item item1 = new Item();
		// item1.itemName = "Item #1";
		// item1.price = 200;
		// itemList.Add(item1);
		// Debug.Log("start " + itemList.Count);// + "/" + gameManager.instance;
		// AddItem(item1, itemList);
		RefreshDisplay();
	}

	void RefreshDisplay()
	{
		// myGoldDisplay.text = "Gold: " + gold.ToString();
		RemoveButtons();
		AddButtons();
	}

	private void RemoveButtons()
	{
		while (contentPanel.childCount > 0)
		{
			GameObject toRemove = transform.GetChild(0).gameObject;
			incidentObjectPool.ReturnObject(toRemove);
		}
	}

	private void AddButtons()
	{
		Debug.Log("AddButtons");
		Debug.Log(itemList.Count);
		for (int i = 0; i < itemList.Count; i++)
		{
			Item item = itemList[i];
			GameObject newButton = incidentObjectPool.GetObject();
			newButton.transform.SetParent(contentPanel);

			IncidentCard sampleButton = newButton.GetComponent<IncidentCard>();
			sampleButton.Setup(item, this);
		}
	}

	public void TryTransferItemToOtherShop(Item item)
	{
		if (otherShop.gold >= item.price)
		{
			gold += item.price;
			otherShop.gold -= item.price;

			// AddItem(item, otherShop);
			RemoveItem(item, this);

			RefreshDisplay();
			otherShop.RefreshDisplay();
			Debug.Log("enough gold");

		}
		Debug.Log("attempted");
	}

	public void AddItem(Item itemToAdd)//, IncidentScrollList shopList)
	{
		Debug.Log("== IncidentScrollList.addItem == " + itemToAdd);
		// this.itemList.Add(itemToAdd);
		// add item to beginning of array
		this.itemList.Insert(0, itemToAdd);
		RefreshDisplay();
	}

	private void RemoveItem(Item itemToRemove, IncidentScrollList shopList)
	{
		for (int i = shopList.itemList.Count - 1; i >= 0; i--)
		{
			if (shopList.itemList[i] == itemToRemove)
			{
				shopList.itemList.RemoveAt(i);
			}
		}
	}
}
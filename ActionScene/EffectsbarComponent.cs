using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class EffectsbarComponent : MonoBehaviour
{
	// public Slider slider;
	// public int healthMax;
	// public int health;
	private GameObject panel;
	private GameObject condi1;
	private GameObject condi1Stack;
	private TextMeshProUGUI condi1StackLabel;
	private GameObject condi2;

	private GameObject condi2Stack;
	private TextMeshProUGUI condi2StackLabel;
	private GameObject cc1;
	private GameObject cc1Stack;
	private TextMeshProUGUI cc1StackLabel;
	private GameObject cc2;
	private GameObject cc2Stack;
	private TextMeshProUGUI cc2StackLabel;
	private GameObject boon1;
	private GameObject boon1Stack;
	private TextMeshProUGUI boon1StackLabel;
	private GameObject boon2;
	private GameObject boon2Stack;
	private TextMeshProUGUI boon2StackLabel;

	void Start()
	{
		// slider = gameObject.GetComponent<Slider>();
		// slider.wholeNumbers = true;
		panel = transform.GetChild(0).gameObject;
		// Debug.Log("* got panel " + panel);

		// panel.transform.Find("condi-1")

		Transform[] allChildren = panel.GetComponentsInChildren<Transform>();
		int count = 1;

		// assign slot refs
		foreach (Transform child in allChildren)
		{
			// only track slots, not slot children
			if (child.GetComponent<EffectSlotComponent>())
			{
				// Debug.Log("<color=orange>" + count + " / " + child + "</color>");
				switch(count)
				{
					case 1: 
						condi1 = child.gameObject; 
						condi1Stack = child.gameObject.transform.GetChild(0).gameObject;
						condi1StackLabel = condi1Stack.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
					break;

					case 2: 
						condi2 = child.gameObject; 
						condi2Stack = child.gameObject.transform.GetChild(0).gameObject;
						condi2StackLabel = condi2Stack.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
					break;

					case 3: 
						cc1 = child.gameObject; 
						cc1Stack = child.gameObject.transform.GetChild(0).gameObject;
						cc1StackLabel = cc1Stack.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
					break;

					case 4: 
						cc2 = child.gameObject; 
						cc2Stack = child.gameObject.transform.GetChild(0).gameObject;
						cc2StackLabel = cc2Stack.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
					break;

					case 5: 
						boon1 = child.gameObject; 
						boon1Stack = child.gameObject.transform.GetChild(0).gameObject;
						boon1StackLabel = boon1Stack.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
					break;

					case 6: 
						boon2 = child.gameObject; 
						boon2Stack = child.gameObject.transform.GetChild(0).gameObject;
						boon2StackLabel = boon2Stack.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
					break;
				}
				count++;
			}
		}
	}
	private Sprite GetSpriteByImageString(string image)
	{
		return GameManager.instance.assetBundleCombat.LoadAsset<Sprite>(image);
	}
	public GameObject GetSlotByModifier(int slot, int modifier)
	{
		GameObject go = null;

		switch(modifier)
		{
			case EffectVO.EFFECT_MODIFIER_CONDITION:
				if (slot == 1)
					go = condi1;
				else go = condi2;
			break;

			case EffectVO.EFFECT_MODIFIER_CC:
				if (slot == 1)
					go = cc1;
				else go = cc2;
			break;

			case EffectVO.EFFECT_MODIFIER_BOON:
				if (slot == 1)
					go = boon1;
				else go = boon1;
			break;	
		}

		return go;
	}
	public GameObject GetStackByModifier(int slot, int modifier)
	{
		GameObject go = null;

		switch (modifier)
		{
			case EffectVO.EFFECT_MODIFIER_CONDITION:
				if (slot == 1)
					go = condi1Stack;
				else go = condi2Stack;
				break;

			case EffectVO.EFFECT_MODIFIER_CC:
				if (slot == 1)
					go = cc1Stack;
				else go = cc2Stack;
				break;

			case EffectVO.EFFECT_MODIFIER_BOON:
				if (slot == 1)
					go = boon1Stack;
				else go = boon2Stack;
				break;
		}

		return go;
	}
	public TextMeshProUGUI GetStackLabelByModifier(int slot, int modifier)
	{
		TextMeshProUGUI go = null;

		switch (modifier)
		{
			case EffectVO.EFFECT_MODIFIER_CONDITION:
				if (slot == 1)
					go = condi1StackLabel;
				else go = condi2StackLabel;
				break;

			case EffectVO.EFFECT_MODIFIER_CC:
				if (slot == 1)
					go = cc1StackLabel;
				else go = cc2StackLabel;
				break;

			case EffectVO.EFFECT_MODIFIER_BOON:
				if (slot == 1)
					go = boon1StackLabel;
				else go = boon1StackLabel;
				break;
		}

		return go;
	}
	public void UpdateEffectsBar(int type, EffectVO vo, List<EffectVO> list)
	{
		// Debug.Log("<color=orange>== EffectsbarComponent.UpdateEffectsBar ==</color>");
		// Debug.Log("<color=orange>type " + type + " / " + list.Count + "</color>");

		GameObject slot1 = GetSlotByModifier(1, vo.effectModifier);
		GameObject slot2 = GetSlotByModifier(2, vo.effectModifier);
		GameObject stack1 = GetStackByModifier(1, vo.effectModifier);
		GameObject stack2 = GetStackByModifier(2, vo.effectModifier);
		TextMeshProUGUI stack1Label = GetStackLabelByModifier(1, vo.effectModifier);
		TextMeshProUGUI stack2Label = GetStackLabelByModifier(2, vo.effectModifier);
		EffectSlotComponent effectComponent1 = slot1.GetComponent<EffectSlotComponent>();
		EffectSlotComponent effectComponent2 = slot2.GetComponent<EffectSlotComponent>();

		Image img1;
		Image img2;
		GameObject stackView1;
		GameObject stackView2;

		if (list.Count > 0)
		{
			img1 = slot1.GetComponent<Image>();
			// stackView1 = slot1.transform.GetChild(0);

			// if (slot1.GetComponent<Image>().color.a == 0)
			effectComponent1.effect = list[0];

			switch(type)
			{
				case EffectVO.EFFECT_UPDATE_TYPE_ADD:
					img1.color = new Color(img1.color.r, img1.color.g, img1.color.b, 1f);
					img1.sprite = GetSpriteByImageString(list[0].image);
				break;

				case EffectVO.EFFECT_UPDATE_TYPE_REMOVE:
					img1.color = new Color(img1.color.r, img1.color.g, img1.color.b, 0f);
					img1.sprite = null;
					stack1.SetActive(false);
				break;

				case EffectVO.EFFECT_UPDATE_TYPE_UPDATE:

				break;
			}

			int activeStacks = list[0].GetNumberOfActiveStacks(true);
			Debug.Log("<color=orange>" + list[0].label + " effectStacks.Count " + activeStacks + "</color>");
			// if (list[0].effectStacks.Count > 1)
			if (activeStacks > 1)
			{
				stack1.SetActive(true);
				// stack1Label.gameObject.SetActive(true);
				stack1Label.text = activeStacks.ToString();
			}
			else 
			{
				stack1.SetActive(false);
				// stack1Label.gameObject.SetActive(false);	
			}

			// if only 1 stack, hide stack 2
			if (list.Count == 1)
				stack2.SetActive(false);
		}
		if (list.Count == 2)
		{
			img2 = slot2.GetComponent<Image>();

			// if (slot2.GetComponent<Image>().color.a == 0)
			img2.color = new Color(img2.color.r, img2.color.g, img2.color.b, 1f);
			effectComponent2.effect = list[1];
			img2.sprite = GetSpriteByImageString(list[1].image);

			if (list[1].effectStacks.Count > 1)
			{
				stack2.SetActive(true);
				// stack2Label.gameObject.SetActive(true);
				stack2Label.text = list[1].effectStacks.Count.ToString();
			}
			else
			{ 
				stack2.SetActive(false);
				// stack2Label.gameObject.SetActive(false);
			}
		}
		if (list.Count == 0) // hide all stacks
		{
			stack1.SetActive(false);
			stack2.SetActive(false);
		}
	}

}
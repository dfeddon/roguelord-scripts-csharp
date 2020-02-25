using UnityEngine;
using System.Collections;
// using System.Reflection;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AbilityTooltipView : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
{
	private AbilityVO _ability;
	private GameObject mainView;
	private GameObject modLockedView;
	public Roguelord.UIController uIController;

	// Start is called before the first frame update
	void Awake()
	{
		mainView = transform.GetChild(0).gameObject;
		modLockedView = transform.GetChild(1).gameObject;

		// hide on startup
		mainView.SetActive(false);
		modLockedView.SetActive(false);
	}

	private void Start() 
	{
		modLockedView.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => UnlockModHandler());
	}

	void MainView()
	{
	}
	void ModLockedView()
	{
		Debug.Log("* ModLockedView");
		modLockedView.SetActive(true);

		TextMeshProUGUI header = modLockedView.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
		TextMeshProUGUI desc = modLockedView.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
		Button unlockButton = modLockedView.transform.GetChild(0).GetComponent<Button>();
		string headerTxt = (ability.uid == 1001) ? "Weapon Mod" : "Gear Mod";
		string descTxt;
		int powerBankA = GameManager.instance.uiController.powerBankA;
		Color color;
		if (ability.uid == 1001)
		{
			if (powerBankA >= 4)
			{
				descTxt = "Add weapon modifications here. This slot is currently locked. Select the Unlock Now button and pay the required crypto cost.";
				ColorUtility.TryParseHtmlString("#67FA73", out color);
				unlockButton.enabled = true;
			}
			else
			{ 
				descTxt = "Not enough funds...";
				unlockButton.enabled = false;
				color = Color.red;
			}
			unlockButton.image.color = color;
		}
		else {
			if (powerBankA >= 3)
			{
				descTxt = "Add gear modifications here. This slot is currently locked. Select the Unlock Now button and pay the required crypto cost.";
				ColorUtility.TryParseHtmlString("#67FA73", out color);
				unlockButton.enabled = true;
			}
			else 
			{
				descTxt = "Not enough funds...";
				unlockButton.enabled = false;
				color = Color.red;
			}
			unlockButton.image.color = color;
		}
		header.text = headerTxt;
		desc.text = descTxt;
	}

	private void UnlockModHandler()
	{
		uIController.UnlockModHandler(ability);
	}

	public void OnPointerEnter(PointerEventData e)
	{
		Debug.Log("* AbilityPanel Enter Handler");// + abilityVO.name);
		// EventParam eventParam = new EventParam();
		// eventParam.value = abilityNumber;//characterView.model;
		// eventParam.name = "abilityEnter";
		// eventParam.data = abilityVO;
		// EventManager.TriggerEvent("combatEvent", eventParam);
	}

	public void OnPointerExit(PointerEventData e)
	{
		Debug.Log("* AbilityPanel Exit");
		if (e.pointerEnter.name == "Image")
			Debug.Log("IMAGE!");
		else Debug.Log("OUT?");
		// EventParam eventParam = new EventParam();
		// eventParam.value = abilityNumber;//characterView.model;
		// eventParam.name = "abilityExit";
		// eventParam.data = abilityVO;
		// EventManager.TriggerEvent("combatEvent", eventParam);
	}

	// getters & setters
	public AbilityVO ability
	{
		get { return _ability; }
		set
		{
			_ability = value;
			if (value != null && value.uid > 1000)
				ModLockedView();
			else MainView();
		}
	}

}
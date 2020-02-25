using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class SprawlHUD : MonoBehaviour
{
	private Action<EventParam> cellClickHandler;
	private TGS.SprawlController sprawlController;
	private Button droneButton;
	private Button lookoutButton;

	void Awake()
	{
		Debug.Log("== SprawlHUD.Awake ==");

		// cellClickHandler = new UnityAction(cellClickHandlerFunction);
		// cellClickHandler = new UnityAction<EventParam>(cellClickHandlerFunction);
	}

	void Start()
	{
		// EventManager.StartListening("cellClickEvent", cellClickHandler);

		sprawlController = GetComponent<TGS.SprawlController>();

		droneButton = GameObject.Find("Button_ActionButton").GetComponent<Button>();
		lookoutButton = GameObject.Find("Button_ActionButton2").GetComponent<Button>();

		// listeners
		droneButton.onClick.AddListener(droneButtonHandler);
		lookoutButton.onClick.AddListener(lookoutButtonHandler);
	}

	void Update()
	{

	}

	void cellClickHandlerFunction(EventParam eventParam)
	{
		Debug.Log("== SprawlHUD.cellClickHandlerFunction ==");
		Debug.Log("* eventParam " + eventParam.data);
		Debug.Log("* index selected " + GameManager.instance.currentSelectedIndex);

		// populate map info UI
		Text sectorText = GameObject.Find("SectorText").GetComponent<Text>();
		sectorText.text = "Sector " + GameManager.instance.currentSelectedIndex.ToString();

		Button droneButton = GameObject.Find("Button_ActionButton").GetComponent<Button>();
		droneButton.GetComponentInChildren<Text>().text = "Drone!";
		Button lookoutButton = GameObject.Find("Button_ActionButton2").GetComponent<Button>();
		lookoutButton.GetComponentInChildren<Text>().text = "Lookout!";

		EventParam evt = new EventParam();
		evt.data = "hi";
		EventManager.TriggerEvent("cellClickBubbleEvent", evt);
	}

	void droneButtonHandler()
	{
		Debug.Log("* droneButtonHandler " + GameManager.instance.sprawlController);
		GameManager.instance.sprawlController.AddDroneToMap();
	}

	void lookoutButtonHandler()
	{
		Debug.Log("* lookoutButtonHandler");
		GameManager.instance.sprawlController.AddLookoutToMap();
	}
}
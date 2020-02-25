using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// namespace System
// {
// 	public delegate void Action();
// }
public class LobbyController : MonoBehaviour
{
	public Button startCombatButton;
	private UnityAction<EventParam> cellClickHandler;
	private UnityAction<EventParam> incidentClickHandler;
	public GameObject gameManager;
	// public PubNubService pubNubService;

	void Awake() 
	{
		// pubnub service
		// pubNubService = new PubNubService();
		// pubNubService.SubscribeChannel();
		// mongodb stitch
		// MongoStitchService db = idleWindow.AddComponent<MongoStitchService>();

		// cellClickHandler = new Action(cellClickHandlerFunction);
		// cellClickHandler = new Action(cellClickHandlerFunction);
		// incidentClickHandler = new Action(incidentClickHandlerFunction);

		cellClickHandler = new UnityAction<EventParam>(cellClickHandlerFunction);
		incidentClickHandler = new UnityAction<EventParam>(incidentClickHandlerFunction);

	}
	// Use this for initialization
	void Start()
	{
		Debug.Log("== LobbyController.Start ==");

		//Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
		if (GameManager.instance == null)
		{
			//Instantiate gameManager prefab
			Instantiate(gameManager);
		}
		Debug.Log("* gameManager " + GameManager.instance);
		// startCombatButton = GameObject.FindWithTag("ShootButton") as Button;
		// startCombatButton.onClick.AddListener(() => 
		// {
		// 	StartCombatHandler();
		// });

		EventManager.StartListening("cellClickBubbleEvent", cellClickHandler);
		EventManager.StartListening("incidentClickEvent", incidentClickHandler);

		// someListener1 = new Action<EventParam>(SomeFunction);

	}

	// Update is called once per frame
	void Update()
	{

	}

	void onEnable()
	{
		Debug.Log("* LobbyController.onEnable...");
		// EventManager.StartListening("test", cellClickHandler);
	}

	void incidentClickHandlerFunction(EventParam eventParam)
	{
		Debug.Log("== LobbyController.incidentClickHandler ==");
		Debug.Log("data " + eventParam.data);
	}

	void cellClickHandlerFunction(EventParam eventParam)
	{
		Debug.Log("== LobbyController.cellClickHandlerFunction ==");
		Debug.Log("data " + eventParam);
		Debug.Log("* index selected " + GameManager.instance.currentSelectedIndex);
		Item item1 = new Item();
		item1.itemName = "Sensors alerted near Sinjun Corps red tower just outside 'Frisco Sprawl. Too-tall Redline Hackers suspected.";
		item1.price = GameManager.instance.currentSelectedIndex;
		GameManager.instance.incidentScrollList.AddItem(item1);
	}

	void StartCombatHandler()
	{
		Debug.Log("* start combat...");
		StartCoroutine(LoadNewScene());
	}

	IEnumerator LoadNewScene()
	{
		AsyncOperation async = SceneManager.LoadSceneAsync("ActionScene", LoadSceneMode.Single);
		while (!async.isDone)
		{
			Debug.Log("loading...");
			yield return null;
		}
	}
}

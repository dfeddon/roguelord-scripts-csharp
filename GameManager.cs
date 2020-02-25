using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Guirao.UltimateTextDamage;

public class GameManager : MonoBehaviour {


	public static GameManager instance = null; //Static instance of GameManager which allows it to be accessed by any other script.
	// private BoardManager boardScript; //Store a reference to our BoardManager which will set up the level.
	public TGS.SprawlController sprawlController;
	public Missions.Missions_MapGenerator missions_MapGenerator;
	private int level = 3; //Current level number, expressed in game as "Day 1".
	public IncidentScrollList incidentScrollList;

	/////////////////////////////////////////////
	
	public CinecamController cinecamController;
	public AbilityVO currentAbility;
	public AssetBundle assetBundleCombat;
	public AssetBundle assetBundleCards;
	public CombatCinematicController combatCinematicController;
	public FloorController floorController;
	public UltimateTextDamageManager ultimateTextDamageManager;
	public Roguelord.UIController uiController;
	public Roguelord.CardController cardController;
	public int currentPhaseTotal;

	public int currentSelectedIndex;

	public List<CharacterVO> allcharacters;// = new List<CharacterVO>();
	public int combatActionState = SceneActionMain.COMBAT_ACTION_STATE_INACTIVE;
	public bool roundIsLocked = false;
	// public bool movementIsLocked = false;
	public IEnabler activeCameraFilter = null;
	public GameSparksService gameSparksService;
	
	void Awake()
	{
		Debug.Log("== GameManager.Awake ==");
		
		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);

		//Sets this to not be destroyed when reloading scene
		// DontDestroyOnLoad(gameObject);

		//Get a component reference to the attached BoardManager script
		// boardScript = GetComponent<BoardManager>();

		// LoadAssetsBundle("combat");
		// LoadAssetsBundle("cards");

		//Call the InitGame function to initialize the first level 
		InitGame();
	}

	//Initializes the game for each level.
	void InitGame()
	{
		//Call the SetupScene function of the BoardManager script, pass it current level number.
		// boardScript.SetupScene(level);
		// cinecamController = new CinecamController();
		cinecamController = GetComponent<CinecamController>();
		gameSparksService = transform.gameObject.AddComponent<GameSparksService>();

		/* set game type: casual play by play (mobile), turn-sync (2 turns at-a-time)
			- casual: 		play-by-play (mobile)
			- turn-sync:	2 turns at-a-time (mobile/desktop)
		*/
	}

	void Start () {

	}


	// public void FloorParticleCollisionHandler(GameObject TargetedParticle)
	// {

	// }

	// public void FloorParticleTriggerHandler()
	// {

	// }

	// public void FloorCollisionEnterHandler(object sender, CollisionInfo e)
	// {

	// }

	public void FloorCollisionHandler()
	{
		if (currentAbility.isFloorTriggered == true)
			combatCinematicController.FloorCollisionHandler();
	}

	public AssetBundle LoadAssetsBundle(string bundleName)
	{
		AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(System.IO.Path.Combine(Application.streamingAssetsPath, bundleName));
		Debug.Log(myLoadedAssetBundle == null ? "Failed to load AssetBundle" : "AssetBundle successfully loaded");

		switch(bundleName)
		{
			case "combat": assetBundleCombat = myLoadedAssetBundle; break;
			case "cards": assetBundleCards = myLoadedAssetBundle; 
			break;
		}
		
		return myLoadedAssetBundle;
	}

	public void UnloadAssetsBundle()
	{
		
	}
}


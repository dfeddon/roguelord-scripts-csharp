using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
// using PubNubAPI;
using Cinemachine;
// using Newtonsoft.Json;
using HighlightPlus;
using TMPro;
using Guirao.UltimateTextDamage;
using DG.Tweening;
// using Roguelord;
using GameSparks.Api;
using GameSparks.Core;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;


public class SceneActionMain : MonoBehaviour {
    public AnimationCurve moveCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    public const int COMBAT_ACTION_STATE_ACTIVE = 1;
    public const int COMBAT_ACTION_STATE_INACTIVE = 0;
    public const int COMBAT_ACTION_STATE_PREACTIVE = 2;
    private const float COMBAT_SCALE_UP_VALUE = 1.1f;//65f;

	public Button StartMapButton;
	private bool combatActive;
    // private int _combatActionState = SceneActionMain.COMBAT_ACTION_STATE_INACTIVE;
	// public int combatActionState
	// {
	// 	get { return _combatActionState; }
	// 	set { 
    //         _combatActionState = value; 
    //         GameManager.instance.combatActionState = value;
    //     }
	// }
    private GameObject idleWindow;
    private GameObject combatWindow;
    // private GameObject cardsUI;
    private GameObject defendStack;
    private GameObject attackStack;
    private GameObject canvasUI;
    private GameObject characterUI;
    private GameObject turnCounter;
    // private GameObject roundCounter;
    // private GameObject turnCounterInfoText;
    private GameObject background;
    private GameObject activeIndicator;
    private GameObject crewAttackGroup;
    private GameObject crewDefendGroup;
    private CrewView _crewAttackGroup;
    private List<Vector3> crewAttackersPositions;
    private List<Vector3> crewDefendersPositions;
    private CrewView _crewDefendGroup;
	private List<CharacterVO> attackersData;
    private List<CharacterVO> defendersData;
    private List<CharacterVO> allCharactersData;
    private List<CharacterView> allCharactersView;
    private List<int> currentTargets = new List<int>();
    private CharacterVO selectedCharacter = null;
    private AbilityVO _currentAbility;
    // private bool combatBegin = false;
    // public PubNubService pubNubService;
    CharacterView combatSourceViewPassive;
    CharacterView combatSourceViewActive;
    List<CharacterView> combatTargetsViewPassive = new List<CharacterView>();
    List<CharacterView> combatTargetsViewActive = new List<CharacterView>();
    MongoStitchService db;
	private AssetBundle assetBundle;
    private AssetBundle cardBundle;
    // Camera cam1;
    // Camera cam2;
	CinemachineBlendDefinition someCustomBlend;
	CinemachineVirtualCamera vcam1;
    CinemachineVirtualCamera vcam2;
    CinemachineFreeLook freelook;
	CinemachineBrain brain;
	private UnityAction<EventParam> combatEventHandler;
    private UnityAction<EventParam> cardEventHandler;
    public GameObject gameManager;
    public GameObject soundManager;
    public GameObject model_Shaman;
    public GameObject model_Anarchist;
    public GameObject model_Blackhatter;
    public GameObject model_Chromer; 
    public GameObject model_Saboteur;
    public GameObject model_Medic;
    public GameObject model_Nanomancer;
    public GameObject model_Engineer;
    private GameObject[] modelsArray;
    private GameObject card1;
	private GameObject card2;
	private GameObject card3;
    private ArrowRenderer arrowRenderer;
    private GameObject arrowRendererGO;
    private ArrowMouseFollower arrowMouseFollower;
    private PlayingCardView stowedCardTemp; // TODO: REMOVE THIS
    private PlayingCardView activeCard;
    private CharacterVO highlightedCharacter;
	// GameObject uiController.healthbarsUI;
	// GameObject attackerHistory;
	// GameObject attackerInfoView;
	// GameObject defenderHistory;

	// GameObject defenderInfoView;
    CharacterView turnCharacter;
    Roguelord.UIController uiController;
    Roguelord.CharacterUIController characterUIController;
    private TimerClass timerClass;
    // Roguelord.CardController cardController;

    public int currentPhaseTotal = 0;
    public int currentTurn = 0;
    private int totalTurns = -1; // first turn is actually 0 completed
    private int[] turnOrder;
    private bool _turnCompletePending = false;
    private int charactersCreated = 0;
    private TextMesh titleMain;

	void Awake()
    {
		// cinecamController = new CinecamController();
		combatEventHandler = new UnityAction<EventParam>(combatEventHandlerFunction);
        cardEventHandler = new UnityAction<EventParam>(cardEventHandlerFunction);
        arrowRendererGO = GameObject.FindWithTag("ArrowRenderer");
        arrowRenderer = arrowRendererGO.GetComponent<ArrowRenderer>();
        canvasUI = GameObject.FindWithTag("CanvasUI");
        characterUI = GameObject.FindWithTag("CharacterUI");

        modelsArray = new GameObject[9];
        modelsArray[0] = null;
        modelsArray[CharacterVO.ROLE_ANARCHIST] = model_Anarchist;
        modelsArray[CharacterVO.ROLE_BLACKHATTER] = model_Blackhatter;
        modelsArray[CharacterVO.ROLE_CHROMER] = model_Chromer;
        modelsArray[CharacterVO.ROLE_ENGINEER] = model_Engineer;
        modelsArray[CharacterVO.ROLE_MEDIC] = model_Medic;
        modelsArray[CharacterVO.ROLE_NANOMANCER] = model_Nanomancer;
        modelsArray[CharacterVO.ROLE_SABOTEUR] = model_Saboteur;
        modelsArray[CharacterVO.ROLE_SHAMAN] = model_Shaman;

    }
    private void Start() {
		Debug.Log("== SceneActionMain.Start ==");
		GameManager.instance.gameSparksService.Authenticate((resp) => {
            if (resp.HasErrors == true)
            {
                Debug.Log("Authentication error");
            }
            else
            {
				Debug.Log("<color=green>* gameSparksService Authenticate " + resp + "</color>");
				StartGame();
            }
                
        });
        // Start2();
    }
    void StartGame() 
	{
		Debug.Log("== SceneActionMain.Start == " + UnityEngine.SystemInfo.deviceUniqueIdentifier);

        DOTween.SetTweensCapacity(2000, 100);

		if (GameManager.instance == null)
			Instantiate(gameManager);
        if (SoundManager.instance == null)
            Instantiate(soundManager);

        // load asset bundles
		assetBundle = GameManager.instance.LoadAssetsBundle("combat");
		cardBundle = GameManager.instance.LoadAssetsBundle("cards");

		// get UI/Card controller
		uiController = canvasUI.GetComponent<Roguelord.UIController>();
        GameManager.instance.uiController = uiController;

		// get character ui controller
		characterUIController = characterUI.GetComponent<Roguelord.CharacterUIController>();
		// GameManager.instance.characterUIController = characterUIController;
		
        // cardController = canvasUI.GetComponent<Roguelord.CardController>();
		// GameManager.instance.cardController = cardController;

		ArrowRendererActivate(false);

        GameManager.instance.ultimateTextDamageManager = GameObject.FindWithTag("TextDamageManager").GetComponent<UltimateTextDamageManager>();

		// Create a Cinemachine brain on the main camera
		brain = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();
        Debug.Log("brain " + brain);
		brain.m_ShowDebugText = true;
		brain.m_DefaultBlend.m_Time = 1;

		EventManager.StartListening("combatEvent", combatEventHandler);
        EventManager.StartListening("cardEvent", cardEventHandler);

		idleWindow = GameObject.FindWithTag("IdleWindow");
		combatWindow = GameObject.FindWithTag("CombatWindow");

        // dealer UI
        // cardsUI = GameObject.FindWithTag("CardsUI");
        // dealer stacks
        attackStack = GameObject.FindWithTag("AttackStack");
        defendStack = GameObject.FindWithTag("DefendStack");

        // UI Canvas
        // canvasUI = GameObject.FindWithTag("CanvasUI");
        // attackerHistory = GameObject.FindWithTag("AttackerHistory");
        // attackerInfoView = GameObject.FindWithTag("AttackerInfo");
        // defenderHistory = GameObject.FindWithTag("DefenderHistory");
        // defenderInfoView = GameObject.FindWithTag("DefenderInfo");
        // turnCounter = GameObject.FindWithTag("TurnCounter");
        // turnCounterInfoText = GameObject.FindWithTag("TurnCounterInfoText");
        // roundCounter = GameObject.FindWithTag("RoundCounter");
		// uiController.healthbarsUI = GameObject.FindWithTag("Healthbars");

        // background
		background = GameObject.FindWithTag("Background");

		crewAttackGroup = GameObject.FindWithTag("AttackGroup");
		crewDefendGroup = GameObject.FindWithTag("DefendGroup");

        activeIndicator = GameObject.FindWithTag("ActiveIndicator");
        activeIndicator.SetActive(false);
		
        arrowMouseFollower = idleWindow.GetComponent<ArrowMouseFollower>();
        // timerClass = GameObject.FindWithTag("TurnTimerPanel").gameObject.transform.GetChild(0).GetComponent<TimerClass>();

        titleMain = GameObject.FindWithTag("TitleMain").GetComponent<TextMesh>();

		// disable combat window
		combatWindow.SetActive(false);

        // disable character ui
        characterUI.SetActive(false);

		// pubnub service
		// pubNubService = new PubNubService();
		// pubNubService.SubscribeChannel();

		// mongodb stitch
		/*db = idleWindow.AddComponent<MongoStitchService>();
        string id1 = "5b93ab94762b362950f822c2";
        string id2 = "5bb7c8e51c9d440000b40f8d";
        StartCoroutine(db.CombatInit(id1, id2, (response) => {
            // response = response.Replace("{\"$oid\":", "{\"val\":");
            // response = response.Replace("\"$numberInt\":", "\"val\":");
            Debug.Log(response);
            Debug.Log("<color=gray>CombatInit response: " + response + "</color>");
            // Debug.Log(response.crews);
            // return;
			// parse data
			// var jsonString = JsonConvert.SerializeObject(response, Formatting.None);

            // MongoDB.Bson.BsonDocument bson = response as MongoDB.Bson.BsonDocument;
            // var jsonString2 = JsonConvert.DeserializeObject(response, Formatting.None);
            // Debug.Log(jsonString2);
			// var combatInitVO = JsonConvert.DeserializeObject<CombatInitVO>(response);
            // var combatInitVO = new CombatInitVO(response);


            // Debug.Log(combatInitVO.crews.Length);
            // Debug.Log(combatInitVO.crews[0].id.ToString());//.crew[0].position);
			// Debug.Log(combatInitVO.crews[0].crew[0].character.handle);
			// Debug.Log(combatInitVO.dealer.cards[0].name);

            // int[] order = {1003, 1005, 1001, 1006, 1002, 1008, 1004, 1007};
            int[] order = {1001, 2001, 1002, 2002, 1003, 2003, 1004, 2004};//, 1008, 1003, 1005 };//, 1003, 1005, 1008, 1003, 1008, 1003};
            turnOrder = order;

			IdleBegin();
		}));*/
        // GS_Authenticate();
		IdleBegin();
	}

    void GS_Authenticate()
    {
		Debug.Log("<color=yellow>== GS_Authenticate ==</color>");
        // GameManager.instance.gameSparksService.Authenticate();
        //*
        new AuthenticationRequest()
		// .SetLanguage(language)
		.SetPassword("cipher1pass")
		.SetUserName("cipher1")
		.Send((response) =>
		{
            if (!response.HasErrors)
            {
				string authToken = response.AuthToken;
				string displayName = response.DisplayName;
				bool? newPlayer = response.NewPlayer;
				GSData scriptData = response.ScriptData;
				AuthenticationResponse._Player switchSummary = response.SwitchSummary;
				string userId = response.UserId;
				Debug.Log("authentication success! " + response);
            }
            else
            {
				Debug.Log("authentication error: " + response.Errors.JSON);
			}
		});
        //*/
	}

    // IEnumerator CombatInitData()
    // {
    //     Debug.Log("CombatInitData");
	// 	/*CoroutineService cd = new CoroutineService(this, db.CombatInit());
	// 	yield return cd.coroutine;
    //     Debug.Log("* result is " + cd.result.ToString());*/
	// }

    void IdleBegin()
    {
        Debug.Log("<color=yellow>== Idle Begin ==</color>");
        this.combatActive = true;

		_crewAttackGroup = crewAttackGroup.GetComponent<CrewView>();
        _crewAttackGroup.assetBundle = assetBundle;
        _crewAttackGroup.isDefendingCrew = false;

        _crewDefendGroup = crewDefendGroup.GetComponent<CrewView>();
        _crewDefendGroup.assetBundle = assetBundle;
        _crewDefendGroup.isDefendingCrew = true;

		// stub character data
		this.attackersData = new List<CharacterVO>();
        this.defendersData = new List<CharacterVO>();
        this.allCharactersData = new List<CharacterVO>();
        this.allCharactersView = new List<CharacterView>();

		// TODO: sort characters by position

		// create models
		// attackers
		CharacterVO a1 = new CharacterVO(1001, 1, CharacterVO.ROLE_CHROMER, CharacterVO.CREW_TYPE_ATTACKER, 100, "Chromer", "Chr");
        a1.attributes = new CharacterAttributesVO(a1.uid);
		CharacterVO a2 = new CharacterVO(1002, 2, CharacterVO.ROLE_NANOMANCER, CharacterVO.CREW_TYPE_ATTACKER, 100, "Nanomancer", "Nan");
		a2.attributes = new CharacterAttributesVO(a2.uid);
		CharacterVO a3 = new CharacterVO(1003, 3, CharacterVO.ROLE_BLACKHATTER, CharacterVO.CREW_TYPE_ATTACKER, 100, "Blackhatter", "Hat");
		a3.attributes = new CharacterAttributesVO(a3.uid);
		CharacterVO a4 = new CharacterVO(1004, 4, CharacterVO.ROLE_MEDIC, CharacterVO.CREW_TYPE_ATTACKER, 100, "Medic", "Med");
		a4.attributes = new CharacterAttributesVO(a4.uid);

		// defenders
		CharacterVO d1 = new CharacterVO(2001, 1, CharacterVO.ROLE_ANARCHIST, CharacterVO.CREW_TYPE_DEFENDER, 101, "Anarchist", "Ana");
		d1.attributes = new CharacterAttributesVO(d1.uid);
		CharacterVO d2 = new CharacterVO(2002, 2, CharacterVO.ROLE_ENGINEER, CharacterVO.CREW_TYPE_DEFENDER, 101, "Engineer", "Eng");
		d2.attributes = new CharacterAttributesVO(d2.uid);
		CharacterVO d3 = new CharacterVO(2003, 3, CharacterVO.ROLE_SABOTEUR, CharacterVO.CREW_TYPE_DEFENDER, 101, "Saboteur", "Sab");
		d3.attributes = new CharacterAttributesVO(d3.uid);
		CharacterVO d4 = new CharacterVO(2004, 4, CharacterVO.ROLE_SHAMAN, CharacterVO.CREW_TYPE_DEFENDER, 101, "Shaman", "Sha");
		d4.attributes = new CharacterAttributesVO(d4.uid);
		// CharacterVO d4 = new CharacterVO(1008, 4, CharacterVO.ROLE_BLACKHATTER, CharacterVO.CREW_TYPE_DEFENDER, 101, "Blackhatter", "Blh");

		// add to list
		this.attackersData.Add(a1);
        this.attackersData.Add(a2);
        this.attackersData.Add(a3);
        this.attackersData.Add(a4);

        this.defendersData.Add(d1);
        this.defendersData.Add(d2);
        this.defendersData.Add(d3);
        this.defendersData.Add(d4);

        allCharactersData.Add(a1);
		allCharactersData.Add(a2);
		allCharactersData.Add(a3);
		allCharactersData.Add(a4);
		allCharactersData.Add(d1);
		allCharactersData.Add(d2);
		allCharactersData.Add(d3);
		allCharactersData.Add(d4);

        GameManager.instance.allcharacters = allCharactersData;

		// position crew containers then add characters to crew
        float startX = 0.5f;
		// attacker group
		_crewAttackGroup.transform.position = new Vector3(-startX, 0, 0);
        _crewAttackGroup.addCharactersToCrew(attackersData, modelsArray);
        // store character position (vector3)
        crewAttackersPositions = _crewAttackGroup.GetPositions();

        // defender group
        _crewDefendGroup.transform.position = new Vector3(startX, 0, 0);
        _crewDefendGroup.addCharactersToCrew(defendersData, modelsArray);
		// store character position (vector3)
		crewDefendersPositions = _crewDefendGroup.GetPositions();

		// add all characters
		allCharactersView.AddRange(_crewAttackGroup.pool);//GetComponentsInChildren<CharacterView>();
		allCharactersView.AddRange(_crewDefendGroup.pool);

        // StartCoroutine("StartCam1");
		// Create a virtual camera that looks at object "Cube", and set some settings
		GameManager.instance.cinecamController.idleWindow = this.idleWindow;
        GameManager.instance.cinecamController.getCamByInt(1);

        // cardController.InitPool();
        // start round
        // TODO: execute RoundBegin once the scene has completely loaded (camera event?)
        Debug.Log("<color=red>======= INIT ROUND! =========</color>");

        // RoundBegin();


		// StartCoroutine("dealCards");
        // StartCoroutine("buildHealthbars");
	}

    void NextTurn(bool firstRun = false)
    {
        Debug.Log("<color=yellow>SceneActionMain.NextTurn " + firstRun + "</color>");
		Debug.Log("<color=gray>Turn " + currentTurn + " of " + turnOrder.Length + "</color>");
        if (currentTurn == 0)
            Debug.Log("<color=green>First turn of new Round " + currentPhaseTotal + "</color>");
            
        turnCompletePending = false;

		if (currentTurn == turnOrder.Length) // TODO: assign total to variable which will be reduced when chars die
        {
            // // reset
            // currentTurn = 0;

            // // start new round
            // RoundBegin();
            RoundComplete();
        }
		else {
            // update turns
			totalTurns++;
            // update current turn
			currentTurn++;
            // update UI
            // uiController.turnCounterInfoText.GetComponent<TextMeshProUGUI>().text = currentTurn.ToString() + " of " + turnOrder.Length.ToString();
    
            Debug.Log("<color=gray>totalTurns " + totalTurns + "</color>");
            Debug.Log("<color=gray>currentTurn " + currentTurn + "</color>");

            TurnBegin(firstRun);
        }
	}

    void TurnBegin(bool firstRun = false)
    {
		Debug.Log("<color=yellow>SceneActionMain.TurnBegin " + firstRun + "</color>");
		Debug.Log("<color=gray>Turn " + currentTurn + "</color>");
		// get character on turn
		CharacterVO c = CharacterVO.getCharacterById(turnOrder[currentTurn - 1]);
		// get character ref
		if (c.crewType == CharacterVO.CREW_TYPE_ATTACKER)
			turnCharacter = _crewAttackGroup.getCharacterById(c.uid);
		else turnCharacter = _crewDefendGroup.getCharacterById(c.uid);

        // assign turnCharacter.model reference to uiController
        uiController.turnCharacter = turnCharacter.model;

		// set indicator
		// SetTurnCharacterByVo(turnCharacter.model);
		// if (GameManager.instance.roundIsLocked == false)
    	// 	activeIndicator.transform.position = new Vector3(turnCharacter.prefab.transform.position.x, 0.025f, 0.85f);

		// clear ability from previous turn
		currentAbility = null;

        // if (currentPhaseTotal == 1 && firstRun == true)
        if (currentTurn <= 1)
        {
			uiController.BuildAbilityBar(c);
			// TurnBegin();//StartCoroutine(TurnReady(true));
            
			////////////////////////////////////
			// process current turn character
			////////////////////////////////////
			foreach (CharacterView tc in allCharactersView)
			{
                // process effects
				tc.model.TurnBegin();
                // store pre-round health
                tc.model.health.prePhase = tc.model.health.current;
			}
		}
		// populate action bar with abilities (based on class)
		else 
        {
            Debug.Log("* next char (Turn Begin) " + turnCharacter.model.handle);
			// if (firstRun == true)
			// uiController.BuildAbilityBar(c);
			ProcessRoundActions();

            // // uiController.updateCharacterStatsUI(turnCharacter.model, 1);        
        }
	}

	void RoundComplete()
	{
		Debug.Log("<color=yellow>SceneActionMain.RoundComplete</color>");
		// reset turn counter
		currentTurn = 0;

		// * process RoundCompleteResults
		// * remove round-based effects
		// * grade each crew based on phase results (damage dealt vs damage received)
		// * inspired checks
		// * if dealer phase next, deal cards

        // unlock
		GameManager.instance.roundIsLocked = false;

        // update power/crypto bank
        int powerBankA = 0;
        int powerBankD = 0;
        // int powerBankB = 0;

		////////////////////////////////////
		// update character actions (w/results)
		////////////////////////////////////
        // int totalDamageReceivedA = 0;
        // int totalDamageReceivedD = 0;
		// process effect and ability coodowns
		foreach (CharacterView c in allCharactersView)
		{
			// remove actions
			if (c.actions != null)
			{
				ActionsViewController controller = c.actions.GetComponent<ActionsViewController>();
				if (controller.hasTarget() == true)
					controller.RemoveTarget();
				if (controller.hasAbility() == true)
					controller.RemoveAbility(true);
			}

			// set hasTurned to false
			c.model.hasTurned = false;
			// ... and set hasMoved to false
			c.model.hasMoved = false;

			// process abilityCooldowns
			// c.model.ProcessAbilityCooldowns();

            // re-enable highlight fx
            c.charController.SetHightLightOOP();

			// convert remaining power to crypto
			// if econ phase, multiply crypto x2

			GameManager.instance.uiController.ShowAll(true);//ShowFooter(true);

			if (c.model.crewType == CharacterVO.CREW_TYPE_ATTACKER)
                powerBankA += c.model.attributes.power;
            else powerBankD += c.model.attributes.power;

            // reduce health based on pre-round health value
            int type = HealthVO.HEALTH_MODIFIER_DAMAGE;
            if (c.model.health.current > c.model.health.prePhase)
            {
                type = HealthVO.HEALTH_MODIFIER_HEAL;
				c.model.updateHealth(type, c.model.health.current - c.model.health.prePhase);
            }
            else if (c.model.health.prePhase != c.model.health.current) {
				c.model.updateHealth(type, c.model.health.prePhase - c.model.health.current);
            }
		}
        
        Debug.Log("powerBankA Total = " + powerBankA);
		Debug.Log("powerBankD Total = " + powerBankD);

        // * apply any power bonuses (econ phase)
		GameManager.instance.uiController.updateBankTotals(powerBankA, powerBankD);

		// * if character inspired, show it here!

		// start new phase
        StartCoroutine(PhaseComplete());
	}
	IEnumerator PhaseComplete()
	{
		Debug.Log("<color=white>== PhaseComplete ==</color>");

		// * evaluate phase results
		// * outstanding wagers
		// * state of mind evaluations (health.prePhase diff)
        int totalDamageDealt = 0;
        int totalDamageReceived = 0;
        int totalTargets = 0;
        int totalSources = 0;
        float diff = 0;
        bool wasAttacked = false;
        bool didAttack = false;
        bool attackBonus = false;
        bool attackPenalty = false;
        bool defenseBonus = false;
        bool defensePenalty = false;
        foreach (CharacterVO c in allCharactersData)
        {
			// get total average damage dealt and targets/damage received
			// total damage dealt...
			//.../ total targets (AoE)

			// reset flags
			wasAttacked = false;
			didAttack = false;
			attackBonus = false;
			attackPenalty = false;
			defenseBonus = false;
			defensePenalty = false;

			// damage received
            // determine was attacked
            if (totalSources > 0) {
                // determine by how many sources
                totalDamageReceived = c.health.prePhase - c.health.current;
                diff = totalDamageReceived / c.health.prePhase / totalSources;
                if (diff > 0.15f)
                    defensePenalty = true;
                else if (diff < 0.5f)
                    defenseBonus = true;
            }
            
            // did attack?
            // damage dealt
            // get target(s) from action
            // divide total damage dealt by total targets
            // get average
		}

		yield return new WaitForSeconds(5);

		StartCoroutine(PhaseBegin());
	}

	IEnumerator PhaseBegin()
    {
        Debug.Log("<color=white>== PhaseBegin ==</color>");

        // if (titleMain.gameObject.activeSelf != true)
            // titleMain.gameObject.SetActive(true);
        // * evaluate previous round results
        // titleMain.text = "Phase 1\nEcon";
        
        yield return new WaitForSeconds(3);

		// titleMain.gameObject.SetActive(false);

		RoundBegin();
    }

	void RoundBegin()
    {
		Debug.Log("<color=yellow>SceneActionMain.RoundBegin</color>");

        // * intro phase type
        // * wager checks

		////////////////////////////////////
		// update round locally
		////////////////////////////////////
		currentPhaseTotal++;
		// uiController.roundCounter.GetComponent<TextMeshProUGUI>().text = "Phase " + currentPhaseTotal.ToString() + " - 5: Econ";
        GameManager.instance.currentPhaseTotal = currentPhaseTotal;
		// Debug.Log("<color=gray>* round " + currentPhaseTotal + "</color>");

		////////////////////////////////////
		// update remote (roundCompleteResultsVO)
		////////////////////////////////////
		// send data params (sourceId, targetIds, abilityId, cardsIds)
		// stub roundCompleteResults
		RoundCompleteResultsVO results = new RoundCompleteResultsVO().GetStubResults();

		////////////////////////////////////
		// revised turn order
		////////////////////////////////////
		// int[] order = { 1003, 1005, 1001, 1006, 1002, 1008, 1004, 1007 };
        int[] order = { 1001, 2001, 1002, 2002, 1003, 2003, 1004, 2004 };//, 1008, 1003, 1005 };
		turnOrder = order;

		////////////////////////////////////
		// update characters
		////////////////////////////////////
		// process effect and ability coodowns
		foreach (CharacterView c in allCharactersView)
        {
			// process abilityCooldowns
			c.model.ProcessAbilityCooldowns();

			// //doUpdate = c.model.RoundComplete();

			// // remove actions
			// if (c.actions != null)
			// {
			//     ActionsViewController controller = c.actions.GetComponent<ActionsViewController>();
			//     if (controller.hasTarget() == true)
			//         controller.RemoveTarget();
			//     if (controller.hasAbility() == true)
			//         controller.RemoveAbility();
			//     c.actions = null;
			// }

			// // set hasTurned to false
			// c.model.hasTurned = false;
			// // ... and set hasMoved to false
			// c.model.hasMoved = false;

			// // process abilityCooldowns
			// c.model.ProcessAbilityCooldowns();
		}

        // if (doUpdate == true)
        // {

        // }

		////////////////////////////////////
		// deal cards
		////////////////////////////////////
		// StartCoroutine("dealCards");
        /*if (currentPhaseTotal > 1)
        {
            uiController.ShowAll();
            StartCoroutine(GameManager.instance.cardController.RoundComplete());//DealCards());
        }*/

		////////////////////////////////////
		// add healthbars?
		////////////////////////////////////
        // TODO: run only if currentRount == 1
		// StartCoroutine("buildHealthbars");
        bool firstRun = false;
        if (currentPhaseTotal == 1)
        {
            firstRun = true;
			StartCoroutine(uiController.BuildHealthbars(_crewAttackGroup, _crewDefendGroup, idleWindow));
            // timerClass.StartTimer();
        }
        else StartCoroutine(ShowHealthbars(true));

        // start timer
		// timerClass.gameObject.SetActive(true);
		// timerClass.StartTimer();
        
		// stub
		// firstRun = true;
		NextTurn(firstRun);
		// StartCoroutine("PreNextTurn");
		// StartCoroutine("TurnReady");

	}

    void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown)
        {
            this.inputHandler(Event.current.keyCode, 2);
        }
        else if (Event.current.type == EventType.KeyUp)
        {
            inputHandler(Event.current.keyCode, 1);
        }
        else if (Event.current.type == EventType.MouseDown)
        {
            Debug.Log("Mouse down");

			if (arrowMouseFollower.isActive == true && highlightedCharacter == null)
			{
                Debug.Log("* card not activated");
				// remove arrow
				ArrowRendererActivate(false);
				// stow card
				EventParam eventParam = new EventParam();
				// eventParam.data = "data";
				eventParam.name = "activeCardStowed";
				eventParam.value = -1;//this.cardIndex;
				// eventParam.card = activeCard;//this;
				EventManager.TriggerEvent("cardEvent", eventParam);
                // activeCard = null; // reset
			}
		}
    }

    // handle mouse and key events
    void inputHandler(KeyCode e, int s)
    {
        Debug.Log("key event " + e + " " + s); // s => 1 up, 2 down
        if (s == 2) // keyDown
        {
            switch (e)
            {
                case KeyCode.RightArrow:
                    // if (_crewAttackGroup.isDefendingCrew == false && this.combatActive != true)
                        // _crewAttackGroup.stateKey = 1;
                    break;

                case KeyCode.LeftArrow:
                    // if (_crewAttackGroup.isDefendingCrew == false && this.combatActive != true)
                        // _crewAttackGroup.stateKey = 2;
                    break;
                
                case KeyCode.Escape:
					// hide character ui
					characterUI.SetActive(false);

                    // change cam
					GameManager.instance.cinecamController.getCamByInt(1);

                    // restore UI
                    GameManager.instance.uiController.ShowAll(true);
                break;

                case KeyCode.Alpha5:
					// _crewAttackGroup.stateKey = 3;
					if (this.combatActive && GameManager.instance.combatActionState == SceneActionMain.COMBAT_ACTION_STATE_INACTIVE)
					{
						// this.currentSource = 3; // attacker position
						// this.currentTargets = new int[] { 2, 4 }; // target position(s)
						// this.currentAbility = 5;
						// this.combatManager(this.currentTargets, this.currentAbility);
					}
					// else /*if (Time.timeScale == 0)*/
					//     combatSourceViewActive.animationEventHandler(1);//Time.timeScale = 1;
					break;
				case KeyCode.Alpha3:
                    // _crewAttackGroup.stateKey = 3;
                    if (this.combatActive && GameManager.instance.combatActionState == SceneActionMain.COMBAT_ACTION_STATE_INACTIVE)
                    {
                        // this.currentSource = 3; // attacker position
                        // this.currentTargets = new int[] {2, 4}; // target position(s)
                        // this.currentAbility = 3;
                        // this.combatManager(this.currentTargets, this.currentAbility);
                    }
                    // else /*if (Time.timeScale == 0)*/
                    //     combatSourceViewActive.animationEventHandler(1);//Time.timeScale = 1;
                    break;
                
                case KeyCode.C:
                    Debug.Log("... dealCards ...");
                    // turnCounter.SetActive(false);

                    // deal 'em
					StartCoroutine(GameManager.instance.cardController.DealCards());
                    break;
                case KeyCode.D:
                    allCharactersData[Random.Range(0, 8)].Speak("I feel... tired.");
                    break;
                case KeyCode.A:
                    Debug.Log("* A: SubmitActions");
					// this.SubmitActions();
                    this.ProcessRoundActions();
                break;

			}

        }
        else // keyUp
        {
            switch (e)
            {
                case KeyCode.RightArrow:
                    _crewAttackGroup.stateKey = 0;
                    break;

                case KeyCode.LeftArrow:
                    _crewAttackGroup.stateKey = 0;
                    break;
            }
        }
        // this._crewAttackGroup.changeAnimation(_crewAttackGroup.stateKey);
    }//*/

    void addCards()
    {
        Debug.Log("<color=yellow>== SceneActionMain.addCards ==");

		// hide turn counter UI
		turnCounter.SetActive(false);

        this.card1 = Instantiate(cardBundle.LoadAsset<GameObject>("CardBase")) as GameObject;
		this.card2 = Instantiate(cardBundle.LoadAsset<GameObject>("CardBase")) as GameObject;
		this.card3 = Instantiate(cardBundle.LoadAsset<GameObject>("CardBase")) as GameObject;
        this.card1.layer = 8;
        this.card2.layer = 8;
        this.card3.layer = 8;
        this.card1.transform.SetParent(attackStack.transform);
		this.card2.transform.SetParent(attackStack.transform);
		this.card3.transform.SetParent(attackStack.transform);
        
		PlayingCardView cview1 = card3.GetComponent<PlayingCardView>();
		PlayingCardView cview2 = card2.GetComponent<PlayingCardView>();
		PlayingCardView cview3 = card3.GetComponent<PlayingCardView>();
		cview1.SetCardView(cardBundle, 0, 1);
		cview2.SetCardView(cardBundle, 1, 2);
		cview3.SetCardView(cardBundle, 2, 3);

		// CinemachineVirtualCamera cam = GameManager.instance.cinecamController.getCurrentCam();
		// float distanceFromCamera = cam.nearClipPlane;
		// Vector3 topLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distanceFromCamera));
		// this.card1.transform.localScale = new Vector3(this.card1.transform.localScale.x * 2, this.card1.transform.localScale.y * 2, this.card1.transform.localScale.z * 2);
		// float baseZ = transform.forward.z;
		Vector3 cam = Camera.main.transform.position;

		Vector3 screenBottomLeft = new Vector3(Screen.width / 2, Screen.height / 2, cam.z);//Camera.main.nearClipPlane);
		Vector3 inWorld = Camera.main.ScreenToWorldPoint(screenBottomLeft);
        this.card1.transform.position = inWorld;//new Vector3(0, 8, -5f);

		// this.card2.transform.localScale = new Vector3(this.card2.transform.localScale.x * 2, this.card2.transform.localScale.y * 2, this.card2.transform.localScale.z * 2);
		Vector3 screenBottomLeft1 = new Vector3((Screen.width / 2) - 5, Screen.height / 2, cam.z);//Camera.main.nearClipPlane);
		Vector3 inWorld2 = Camera.main.ScreenToWorldPoint(screenBottomLeft1);
		this.card2.transform.position = inWorld;//new Vector3(0, 2, cam.z);

		// this.card3.transform.localScale = new Vector3(this.card3.transform.localScale.x * 2, this.card3.transform.localScale.y * 2, this.card3.transform.localScale.z * 2);
		Vector3 screenBottomLeft2 = new Vector3((Screen.width / 2) - 25, Screen.height / 2, cam.z);//Camera.main.nearClipPlane);
		Vector3 inWorld3 = Camera.main.ScreenToWorldPoint(screenBottomLeft2);
		this.card3.transform.position = inWorld;//new Vector3(0, 2, cam.z);

		// card.layer = 8; // "Crew" layer
		// SpriteRenderer cardRend = card.GetComponent<SpriteRenderer>();
		// cardRend.sortingLayerName = "Cards";
		// Vector3 scale = new Vector3(0.05f, 0.05f, 1f);
		// cardRend.transform.localScale = scale;
		// this.card1.transform.SetParent(cardsUI.transform, false);
		// this.card2.transform.SetParent(cardsUI.transform, false);
		// this.card3.transform.SetParent(cardsUI.transform, false);
	}

    void setHighlightState(bool hide)
    {
        HighlightEffect hfx;
		// foreach (CharacterView c in characters)
        foreach(CharacterView c in allCharactersView)
		{
			hfx = c.prefab.GetComponent<HighlightEffect>();
			if (hfx) {
                // ...
                if (c.charController.highlightTrigger != null)
					Destroy(c.charController.highlightTrigger);
                // ignore mouse-over activation
				hfx.ignore = hide;
                // remove highlighted
                hfx.SetHighlighted(false);
                // update
                hfx.Refresh();
            }
		}
	}

    /**
        * changeCombatActionState

        * @param state
     */
    void changeCombatActionState(int state)
    {
        Debug.Log("<color=yellow>== SceneActionMain.changeCombatActionState ==</color>");
		Debug.Log("<color=gray>* state " + state + "</color>");

		GameManager.instance.combatActionState = state;

        // List<CharacterView> attackers;
        // List<CharacterView> defenders;
        // HighlightEffect he;
        switch(state)
        {
			case SceneActionMain.COMBAT_ACTION_STATE_ACTIVE:
                // activate combat window
                this.combatWindow.SetActive(true);
				// deactive cards UI
				// this.cardsUI.SetActive(false);
				// deactivate UI
                uiController.ShowAll(false);

				// disable highlighteffect from passive characters (through CrewView?)
				setHighlightState(true);

				break;
                
            case SceneActionMain.COMBAT_ACTION_STATE_INACTIVE:

				// disable combat window
				this.combatWindow.SetActive(false);
				// re-enable UI
				// this.canvasUI.SetActive(true);
                // this.cardsUI.SetActive(true);

                // StartCoroutine("activateUI");

				// restore passives to active
				this.combatSourceViewPassive.prefab.SetActive(true);
				foreach (CharacterView i in combatTargetsViewPassive)
				{
					i.prefab.SetActive(true);
				}

                // clear target passives list
                combatTargetsViewPassive.Clear();

				// restore crews
				setHighlightState(false);

				// // start next turn
				// NextTurn();
                TurnComplete();

			break;
        }
    }

	IEnumerator activateUI()
	{
		// print(Time.time);
		yield return new WaitForSeconds(1.5f);
		// print(Time.time);
        // this.canvasUI.SetActive(true);
		// re-enable cards ui window
		// this.cardsUI.SetActive(true);
	}

    void combatManager(CombatResultsVO results)//List<int> targets)//, AbilityVO ability)
    {
        Debug.Log("<color=yellow>== SceneActionMain.combatManager ==</color>");
        // Debug.Log("* targets " + targets + " ability " + ability);

        // update passive characters with new results

        // TODO: replace 'targets' with global 'currentTargets' to avoid confusion
        // List<int> targets = currentTargets;

        //////////////////////////////////////////////////////
        // set y & z values for action window
        float baseY = 0f;//1.25f;
        float baseZ = -2;

        float baseScale = 1.0f;
        //////////////////////////////////////////////////////

        //////////////////////////////////////////////////////
        // activate combat state
        //////////////////////////////////////////////////////
        this.changeCombatActionState(SceneActionMain.COMBAT_ACTION_STATE_ACTIVE);

        //////////////////////////////////////////////////////
        // get source character from crew pool
        //////////////////////////////////////////////////////
        // combatSourceViewPassive = ListHelper.getCharacterViewByName(this._crewAttackGroup.pool, "attacker" + source.ToString());
        // ...hide character in crew
        combatSourceViewPassive = turnCharacter;
        combatSourceViewPassive.prefab.SetActive(false);//GetComponent<MeshRenderer>().material.color = colorNoAlpha;

        // copy passive source characterView data to combatSourceViewActive
        if (combatSourceViewActive == null)
            combatSourceViewActive = new GameObject("CombatSourceViewActive").AddComponent<CharacterView>();
        else Debug.Log("ALREADY EXISTS!");
        // assign active ability and set isSource to true
        combatSourceViewActive.activeAbility = currentAbility;
        combatSourceViewActive.isSource = true;
		// inherit model
		combatSourceViewActive.model = combatSourceViewPassive.model;
        // set results
        combatSourceViewActive.combatResultsVO = results;
		// new prefab
		// combatSourceViewActive.prefab = Instantiate(assetBundle.LoadAsset<GameObject>(CharacterVO.getPrefabFromRole(combatSourceViewActive.model.role))) as GameObject;
		combatSourceViewActive.prefab = Instantiate(modelsArray[combatSourceViewActive.model.role]);// as GameObject;
		// inherit scale from passive
        baseScale = combatSourceViewPassive.model.viewScale;
		combatSourceViewActive.prefab.transform.localScale = combatSourceViewPassive.prefab.transform.localScale;
		// inherit rotation from passive
		combatSourceViewActive.prefab.transform.rotation = combatSourceViewPassive.prefab.transform.rotation;
        // listen for events in CharController
        CharController charController = combatSourceViewActive.prefab.GetComponent<CharController>();
        charController.isIdleCharacter = false;
        charController.characterView = combatSourceViewActive;
		// scale up for combat
		// combatSourceViewActive.prefab.transform.localScale = new Vector3(SceneActionMain.COMBAT_SCALE_UP_VALUE , SceneActionMain.COMBAT_SCALE_UP_VALUE, SceneActionMain.COMBAT_SCALE_UP_VALUE);
		combatSourceViewActive.prefab.transform.localScale = new Vector3(baseScale + 0.1f, baseScale + 0.1f, baseScale + 0.1f);
		// ... and add to combat window
		combatSourceViewActive.prefab.transform.SetParent(this.combatWindow.transform, false);
		// and position
		// if ability is melee, position source char closer to center
		if (currentAbility.actionType == AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_MELEE)
		{
			Vector3 centerPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));//distanceFromCamera));
            if (combatSourceViewActive.model.crewType == CharacterVO.CREW_TYPE_ATTACKER)
			    combatSourceViewActive.prefab.transform.position = new Vector3(centerPos.x - 0.75f, baseY, baseZ);
            else combatSourceViewActive.prefab.transform.position = new Vector3(centerPos.x + 0.75f, baseY, baseZ);
		}
		else // standard positioning
        {
            combatSourceViewActive.prefab.transform.position = new Vector3(combatSourceViewPassive.prefab.transform.position.x, baseY, baseZ);
        }
		// ignore highlighting
		// HighlightEffect hlight = combatSourceViewActive.prefab.GetComponent<HighlightEffect>(); 
        // hlight.ignore = true;

		//////////////////////////////////////////////////////
		// do we have targets?
		//////////////////////////////////////////////////////
		if (currentTargets.Count > 0)
        {
            // are targets attackers or defenders
            // string targetType = "defender";
            CrewView targetCrew = _crewDefendGroup;
            float rotatePrefab = -90f;
            // if attacking crew is "defenders" or if attacking crew is "attackers" AND ability is colleagues
            if ( (combatSourceViewPassive.model.crewType == CharacterVO.CREW_TYPE_DEFENDER && currentAbility.targetType != AbilityVO.TARGET_TYPE_COLLEAGUE) || (combatSourceViewPassive.model.crewType == CharacterVO.CREW_TYPE_ATTACKER && currentAbility.targetType == AbilityVO.TARGET_TYPE_COLLEAGUE) )
            {
                // targetType = "attacker";
                targetCrew = _crewAttackGroup;
                rotatePrefab = 90f;
            }
            // if targets are colleagues, omit source position from targets array (because source is already active)
            if (currentAbility.targetType == AbilityVO.TARGET_TYPE_COLLEAGUE)
				currentTargets.Remove(combatSourceViewPassive.model.position);

            // get targets from crew pool (and hide from action window)...
            for (int i = 0; i < currentTargets.Count; i++)
            {
                // combatTargetsViewPassive.Add(ListHelper.getCharacterViewByName(targetCrew.pool, targetType + targets[i].ToString()));
				combatTargetsViewPassive.Add(ListHelper.getCharacterViewByPosition(targetCrew.pool, currentTargets[i]));
			}
            
            // hide targets in crew
            foreach (CharacterView i in combatTargetsViewPassive)
            {
                i.prefab.SetActive(false);
            }

			// store target refs
			// GameObject[] targets;
            CharController cc;
			// add defender(s) to list
			for (int i = 0; i < currentTargets.Count; i++)
            {
                // Debug.Log("* target " + currentTargets[i]);
				// combatTargetsViewActive.Add(Instantiate(Resources.Load<GameObject>("Prefabs/CharacterPrefab")) as GameObject);
				CharacterView cview = ListHelper.getCharacterViewByPosition(targetCrew.pool, currentTargets[i]);//.ToString());
				CharacterView att = new GameObject("ActiveTargets").AddComponent<CharacterView>();
                att.model = cview.model;
				// assign active ability
				att.activeAbility = currentAbility;//currentAbility;
				// assign combat results
				att.combatResultsVO = results;

				// if (i == 0)
				// {
				// att.prefab = Instantiate(assetBundle.LoadAsset<GameObject>(CharacterVO.getPrefabFromRole(cview.model.role))) as GameObject;
				att.prefab = Instantiate(modelsArray[cview.model.role]) as GameObject;
				// att.prefab.GetComponent<Animator>().Play("Idle");
				// }
				// else att.prefab = Instantiate(assetBundle.LoadAsset<GameObject>("Dark Witch_5")) as GameObject;
				// set parent
				att.prefab.transform.SetParent(this.combatWindow.transform, false);
				// flip horizontally (if defender)
				att.prefab.transform.Rotate(0, rotatePrefab, 0);
                // inherit scale first...
                att.prefab.transform.localScale = cview.prefab.transform.localScale;
				// ... and scale
                baseScale = cview.model.viewScale;
				att.prefab.transform.localScale = new Vector3(baseScale + 0.1f, baseScale + 0.1f, baseScale + 0.1f);
				// ... and position
				// if melee ability, position target (if solo target) near center
                if (currentAbility.actionType == AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_MELEE)
                {
                    Vector3 centerPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));//distanceFromCamera));
					if (att.model.crewType == CharacterVO.CREW_TYPE_DEFENDER)
                        att.prefab.transform.position = new Vector3(centerPos.x + 0.75f, baseY, baseZ);
                    else att.prefab.transform.position = new Vector3(centerPos.x - 0.75f, baseY, baseZ);
                }
				else { // standard positioning
                    att.prefab.transform.position = new Vector3(cview.prefab.transform.position.x, baseY, baseZ);
                }
				// listen for events in CharController
				cc = att.prefab.GetComponent<CharController>();
				cc.isIdleCharacter = false;
				// cc.characterView = combatSourceViewActive;
				// ... and set isTrigger and foremost characters (< len)
				CapsuleCollider capsuleCollider = att.prefab.GetComponent<CapsuleCollider>() as CapsuleCollider;
                // Debug.Log("i="+i+"/"+targets.Count);
                if (currentTargets.Count > 1 && i < currentTargets.Count - 1)
                {
                    Debug.Log("<color=blue>is Trig!" + att.model.handle + "</color>");
					// CapsuleCollider toTrig = att.prefab.GetComponent<CapsuleCollider>() as CapsuleCollider;
					capsuleCollider.isTrigger = true;
                }
                else 
                {
					Debug.Log("<color=red>not Trig!" + att.model.handle + "</color>");
					// CapsuleCollider noTrig = att.prefab.GetComponent<CapsuleCollider>() as CapsuleCollider;
					capsuleCollider.isTrigger = false;
                }
			
            	combatTargetsViewActive.Add(att);
			}
            // assign targets and colleagues to source view
            combatSourceViewActive.targets = combatTargetsViewActive;
            // combatSourceViewActive.colleagues = ;
            // ... and to all available targets
			for (int i = 0; i < combatTargetsViewActive.Count; i++)
            {
                combatTargetsViewActive[i].targets = combatTargetsViewActive;
            }
        }

		//////////////////////////////////////////////////////
		// start combat animation stages
		//////////////////////////////////////////////////////
		// this.combatBegin = true;
        ActionBegin();//ability);
    }

    void ActionBegin()//int abilityNumber)
    {
        Debug.Log("<color=yellow>SceneActionMain.ActionBegin</color>");
		activeIndicator.SetActive(false);
        // cam1.enabled = false;
        // cam2.enabled = true;
        // cam2.transform.SetParent(combatSourceViewActive.prefab.transform);

		// cam2.GetComponent<CameraFollow>().setTarget(combatSourceViewActive.prefab.transform);


		// combatSourceViewActive.changeAnimation(CharacterView.CHARACTER_ANIMATION_SKILL_1);
        StartCoroutine("changeCam1");//("startCam2");
        CombatCinematicController cinecontroller = new CombatCinematicController();

        // add actors and set action (ability, movement, etc)
        cinecontroller.addActors(combatSourceViewActive, combatTargetsViewActive, currentAbility);// abilityNumber);
	}

    void SetTurnCharacterByVo(CharacterVO vo)
    {
        Debug.Log("* SetTurnCharacterBYVO " + GameManager.instance.roundIsLocked);// + );
		// get characterView from vo
		turnCharacter = _crewAttackGroup.getCharacterById(vo.uid);
		if (GameManager.instance.roundIsLocked == false) 
        {
			/*
            Vector3 c = turnCharacter.prefab.GetComponent<BoxCollider>().bounds.center;
			// move indicator to selected...
			activeIndicator.SetActive(true);
			activeIndicator.transform.position = new Vector3(c.x, 0.025f, 0.85f);//new Vector3(turnCharacter.prefab.transform.position.x, 0.1f, turnCharacter.prefab.transform.position.z);
            */
		}
		// hide turn counter UI
		/*if (turnCounter == null)
			turnCounter = GameObject.FindWithTag("TurnCounter");
		turnCounter.SetActive(false);*/
		// hide ui
		GameManager.instance.uiController.ShowAll(false);
		// show character ui
		characterUI.SetActive(true);
		// zoom in on character
		GameManager.instance.cinecamController.getCamByInt(25, false, turnCharacter);
		// change ability bar
		characterUIController.BuildAbilityBar(vo);
        // uiController.BuildAbilityBar(vo);
	}

	void combatEventHandlerFunction(EventParam eventParam)
	{
		Debug.Log("<color=yellow>== SceneActionMain.combatEventHandlerFunction ==</color>");
		Debug.Log("<color=green>data " + eventParam.name + "</color>");

		CharacterVO c;
        switch(eventParam.name)
        {
            case "characterCreated":
                charactersCreated++;
                if (charactersCreated == 8)
                {
                    Debug.Log("start game!");
					StartCoroutine(PhaseBegin());//RoundBegin();
                }
            break;
            case "changeTurnCharacter":
                if (eventParam.character != null)
                    SetTurnCharacterByVo(eventParam.character as CharacterVO);
            break;
            case "combatComplete":
                CombatComplete();
            break;

            case "changeTarget":
				c = eventParam.character as CharacterVO;
                CharacterView view;
                if (c.crewType == CharacterVO.CREW_TYPE_ATTACKER)
    				view = ListHelper.getCharacterViewByPosition(_crewAttackGroup.pool, c.position);
                else view = ListHelper.getCharacterViewByPosition(_crewDefendGroup.pool, c.position);
				Transform trans = view.prefab.GetComponent<CharController>().weapon[0].endpoint;
				// add arrow
				arrowRenderer.SetPositions(trans.position, new Vector3(0, 0, trans.position.z));
				arrowRendererGO.SetActive(true);//.gameObject.SetActive(true);
				arrowMouseFollower.start = trans.position;
				arrowMouseFollower.isActive = true;
                SetTurnCharacterByVo(c);
			break;

            case "characterSelected":
                Debug.Log("* characterSelected " + eventParam.data + " / " + turnCharacter + " / " + currentAbility);

				c = eventParam.data as CharacterVO;//turnOrder[currentTurn - 1]);
                
                // update ui
                /*GameManager.instance.uiController.updateUIByCharacter(c);

				UnityEngine.UI.Image moveImg = GameObject.FindWithTag("AbilityBar").transform.GetChild(2).transform.GetChild(0).GetComponent<Image>();
                if (c.hasMoved == true)
                    moveImg.sprite = GameManager.instance.assetBundleCombat.LoadAsset<Sprite>("86-off");
                else moveImg.sprite = GameManager.instance.assetBundleCombat.LoadAsset<Sprite>("86");
                */

				// else turnCharacter = _crewDefendGroup.getCharacterById(c.uid);

				// is dealer card activated
				if (arrowMouseFollower.isActive == true)
                {
                    Debug.Log("* action activated on character " + c.handle);
                    // remove arrow
					ArrowRendererActivate(false);//, eventParam.value);
                    // get card model
                    // start card-to-character animation

                    // add target (character profile) to source actions
                    turnCharacter.actions.GetComponent<ActionsViewController>().AddTarget(c);
                    arrowMouseFollower.isActive = false;
                }
                else if (currentAbility != null && currentAbility.uid > 0)// && c.hasMoved == false)
                {
					// CharacterVO vo = eventParam.data as CharacterVO;

                    // ability uid 1000 == moving
                    if (currentAbility.uid == 1000)
                    {
						Debug.Log("== Moving... ==");

                        // only move once per push
                        currentAbility = null;
                        // c.hasMoved = true; // TODO: at end of waves, reset all chars .hasMoved prop to 'false'
                        // on hasMoved setter, if == true disable Move button

						CharacterVO target = eventParam.data as CharacterVO;
						// validate target position against source movement speed...
                        // validate target is in crew
                        // TODO: also validate that target is NOT rooted
                        bool isValid = false;
                        if (target.crewType == turnCharacter.model.crewType)
                        {
                            // only allow characters next to turn character
                            if (target.position == turnCharacter.model.position + 1)
                            {
                                isValid = turnCharacter.model.movement.MoveBackward(turnCharacter.model.uid);
                            }
                            else if (target.position == turnCharacter.model.position - 1)
                            {
                                isValid = turnCharacter.model.movement.MoveForward(turnCharacter.model.uid);
                            }
                            if (isValid == true)
                            {
						        ccSwitchPositions(turnCharacter, target.position);
								// ccRepositionCrew(turnCharacter, target.position);
								// UnityEngine.UI.Image img = GameObject.FindWithTag("AbilityBar").transform.GetChild(2).transform.GetChild(0).GetComponent<Image>();
                                turnCharacter.model.hasMoved = true;
								// moveImg.sprite = GameManager.instance.assetBundleCombat.LoadAsset<Sprite>("86-off");
                                // clear moving ability
								this.currentAbility = null;//eventParam.value;
								// GameManager.instance.currentAbility = null;
							}
                        }
					}
                    else {
                        // get actual ability from character
                        // Debug.Log("<color=purple>add target icon above character</color>");
                        // precombatManager(vo);
                    }
				}
				// get character ref
				else if (c.crewType == CharacterVO.CREW_TYPE_ATTACKER)
				{
					// if different that current turnCharacter, remove indicator from previous turnCharacter
					// get characterView from vo
                    SetTurnCharacterByVo(c);
					// turnCharacter = _crewAttackGroup.getCharacterById(c.uid);
					// // move indicator to selected...
					// activeIndicator.transform.position = new Vector3(turnCharacter.prefab.transform.position.x, 0.1f, turnCharacter.prefab.transform.position.z);
                    // // change ability bar
					// uiController.BuildAbilityBar(c);
				}
				// else Debug.Log("selected Target Character");

				// else {
                //     // Debug.Log("add ability icon above characater");
				// 	// CharacterVO vo = eventParam.data as CharacterVO;
                //     // if (currentAbility == null) // TODO: clear this after assignment to characterVIew
                //         // vo.actionIndicatorAdd(currentAbility);
				// 	// Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(c.prefab.transform.position.x, 0, 0));
                // }
                // get ability history and stats view
				// uiController.updateCharacterStatsUI(eventParam.data as CharacterVO, 1);

            break;

            case "characterMouseEnter":
                // TODO: account for oop
                highlightedCharacter = eventParam.data as CharacterVO;
                // uiController.updateCharacterStatsUI(highlightedCharacter, 1);
            break;

            case "characterMouseExit":
                // TODO: account for oop
                highlightedCharacter = null;
				// uiController.updateCharacterStatsUI(highlightedCharacter, 0);
				break;

            case "abilityEnter":
                // TODO: popup ability info, highlight available targets
            break;

            case "abilityExit":
                // TODO: clear ability info, clear target highlights
            break;

            case "abilitySelected":
                // cast eventParam.data as AbilityVO
                AbilityVO abilityVO = (AbilityVO)eventParam.data as AbilityVO;

                Debug.Log("* ability selected " + eventParam.value + " / " + abilityVO.name);

				/////////////////////////////////////////
                // * Unlock Mod Slot
            	/////////////////////////////////////////
				if (abilityVO.uid > 1000) // locked mod slot
                {
                    // slot is locked... activate tooltip (pass ref not new vo)
                    uiController.ShowAbilityTooltip(eventParam.data as AbilityVO);//abilityVO);
                }

				_crewAttackGroup.stateKey = 3;

				if (this.combatActive && GameManager.instance.combatActionState == SceneActionMain.COMBAT_ACTION_STATE_INACTIVE)
				{
					// attacker position(s)
					// this.currentTargets = new int[] { 2, 3 }; // target position(s)

					/////////////////////////////////////////
					// * Character Move
					/////////////////////////////////////////
					if (eventParam.value == 7)
                    {
                        // ignore move ability if character has already moved...
                        if (turnCharacter.model.hasMoved == true) return;

                        Debug.Log("* Moving...");
                        abilityVO = new AbilityVO();
                        abilityVO.uid = 1000; // <- Moving ID
                    }
                    else
                    {
						/////////////////////////////////////////
						// * Ability
						/////////////////////////////////////////

                        // highlight active ability
                        
						// add image to healthbars container with a greater y value...
						// Debug.Log("Add ability icon to turnCharacter");
						ActionsViewController controller = turnCharacter.actions.GetComponent<ActionsViewController>();
                        controller.AddAbility(abilityVO);
                        if (abilityVO.targets > 0 && abilityVO.targetType == AbilityVO.TARGET_TYPE_OPPONENT)
                        {
                            // if target already selected (changed ability) remove it?
                            if (controller.hasTarget() == true)
                                controller.RemoveTarget();

                            // if (Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.Android) {
                                Transform transform = turnCharacter.prefab.GetComponent<CharController>().weapon[0].endpoint;
                                // add arrow
                                arrowRenderer.SetPositions(transform.position, new Vector3(0, 0, transform.position.z));
                                arrowRendererGO.SetActive(true);//.gameObject.SetActive(true);
                                arrowMouseFollower.start = transform.position;
                                arrowMouseFollower.isActive = true;
                            // }
						}

					}
                    // store globally only if Moving...
                    if (abilityVO.uid == 1000)
                    {
                        this.currentAbility = abilityVO;//eventParam.value;
                        // GameManager.instance.currentAbility = abilityVO;
                    }

					// remove power cost
                    if (abilityVO != null && abilityVO.cost > 0)
    					turnCharacter.model.attributes.power -= abilityVO.cost;

					// TODO: both on select and mouseenter, highlight avialable targets based on ability

					// if solo ability, let's go!
					// if (abilityVO.actionType == AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SELF && abilityVO.targets == 1)
					//     precombatManager(turnCharacter.model);

					// start combat
					// this.combatManager(this.currentTargets, this.currentAbility);
				}
            break;

            case "nextTurn":
                if (turnCompletePending == true)
                {
                    turnCompletePending = false;
                    NextTurn();
                }
            break;

            case "turnCounterComplete":
                Debug.Log("<color=red>Turn Counter Expired!</color>");
				// remove timer
				// GameObject timer = GameObject.FindWithTag("TurnTimerPanel");
				// timer.SetActive(false);
                // set global flag
                // GameManager.instance.roundIsLocked = true;
                
                // GameManager.instance.movementIsLocked = false; // reset

                // SubmitActions();

				// ProcessRoundActions();
                // NextTurn();
            break;

            case "turnCounterMidpoint":
                Debug.Log("<color=red>Turn Counter Midpoint reached!</color>");

                // disable movement
                /*
                foreach(CharacterVO a in attackersData)
                    a.hasMoved = true;
                */
                // * disable move button (global UI)
            break;
        }
    }

    private void SubmitActions()
    {
		Debug.Log("<color=green>== SceneActionMain.SubmitActions ==</color>");
		// ProcessRoundActions();
		GameManager.instance.gameSparksService.SendActions((resp) =>
		{
			Debug.Log("<color=green>resp " + resp + "</color>");
			if (resp.HasErrors == true)
			{
				Debug.Log("Authentication error");
			}
			else
			{
				ProcessRoundActions(resp);
			}

		});
	}

    private void ProcessRoundActions(LogChallengeEventResponse logChallengeEventResponse = null)
    {
		Debug.Log("<color=red>== SceneActionMain.ProcessRoundAction ==</color>");
        Debug.Log("turn " + currentTurn + " of " + turnOrder.Length);

        // iterate based on turn order
        // for (var i = currentTurn - 1; i < turnOrder.Length; i++)
		// foreach (CharacterVO vo in allCharactersData)
		// {
        CharacterVO vo = CharacterVO.getCharacterById(turnOrder[currentTurn - 1]);
        Debug.Log("* next char (ProcessRoundActions) " + vo.handle);        
        // validate actions

        // automate actions (for testing)
        switch(vo.role) 
        {
            case CharacterVO.ROLE_ANARCHIST:
                vo.actions.ability = AbilityLibraryVO.DefaultKatana(); // id: 202
                vo.actions.target = CharacterVO.getCharacterById(turnOrder[0]); // chromer
            break;

            case CharacterVO.ROLE_ENGINEER:
				vo.actions.ability = AbilityLibraryVO.Portal();// DefaultWeapon4(); // id: 204
				vo.actions.target = CharacterVO.getCharacterById(turnOrder[2]); // nanomancer
				break;

            case CharacterVO.ROLE_SABOTEUR:
				vo.actions.ability = AbilityLibraryVO.DefaultWeapon6();
				vo.actions.target = CharacterVO.getCharacterById(turnOrder[4]); // nanomancer
				break;

            case CharacterVO.ROLE_SHAMAN:
				vo.actions.ability = AbilityLibraryVO.DefaultWeapon7();
				vo.actions.target = CharacterVO.getCharacterById(turnOrder[6]); // nanomancer
				break;
        }
        // if blank...? skip turn?
        if (vo.actions.ability != null && (vo.actions.ability.actionType != AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SELF))
        {
            Debug.Log(":: " + vo.actions.ability.name);// + " / " + vo.actions.target.handle);

            // if defense of self, target is self
            if (vo.actions.ability.actionType == AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SELF)
                vo.actions.target = vo;
            precombatManager(vo);
            // break;
        }
        else 
        {
            Debug.Log("char has no action!");
            // TurnComplete();
            if (turnCompletePending == true)
            {
                turnCompletePending = false;
            }
			NextTurn();
			// CombatComplete();
		}
		// }
	}

    void precombatManager(CharacterVO source)
    {
        Debug.Log("<color=yellow>SceneActionMain.precombatManager</color>");        
		GameManager.instance.combatActionState = COMBAT_ACTION_STATE_PREACTIVE;

        currentAbility = source.actions.ability;
        // GameManager.instance.currentAbility = currentAbility;

        turnCharacter = ListHelper.getCharacterViewById(allCharactersView, source.uid);
        CharacterVO vo = source.actions.target;
        
		// target validation
        if (currentAbility.actionType == AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SELF)
        {
            if (vo.uid != turnCharacter.model.uid)
                return;
        }
        else
        {
            switch(currentAbility.targetType)
            {
                case AbilityVO.TARGET_TYPE_COLLEAGUE:
                    if (vo.crewType != turnCharacter.model.crewType) return;
                break;

                case AbilityVO.TARGET_TYPE_OPPONENT:
                    if (vo.crewType == turnCharacter.model.crewType) return;
                break;
            }
        }

		// assign character as primary target
		vo.isPrimaryTarget = true; // TODO: <-- be sure this value is cleared/deleted post-combat

		// clear extant targets
		this.currentTargets.Clear();
		int primaryTargetPosition;

		// if ability type is defense self, only caster is target
		if (currentAbility.actionType == AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SELF && currentAbility.targets == 1)
		{
			currentTargets.Add(turnCharacter.model.position);
			primaryTargetPosition = turnCharacter.model.position;
		}
		else // ability not relegated to caster only...
		{
			primaryTargetPosition = vo.position;
			// add target position
			this.currentTargets.Add(vo.position);//primaryTargetPosition); // target position(s)
												 // is ability AoE (multi-target)?
			if (currentAbility.targets > 1)
			{
				int totalTargets = currentAbility.targets;
				// if so, include positional range 'around' target (provided there are such positions available)
				List<int> crew = new List<int> { 1, 2, 3, 4 };
				// if index out of bounds, adjust
				if (primaryTargetPosition + currentAbility.targets > crew.Count)
				{
					// ensure primary targets is 'front of the line'
					primaryTargetPosition = 4 - currentAbility.targets;
					// clear original primary target pos
					currentTargets.Clear();
				}
				else totalTargets -= 1; // <- already added primary
										// determine range ( - 1 to exclude primary target)
				List<int> targets = crew.GetRange(primaryTargetPosition, totalTargets);// - 1);
                // if targets are colleagues, remove "self" from targets list
                // int positionToOmit = 0;
                // if (currentAbility.targetType == AbilityVO.TARGET_TYPE_COLLEAGUE)
                //     positionToOmit = vo.position;
                // add ranged positions to currentTargets
				foreach (int pos in targets)
				{
					// if (pos != positionToOmit)
					currentTargets.Add(pos);
				}
			}
		}

        // manage cooldowns
        // turnCharacter.model, currentAbility
        if (currentAbility.cooldownTotal > 0)
            turnCharacter.model.AbilityCooldownUpsert(currentAbility);

		/*if (primaryTargetPosition < 4)
			this.currentTargets.Add(primaryTargetPosition + 1);*/
        combatResults();//currentTargets);
		// start combat
		// this.combatManager(currentTargets, currentAbility);

	}

    void combatResults()//currentTargets)
    {
        Debug.Log("<color=yellow>=== SceneActionMain.combatResults ===</color>");
		// TODO: send turnCharacter, currentTargets and currentAbility to backend and return results
		// first, get currentTargets by crew position
		// turnCharacter (source) currentTargets (targets) currentAbility (ability)
		// make a local copy of currentTargets, because the combatManager will parse them differently
		List<int> targets = new List<int>(currentTargets);
        // get source id, targets id, ability

		// HealthbarComponent healthbarComponent;
		// EffectsbarComponent effectsbarComponent;

        // get source uid
        int sourceId = turnCharacter.model.uid;
        Debug.Log("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% sourceId " + sourceId + " / " + turnCharacter.model.handle);
		// get target uids
		List<int> targetIds = new List<int>();
		List<CharacterView> pool = new List<CharacterView>();
		// pool the appropriate characters
    	if ( 
            (turnCharacter.model.crewType == CharacterVO.CREW_TYPE_ATTACKER && currentAbility.effect.effectModifier != EffectVO.EFFECT_MODIFIER_BOON && currentAbility.healthModifier != HealthVO.HEALTH_MODIFIER_HEAL) || 
            (turnCharacter.model.crewType == CharacterVO.CREW_TYPE_DEFENDER && (currentAbility.effect.effectModifier == EffectVO.EFFECT_MODIFIER_BOON || currentAbility.healthModifier == HealthVO.HEALTH_MODIFIER_HEAL))
        )
			pool = _crewDefendGroup.pool;
        else /*if (turnCharacter.model.crewType == CharacterVO.CREW_TYPE_DEFENDER && currentAbility.effectModifier != EffectVO.EFFECT_MODIFIER_BOON && currentAbility.healthModifier != HealthVO.HEALTH_MODIFIER_HEAL)*/
            pool = _crewAttackGroup.pool;
        // else pool = _crewAttackGroup.pool;
        // now get the target ids by position
        foreach(int t in targets)
        {
            targetIds.Add(ListHelper.getCharacterViewByPosition(pool, t).model.uid);//.characterResultsVO;// );
        }
        // get ability id
        int abilityId = currentAbility.uid;
		// get any used card ids?

		// process next turn effects and/or health mods
        // ignore if last player of round
        CharacterVO nextTurnCharacter = null;
        if (turnOrder.Length != currentTurn)
		    nextTurnCharacter = CharacterVO.getCharacterById(turnOrder[currentTurn]);
        
        // send data params (sourceId, targetIds, abilityId, nextTurnId, cardsIds)
		// stub combatResults
		CombatResultsVO results = new CombatResultsVO().GetStubResults(sourceId, targetIds, abilityId, (nextTurnCharacter != null) ? nextTurnCharacter.uid : 0);
        // assign currentAbility to results
        results.abilityId = currentAbility.uid;

		// assign combat results to *all* passive characters
		foreach (CharacterView c in allCharactersView)//crews)
		{
			// look for new/expiring effects
			// Debug.Log(c.model.handle + " " + c.characterResultsVO.effect.effectModifier);
            c.combatResultsVO = results;
        }

		this.combatManager(results);
    }

    void TurnComplete()
    {
        Debug.Log("<color=yellow>== TurnComplete ==</color>");

        // set current character's hasTurned prop to true
        turnCharacter.model.hasTurned = true;

		// update all characters cooldowns, healthbars and effectsbars...
        // turnCharacter.model.AddAbilityToHistory(currentAbility.image);//uid);

        // clear character movement values
        turnCharacter.model.movement.Reset();

		// restore health bars
		StartCoroutine(ShowHealthbars(true));

		// first, assemble all characterViews
		// List<CharacterView> crews = new List<CharacterView>();
		// crews.AddRange(_crewAttackGroup.pool);//GetComponentsInChildren<CharacterView>();
		// crews.AddRange(_crewDefendGroup.pool);

		////////////////////////////////////
		// add new effects
		////////////////////////////////////
		foreach (CharacterView c in allCharactersView)// crews)
		{
            // only characters with character results
            if (c.characterResultsVO == null)// || c.combatResultsVO.source == null)
                continue;
            // only process source character if not damanged (healed or booned)
            Debug.Log("=== " + c.model.handle + " ===");
            Debug.Log(c.combatResultsVO + " / " + c.combatResultsVO.source);
            if (c.combatResultsVO.source != null)
                Debug.Log("* sourceId " + c.combatResultsVO.source.uid);
            else Debug.Log("* source is nulll");
            Debug.Log(c.characterResultsVO);
            Debug.Log(c.model);
            if (c.combatResultsVO.source != null && c.combatResultsVO.source.uid == c.model.uid && c.characterResultsVO.healthModifier == HealthVO.HEALTH_MODIFIER_DAMAGE)
            {
				c.characterResultsVO = new CharacterResultsVO();//.effectModifier = 0;
				c.combatResultsVO = new CombatResultsVO();
				continue;
            }
			////////////////////////////////////
			// first, reduce and/or remove expired effects
			////////////////////////////////////

			// look for new/expiring effects
			Debug.Log(c.model.handle + " " + c.characterResultsVO.effect.effectModifier + " update? " + turnCompletePending);
            if (turnCompletePending == true)
                Debug.Log("<color=red>UPDATE " + c.model.handle + "</color>");

			// first, add new effects to targeted characters
			if (c.characterResultsVO.effect.effectModifier > 0)
            {
                Debug.Log("<color=red>effectMod " + c.characterResultsVO.effect.effectModifier + " / " + c.characterResultsVO.effectValue + "</color>");

				// get fx
				/*EffectVO vo = null;
                int rng = Random.Range(0, 13);
                switch (rng)
                {
				    // case 0: vo = EffectMatrixVO.GetBleedEffect(new EffectVO()); break;
                    // case 1: vo = EffectMatrixVO.GetPoisonEffect(new EffectVO()); break;
					// case 2: vo = EffectMatrixVO.GetBurnEffect(new EffectVO()); break;
					// case 3: vo = EffectMatrixVO.GetChillEffect(new EffectVO()); break;
                    // case 4: vo = EffectMatrixVO.GetConfusionEffect(new EffectVO()); break;
					// case 5: vo = EffectMatrixVO.GetBlindEffect(new EffectVO()); break;
					// case 6: vo = EffectMatrixVO.GetFearEffect(new EffectVO()); break;
					// case 7: vo = EffectMatrixVO.GetWeaknessEffect(new EffectVO()); break;
					// case 8: vo = EffectMatrixVO.GetAegisEffect(new EffectVO()); break;
					// case 9: vo = EffectMatrixVO.GetStabilityEffect(new EffectVO()); break;
                    // case 10: vo = EffectMatrixVO.GetMightEffect(new EffectVO()); break;
					// case 11: vo = EffectMatrixVO.GetRegenerationEffect(new EffectVO()); break;
					// case 12: vo = EffectMatrixVO.GetAlacrityEffect(new EffectVO()); break;
                    
					default: vo = EffectMatrixVO.GetPoisonEffect(new EffectVO()); break;
                }*/
				////////////////////////////////////
				// add new effect to charactervo
				////////////////////////////////////
				turnCompletePending = true;
                StartCoroutine(c.ProcessEffect(null, true));//c.characterResultsVO.effect));
				// c.model.AddEffect(vo);
				// // trigger floater
				// Vector3 temp = new Vector3(c.prefab.transform.position.x, c.prefab.transform.position.y + 2.05f, c.prefab.transform.position.z);
                // c.damageText(vo.label, temp, "default");
			}
            // Heal?
            else if (c.characterResultsVO.healthModifier > 0)
            {
				// typically, heals!
				// turnCompletePending = true;
				uiController.ShowHealthbars(true);
				StartCoroutine(c.ProcessHealth());//, vo));
            }
            else
            {
				////////////////////////////////////
				// update healthbars
				// combat dmg + effect dmg
				////////////////////////////////////
				// turnCompletePending = true;
				uiController.ShowHealthbars(true);
				c.model.updateHealth(HealthVO.HEALTH_MODIFIER_DAMAGE, c.characterResultsVO.healthValue + c.characterResultsVO.effectValue);

				// next turn
				// if (currentPhaseTotal == 1 || currentTurn != 1)
				// NextTurn();
			}

			////////////////////////////////////
			// determine expired effect cooldowns
			////////////////////////////////////

			////////////////////////////////////
			// determine expired ability cooldowns?
			////////////////////////////////////

			////////////////////////////////////
			// and deaths...
			////////////////////////////////////

			////////////////////////////////////
			// update turns for effects and health
			////////////////////////////////////

		}

        // if no character updates processed, force it here
        if (turnCompletePending == false)
            NextTurn();
	}

    /*void updateCharacterStatsUI(int state)
    {
        // state: 1=enter, 0=exit
        if (highlightedCharacter == null) return;

        GameObject panel;
        switch(highlightedCharacter.crewType)
        {
            case CharacterVO.CREW_TYPE_ATTACKER:
                Debug.Log("* attacker " + highlightedCharacter.handle);
                // stats panel
				panel = uiController.attackerInfoView.transform.Find("Panel").gameObject;
				// update stats
				panel.transform.Find("Handle").gameObject.GetComponent<TextMeshProUGUI>().text = highlightedCharacter.fullname;
				// handle.text = "Hello";
				// update history

				// if mouse exit, default to active character
				if (state == 0)
                {

                }
            break;

            case CharacterVO.CREW_TYPE_DEFENDER:
				Debug.Log("* defender " + highlightedCharacter.handle);
				// stats panel
				panel = uiController.defenderInfoView.transform.Find("Panel").gameObject;
				// update stats
				panel.transform.Find("Handle").gameObject.GetComponent<TextMeshProUGUI>().text = highlightedCharacter.fullname;

				// if mouse exit, default to active target (if set and single)
				if (state == 0)
				{

				}
				break;
        }
    }*/
	void cardEventHandlerFunction(EventParam eventParam)
	{
		Debug.Log("== LobbyController.cardEventHandlerFunction ==");
		Debug.Log("name " + eventParam.name);
		Debug.Log("card # selected" + eventParam.value);
        
        CardVO vo = null;
        CardLevelDataVO action = null;
        if (eventParam.card)
        {
            vo = eventParam.card.model;
            action = vo.levelData[vo.level - 1];
        }
        
        switch(eventParam.name)
        {
            case "dealCards":
				StartCoroutine(GameManager.instance.cardController.DealCards(true));
            break;
        /*if (eventParam.name == "dealerCardSelected")
        {
            switch (eventParam.value)
            {
                case 0: // <-- bug! 1 is registering as 0???
                case 1:
                    Debug.Log("* removing cards 2 & 3");
					// scale it down
					// card1.transform.localScale -= new Vector3(card1.transform.localScale.x / 2, 0, card1.transform.localScale.z / 2);//Vector3.one * 0.1f;				// stow cards!
					StartCoroutine(stowCards(card1, this.card2, this.card3));
					break;
                case 2:
                    Debug.Log("* removing cards 1 & 3");
					// scale it down
					// card2.transform.localScale -= new Vector3(card2.transform.localScale.x / 2, 0, card2.transform.localScale.z / 2);//Vector3.one * 0.1f;				// stow cards!
					StartCoroutine(stowCards(card2, this.card1, this.card3));
					break;
                case 3:
                    Debug.Log("* removing cards 1 & 2");
					// scale it down
					// card3.transform.localScale -= new Vector3(card3.transform.localScale.x / 2, 0, card3.transform.localScale.z / 2);//Vector3.one * 0.1f;
                    // stow cards!
                    StartCoroutine(stowCards(card3, this.card1, this.card2));
                    break;
            }
        }*/
        case "stowedCardActivated":
            Debug.Log("* stowedCardActivated");
            Debug.Log("* card # Selected" + eventParam.card.model.levelData[0]);
            switch(eventParam.card.model.targetType)
            {
                case CardVO.TargetType.Cards:
                    uiController.calloutPanel.SetActive(true);
                break;

                case CardVO.TargetType.Character:

                    if (vo.targetSource == CardVO.TargetSource.Self)
                    {
                        Debug.Log("action card!");
                        StartCoroutine(GameManager.instance.cardController.CardActionOnPlayer(eventParam.card, turnCharacter));
                        // StartCoroutine(eventParam.card.CardActionOnPlayer(turnCharacter));
                        // turnCharacter.model.AssignedCard(vo, action);
                    }
                    else ArrowRendererActivate(true, eventParam.card);
                break;

                case CardVO.TargetType.None:
                    // move card to center, dissipate
                break;
            }
        break;
            // StartCoroutine(ActivateCard(eventParam.card, eventParam.value));
			// ArrowRendererActivate(true, eventParam.value);
            // unparent card from stack
            /*eventParam.card.transform.parent = null;

			// reindex stack
			attackStack.GetComponent<CardStackController>().ReindexStack();

			// move card to center stage
			Vector3 pointA = eventParam.card.transform.position;
            Vector3 pointB = new Vector3(-3f, 3, eventParam.card.transform.position.z);

			StartCoroutine(MoveObject(eventParam.card.transform, pointA, pointB, 0.5f));
            yield return new WaitForSeconds(0.75f);
			ArrowRendererActivate(true, 0);//eventParam.value);*/
		}
        // else if (eventParam.name == "activeCardStowed")
        // {
		// 	Debug.Log("* activeCardStowed");
        //     StartCoroutine(stowCards(eventParam.card.gameObject, null, null));
        // }
    }
    IEnumerator ActivateCard(PlayingCardView card, int index)
    {
        // set active card
        activeCard = card;

		// unparent card from stack
		card.transform.parent = null;

		// reindex stack
		attackStack.GetComponent<CardStackController>().ReindexStack();

		// move card to center stage
		Vector3 pointA = card.transform.position;
		Vector3 pointB = new Vector3(-3f, 3, card.transform.position.z);

		StartCoroutine(MoveObject(card.transform, pointA, pointB, 0.5f));
		yield return new WaitForSeconds(0.75f);
		ArrowRendererActivate(true);//, 0);//index);
	}

    void ArrowRendererActivate(bool b, CardView card = null)//int stowedCardIndex=-1)
    {
        if (b == true)
        {
			// Vector3 cardPos = Camera.main.WorldToScreenPoint(new Vector3(card.transform.position.x, 0, 0));
			Vector3 cardPos = Camera.main.ScreenToWorldPoint(new Vector3(card.transform.position.x, card.transform.position.y, Camera.main.nearClipPlane));
			// GameObject arrowRenderer = GameObject.FindWithTag("ArrowRenderer");
			// arrowRenderer.SetPositions
			// ArrowRenderer arLocal = arrowRenderer.GetComponent<ArrowRenderer>();
            // cardPos.z = -5f;//-8f;
			Debug.Log("stowed " + cardPos.x + " / " + cardPos.y + " / " + cardPos.z);
			arrowRenderer.SetPositions(cardPos, new Vector3());
			arrowRendererGO.SetActive(true);//.gameObject.SetActive(true);
            arrowMouseFollower.start = cardPos;
            arrowMouseFollower.isActive = true;
		}
        else {
			arrowRendererGO.SetActive(false);
        }
    }

	void CombatComplete()
    {
        Debug.Log("<color=yellow>== SceneActionMain.CombatComplete ==</color>");

		GameManager.instance.combatActionState = SceneActionMain.COMBAT_ACTION_STATE_INACTIVE;
        // deactivate filters
        if (GameManager.instance.activeCameraFilter != null)
            GameManager.instance.activeCameraFilter.Disabler();// cinecamController.EnableFilter(GameManager.instance.activeCameraFilter, false);
        // destory source
        // Destroy(this.combatSourceViewActive.prefab);
        this.combatSourceViewActive.doRelease();
        // this.combatSourceViewActive.targets.Clear();
        // DestroyImmediate(this.combatSourceViewActive);
        // destroy target(s)
		foreach (CharacterView i in this.combatTargetsViewActive)
		{
            // Destroy(i.prefab);
            // Destroy(i);
            if (i != null)
                i.doRelease();
		}
        // clear list
        this.combatTargetsViewActive.Clear();

		this.changeCombatActionState(SceneActionMain.COMBAT_ACTION_STATE_INACTIVE);

		// reset camera
		GameManager.instance.cinecamController.getCamByInt(1);
	}

    IEnumerator stowCards(GameObject stowed, GameObject other1, GameObject other2)
    {
		// // set as stowed
		// PlayingCardView stowedView = stowed.GetComponent<PlayingCardView>();
		// stowedView.SetStowed(true);// isStowed = true;
		// stowedCardTemp = stowedView;
        // // move
		// Vector3 pointA = stowed.transform.position;
		// // Vector3 screenBottomCenter = new Vector3(Screen.width / 2, Screen.height, Camera.main.nearClipPlane);
		// Vector3 cam = Camera.main.transform.position;
		// Vector3 screenBottomLeft = new Vector3(Screen.width / 4, Screen.height / 2, cam.z);//Camera.main.nearClipPlane);
        // // convert UI CardStack element via screenToWorldPoint
        // // move selected dealer card to this world point, then parent
		// Vector3 pointB1 = Camera.main.ScreenToWorldPoint(attackStack.transform.position);
        // Vector3 screenPos = Camera.main.WorldToScreenPoint(stowed.transform.position);
        // Debug.Log("<color=purple>* screenPos " + screenPos.x + " / " + screenPos.y + " / " + screenPos.z + "</color>");
        // Vector3 pointB = new Vector3(-2f, 0.45f, -5f);//GetComponent<Camera>().nearClipPlane);//Camera.main.transform.position.z);//, rect.position.z);//new Vector3(-5, 5, 0);
        // Debug.Log("<color=red>card stack " + pointB.x + " / " + pointB.y + "</color>");
		// yield return StartCoroutine(MoveObject(stowed.transform, pointA, pointB, 0.25f));
        // if (other1 != null)
        // {
        //     Vector3 pointC = other1.transform.position;
        //     Vector3 pointD = other1.transform.position;
        //     pointD.y = 8;
        //     pointD.x = 8;
        //     yield return StartCoroutine(MoveObject(other1.transform, pointC, pointD, 0.25f));
        //     Destroy(other1, 0.3f);
        // }
        // if (other2 != null)
        // {
        //     Vector3 pointE = other2.transform.position;
        //     Vector3 pointF = other2.transform.position;
        //     pointF.y = 8;
        //     pointF.x = 8;
        //     yield return StartCoroutine(MoveObject(other2.transform, pointE, pointF, 0.25f));
        //     Destroy(other2, 0.3f);
        // }

        // // yield return new WaitForSeconds(2);

        // // remove
        // // if (other1 != null)
        // //     Destroy(other1);
        // // if (other2 != null)
        // //     Destroy(other2);
        // // Destroy(stowed);

        // // place stowed card in UI window
        // stowed.layer = 8;
		// stowed.transform.SetParent(attackStack.transform, true);
        // Debug.Log("* attackStack " + attackStack);
        // attackStack.GetComponent<CardStackController>().NewCardAdded(stowed);

		// // restore turnCounter
		// turnCounter.SetActive(true);

        // // clear activeCard
        // activeCard = null;
        yield return null;
    }

    IEnumerator dealCards()
    {
        Debug.Log("<color=yellow>== SceneActionMain.dealCards ==</color>");
		if (turnCounter == null)
			turnCounter = GameObject.FindWithTag("TurnCounter");

		yield return new WaitForSeconds(1.5f);
		// hide turn counter UI
		turnCounter.SetActive(false);

        // activate cards
		// cardsUI.SetActive(true);
        
		//*
        addCards();
		Vector3 pointA = this.card1.transform.position;
        // float baseZ = transform.forward + 1;
        pointA.x += 1.5f;
		Vector3 pointB = new Vector3(0 + 0.75f, 3f, -5f);
		// time
		float seconds = 0.15f;
		yield return StartCoroutine(MoveObject(this.card1.transform, pointA, pointB, seconds));

		// yield return new WaitForSeconds(0.15f);
		pointA = this.card2.transform.position;
		pointA.x += 1.5f;
		Vector3 pointC = new Vector3(0, 3f, -5f);//pointA.z);
		yield return StartCoroutine(MoveObject(this.card2.transform, pointA, pointC, seconds));

		// yield return new WaitForSeconds(0.15f);
		pointA = this.card3.transform.position;
		pointA.x += 1.5f;
		Vector3 pointD = new Vector3(0 - 0.75f, 3f, -5);//pointA.z);
		yield return StartCoroutine(MoveObject(this.card3.transform, pointA, pointD, seconds));
        //*/

		yield return null;
	}

    // IEnumerator StartCam1()
    // {
	// 	GameManager.instance.cinecamController.idleWindow = this.idleWindow;
	// 	GameManager.instance.cinecamController.getCamByInt(1);

	// 	yield return new WaitForSeconds(2);

    //     // start round
    //     // TODO: execute RoundBegin once the scene has completely loaded (camera event?)
    //     PhaseBegin();
    //     // RoundBegin();
	// 	// StartCoroutine("dealCards");
	// 	// StartCoroutine("buildHealthbars");
	// }

	IEnumerator changeCam1()
    {
        Debug.Log("* changeCam1");
        // yield return new WaitForSeconds(0.1f);
		// CinemachineVirtualCamera cam1 = cinecamController.getCamByInt(1);
		// cam1.gameObject.SetActive(false);


		GameManager.instance.cinecamController.combatSourceViewActive = this.combatSourceViewActive;
		GameManager.instance.cinecamController.getCamByInt(2);
        // cam.gameObject.SetActive(true);


		// vcam2 = new GameObject("VirtualCamera").AddComponent<CinemachineVirtualCamera>();
		// // vcam1.m_LookAt = idleWindow.transform;//GameObject.Find("Cube").transform;
		// // CinemachineGroupComposer group = vcam1.AddComponent("CinemachineGroupComposer")();
		// // Debug.Log("* group " + group);
		// vcam2.m_Follow = combatSourceViewActive.prefab.transform;
		// vcam2.m_Priority = 20;
		// vcam2.m_Lens.FieldOfView = 20; // zoom
		// vcam2.gameObject.transform.position = new Vector3(0, 1, 0);

		// // frame shot
		// var composer = vcam2.AddCinemachineComponent<CinemachineFramingTransposer>();
		// composer.m_ScreenY = 0.90f;
		// composer.m_AdjustmentMode = CinemachineFramingTransposer.AdjustmentMode.DollyThenZoom;
        // vcam2.gameObject.SetActive(true);



        // m_FollowOffset = new Vector3(0, 
        // zoom in
		// var transposer = vcam2.GetCinemachineComponent<CinemachineFramingTransposer>();
		// transposer.m_AdjustmentMode = CinemachineFramingTransposer.AdjustmentMode.ZoomOnly;// m_FollowOffset = new Vector3(0, 1, 2);
		// composer.m_DeadZoneWidth = 0.1f;//30f;
		// composer.m_DeadZoneHeight = 0.1f;//35f;

        // vcam1.m_SetActive = false;
        // vcam2.m_SetActive = true;
        yield return null;
	}

    IEnumerator startCam2()
    {
        yield return new WaitForSeconds(1);

		vcam2 = new GameObject("VirtualCamera").AddComponent<CinemachineVirtualCamera>();
		vcam2.m_Follow = combatWindow.transform;// prefab.transform;
		vcam2.m_Priority = 30;
		// vcam1.m_SetActive = false;
        // vcam2.gameObject.SetActive = true;
		// vcam1.gameObject.transform.position = new Vector3(0, 1, 0);
		// vcam2.gameObject.transform.position = new Vector3(prefab.transform.position.x + 6f, prefab.transform.position.y + 1, prefab.transform.position.z - 6);

		// frame shot
		var composer = vcam2.AddCinemachineComponent<CinemachineFramingTransposer>();
		composer.m_ScreenY = 0.80f;
		composer.m_DeadZoneWidth = 0.30f;
		composer.m_DeadZoneHeight = 0.35f;
	}

	private void ccRepositionCrew(CharacterView sourceView, int targetPos)
	{
        // CharacterEvent => "moveTo"
		Debug.Log("<color=yellow>== SceneActionMain.ccRepositionCrew ==</color>");
        Debug.Log("targetPos " + targetPos);

		// create animation movers
		List<AnimationMover> movers = new List<AnimationMover>();

		bool isMovingUp = (sourceView.model.position > targetPos) ? true : false;

		// which crew?
		CrewView targetCrew;
        List<Vector3> positionList;
		if (sourceView.model.crewType == CharacterVO.CREW_TYPE_DEFENDER)
        {
			targetCrew = _crewDefendGroup;
			positionList = crewDefendersPositions;
        }
		else
        { 
            targetCrew = _crewAttackGroup;
			positionList = crewAttackersPositions;
        }

        // get source current position
        int sourcePosition = sourceView.model.position;

        // reorder positions
        foreach(CharacterView c in targetCrew.pool)
        {
            if (c.model.uid == sourceView.model.uid)
            {
				// move to target position
                sourceView.model.positionTemp = targetPos;
				movers.Add(new AnimationMover(ListHelper.getCharacterViewByPosition(targetCrew.pool, targetPos).prefab.transform.position, sourceView.prefab));
				// move source health/effect bar container
				Vector3 _x = Camera.main.WorldToScreenPoint(positionList[targetPos - 1]);
				movers.Add(new AnimationMover(new Vector3(_x.x, sourceView.healthbarContainer.transform.position.y, 0), sourceView.healthbarContainer));
			}
            else if (c.model.position < targetPos) // character in front of target position
            {
                if (sourceView.model.position < c.model.position)
                {
					// move up 1 spot
                    c.model.positionTemp = c.model.position - 1;
					movers.Add(new AnimationMover(ListHelper.getCharacterViewByPosition(targetCrew.pool, c.model.positionTemp).prefab.transform.position, c.prefab));
					// move source health/effect bar container
					Vector3 _x = Camera.main.WorldToScreenPoint(positionList[c.model.positionTemp - 1]);
					movers.Add(new AnimationMover(new Vector3(_x.x, c.healthbarContainer.transform.position.y, 0), c.healthbarContainer));
				}
            }
            else if (c.model.position > targetPos) // character behind target position
            {
                if (sourceView.model.position > c.model.position)
                {
					// move back 1 spot
					c.model.positionTemp = c.model.position + 1;
					movers.Add(new AnimationMover(ListHelper.getCharacterViewByPosition(targetCrew.pool, c.model.positionTemp).prefab.transform.position, c.prefab));
					// move source health/effect bar container
					Vector3 _x = Camera.main.WorldToScreenPoint(positionList[c.model.positionTemp - 1]);
					movers.Add(new AnimationMover(new Vector3(_x.x, c.healthbarContainer.transform.position.y, 0), c.healthbarContainer));
				}
            }
            else // character occupies target position
            {
                if (c.model.position < sourceView.model.position)
                {
					// move back 1 spot
					c.model.positionTemp = c.model.position + 1;
					movers.Add(new AnimationMover(ListHelper.getCharacterViewByPosition(targetCrew.pool, c.model.positionTemp).prefab.transform.position, c.prefab));
					// move source health/effect bar container
					Vector3 _x = Camera.main.WorldToScreenPoint(positionList[c.model.positionTemp - 1]);
					movers.Add(new AnimationMover(new Vector3(_x.x, c.healthbarContainer.transform.position.y, 0), c.healthbarContainer));
				}
                else
                {
					// move up 1 spot
					c.model.positionTemp = c.model.position - 1;
					movers.Add(new AnimationMover(ListHelper.getCharacterViewByPosition(targetCrew.pool, c.model.positionTemp).prefab.transform.position, c.prefab));
					// move source health/effect bar container
					Vector3 _x = Camera.main.WorldToScreenPoint(positionList[c.model.positionTemp - 1]);
					movers.Add(new AnimationMover(new Vector3(_x.x, c.healthbarContainer.transform.position.y, 0), c.healthbarContainer));
				}
            }
        }

        // done sorting, now assign positionTemp to position;
        foreach(CharacterView c in targetCrew.pool)
        {
            if (c.model.positionTemp > 0)
                c.model.position = c.model.positionTemp;
            c.model.positionTemp = 0; // reset
        }

        // move them!
		StartCoroutine(MoveObjects(movers, 0.45f));

	}

	private void ccSwitchPositions(CharacterView sourceView, int targetPos)
    {
        // CharacterEvent => "switchTo"
        Debug.Log("<color=yellow>== ccSwitchPositions ==</color>");

        // which crew?
        CrewView targetCrew;
        List<Vector3> positionList;
        if (sourceView.model.crewType == CharacterVO.CREW_TYPE_DEFENDER)
        {
            targetCrew = _crewDefendGroup;
            positionList = crewDefendersPositions;
        }
        else 
        { 
            targetCrew = _crewAttackGroup;
            positionList = crewAttackersPositions;
        }

        // get target view
        CharacterView targetView = ListHelper.getCharacterViewByPosition(targetCrew.pool, targetPos);

        // create animation movers
        List<AnimationMover> movers = new List<AnimationMover>();
        movers.Add(new AnimationMover(positionList[targetPos - 1], sourceView.prefab));
        movers.Add(new AnimationMover(positionList[sourceView.model.position - 1], targetView.prefab));
		
        // move source health/effect bar container
        Vector3 _x = Camera.main.WorldToScreenPoint(positionList[targetPos - 1]);
		movers.Add(new AnimationMover(new Vector3(_x.x, sourceView.healthbarContainer.transform.position.y, 0), sourceView.healthbarContainer));
        // ... and target
        _x = Camera.main.WorldToScreenPoint(positionList[sourceView.model.position - 1]);
		movers.Add(new AnimationMover(new Vector3(_x.x, targetView.healthbarContainer.transform.position.y, 0), targetView.healthbarContainer));

		// switch position values
		int[] newPositions = { targetView.model.position, sourceView.model.position };
        sourceView.model.position = newPositions[0];
        targetView.model.position = newPositions[1];

        // if source character is turnCharacter, move activeIndicator
        if (sourceView.model.uid == turnCharacter.model.uid)
        {
            movers.Add(new AnimationMover(new Vector3(positionList[targetPos - 1].x, activeIndicator.transform.position.y, activeIndicator.transform.position.z), activeIndicator));
        }

        // update abilitybar for active character
        uiController.BuildAbilityBar(sourceView.model);
        // animation!
        // foreach(AnimationMover m in movers)
        //     Debug.Log(m.endPosition);
        StartCoroutine(MoveObjects(movers, 0.45f));
    }
    IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        Debug.Log("== MoveObject ==");

        /*var i = 0.0f;
        var rate = 1.0f / time;
        while (i < 1.0f)
        {
            i += Time.deltaTime / time;// * rate;
			thisTransform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0.0f, 1.0f, i));
			yield return null;
        }*/

		// AnimationCurve curve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
		thisTransform.DOMove(endPos, time)
			.ChangeStartValue(startPos)
			.SetEase(moveCurve);
        yield return null;

	}
    IEnumerator MoveObjects(List<AnimationMover> list, float time)
    {
        Debug.Log("== MoveObjects ==");
        
        foreach(AnimationMover mover in list)
        {
            Debug.Log("mover " + mover.endPosition.x);
            StartCoroutine(MoveObject(mover.obj.transform, mover.obj.transform.position, mover.endPosition, time));
        }
        yield return null;
    }
    IEnumerator ShakeObject(Transform thisTransform, float speed, float amount)
    {
        float i = 0.0f;
        float time = 1;
        float rate = 1.0f / time;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = new Vector3(Mathf.Sin(Time.time * speed) * amount, thisTransform.position.y, thisTransform.position.z);
            yield return null;
        }
    }
	void StartMapHandler()
	{
		Debug.Log("* start map...");
		StartCoroutine(LoadNewScene());
	}

	IEnumerator LoadNewScene()
	{
		AsyncOperation async = SceneManager.LoadSceneAsync("Sprawl Map", LoadSceneMode.Single);
		while (!async.isDone)
		{
			Debug.Log("loading...");
			yield return null;
		}
	}

	// restore health bars
	IEnumerator ShowHealthbars(bool b = true)
    {
        yield return new WaitForSeconds(1.0f);
		uiController.ShowHealthbars(b);
	}

	// getters & setters
	public bool turnCompletePending
	{
		get { return _turnCompletePending; }
		set 
        { 
            _turnCompletePending = value; 
        }
	}

	public AbilityVO currentAbility
	{
		get { return _currentAbility; }
		set
		{
            _currentAbility = value;
			GameManager.instance.currentAbility = value;
		}
    }
}

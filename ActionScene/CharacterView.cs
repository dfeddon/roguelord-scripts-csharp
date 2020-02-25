using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
// using System;
// using UnityStandardAssets.ImageEffects;
public class CharacterView : MonoBehaviour {
    public const string CHARACTER_PREFAB_ANARCHIST = "Cyborg_Melee 1";
	public const string CHARACTER_PREFAB_CHROMER = "Cyborg_Kamikaze 1";
	public const string CHARACTER_PREFAB_MEDIC = "Cyborg_Soldier 1";
	public const string CHARACTER_ANIMATOR_CONTROLLER_ANARCHIST = "Cyborg_MeleeController";
    public const string CHARACTER_ANIMATOR_CONTROLLER_CHROMER = "Cyborg_KamikazeController";
    public const string CHARACTER_ANIMATOR_CONTROLLER_MEDIC = "Cyborg_SoldierController";
    // public const string CHARACTER_ANIMATOR_CONTROLLER_TURRET = "TurretController";
    // public const string CHARACTER_ANIMATOR_CONTROLLER_HERO = "HeroController";
    public const string CHARACTER_ANIMATION_WALK = "Walk";
    public const string CHARACTER_ANIMATION_IDLE = "Idle";
    public const string CHARACTER_ANIMATION_HIT = "Hit";
    public const string CHARACTER_ANIMATION_ATTACK_BEGIN = "AttackBegin";
    public const string CHARACTER_ANIMATION_SKILL_1 = "Skill_0";
    public const string CHARACTER_ANIMATION_SKILL_2 = "Skill_1";
    public const string CHARACTER_ANIMATION_SKILL_3 = "Skill_2";
    public const string CHARACTER_ANIMATION_SKILL_4 = "Skill_3";
    public const string CHARACTER_ANIMATION_SKILL_5 = "Skill_4";
	public const string CHARACTER_ANIMATION_RUN_TO_STOP = "RunToStop";

	private CharacterVO _model;
    private Vector3 size;
    // private bool floaterBegin = false;
    // private TextMesh floaterTextMesh;
    // private SpriteRenderer floaterSprite;
    private GameObject floatContainer;
    public GameObject prefab;
	public GameObject healthbarContainer;
	public GameObject capsuleContainer;
    public HealthbarComponent healthbar;
    public EffectsbarComponent effectsbar;
	public GameObject capsule;
	public GameObject actions;
    public CharController charController;
    public Animator animator;
    public Rigidbody rigidBody;
	public AudioSource audioSource;
    public TimeManager timeManager;
    // public GameObject combatView;
	// private Action<EventParam> animationEventHandler;
	// public Blur blur;
	// private int lastStateKey = 0;
    private CombatResultsVO _combatResultsVO;
    public CharacterResultsVO characterResultsVO;
    public CharacterNextTurnResultsVO characterNextTurnResultsVO;

	public GameObject[] TextFloat;
	public int index = 0;
	public float LifeTime = 5;
    public GameObject currentEffect;
    public bool isTarget = false;
    public bool isSource = false;
    public List<CharacterView> targets;
    public List<CharacterView> colleagues;
    public AbilityVO activeAbility;
    private CharacterView primaryTarget;
    private bool hasHit = false;
	private Vector3 collisionPoint;// = new Vector3();
	private GameObject sidecar;
	private UnityAction<EventParam> characterEventHandler;
	// public ParticleSystem part;
	public List<ParticleCollisionEvent> collisionEvents;
	// private GameObject fxRef = null;
	private List<EffectItemVO> effectsQueueList = new List<EffectItemVO>();
	private List<EventParam> fxQueueList = new List<EventParam>();

	// CinemachineVirtualCamera vcam2;


	void Awake () 
    {
        Debug.Log("== CharacterView.Awake ==");
        characterEventHandler = new UnityAction<EventParam>(characterEventHandlerFunction);
    }
    // Use this for initialization
    void Start () 
    {
        // Instantiate(prefab);//, pos, Quaternion.identity);
		Debug.Log("== CharacterView.Start ==");//, this);
        // size = this.GetComponent<SpriteRenderer>().bounds.size * this.transform.localScale.x;
        // GetComponent<UnityStandardAssets.ImageEffects.MotionBlur>().enabled = true;
        animator = prefab.GetComponent<Animator>();
        charController = prefab.GetComponent<CharController>();
        // rigidBody = charController.transform.GetComponent<Rigidbody>();
		audioSource = this.gameObject.AddComponent<AudioSource>();
		// audioSource = prefab.GetComponent<AudioSource>();
        timeManager = prefab.AddComponent<TimeManager>();

		if (charController.isIdleCharacter != true)
		{
			// part = gameObject.AddComponent<ParticleSystem>();
			collisionEvents = new List<ParticleCollisionEvent>();
		}

		// renderer = prefab.GetComponent<Renderer>();
		// capCollider = prefab.GetComponent<CapsuleCollider>();
		// set isTrigger for mouse event support
		// if (capCollider)
		// capCollider.isTrigger = true;

		// Debug.Log("<color=yellow>HI</yellow>");
		// pass in ref to self
		// Debug.Log("<color=blue>fx " + charController + "</color>");
		if (charController)
		    charController.characterView = this;
		// Debug.Log("* got animator " + animator + "/" + charController + "/" + rigidBody);
		// EventManager.StartListening("animationEvent", animationEventHandler);

		EventManager.StartListening("characterEvent", characterEventHandler);

		EventParam eventParam = new EventParam();
		eventParam.name = "characterCreated";
		EventManager.TriggerEvent("combatEvent", eventParam);

	}

	void OnDisable()
	{
		if (model.sidecar != null && model.sidecar.isActivated != true)
		{
			Debug.Log("== adding sidecar ==");
			model.sidecar.isActivated = true;
			// if defending crew is attacker then flip position/rotation offsets
			if (model.crewType == CharacterVO.CREW_TYPE_DEFENDER)
			{
				// activeAbility.rotationOffset.x *= -1;
				// activeAbility.rotationOffset.y *= -1;
				// activeAbility.rotationOffset.z *= -1;
			}
			Vector3 updatePosition = new Vector3(prefab.transform.position.x, 0, -0.5f);

			// position offsets
			// Vector3 updatePosition = new Vector3(
			// 	prefab.transform.position.x + activeAbility.positionOffset.x,
			// 	prefab.transform.position.y + activeAbility.positionOffset.y,
			// 	prefab.transform.position.z + activeAbility.positionOffset.z
			// );
			Vector3 updateRotation = new Vector3(
				prefab.transform.rotation.x + activeAbility.rotationOffset.x,
				prefab.transform.rotation.y + activeAbility.rotationOffset.y,
				prefab.transform.rotation.z + activeAbility.rotationOffset.z
			);
			sidecar = Instantiate(model.sidecar.prefab, updatePosition, Quaternion.Euler(updateRotation));//Quaternion.identity);
			sidecar.transform.localScale = Vector3.one * activeAbility.localScaleOffset.x;

		}
	}

    // Update is called once per frame
    // void Update () 
    // {
    //     if (floaterBegin == true)
    //     {
    //         floaterBegin = false;
    //         StartCoroutine("floatAnimation");
    //     }


		/*if (Input.GetMouseButtonDown(0))
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100))
			{
                GameObject go = (GameObject)Resources.Load("Prefabs/FloatText", typeof(GameObject));
				if (go)
				{
                    // Debug.Log("HI");
					GameObject ob = GameObject.Instantiate(go, hit.point, Quaternion.identity);
					GameObject.Destroy(ob, LifeTime);
					FloatingText floattext = ob.GetComponentInChildren<FloatingText>();
					if (floattext != null)
						floattext.SetText("+" + Random.Range(5, 100));
				}
			}
        }*/


	// }

	void characterEventHandlerFunction(EventParam eventParam)
    {
        // Debug.Log("<color=green>characterEventHandlerFunction</color>");
		// Debug.Log("<color=green>" + eventParam.name +  "</color>");

		// validate user (value is source's uid)
		if (eventParam.value != model.uid) return;

        if (this == null) return; // in the event a vo has been removed (eg: has died)...

		Debug.Log("<color=green>characterEventHandlerFunction</color>");
		Debug.Log("<color=green>" + eventParam.name + "</color>");

		switch(eventParam.name)
        {
            case "effectAdd":
				// move eventParam.data to slot1
				EffectProcessQueue(eventParam, false);
                // effectsbar.UpdateEffectsBar(EffectVO.EFFECT_UPDATE_TYPE_ADD, effect, eventParam.effectsList);
				// ApplyEffectToCharacter(effect.uid, 1);
            break;

			case "effectUpdate":
				// simply cycling extant effects
				// StartCoroutine(ProcessEffect(eventParam.data as EffectVO, false));
				EffectProcessQueue(eventParam, false);
				// update effects bar
				// effectsbar.UpdateEffectsBar(EffectVO.EFFECT_UPDATE_TYPE_UPDATE, effect, eventParam.effectsList);
				// ApplyEffectToCharacter(effect.uid, 2);
				break;

			case "effectRemove":
				// move eventParam.data to slot1
				EffectVO effect = eventParam.data as EffectVO;
				// remove last-used currentDuration
				effectsbar.UpdateEffectsBar(EffectVO.EFFECT_UPDATE_TYPE_REMOVE, effect, eventParam.effectsList);
			break;

            case "healthUpdate":
				// if (healthbar.gameObject.activeSelf != true) return;
				if (GameManager.instance.roundIsLocked == true) return;

				healthbar.gameObject.SetActive(true);
                healthbar.UpdateHealthBar(eventParam.value2, eventParam.data as HealthVO);
            break;

			case "actionIndicatorAdd":
				Debug.Log("* actionIndicatorAdd");
			break;

			case "dialogue":
				GameObject temp = new GameObject();
				Destroy(temp, 5);
				Transform trans = temp.transform;
				Vector3 newPos = new Vector3(prefab.transform.position.x, prefab.transform.position.y + 1.55f, prefab.transform.position.z);
				if (model.crewType == CharacterVO.CREW_TYPE_DEFENDER)
					newPos.x -= 0.35f;
				trans.position = newPos;
				VikingCrew.Tools.UI.SpeechBubbleManager.SpeechbubbleType speechType = VikingCrew.Tools.UI.SpeechBubbleManager.SpeechbubbleType.NORMAL;
				if (eventParam.value2 == 1)
					speechType = VikingCrew.Tools.UI.SpeechBubbleManager.SpeechbubbleType.THINKING;
				if (model.crewType == CharacterVO.CREW_TYPE_DEFENDER)
				{
					if (eventParam.value2 == 1)
						speechType = VikingCrew.Tools.UI.SpeechBubbleManager.SpeechbubbleType.ANGRY;
					else speechType = VikingCrew.Tools.UI.SpeechBubbleManager.SpeechbubbleType.SERIOUS;
				}
				VikingCrew.Tools.UI.SpeechBubbleManager.Instance.AddSpeechBubble(trans, eventParam.message, speechType);
			break;

			case "attackUpdate":
				// only if out of combat
				if (GameManager.instance.combatActionState == SceneActionMain.COMBAT_ACTION_STATE_INACTIVE)
					healthbar.UpdateAttack(eventParam.value2, eventParam.value3);
			break;

			case "defenseUpdate":
				// only if out of combat
				if (GameManager.instance.combatActionState == SceneActionMain.COMBAT_ACTION_STATE_INACTIVE)
					healthbar.UpdateDefense(eventParam.value2, eventParam.value3);
			break;

			case "oopStateUpdated":
				Debug.Log("oopStateUpdated " + model.oopState);
				if (charController.isIdleCharacter == true)
					charController.SetHightLightOOP();
			break;

			case "powerUpdate":
				// GameManager.instance.uiController.powerBankA = model.attributes.power;
				GameManager.instance.uiController.updateAttackerPowerUI(model.attributes);
			break;
		}
    }

	private void EffectProcessQueue(EventParam eventParam, bool inQueue = false)
	{
		// add to queue
		if (inQueue != true)
			fxQueueList.Insert(0, eventParam);
		if (fxQueueList.Count > 1)
			return;

		EffectVO effect = eventParam.data as EffectVO;

		switch (eventParam.name)
		{
			case "effectAdd":
				// move eventParam.data to slot1
				effectsbar.UpdateEffectsBar(EffectVO.EFFECT_UPDATE_TYPE_ADD, effect, eventParam.effectsList);
				ApplyEffectToCharacter(effect.uid, 1);
				break;

			case "effectUpdate":
				// simply cycling extant effects
				StartCoroutine(ProcessEffect(eventParam.data as EffectVO, false));
				// update effects bar
				effectsbar.UpdateEffectsBar(EffectVO.EFFECT_UPDATE_TYPE_UPDATE, effect, eventParam.effectsList);
				ApplyEffectToCharacter(effect.uid, 2);
				break;
		}
	}

	public void DoLevelUp()
	{
		GameObject fx;
		GameObject asset;
		audioSource.PlayOneShot(new SoundVO(1, "Level Up", "Quest_Game_Magic_Level_Up_Blast_Impact_1v_Positive_Win").audioClip, 0.5f);
		asset = GameManager.instance.assetBundleCombat.LoadAsset<GameObject>("Level_Up_FX Variant");
		fx = Instantiate(asset) as GameObject;
		fx.transform.SetParent(prefab.transform);
		asset.transform.localPosition = Vector3.zero;
		fx.transform.position = new Vector3(prefab.transform.position.x, 0, 0);
		Destroy(fx, 8.0f);
	}

	private void ApplyEffectToCharacter(int effectId, int state)
	{
		// DoLevelUp();return;
		Debug.Log("<color=green>== ApplyEffectToCharacter ==</color>");
		Debug.Log("effectId " + effectId + " / state " + state);
		// state 1 = Add, state 2 = Update

		// GameObject asset = null;
		// GameObject fx = null;
		EffectItemVO effectItem = new EffectItemVO();
		switch (effectId)
		{
			// poison
			case 100:
				// VO
				if (state == 1)
					audioSource.PlayOneShot(new SoundVO(1, "Help", "BarbarianVP_HelpSoft2").audioClip, 0.75f);
				else audioSource.PlayOneShot(new SoundVO(1, "Dark", "Quest_Game_Award_Positive_Star_3_Dark").audioClip, 0.75f);
				// poison fx
				effectItem.asset = GameManager.instance.assetBundleCombat.LoadAsset<GameObject>("Poisonous puddle Variant");
				effectItem.asset.transform.localScale = new Vector3(0.5f, 1, 3);
				effectItem.fx = Instantiate(effectItem.asset) as GameObject;
				effectItem.fx.transform.SetParent(prefab.transform);
				effectItem.fx.transform.localPosition = Vector3.zero;
				Destroy(effectItem.fx, 5.0f); // <-- TODO: 
				// effectItem.fx = fx;
				effectItem.autoDelete = false;
				// effectItem.numLoops = 5;

				break;
			// bleed
			case 101:
				// VO
				if (state == 1)
					audioSource.PlayOneShot(SoundLibraryVO.GetBleed(model.crewType).audioClip, 0.75f);
				else audioSource.PlayOneShot(new SoundVO(1, "Dark", "Quest_Game_Award_Positive_Star_3_Dark").audioClip, 0.75f);
				// bloodFX
				effectItem.asset = GameManager.instance.assetBundleCombat.LoadAsset<GameObject>("Magic circle 20 Variant");
				effectItem.fx = Instantiate(effectItem.asset) as GameObject;
				effectItem.fx.transform.SetParent(prefab.transform);
				effectItem.fx.transform.localPosition = Vector3.zero;
				// Destroy(fx, 2.0f);
				break;

			// burn
			case 102:
				// VO
				if (state == 1)
					audioSource.PlayOneShot(SoundLibraryVO.GetBurn(model.crewType).audioClip, 0.75f);
				else audioSource.PlayOneShot(new SoundVO(1, "Dark", "Quest_Game_Award_Positive_Star_3_Dark").audioClip, 0.75f);
				// fire fx
				effectItem.asset = GameManager.instance.assetBundleCombat.LoadAsset<GameObject>("Fire explosion Variant");
				effectItem.fx = Instantiate(effectItem.asset) as GameObject;
				effectItem.fx.transform.SetParent(prefab.transform);
				effectItem.fx.transform.localPosition = Vector3.zero;
				effectItem.fx.transform.position = new Vector3(effectItem.fx.transform.position.x, effectItem.fx.transform.position.y + 1, effectItem.fx.transform.position.z);

				// charController.highlightEffect.enabled = false;
				// charController.meshRenderer.enabled = true;
				// GameObject a = GameManager.instance.assetBundleCombat.LoadAsset<GameObject>("Effect7 Variant");
				// GameObject fx = Instantiate(a) as GameObject;
				// PSMeshRendererUpdater meshUpdater = fx.AddComponent<PSMeshRendererUpdater>();
				// meshUpdater.MeshObject = fx;

				// fx.transform.SetParent(charController.meshRenderer.transform);
				// meshUpdater.UpdateMeshEffect(prefab.MeshObject);// = fx;
				
				// Destroy(fx, 0.75f);
				break;

			// confusion
			case 103:
				// VO
				if (state == 1)
					audioSource.PlayOneShot(new SoundVO(1, "What Happened", "BarbarianVP_WhatHappenedSoft").audioClip, 0.75f);
				else audioSource.PlayOneShot(new SoundVO(1, "Dark", "Quest_Game_Award_Positive_Star_3_Dark").audioClip, 0.75f);
				// fire fx
				effectItem.asset = GameManager.instance.assetBundleCombat.LoadAsset<GameObject>("Fire explosion Variant");
				effectItem.fx = Instantiate(effectItem.asset) as GameObject;
				effectItem.fx.transform.SetParent(prefab.transform);
				effectItem.fx.transform.localPosition = Vector3.zero;
				// Destroy(fx, 0.75f);
				break;

			// might
			case 302:
				// if (state != 1) return;
				if (state == 1)
					audioSource.PlayOneShot(new SoundVO(1000, "Witness My Power", "BarbarianVP_WitnessMyPowerSoft").audioClip, 1.0f);
				else audioSource.PlayOneShot(new SoundVO(1, "Dark", "Quest_Game_Award_Positive_Star_3_Dark").audioClip, 0.75f);
				// fire fx
				effectItem.asset = GameManager.instance.assetBundleCombat.LoadAsset<GameObject>("Magic circle 5 Variant");
				effectItem.asset.transform.localScale = new Vector3(0.75f, 1, 1);
				effectItem.fx = Instantiate(effectItem.asset) as GameObject;
				effectItem.fx.transform.SetParent(prefab.transform);
				effectItem.fx.transform.localPosition = Vector3.zero;
				// animation
				charController.animator.SetTrigger("battlecry");
			break;

			default: return;
		}

		if (effectItem.fx != null)
		{
			// effectItem.fx = fx;
			if (effectsQueueList.Count == 0)
			{
				Debug.Log("<color=red>adding... " + effectId + "</color>");
				effectsQueueList.Add(effectItem);
				ParticleEffectsWrapper wrapper = effectItem.fx.AddComponent<ParticleEffectsWrapper>();
				wrapper.AddItem(effectItem);// = effectItem;//.wrapper = fx;
				wrapper.methodToCall = fxCallbackHandler;
				// fxRef = fx;
			}
			else // queue
			{
				Debug.Log("<color=red>queuing... " + effectId + "</color>");
				// add item
				effectsQueueList.Insert(0, effectItem);
				// and... wait...
			}
		}
	}

	public void fxCallbackHandler()
	{
		StartCoroutine(fxCallbackCoroutine());
	}
	IEnumerator fxCallbackCoroutine()
	{
		Debug.Log("<color=red>fxHandler</color>");
		// get eldest item
		EffectItemVO item = effectsQueueList[effectsQueueList.Count - 1];

		// 3.5 second minimum between effects (if more than one...)
		float additive = 0f;
		float diff = item.timeEnd - item.timeStart;
		if (diff < 3.5f)
			additive = 3.5f - diff;

		// remove it from list
		effectsQueueList.Remove(effectsQueueList[effectsQueueList.Count - 1]);
		// if not autodeleted, destroy it now
		if (item.autoDelete == true)
		{
			Destroy(item.fx);
			item = null;
		}
		// execute next effect (if available)
		if (effectsQueueList.Count > 0)
		{
			Debug.Log("<color=red>next effect item...</color>");
			EffectItemVO nextItem = effectsQueueList[effectsQueueList.Count -1];
			ParticleEffectsWrapper wrapper = nextItem.fx.AddComponent<ParticleEffectsWrapper>();//.wrapper = fx;
			wrapper.effectItem = nextItem;
			wrapper.methodToCall = fxCallbackHandler;
		}

		if (fxQueueList.Count > 0)
		{
			// EventParam e = fxQueueList[fxQueueList.Count - 1];
			fxQueueList.Remove(fxQueueList[fxQueueList.Count - 1]);

			if (fxQueueList.Count > 0)
			{
				if (additive > 0)
					yield return new WaitForSeconds(additive);

				EventParam e = fxQueueList[fxQueueList.Count - 1];
				EffectProcessQueue(e, true);
			}
		}

		yield return null;
	}

    /**

    **/
	public IEnumerator ProcessEffect(EffectVO vo = null, bool isNew = false)
	{
		Debug.Log("<color=yellow>CharacterView.ProcessEffects " + this.model.handle + " isNew " + isNew + "</color>");

        if (vo == null)
            vo = this.characterResultsVO.effect;
        
		////////////////////////////////////
		// add new effect to charactervo
		////////////////////////////////////
        if (isNew == true)
    		this.model.AddEffect(vo);
		// else
		// else if (characterNextTurnResultsVO.effect != null)
		// else vo.effectValue = characterNextTurnResultsVO.effectValue;
		// vo = characterNextTurnResultsVO.effect;
		// TODO: assign (and don't clear too soon) CharacterNextTurnResult value to model! because we don't have the damage value (characterResultsVO.effectValue)

		////////////////////////////////////
		// trigger effect label floater
		// *or* resist if effect does not succeed
		////////////////////////////////////
		Vector3 temp = new Vector3(this.prefab.transform.position.x, this.prefab.transform.position.y + 2.05f, this.prefab.transform.position.z);
		yield return new WaitForSeconds(0.5f);
		this.damageText(vo.label, temp, "default");

		// if damage/heal effect, trigger damage floater
		// should the below block require that effectValue > 0?
		switch (vo.effectModifier)
		{
			case EffectVO.EFFECT_MODIFIER_CONDITION:
				// if effect inflicts damange... and effectValue > 0
				yield return new WaitForSeconds(1.5f);
				////////////////////////////////////
				// trigger damage floater
				////////////////////////////////////
				Vector3 temp2 = new Vector3(this.prefab.transform.position.x, this.prefab.transform.position.y + 2.05f, this.prefab.transform.position.z);
				this.damageText("-" + this.characterResultsVO.effectValue.ToString(), temp2, "default");
			break;

			case EffectVO.EFFECT_MODIFIER_BOON:
				// if effect applies heal...and effectValue > 0
				yield return new WaitForSeconds(1.5f);
				////////////////////////////////////
				// trigger damage floater
				////////////////////////////////////
				Vector3 temp3 = new Vector3(this.prefab.transform.position.x, this.prefab.transform.position.y + 2.05f, this.prefab.transform.position.z);
				this.damageText("+" + this.characterResultsVO.effectValue.ToString(), temp3, "default");
				break;
		}

		yield return new WaitForSeconds(1.5f);
		////////////////////////////////////
		// update healthbars
		// combat dmg + effect dmg
		////////////////////////////////////
		// if (this.characterResultsVO.healthValue)
		this.model.updateHealth(HealthVO.HEALTH_MODIFIER_DAMAGE, this.characterResultsVO.healthValue + this.characterResultsVO.effectValue);

		////////////////////////////////////
		// finally, clear the now-expired characterResultsVO and combatResultsVO
		////////////////////////////////////
		characterResultsVO = new CharacterResultsVO();//.effectModifier = 0;
		combatResultsVO = new CombatResultsVO();

		// next turn (only if new)
		if (isNew == true)
		{
			EventParam eventParam = new EventParam();
			eventParam.name = "nextTurn";
			EventManager.TriggerEvent("combatEvent", eventParam);
		}
		// if (currentPhaseTotal == 1 || currentTurn != 1)
		/*if (turnCompletePending == true)
			NextTurn();*/
	}

	public IEnumerator ProcessHealth()//CharacterView c)//, HealthVO vo)
	{
		Debug.Log("<color=yellow>CharacterView.ProcessHealth " + this.model.handle + "</color>");
		string label = null;

		if (this.characterResultsVO.healthModifier == HealthVO.HEALTH_MODIFIER_HEAL)
		{
			label = "HEAL";

			////////////////////////////////////
			// trigger effect label floater
			// *or* resist if effect does not succeed
			////////////////////////////////////
			Vector3 temp = new Vector3(this.prefab.transform.position.x, this.prefab.transform.position.y + 2.05f, this.prefab.transform.position.z);
			this.damageText(label, temp, "default");

			yield return new WaitForSeconds(1.5f);
			////////////////////////////////////
			// trigger damage floater
			////////////////////////////////////
			Vector3 temp2 = new Vector3(this.prefab.transform.position.x, this.prefab.transform.position.y + 2.05f, this.prefab.transform.position.z);
			this.damageText("+" + this.characterResultsVO.healthValue.ToString(), temp2, "default");

			yield return new WaitForSeconds(1.5f);
		}
		////////////////////////////////////
		// update healthbars
		// combat dmg + effect dmg
		////////////////////////////////////
		this.model.updateHealth(this.characterResultsVO.healthModifier, this.characterResultsVO.healthValue + this.characterResultsVO.effectValue);

		////////////////////////////////////
		// finally, clear the now-expired characterResultsVO and combatResultsVO
		////////////////////////////////////
		// TODO: uncomment or move 2 lines below
		this.characterResultsVO = new CharacterResultsVO();//.effectModifier = 0;
		this.combatResultsVO = new CombatResultsVO();

		// next turn
		EventParam eventParam = new EventParam();
		eventParam.name = "nextTurn";
		EventManager.TriggerEvent("combatEvent", eventParam);
		// if (currentPhaseTotal == 1 || currentTurn != 1)
		/*if (turnCompletePending == true)
			NextTurn();*/
	}

	public void OnTriggerEnterHandler(Collider other)
	{
		Debug.Log("<color=green>== CharController.OnTriggerEnterHandler ==</color>");
        Debug.Log(other + " / " + model.handle);
		// animator.Play("Standing_React_Small_From_Front");
        isHit();
	}
	public void OnTriggerStayHandler(Collider other)
	{
		Debug.Log("<color=green>== CharController.OnTriggerStayHandler ==</color>");
		Debug.Log(other + " / " + model.handle);
	}
	public void OnTriggerExitHandler(Collider other)
	{
		Debug.Log("<color=green>== CharController.OnTriggerExitHandler ==</color>");
		// Time.timeScale = 0.0f;
		// StartCoroutine("bulletTimePause");
        // floater(0, "-35");
	}

	public void OnCollisionEnterHandler(Collision collision)
    {
        Debug.Log("<color=red>== CharacterView.OnCollisionEnterHandler ==</color>");
        // if (collision != null)
		//     Debug.Log("<color=red>" + collision.collider.name + "/" + model.handle + "</color>");
		// foreach (ContactPoint contact in collision.contacts)
		// {
		// 	Debug.DrawRay(contact.point, contact.normal, Color.white);
		// }
		if (collision != null && collision.gameObject.name != "Floor")
		{
			// Vector3 point = collision.contacts[0].point;
			// Debug.Log("<color=red>contact " + point.x + "/" + point.y + "/" + point.z + " </color>");
			// GameObject fx = GameManager.instance.assetBundleCombat.LoadAsset<GameObject>("Blood11_2 Variant");
			// GameObject cfx = Instantiate(fx) as GameObject;
			// cfx.transform.position = new Vector3(point.x, point.y, point.z);
			// Destroy(cfx, 1.75f);
			// GameObject cfx = Instantiate(fx, transform.position, new Quaternion()) as GameObject;
			// Debug.Log("<color=red>prefab " + cfx + " </color>");
			collisionPoint = collision.contacts[0].point;
		}
		
		isHit();
		
		// isHit(point);//collision);
		/*if (collision.collider.name == "Collision")
        {
			animator.Play("Standing_React_Large_From_Front");
			timeManager.DoSlowmotion();
			floater(0, "-35");
			StartCoroutine("bulletTimePause");
        }*/
	}
	public void OnCollisionStayHandler(Collision collision)
	{
		Debug.Log("<color=red>== CharacterView.OnCollisionStayHandler ==</color>");
        // Time.timeScale = 1;
	}
	public void OnCollisionExitHandler(Collision collision)
	{
		Debug.Log("<color=red>== CharacterView.OnCollisionStayHandler ==</color>");
	}

	public void OnParticleCollisionHandler(GameObject other)
    {
        Debug.Log("<color=red>==CharacterView.OnParticleCollision ==</color>");
        Debug.Log("<color=red>" + other + "</color>");

		int numCollisionEvents = other.GetComponent<ParticleSystem>().GetCollisionEvents(prefab, collisionEvents);

		if (numCollisionEvents > 0)
		{
			// Vector3 pos = collisionEvents[0].intersection;
			collisionPoint = collisionEvents[0].intersection;
		}
		
		isHit();

		// Rigidbody rb = other.GetComponent<Rigidbody>();
		/*int i = 0;

		while (i < numCollisionEvents)
		{
			// if (rigidBody)
			// {
				Vector3 pos = collisionEvents[i].intersection;
				Vector3 force = collisionEvents[i].velocity * 10;
				// rigidBody.AddForce(force);
			// }
			i++;
		}*/

    }

    public void isHit()
    {
        if (hasHit == true) return;
        else hasHit = true;
        
        Debug.Log("<color=white>== CharacterView.isHit ==</color>");
		Debug.Log("<color=white>" + model.handle + "</color>");

		if (activeAbility.actionType == AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_MELEE)
		{
			// melee hit!
			Debug.Log("<color=purple>Melee Hit!</color>");
		}
		Vector3 com = prefab.GetComponent<Rigidbody>().centerOfMass;
		// bloodFX
		if (collisionPoint == Vector3.zero)//null)
			collisionPoint = new Vector3(prefab.transform.position.x, com.y, prefab.transform.position.z - 0.5f);
		// string hitPrefab = "Hit 1 Variant"; // Blood11_2 Variant
		GameObject fx = GameManager.instance.assetBundleCombat.LoadAsset<GameObject>(activeAbility.hitFx);
		GameObject cfx = Instantiate(fx) as GameObject;
		cfx.transform.position = new Vector3(collisionPoint.x, collisionPoint.y, collisionPoint.z);
		cfx.transform.rotation = Quaternion.Euler(prefab.transform.rotation.x, prefab.transform.rotation.y, prefab.transform.rotation.z);
		// cfx.transform.SetParent(prefab.transform, true);
		// new Vector3(collisionPoint.position.x, collisionPoint.y, collisionPoint.z);// new Vector3(point.x, point.y, point.z);
		// Destroy(cfx, 2.0f);
		// }
		/*if (collision != null && collision.gameObject.name != "Floor")
		{
			Vector3 point = collision.contacts[0].point;
			Debug.Log("<color=red>contact " + point.x + "/" + point.y + "/" + point.z + " </color>");
			GameObject fx = GameManager.instance.assetBundleCombat.LoadAsset<GameObject>("Blood11_2 Variant");
			GameObject cfx = Instantiate(fx) as GameObject;
			cfx.transform.position = new Vector3(point.x, point.y, point.z);
			Destroy(cfx, 1.75f);
			// GameObject cfx = Instantiate(fx, transform.position, new Quaternion()) as GameObject;
			Debug.Log("<color=red>prefab " + cfx + " </color>");
		}*/

		// HIT animation
		string animation = null;// = "Standing_React_Large_From_Front";
		SoundVO hitSoundFx = activeAbility.soundOnHit;

		if (this.model.actions.ability != null) {
			switch(this.model.actions.ability.actionType)
			{
				case AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SELF:
					int weaponIndex = 0;
					int gearIndex = 0;
					// physical shield
					if (this.charController.gear.Count > 0) {
						// hide weapon
						if (this.charController.weapon.Count > 0)
							this.charController.weapon[weaponIndex].prefab.SetActive(false);
						// activate gear prefab
						this.charController.gear[gearIndex].prefab.SetActive(true);
						// inherit transforms
						this.charController.gear[gearIndex].prefab.transform.position = this.charController.gear[gearIndex].endpoint.position;
						this.charController.gear[gearIndex].prefab.transform.rotation = this.charController.gear[gearIndex].endpoint.rotation;
						this.charController.gear[gearIndex].prefab.transform.localScale = this.charController.gear[gearIndex].endpoint.localScale;
						// set sound
						hitSoundFx = activeAbility.soundOnBlockShieldPhysical;
						// activate camera filter (shield3d)
						GameManager.instance.cinecamController.EnableFilter("shield", true);
						// sparks
						// GameObject asset2 = GameManager.instance.assetBundleCombat.LoadAsset<GameObject>(activeAbility.hitFx);
						// GameObject fx2 = Instantiate(asset2) as GameObject;
						// Destroy(fx2, 2.0f);
						// fx2.transform.SetParent(currentEffect.transform);
						// fx2.transform.position = moveTo;//new Vector3(fx.transform.position.x, fx.transform.position.y + 1, fx.transform.position.z);

					}
					animation = "standing_block_react_large";
				break;

				default: 
					animation = "Standing_React_Large_From_Front"; 
				break;

			}
		}
		else {
			animation = "Standing_React_Large_From_Front";
		}

		animator.Play(animation);//"Standing_React_Large_From_Front");
		// AudioClip clip = GameManager.instance.assetBundleCombat.LoadAsset<AudioClip>("Laser Impact Light_1");

		// hit sound fx
		if (hitSoundFx != null)//activeAbility.soundOnHit != null)
			audioSource.PlayOneShot(hitSoundFx.audioClip, 1.0f);
		// SoundManager.instance.PlaySingle(clip);

		// bullet-time
		timeManager.DoSlowmotion();

        // stub, get combat results for me
        int healthMod = characterResultsVO.healthModifier;
        int healthVal = characterResultsVO.healthValue;
        bool isCrit = characterResultsVO.isCrit;
		if (isCrit == true)
		{
			audioSource.PlayOneShot(SoundLibraryVO.GetCrit(model.crewType).audioClip, 0.75f);
		}
        // floater(3, "Crit");
		floater(healthMod, healthVal.ToString(), isCrit);
		StartCoroutine(bulletTimePause(false));
	}

    /**
        animationEventHandler
        params: a AbilityVO

        caller: CharController.cs
    */
	public void animationEventHandler(AbilityVO a)
    {
        Debug.Log("<color=red>== CharacterView.animationEventHandler ==</color>");
        Debug.Log("ability " + a.name + " / " + a.actionType);

		// if (a.filterType != null)
		// 	GameManager.instance.cinecamController.EnableFilter(a.filterType, true, true);

		// determine type of combat (group heal, ranged, melee, etc)

		// best for ranged attacks?
        // if (a.actionType == AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE || a.actionType == AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_RANGED)
        switch(a.actionType)
        {
            case AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_RANGED:
            case AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE:
			case AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_MELEE:
				// determine primary target
				foreach (CharacterView c in targets)
                {
                    Debug.Log("<color=pink>" + c.model.handle + " / " + c.model.isPrimaryTarget + "</color>");
                    if (c.model.isPrimaryTarget)
                    {
                        primaryTarget = c;
                        // now, reset val
                        c.model.isPrimaryTarget = false;
                        break;
                    }
                }
                // StartCoroutine("attackAnimationStages");
				attackAnimationStages();
            break;

            case AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_CREW:
                // add caster as well if group ability (targets = 4)
                if (a.targets == 4)
                    targets.Add(this);
                StartCoroutine("defendCrewAnimationStages");//(targets));
			break;

            case AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SELF:
                Debug.Log("<color=blue>Defense code here...</color>");
                if (targets.Count == 0)
                    targets.Add(this);
                StartCoroutine("defendSelfAnimationStages");
            break;

			case AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SIDECAR:
				Debug.Log("<color=blue>Sidecar code here...</color>");
				if (targets.Count == 0)
					targets.Add(this);
				StartCoroutine("defendSidecarAnimationStages");
			break;
		}
    }

    public void changeAnimation(string animation)
    {
        Debug.Log("== CharacterView.changeAnimation ==");
        Debug.Log("* animation " + animation);
        prefab.GetComponent<Animator>().Play(animation);
    }

    public void damageText(string text, Vector3 position, string key = "default")
    {
		GameManager.instance.ultimateTextDamageManager.Add(text, position, key);
    }

    public void floater(int type, string val, bool isCrit = false)
    {
        // type: 0 = damage
        // type: 1 = heal
        Debug.Log("== floater == " + type + "/" + val);
		// GameObject TextFloat = new GameObject("TextFloat");

		// GameObject ob = GameObject.Instantiate(TextFloat[index], hit.point, Quaternion.identity);
		GameObject go = (GameObject)Resources.Load("Prefabs/FloatText", typeof(GameObject));
		if (go)
		{
            Vector3 pos = new Vector3(prefab.transform.position.x, prefab.transform.position.y + 2, prefab.transform.position.z);
			GameObject ob = GameObject.Instantiate(go, pos, Quaternion.identity);
			GameObject.Destroy(ob, LifeTime);
			FloatingText floattext = ob.GetComponentInChildren<FloatingText>();
            TextMesh textMesh = ob.GetComponentInChildren<TextMesh>();
			string assignment = "";
			if (isCrit == true)
                assignment += "Crit!\n";
            /* type: 1 = dmg, 2 = heal, 3 = boon/text */
            switch(type)
            {
                case HealthVO.HEALTH_MODIFIER_NONE: 
                    return; 
                // break;
                
                case HealthVO.HEALTH_MODIFIER_DAMAGE: 
                    assignment += "-"; 
                    floattext.TextColor = Color.red; 
                    textMesh.fontSize = 28; 
                break;

                // for heal, just float heal with other floater animation
                // numbers will float during end of turn
                case HealthVO.HEALTH_MODIFIER_HEAL: 
                    // assignment = "HEAL"; 
                    // floattext.TextColor = Color.green; 
                    // textMesh.fontSize = 28; 
                    // floattext.LifeTime = 1; 
                    // floattext.FadeEnd = true; 
                break;

				case HealthVO.HEALTH_MODIFIER_OTHER: 
                    assignment += ""; 
                    floattext.TextColor = Color.yellow; 
                    textMesh.fontSize = 28; 
                break;
			}

			if (type == HealthVO.HEALTH_MODIFIER_DAMAGE || type == HealthVO.HEALTH_MODIFIER_OTHER)
				floattext.SetText(assignment + val);//Random.Range(5, 100));
            // else floattext.SetText(assignment + val);
		}
	}

    IEnumerator hitPause()
    {
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 0.05f;
    }
    IEnumerator bulletTimePause(bool skipCam = false)
    {
        Debug.Log("* bulletTimePause");

		CinemachineVirtualCamera cam;
        if (skipCam != true)
        {
            GameManager.instance.cinecamController.targets = this.targets;
            cam = GameManager.instance.cinecamController.getCamByInt(4);
            cam.gameObject.SetActive(true);
        }
        else
        {
            cam = GameManager.instance.cinecamController.getCurrentCam();
        }

		/*CinemachineVirtualCamera vcam2 = new GameObject("VirtualCamera").AddComponent<CinemachineVirtualCamera>();

        GameObject go = new GameObject();
        go.transform.position = Vector3.Lerp(targets[0].prefab.transform.position, targets[1].prefab.transform.position, 0.5f);
        Debug.Log("* " + go.transform.position.x + go.transform.position.y + go.transform.position.z);

		vcam2.m_Follow = go.transform;// prefab.transform;
		vcam2.m_Priority = 30;
		vcam2.m_Lens.FieldOfView = 20; // zoom
		// vcam1.m_SetActive = false;
		// vcam2.gameObject.SetActive = true;
		// vcam1.gameObject.transform.position = new Vector3(0, 1, 0);
		// vcam2.gameObject.transform.position = new Vector3(prefab.transform.position.x + 6f, prefab.transform.position.y + 1, prefab.transform.position.z - 6);

		// frame shot
		var composer = vcam2.AddCinemachineComponent<CinemachineFramingTransposer>();
		composer.m_ScreenY = 0.80f;
		composer.m_DeadZoneWidth = 0.30f;
		composer.m_DeadZoneHeight = 0.55f;*/
		// assign cam to targets
		/*GameObject targetGroupObject = new GameObject("TargetGroup");
		CinemachineTargetGroup camTargetGroup = targetGroupObject.AddComponent<CinemachineTargetGroup>();
        // camTargetGroup.SetActive(true);// = 50;
        List<CinemachineTargetGroup.Target> targets1 = new List<CinemachineTargetGroup.Target>();
        // Debug.Log(this.targets);//[0].prefab);
		targets1.Add(new CinemachineTargetGroup.Target { target = targets[0].prefab.transform, radius = 1f, weight = 1f });
		targets1.Add(new CinemachineTargetGroup.Target { target = targets[1].prefab.transform, radius = 1f, weight = 1f });
		camTargetGroup.m_Targets = targets1.ToArray();
		// camTargetGroup.m_RotationMode.GroupAverage;

		vcam.gameObject.transform.position = new Vector3(2, 10, 6);

		vcam.m_LookAt = camTargetGroup.transform;
        // vcam.m_Follow = camTargetGroup.transform;


		CinemachineGroupComposer composer = vcam.AddCinemachineComponent<CinemachineGroupComposer>();
        composer.m_GroupFramingSize = 0.5f;//1;
        composer.m_FramingMode = CinemachineGroupComposer.FramingMode.Horizontal;//AndVertical;
		// composer.m_ScreenY = 5f;
        // composer.m_ScreenX = 2f;//0.5f;
		composer.m_AdjustmentMode = CinemachineGroupComposer.AdjustmentMode.DollyThenZoom;*/


		// cam.SetActive(true);	
		// vcam2.m_Targets = new CinemachineTargetGroup.Target[blabla];
		// vcam1.m_LookAt = idleWindow.transform;//GameObject.Find("Cube").transform;
		// CinemachineGroupComposer group = vcam1.AddComponent("CinemachineGroupComposer")();
		// Debug.Log("* group " + group);
		// camTargetGroup.m_Follow = combatSourceViewActive.prefab.transform;
		// camTargetGroup.m_Priority = 20;
		// camTargetGroup.m_Lens.FieldOfView = 20; // zoom
		// camTargetGroup.gameObject.transform.position = new Vector3(0, 1, 0);

		// frame shot
		// var composer = camTargetGroup.AddCinemachineComponent<CinemachineFramingTransposer>();
		// composer.m_ScreenY = 0.90f;
		// composer.m_AdjustmentMode = CinemachineFramingTransposer.AdjustmentMode.DollyThenZoom;

		yield return new WaitForSeconds(1.0f);
        // Time.timeScale = 1;
        Debug.Log("* done with bullettime");
        cam.gameObject.SetActive(false);
		// reset primary target value
		// if (model.isPrimaryTarget == true)
		//     model.isPrimaryTarget = false;

		EventParam eventParam = new EventParam();
		eventParam.name = "combatComplete";
		EventManager.TriggerEvent("combatEvent", eventParam);
        // yield return new WaitForSeconds(4f);
        // Debug.Log("* releasing...");
        // doRelease();
    }
    
    IEnumerator floatAnimation()
    {

        Debug.Log("== CharacterView.floatAnimation ==");
        Vector3 pointB = new Vector3(floatContainer.transform.position.x, floatContainer.transform.position.y + 2, 0);
        yield return StartCoroutine(MoveObject(floatContainer.transform, floatContainer.transform.position, pointB, 2));
        Destroy(floatContainer);
    }

	public void ActivatePreAttack()
	{
		Debug.Log("preAttack!");
		if (activeAbility.filterType != null)
			GameManager.instance.cinecamController.EnableFilter(activeAbility.filterType, true, true);
	}
	public void ActivateEffectAudio(SoundVO sound)
	{
		if (sound != null)
			audioSource.PlayOneShot(sound.audioClip, 0.75f);
	}

	void attackAnimationStages()
    {
        Debug.Log("<color=yellow>== CharacterView.attackAnimationStages ==</color>");
        Debug.Log("<color=orange>* isPrimaryTarget " + primaryTarget.model.isPrimaryTarget + "</color>");
		Debug.Log("<color=orange>* ability prefab " + primaryTarget.model.isPrimaryTarget + "</color>");
		// animator.SetInteger("SkillNumber", 0);
		// animator.SetTrigger("UseSkill");

		// if activeAbility is projectile, transforms set to target(s)

		GameManager.instance.cinecamController.getCamByInt(3);

        // TODO: reduce this time based on position of defenders (backline)
        // if (activeAbility.targetType == AbilityVO.TARGET_TYPE_OPPONENT)
		//     yield return new WaitForSeconds(0.75f);

		// AudioClip clip = GameManager.instance.assetBundleCombat.LoadAsset<AudioClip>("Laser Impact Light_1");
		// if (activeAbility.soundOnActivate != null)
		// 	audioSource.PlayOneShot(activeAbility.soundOnActivate.audioClip, 0.25f);


		// disable attacker's collider
		CapsuleCollider collider = prefab.GetComponent<CapsuleCollider>();
        collider.enabled = false;

		// animator.SetTrigger("UseSkill");

        // TODO: Include source and target(s) transforms in AbilityVO (this.targets)

        // if defending crew is attacker then flip position/rotation offsets
        if (model.crewType == CharacterVO.CREW_TYPE_DEFENDER)
        {
            activeAbility.positionOffset.x *= -1;
            activeAbility.rotationOffset.y *= -1;
            activeAbility.rotationOffset.z *= -1;
        }

		// Apply effect prefab rotation offsets
		Vector3 updateRotation = new Vector3(
			prefab.transform.rotation.x + activeAbility.rotationOffset.x,
			prefab.transform.rotation.y + activeAbility.rotationOffset.y,
			prefab.transform.rotation.z + activeAbility.rotationOffset.z
		);

		Debug.Log("<color=purple> x offset " + activeAbility.positionOffset.x + " / " + activeAbility.positionXRelativeToPrimaryTarget + "</color>");
		if (activeAbility.positionXRelativeToPrimaryTarget == true)
        {
            Debug.Log("* is positionXRelativeToPrimaryTarget! " + activeAbility.positionOffset.x + " / " + primaryTarget.prefab.transform.position.x);
            activeAbility.positionOffset.x = primaryTarget.prefab.transform.position.x + activeAbility.positionOffset.x;
        }
        Debug.Log("<color=purple> x offset " + activeAbility.positionOffset.x + "</color>");
		// position offsets
		Vector3 updatePosition;

		// does ability emit from weapon?
		if (charController.weapon.Count > 0 && activeAbility.actionType == AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE)
		{
			updatePosition = charController.weapon[0].endpoint.position;
			updatePosition.z = charController.transform.position.z;
			// updatePosition.rotation = new Vector3(0, 90, 0);
		}
		else
		{
			updatePosition = new Vector3(
				prefab.transform.position.x + activeAbility.positionOffset.x,
				prefab.transform.position.y + activeAbility.positionOffset.y, 
				prefab.transform.position.z + activeAbility.positionOffset.z
			);
		}

        // instantiate effect
        if (charController.effect[activeAbility.slot - 1])
        {
			Debug.Log("* instantiating... " + updatePosition.x + " / " + updatePosition.y + " / " + updatePosition.z);
            currentEffect = Instantiate(charController.effect[activeAbility.slot - 1], updatePosition, Quaternion.Euler(updateRotation));
			// Destroy(currentEffect, 10f); // auto-destroy
            
            // define effect settings script
            if (activeAbility.hasFXSettings == true)
            {
                EffectSettings effectSettings = currentEffect.GetComponent<EffectSettings>();
                Debug.Log("settings " + effectSettings + " / " + currentEffect);
                effectSettings.Target = targets[0].prefab;
                effectSettings.EffectRadius = 0.1f;//float.Parse(activeAbility.effectSettings["EffectRadius"]);// 0.1f;
                if (model.crewType == CharacterVO.CREW_TYPE_DEFENDER)
                    effectSettings.MoveVector = Vector3.left;
                else effectSettings.MoveVector = Vector3.right;
                effectSettings.UseMoveVector = true;
            }

            // scale offsets
            currentEffect.transform.localScale = new Vector3(
                currentEffect.transform.localScale.x + activeAbility.localScaleOffset.x,
                currentEffect.transform.localScale.y + activeAbility.localScaleOffset.y,
                currentEffect.transform.localScale.z + activeAbility.localScaleOffset.z
            );
        }
		else Debug.Log("<color=red>* no effect assigned to CharacterView (likely a melee attack)</color>");

		// yield return null;
	}

	IEnumerator defendCrewAnimationStages()
	{
		Debug.Log("<color=yellow>== CharacterView.attackAnimationStages ==</color>");
		Debug.Log("<color=orange>* targets " + targets.Count + "</color>");
		// Debug.Log("<color=orange>* ability prefab " + primaryTarget.model.isPrimaryTarget + "</color>");
		// animator.SetInteger("SkillNumber", 0);
		// animator.SetTrigger("UseSkill");

		// if activeAbility is projectile, transforms set to target(s)

		// GameManager.instance.cinecamController.targets = targets;
		GameManager.instance.cinecamController.getCamByInt(3);//, true); // <-- true addes "headroom" to camera

		// TODO: reduce this time based on position of defenders (backline)
		yield return new WaitForSeconds(0.75f);

		// disable attacker's collider
		CapsuleCollider collider = prefab.GetComponent<CapsuleCollider>();
		collider.enabled = false;

		// animator.SetTrigger("UseSkill");

		// TODO: Include source and target(s) transforms in AbilityVO (this.targets)

		// if defending crew is attacker then flip position/rotation offsets
		if (model.crewType == CharacterVO.CREW_TYPE_DEFENDER)
		{
			activeAbility.positionOffset.x *= -1;
			activeAbility.rotationOffset.y *= -1;
			activeAbility.rotationOffset.z *= -1;
		}

		// Apply effect prefab rotation offsets
		foreach (CharacterView c in targets)
        {
            Vector3 updateRotation = new Vector3(
                prefab.transform.rotation.x + activeAbility.rotationOffset.x,
                prefab.transform.rotation.y + activeAbility.rotationOffset.y,
                prefab.transform.rotation.z + activeAbility.rotationOffset.z
            );

            // Debug.Log("<color=purple> x offset " + activeAbility.positionOffset.x + " / " + activeAbility.positionXRelativeToPrimaryTarget + "</color>");
            // if (activeAbility.positionXRelativeToPrimaryTarget == true)
            // {
            //     Debug.Log("* is Realtive! " + activeAbility.positionOffset.x + " / " + primaryTarget.prefab.transform.position.x);
            //     activeAbility.positionOffset.x = primaryTarget.prefab.transform.position.x + activeAbility.positionOffset.x;
            // }
            // Debug.Log("<color=purple> x offset " + activeAbility.positionOffset.x + "</color>");
            // position offsets
            Vector3 updatePosition = new Vector3(
                c.prefab.transform.position.x,// + activeAbility.positionOffset.x,
                c.prefab.transform.position.y,// + activeAbility.positionOffset.y,
                c.prefab.transform.position.z// + activeAbility.positionOffset.z
            );

            // instantiate effect
            currentEffect = Instantiate(charController.effect[activeAbility.slot - 1], updatePosition, Quaternion.Euler(updateRotation));
			GameObject.Destroy(currentEffect, 4f);

            // scale offsets
            currentEffect.transform.localScale = new Vector3(
                currentEffect.transform.localScale.x + activeAbility.localScaleOffset.x,
                currentEffect.transform.localScale.y + activeAbility.localScaleOffset.y,
                currentEffect.transform.localScale.z + activeAbility.localScaleOffset.z
            );
        }

		yield return new WaitForSeconds(1.75f);

		timeManager.DoSlowmotion();

		foreach (CharacterView c in targets)
        {
			c.floater(2, "-35", false);
		}
		
        StartCoroutine(bulletTimePause(false));
	}

	IEnumerator defendSelfAnimationStages()
	{
		Debug.Log("<color=yellow>== CharacterView.defendSelfAnimationStages ==</color>");
		Debug.Log("<color=orange>* targets " + targets.Count + "</color>");
		// Debug.Log("<color=orange>* ability prefab " + primaryTarget.model.isPrimaryTarget + "</color>");
		// animator.SetInteger("SkillNumber", 0);
		// animator.SetTrigger("UseSkill");

		// if activeAbility is projectile, transforms set to target(s)

		// GameManager.instance.cinecamController.targets = targets;
		GameManager.instance.cinecamController.getCamByInt(3);//, true); // <-- true addes "headroom" to camera

		// TODO: reduce this time based on position of defenders (backline)
		yield return new WaitForSeconds(0.75f);

		// disable attacker's collider
		CapsuleCollider collider = prefab.GetComponent<CapsuleCollider>();
		collider.enabled = false;

		// animator.SetTrigger("UseSkill");

		// TODO: Include source and target(s) transforms in AbilityVO (this.targets)

		// if defending crew is attacker then flip position/rotation offsets
		if (model.crewType == CharacterVO.CREW_TYPE_DEFENDER)
		{
			activeAbility.positionOffset.x *= -1;
			activeAbility.rotationOffset.y *= -1;
			activeAbility.rotationOffset.z *= -1;
		}

		// Apply effect prefab rotation offsets
		foreach (CharacterView c in targets)
		{
			Vector3 updateRotation = new Vector3(
				prefab.transform.rotation.x + activeAbility.rotationOffset.x,
				prefab.transform.rotation.y + activeAbility.rotationOffset.y,
				prefab.transform.rotation.z + activeAbility.rotationOffset.z
			);

			// position offsets
			Vector3 updatePosition = new Vector3(
				c.prefab.transform.position.x + activeAbility.positionOffset.x,
				c.prefab.transform.position.y + activeAbility.positionOffset.y,
				c.prefab.transform.position.z + activeAbility.positionOffset.z
			);

			// instantiate effect
			currentEffect = Instantiate(charController.effect[activeAbility.slot - 1], updatePosition, Quaternion.Euler(updateRotation));
			GameObject.Destroy(currentEffect, 4f);

			// scale offsets
			currentEffect.transform.localScale = new Vector3(
				currentEffect.transform.localScale.x + activeAbility.localScaleOffset.x,
				currentEffect.transform.localScale.y + activeAbility.localScaleOffset.y,
				currentEffect.transform.localScale.z + activeAbility.localScaleOffset.z
			);
		}

		yield return new WaitForSeconds(1.75f);

		timeManager.DoSlowmotion();

		foreach (CharacterView c in targets)
		{
			c.floater(3, activeAbility.floaterText + "!", false);
		}

		StartCoroutine(bulletTimePause(false));
	}

	IEnumerator defendSidecarAnimationStages()
	{
		Debug.Log("<color=yellow>defendSidecarAnimationStages</color>");
		// GameManager.instance.cinecamController.getCamByInt(3);//, true); // <-- true addes "headroom" to camera
		
		audioSource.PlayOneShot(new SoundVO(1, "Dark", "Quest_Game_Tribal_Ancient_Relic_Perc_Drone_Tone_1").audioClip, 0.75f);
		model.sidecar = new SidecarVO(model, model, charController.effect[activeAbility.slot - 1]);

		// TODO: reduce this time based on position of defenders (backline)
		yield return new WaitForSeconds(0.75f);

		// disable attacker's collider
		CapsuleCollider collider = prefab.GetComponent<CapsuleCollider>();
		collider.enabled = false;

		// if defending crew is attacker then flip position/rotation offsets
		if (model.crewType == CharacterVO.CREW_TYPE_DEFENDER)
		{
			// activeAbility.rotationOffset.x *= -1;
			activeAbility.rotationOffset.y *= -1;
			// activeAbility.rotationOffset.z *= -1;
		}

		// position offsets
		Vector3 updatePosition = new Vector3(
			prefab.transform.position.x + activeAbility.positionOffset.x,
			prefab.transform.position.y + activeAbility.positionOffset.y,
			prefab.transform.position.z + activeAbility.positionOffset.z
		);
		Vector3 updateRotation = new Vector3(
			prefab.transform.rotation.x + activeAbility.rotationOffset.x,
			prefab.transform.rotation.y + activeAbility.rotationOffset.y,
			prefab.transform.rotation.z + activeAbility.rotationOffset.z
		);


		// drop prefab (as effect)

		currentEffect = Instantiate(model.sidecar.prefab, updatePosition, Quaternion.Euler(updateRotation));
		// scale it
		currentEffect.transform.localScale = Vector3.one * activeAbility.localScaleOffset.x;
		// Rigidbody rb = currentEffect.gameObject.AddComponent<Rigidbody>();
		// GameObject.Destroy(currentEffect, 4f);

		Vector3 moveTo = new Vector3(currentEffect.transform.position.x, 0, -2.5f);
		StartCoroutine(MoveObject(currentEffect.transform, currentEffect.transform.position, moveTo, 0.25f));

		yield return new WaitForSeconds(0.25f);

		GameObject asset = GameManager.instance.assetBundleCombat.LoadAsset<GameObject>("Fire explosion Variant");
		GameObject fx = Instantiate(asset) as GameObject;
		Destroy(fx, 0.75f);
		fx.transform.SetParent(currentEffect.transform);
		fx.transform.position = moveTo;//new Vector3(fx.transform.position.x, fx.transform.position.y + 1, fx.transform.position.z);
		
		audioSource.PlayOneShot(new SoundVO(1, "Dark", "Quest_Game_Award_Positive_Star_3_Dark").audioClip, 0.75f);
		
		GameObject asset2 = GameManager.instance.assetBundleCombat.LoadAsset<GameObject>(activeAbility.hitFx);
		GameObject fx2 = Instantiate(asset2) as GameObject;
		Destroy(fx2, 2.0f);
		fx2.transform.SetParent(currentEffect.transform);
		fx2.transform.position = moveTo;//new Vector3(fx.transform.position.x, fx.transform.position.y + 1, fx.transform.position.z);
									   // CinecamController.instance.DoShake();

		yield return new WaitForSeconds(2);

		CinemachineVirtualCamera cam = GameManager.instance.cinecamController.getCurrentCam();
		cam.gameObject.SetActive(false);
		EventParam eventParam = new EventParam();
		eventParam.name = "combatComplete";
		EventManager.TriggerEvent("combatEvent", eventParam);
		Destroy(currentEffect);
		
	}

    IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        Debug.Log("== CharacterView.MoveObject ==");

        var i = 0.0f;
        var rate = 1.0f / time;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
    }
	public GameObject createView()
	{
		Debug.Log("== CharacterView.createView ==");

		return new GameObject("CharacterViewCreateView");
	}

    public void doRelease()
    {
        Debug.Log("* doRelease");
		while (transform && transform.childCount > 0)
		{
            Debug.Log("destroy!!");
			Destroy(transform.GetChild(0).gameObject);
		}
		Destroy(prefab);
        Destroy(this.gameObject);
		DestroyImmediate(this);

		// Destroy(transform.parent.gameObject);
		/*foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            // Debug.Log("releasing... " + o);
            // Destroy(o);
            Destroy(prefab);
            Destroy(this);
        }*/
	}
	// getters & setters
	public CharacterVO model
	{
		get { return _model; }
		set
		{
			_model = value;
			// set animation controller here (if not null)
			Debug.Log("**************** " + value.role);

			// healthbar.UpdateAttack(value.attributes.attack);
			// healthbar.UpdateDefense(value.attributes.defense);
			/*switch(value.role)
            {
                case CharacterVO.CHARACTER_ROLE_CHROMER:
                    this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load(CharacterView.CHARACTER_ANIMATOR_CONTROLLER_DAGGERMASTER) as RuntimeAnimatorController;
                break;
                case CharacterVO.CHARACTER_ROLE_ANARCHIST:
                    this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load(CharacterView.CHARACTER_ANIMATOR_CONTROLLER_SNIPER) as RuntimeAnimatorController;
                    break;
                case CharacterVO.CHARACTER_ROLE_MEDIC:
                    this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load(CharacterView.CHARACTER_ANIMATOR_CONTROLLER_HERO) as RuntimeAnimatorController;
                    break;
            }*/

			// set layer order (by position)
			// GetComponent<SpriteRenderer>().sortingOrder = value.position - 1;
		}
	}

    public CombatResultsVO combatResultsVO
    {
        get { return _combatResultsVO; }
        set
        {
            _combatResultsVO = value;

			// set personal results
            // TODO: get character results by id?
            foreach(CharacterResultsVO targ in value.targets)
            {
                if (targ.uid == model.uid)
				    characterResultsVO = targ;//value.targets[0];
			}
            if (value.next != null && value.next.lastTurnOfRound == false && value.next.uid == model.uid)
                characterNextTurnResultsVO = value.next;
        }
    }
}

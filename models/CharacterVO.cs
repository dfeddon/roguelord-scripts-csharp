using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using GameSparks.Core;

//In order to use a collection's Sort() method, this class needs to implement the IComparable interface.
[System.Serializable]
public class CharacterVO// : IComparable<CharacterVO>
{
    public const int ROLE_CHROMER = 1;
    public const int ROLE_ANARCHIST = 2;
    public const int ROLE_MEDIC = 3;
    public const int ROLE_SHAMAN = 4;
    public const int ROLE_BLACKHATTER = 5;
    public const int ROLE_SABOTEUR = 6;
    public const int ROLE_NANOMANCER = 7;
    public const int ROLE_ENGINEER = 8;

    public const int STATUS_AVAILABLE = 1;
    public const int STATUS_UNAVAILABLE = 2;
    public const int STATUS_DECEASED = 3;

    public const int CREW_TYPE_ATTACKER = 1;
    public const int CREW_TYPE_DEFENDER = 2;

	public const int POSITION_TYPE_ANY = 0;
	public const int POSITION_TYPE_REARGUARD = 1; // 3, 4
	public const int POSITION_TYPE_FOREGUARD = 2; // 1, 2
	public const int POSITION_TYPE_MIDGUARD = 3; // 2, 3
	public const int POSITION_TYPE_TRIPS_FORE = 4; // 1, 2, 3
	public const int POSITION_TYPE_TRIPS_REAR = 5; // 2, 3, 4
	public const int POSITION_TYPE_FORE = 6; // 1
	public const int POSITION_TYPE_MID_FORE = 8; // 2
	public const int POSITION_TYPE_MID_REAR = 9; // 3
	public const int POSITION_TYPE_REAR = 7; // 4
    public const string PREFAB_SKIN_CHROMER = "Cyborg_Melee 1";
	public const string PREFAB_SKIN_ANARCHIST = "Dark Witch_5";
	public const string PREFAB_SKIN_MEDIC = "Soldier_Gas Variant";//"Medic_1";
	public const string PREFAB_SKIN_SHAMAN = "AFRO MECANIM PC Variant";
	public const string PREFAB_SKIN_BLACKHATTER = "CyberHunter Variant";
	public const string PREFAB_SKIN_SABOTEUR = "Cyborg_Kamikaze 1";
	public const string PREFAB_SKIN_NANOMANCER = "Nanomancer_1";
	public const string PREFAB_SKIN_ENGINEER = "Shaman_1";// "Prefab-Sci-Fi_Assault";//"Grunt_Mixamo";
	public const string ANIMATOR_CHROMER = "Cyborg_MeleeController";
	public const string ANIMATOR_ANARCHIST = "Dark_Witch_5_Controller";
	public const string ANIMATOR_MEDIC = "Medic_1_Controller";
	public const string ANIMATOR_SHAMAN = "Shaman_1_Controller";
	public const string ANIMATOR_BLACKHATTER = "Blackhatter_Controller";
	public const string ANIMATOR_SABOTEUR = "Cyborg_KamikazeController";
	public const string ANIMATOR_NANOMANCER = "Nanomancer_1_Controller";
	public const string ANIMATOR_ENGINEER = "Grunt_Mixamo_Controller";

	public const float SCALE_CHROMER = 0.90f;
	public const float SCALE_NANOMANCER = 0.80f;
	public const float SCALE_BLACKHATTER = 1.0f;
	public const float SCALE_MEDIC = 1.0f;
	public const float SCALE_ANARCHIST = 1.0f;
	public const float SCALE_ENGINEER = 1.0f;
	public const float SCALE_SABOTEUR = 0.90f;
	public const float SCALE_SHAMAN = 0.90f;
	public enum OOPState { None, Yellow, Orange, Red };
	public enum StateOfMind { Inspired, Fearless, Guarded, Fearful, Unstable, Berserk };

	private int _uid;
    private int _position;
	private OOPState _oopState = OOPState.None;
	// private int _attack = 0;
	// private int _attackMax = 10;
	// private int _defense = 0;
	// private int _defenseMax = 10;
	public int positionTemp;
	public CharacterMovementVO movement = new CharacterMovementVO();
    private string _fullname;
    private string _handle;
    private int _role;
    private int _owner;
    private int _status;
	public int turn;
    private bool _hasTurned = false;
	public bool hasMoved = false;
    private int _crewType;
	// private int _healthMax;
	private float _viewScale = 1f;
	public bool isPrimaryTarget = false;
	public SidecarVO sidecar;
	private HealthVO _health = new HealthVO();
	public List<HealthVO> healthQueue = new List<HealthVO>();
	public List<HealthVO> healthHistory = new List<HealthVO>();


    public List<EffectVO> conditions = new List<EffectVO>();
    public List<EffectVO> cc = new List<EffectVO>();
    public List<EffectVO> boons = new List<EffectVO>();

	// private AbilityVO[] abilities;
	private int _weaponAbility = 0;
	private int _weaponModAbility = 1001; // locked weapon mod
	private int _gearAbility = 0;
	private int _gearModAbility = 1002; // locked gear mod
	public List<string> abilityHistory = new List<string>();
	public List<AbilityCooldownVO> abilityCooldowns = new List<AbilityCooldownVO>();
	public CharacterAttributesVO attributes;// = new CharacterAttributesVO();
	private string profileImage;
	public ActionVO actions = new ActionVO();

	private UnityAction<EventParam> charEventHandler;


	public CharacterVO(int uid, int position, int role, int crewType, int owner, string fullname, string handle)
    {
		Debug.Log("CharacterVO.constructor ");
        this.uid = uid;
        this._position = position;
        this.role = role;
        this.owner = owner;
        this.fullname = fullname;
        this.handle = handle;
        this.crewType = crewType;

        this.health.max = 50; 
		this.health.current = 25; // TODO <-- stub

		switch(role)
		{
			case ROLE_CHROMER: 
				viewScale = SCALE_CHROMER;
				profileImage = "profile-chromer";
			break;
			case ROLE_NANOMANCER: 
				viewScale = SCALE_NANOMANCER; 
				profileImage = "profile-nanomancer";
			break;
			case ROLE_BLACKHATTER: 
				viewScale = SCALE_BLACKHATTER;
				profileImage = "profile-blackhatter";
			break;
			case ROLE_MEDIC: 
				viewScale = SCALE_MEDIC;
				profileImage = "profile-medic";
			break;
			case ROLE_ANARCHIST: 
				viewScale = SCALE_ANARCHIST;
				profileImage = "profile-anarchist";
			break;
			case ROLE_ENGINEER: 
				viewScale = SCALE_ENGINEER;
				profileImage = "profile-engineer";
			break;
			case ROLE_SABOTEUR: 
				viewScale = SCALE_SABOTEUR;
				profileImage = "profile-saboteur";
			break;
			case ROLE_SHAMAN: 
				viewScale = SCALE_SHAMAN;
				profileImage = "profile-chemist";
			break;
		}
		charEventHandler = new UnityAction<EventParam>(charEventHandlerFunction);
		EventManager.StartListening("charEvent", charEventHandler);
	}
	void charEventHandlerFunction(EventParam eventParam)
	{
		Debug.Log("<color=blue>== charEventHandlerFunction ==</color>");
		
		if (eventParam.value != uid) return;

		Debug.Log(eventParam.name);
		

		switch (eventParam.name)
		{
			case "positionUpdateForward":
				Debug.Log("<color=blue>* position updated fore!</color>");
				Debug.Log(this.handle + " / " + this.position + " / " + eventParam.value);
				// this.position++;
				// Debug.Log("new pos " + this.position);
			break;
			case "positionUpdateBackward":
				Debug.Log("<color=blue>* position updated back!</color>");
				Debug.Log(this.handle + " / " + this.position + " / " + eventParam.value);
				// this.position--;
				// Debug.Log("new pos " + this.position);
			break;
		}
	}

	public Sprite GetProfileSprite()
	{
		Sprite sprite = GameManager.instance.assetBundleCombat.LoadAsset<Sprite>(profileImage);
		// Debug.Log(img);
		return sprite;
	}
	public Sprite GetTechSprite()
	{
		// Debug.Log(img);
		switch(role)
		{
			case ROLE_ANARCHIST:
				return GameManager.instance.assetBundleCombat.LoadAsset<Sprite>("icon-tech-aug-nano-fill");
			case ROLE_BLACKHATTER:
				return GameManager.instance.assetBundleCombat.LoadAsset<Sprite>("icon-tech-hacking-fill");
			case ROLE_CHROMER:
				return GameManager.instance.assetBundleCombat.LoadAsset<Sprite>("icon-tech-augmentation-fill");
			case ROLE_ENGINEER:
				return GameManager.instance.assetBundleCombat.LoadAsset<Sprite>("icon-tech-hack-nano-fill");
			case ROLE_MEDIC:
				return GameManager.instance.assetBundleCombat.LoadAsset<Sprite>("icon-tech-electrochem-fill");
			case ROLE_NANOMANCER:
				return GameManager.instance.assetBundleCombat.LoadAsset<Sprite>("icon-tech-nano-fill");
			case ROLE_SABOTEUR:
				return GameManager.instance.assetBundleCombat.LoadAsset<Sprite>("icon-tech-echem-hack-fill");
			case ROLE_SHAMAN:
				return GameManager.instance.assetBundleCombat.LoadAsset<Sprite>("icon-tech-echem-aug-fill");
		}
		return null;
	}
	public Sprite GetAbilitySpriteByUid(int uid)
	{
		AbilityVO a = AbilityMatrixVO.GetAbilityById(uid);
		if (a != null && a.image != null) 
			return GameManager.instance.assetBundleCombat.LoadAsset<Sprite>(a.image);
		return null;
	}

	public void AddAbilityToHistory(string a)
	{
		abilityHistory.Insert(0, a);
		// index cannot exceed 5 items
		if (abilityHistory.Count > 5)
			abilityHistory.RemoveAt(5);
	}
    public static string getPrefabFromRole(int role)
    {
        string prefab = null;

        // if (role == null)
        //     role = this.role;

        switch(role)
        {
            case ROLE_ANARCHIST: prefab = PREFAB_SKIN_ANARCHIST; break;
			case ROLE_CHROMER: prefab = PREFAB_SKIN_CHROMER; break;
			case ROLE_MEDIC: prefab = PREFAB_SKIN_MEDIC; break;
			case ROLE_SHAMAN: prefab = PREFAB_SKIN_SHAMAN; break;
			case ROLE_BLACKHATTER: prefab = PREFAB_SKIN_BLACKHATTER; break;
			case ROLE_SABOTEUR: prefab = PREFAB_SKIN_SABOTEUR; break;
			case ROLE_NANOMANCER: prefab = PREFAB_SKIN_NANOMANCER; break;
			case ROLE_ENGINEER: prefab = PREFAB_SKIN_ENGINEER; break;
		}

        return prefab;
    }

	public static string getAnimatorFromRole(int role)
	{
		string animator = null;

		switch (role)
		{
			case ROLE_ANARCHIST: animator = ANIMATOR_ANARCHIST; break;
			case ROLE_CHROMER: animator = ANIMATOR_CHROMER; break;
			case ROLE_MEDIC: animator = ANIMATOR_MEDIC; break;
			case ROLE_SHAMAN: animator = ANIMATOR_SHAMAN; break;
			case ROLE_BLACKHATTER: animator = ANIMATOR_BLACKHATTER; break;
			case ROLE_SABOTEUR: animator = ANIMATOR_SABOTEUR; break;
			case ROLE_NANOMANCER: animator = ANIMATOR_NANOMANCER; break;
			case ROLE_ENGINEER: animator = ANIMATOR_ENGINEER; break;
		}

		return animator;
	}

    public static CharacterVO getCharacterById(int id, List<CharacterVO> list = null)
    {
		if (list == null)
			list = GameManager.instance.allcharacters;
        CharacterVO ch = null;
		foreach (CharacterVO c in list)
        {
            if (c.uid == id)
                ch = c;
        }
        return ch;
    }

    public void AddEffect(EffectVO vo)
    {
        // bool isStackUpdated = false;
		// int indexUpdated = -1;

		List<EffectVO> list = null;
        switch(vo.effectModifier)
        {
            case EffectVO.EFFECT_MODIFIER_CONDITION:
                list = conditions;
            break;
			case EffectVO.EFFECT_MODIFIER_CC:
                list = cc;
			break;
			case EffectVO.EFFECT_MODIFIER_BOON:
                list = boons;
			break;
		}

        // ensure this is a new effect... or add to extant in list
        if (list.Count > 0 && list[0].uid == vo.uid)
            vo = list[0];
        else if (list.Count > 1 && list[1].uid == vo.uid)
            vo = list[1];

		// add/update stack
		vo.AddStack();

		// assign round added
		vo.roundAdded = GameManager.instance.currentPhaseTotal;

		// manage stack list
		switch(list.Count)
        {
            case 0: // none
                list.Add(vo);
            break;

            case 1:
				// if new effect, insert it
                if (list[0].uid != vo.uid)
    				list.Insert(0, vo);
			break;

            case 2: // if new effect, remove eldest before inserting
                if (list[0].uid != vo.uid && list[1].uid != uid)
                {
				    list.RemoveAt(1);
                    list.Insert(0, vo);
                }
                else if (list[1].uid == vo.uid)// if extant effect if eldest, move it to the front
                {
					list.Reverse(0, 2);
                }
            break;
        }

		// update effectbar
		EventParam eventParam = new EventParam();
        eventParam.name = "effectAdd";
        eventParam.data = vo; // TODO: <-- redundant but still used...
        eventParam.effectsList = new List<EffectVO>(list);
        eventParam.value = uid; // value is source uid
		EventManager.TriggerEvent("characterEvent", eventParam);
	}
    public void TurnBegin()
    {
        Debug.Log("<color=yellow>CharacterVO.TurnBegin " + handle + "</color>");
        
        int stacksRemaining;
        foreach(EffectVO condi in conditions)
        {
            // update stacks (if -1, added this round, so skip)
            // int turnBeginVal = condi.TurnBegin();
            stacksRemaining = condi.TurnBegin();
            // get remaining turns from active stacks
			// stacksRemaining = condi.GetStackTurnsRemaining();
            // stacksRemaining = condi.TurnBegin();
            if (stacksRemaining == -1)
                Debug.Log("<color=red>* ignoring, stack added this round</color>");
			else if (stacksRemaining == 1)
			{
				// activate last tick
				EventParam eventParam = new EventParam();
				eventParam.name = "effectUpdate";
				eventParam.data = condi;//vo; // TODO: <-- redundant but still used...
				eventParam.effectsList = new List<EffectVO>(conditions);// list);
				eventParam.value = uid; // value is source uid
				EventManager.TriggerEvent("characterEvent", eventParam);
				
                // effect expired, remove it
				EventParam eventParam2 = new EventParam();
				eventParam2.name = "effectRemove";
				eventParam2.data = condi;//vo; // TODO: <-- redundant but still used...
				eventParam2.effectsList = new List<EffectVO>(conditions);// list);
				eventParam2.value = uid; // value is source uid
				EventManager.TriggerEvent("characterEvent", eventParam2);
                
				// TODO: remove (or reset) stack locally
                // condi.resetStack();
			}
            else if (stacksRemaining > 1)// && turnBeginVal != -1)
            {
				// stacks were updated but not expired, activate!
				EventParam eventParam3 = new EventParam();
				eventParam3.name = "effectUpdate";
				eventParam3.data = condi;//vo; // TODO: <-- redundant but still used...
				eventParam3.effectsList = new List<EffectVO>(conditions);// list);
				eventParam3.value = uid; // value is source uid
				EventManager.TriggerEvent("characterEvent", eventParam3);

                // returnVal = true; // turnCompletePending
			}
		}

        foreach(EffectVO c in cc)
        {
			stacksRemaining = c.TurnBegin();
			if (stacksRemaining == -1)
				Debug.Log("<color=red>* ignoring, stack added this round</color>");
			else if (stacksRemaining == 1)
			{
				// activate last tick
				EventParam eventParam4 = new EventParam();
				eventParam4.name = "effectUpdate";
				eventParam4.data = c;//vo; // TODO: <-- redundant but still used...
				eventParam4.effectsList = new List<EffectVO>(cc);// list);
				eventParam4.value = uid; // value is source uid
				EventManager.TriggerEvent("characterEvent", eventParam4);

				// update effectbar
				EventParam eventParam5 = new EventParam();
				eventParam5.name = "effectRemove";
				eventParam5.data = c;//vo; // TODO: <-- redundant but still used...
				eventParam5.effectsList = new List<EffectVO>(cc);// list);
				eventParam5.value = uid; // value is source uid
				EventManager.TriggerEvent("characterEvent", eventParam5);
			}
            else if (stacksRemaining > 1)
            {
				// stacks were updated but not expired...
				EventParam eventParam6 = new EventParam();
				eventParam6.name = "effectUpdate";
				eventParam6.data = c;//vo; // TODO: <-- redundant but still used...
				eventParam6.effectsList = new List<EffectVO>(cc);// list);
				eventParam6.value = uid; // value is source uid
				EventManager.TriggerEvent("characterEvent", eventParam6);
			}
		}
        foreach(EffectVO boon in boons)
        {
			stacksRemaining = boon.TurnBegin();
			if (stacksRemaining == -1)
				Debug.Log("<color=red>* ignoring, stack added this round</color>");
			else if (stacksRemaining == 1)
			{
				// activate last tick
				EventParam eventParam7 = new EventParam();
				eventParam7.name = "effectUpdate";
				eventParam7.data = boon;//vo; // TODO: <-- redundant but still used...
				eventParam7.effectsList = new List<EffectVO>(boons);// list);
				eventParam7.value = uid; // value is source uid
				EventManager.TriggerEvent("characterEvent", eventParam7);

				// update effectbar
				EventParam eventParam8 = new EventParam();
				eventParam8.name = "effectRemove";
				eventParam8.data = boon;//vo; // TODO: <-- redundant but still used...
				eventParam8.effectsList = new List<EffectVO>(boons);// list);
				eventParam8.value = uid; // value is source uid
				EventManager.TriggerEvent("characterEvent", eventParam8);
			}
            else if (stacksRemaining > 1)
            {
				// stacks were updated but not expired...
				EventParam eventParam9 = new EventParam();
				eventParam9.name = "effectUpdate";
				eventParam9.data = boon;//vo; // TODO: <-- redundant but still used...
				eventParam9.effectsList = new List<EffectVO>(boons);// list);
				eventParam9.value = uid; // value is source uid
				EventManager.TriggerEvent("characterEvent", eventParam9);
			}
		}
        // if (condiUpdate == true || condiUpdate == true || condiUpdate == true)
        //     doUpdate = true;
        // if (stacksRemaining == null)
        //     stacksRemaining = -100;
        // Debug.Log("<color=orange>stacksRemaining " + stacksRemaining + " for " + handle + "</color>");
        // return returnVal;//stacksRemaining;
    }

	public void updateHealth(int type, int val)
	{
        Debug.Log("<color=green>updateHealth " + type + " / " + val + "</color>");
        // update health
        switch(type)
        {
            case HealthVO.HEALTH_MODIFIER_DAMAGE:
                health.current -= val;
            break;

            case HealthVO.HEALTH_MODIFIER_HEAL:
                health.current += val;
            break;

            case HealthVO.HEALTH_MODIFIER_OTHER:
            break;
        }

		if (GameManager.instance.roundIsLocked == false)
		{
			// inform characterView to update healthbars
			EventParam eventParam = new EventParam();
			eventParam.name = "healthUpdate";
			eventParam.data = health;
			eventParam.value = uid; // required! value is source uid
			eventParam.value2 = type;
			EventManager.TriggerEvent("characterEvent", eventParam);
		}
	}

	public void actionIndicatorAdd(AbilityVO ability)
	{
		// inform characterView to update healthbars
		EventParam eventParam = new EventParam();
		eventParam.name = "actionIndicatorAdd";
		eventParam.data = ability;
		eventParam.value = uid; // required! value is source uid
		// eventParam.value2 = type;
		EventManager.TriggerEvent("characterEvent", eventParam);
	}

	public void AbilityCooldownUpsert(AbilityVO vo)
	{
		foreach(AbilityCooldownVO a in abilityCooldowns)
		{
			if (a.uid == vo.uid)
			{
				a.cooldownCounter = vo.cooldownTotal;
				return;
			}
		}

		// if here, Add new cooldown to abilityCooldowns
		AbilityCooldownVO cd = new AbilityCooldownVO(vo);

		abilityCooldowns.Add(cd);
	}

	public void ProcessAbilityCooldowns()
	{
		foreach(AbilityCooldownVO a in abilityCooldowns)
		{
			if (a.cooldownCounter > 0)
				a.cooldownCounter -= 1;
		}
	}

	public void Speak(string txt, int type = 0)
	{
		EventParam eventParam = new EventParam();
		eventParam.name = "dialogue";
		eventParam.value2 = type;
		eventParam.message = txt;
		eventParam.value = uid; // value is source uid
		EventManager.TriggerEvent("characterEvent", eventParam);
	}

	public bool ValidatePositioning(int abilityPositionType)
	{
		if (abilityPositionType == POSITION_TYPE_ANY)
			return true;

		bool isValid = false;


		switch (position)
		{
			case 1:
				if (abilityPositionType == POSITION_TYPE_FORE ||
					abilityPositionType == POSITION_TYPE_FOREGUARD ||
					abilityPositionType == POSITION_TYPE_TRIPS_FORE)
					isValid = true;
				break;

			case 2:
				if (abilityPositionType == POSITION_TYPE_FOREGUARD ||
					abilityPositionType == POSITION_TYPE_MIDGUARD ||
					abilityPositionType == POSITION_TYPE_MID_FORE ||
					abilityPositionType == POSITION_TYPE_TRIPS_FORE ||
					abilityPositionType == POSITION_TYPE_TRIPS_REAR)
					isValid = true;
				break;

			case 3:
				if (abilityPositionType == POSITION_TYPE_REARGUARD ||
					abilityPositionType == POSITION_TYPE_MIDGUARD ||
					abilityPositionType == POSITION_TYPE_MID_REAR ||
					abilityPositionType == POSITION_TYPE_TRIPS_FORE ||
					abilityPositionType == POSITION_TYPE_TRIPS_REAR)
					isValid = true;
				break;

			case 4:
				if (abilityPositionType == POSITION_TYPE_REAR ||
					abilityPositionType == POSITION_TYPE_REARGUARD ||
					abilityPositionType == POSITION_TYPE_TRIPS_REAR)
					isValid = true;
				break;
		}

		return isValid;
	}

	public bool AssignedCard(CardVO card, CardLevelDataVO action)
	{
		Debug.Log("<color=yellow>== CharacterVO.AssignedCard == </color>");

		bool canAccept = true;
		// first, validate that character can in fact accept the card 
		// 2 card slots max?
		// attack/defense/health full
		if (canAccept == true)
		{
			// reduce power by card.cost
			// TODO: update UI
			attributes.power -= card.cost;

			// process card
			ProcessCardAction(action.actionType1, action.value1);

			if (action.actionType2 != CardLevelDataVO.CardActionType.None)
				ProcessCardAction(action.actionType2, action.value2);
		}

		return canAccept;
	}

	private void ProcessCardAction(CardLevelDataVO.CardActionType type, int value)
	{
		// EventParam eventParam = new EventParam();

		switch (type)
		{
			case CardLevelDataVO.CardActionType.Attack:
				if (attributes.attack < attributes.attackMax)
				{
					if (attributes.attack + value > attributes.attackMax) // can't exceed max
						attributes.attack = attributes.attackMax;
					else if (attributes.attack + value < 0)
						attributes.attack = 0;
					else attributes.attack += value;//attributes.attackMax;
				}
				// eventParam.name = "attackUpdate";
				// eventParam.data = this; // TODO: <-- redundant but still used...
				// // eventParam.effectsList = new List<EffectVO>(list);
				// eventParam.value = uid; // value is source uid
				// eventParam.value2 = attributes.attack;//value;
				// EventManager.TriggerEvent("characterEvent", eventParam);
			break;
			case CardLevelDataVO.CardActionType.Defense:
				if (attributes.defense < attributes.defenseMax)
				{
					if (attributes.defense + value > attributes.defenseMax) // can't exceed max
						attributes.defense = attributes.defenseMax;
					else if (attributes.defense + value < 0)
						attributes.defense = 0;
					else attributes.defense += value;//attributes.defenseMax;
				}
				// EventParam eventParam = new EventParam();
				// eventParam.name = "defenseUpdate";
				// eventParam.data = this; // TODO: <-- redundant but still used...
				// // eventParam.effectsList = new List<EffectVO>(list);
				// eventParam.value = uid; // value is source uid
				// eventParam.value2 = attributes.defense;//value;
				// EventManager.TriggerEvent("characterEvent", eventParam);
				break;
			case CardLevelDataVO.CardActionType.Draw:
				break;
			case CardLevelDataVO.CardActionType.Discard:
				break;
			case CardLevelDataVO.CardActionType.None:
				break;
		}

	}
	private int GetHomePositionByRole()
	{
		switch(role)
		{
			case ROLE_ANARCHIST: return 1;
			case ROLE_CHROMER: return 1;
			case ROLE_NANOMANCER: return 2;
			case ROLE_ENGINEER: return 2;
			case ROLE_BLACKHATTER: return 3;
			case ROLE_SABOTEUR: return 3;
			case ROLE_MEDIC: return 4;
			case ROLE_SHAMAN: return 4;
		}
		return 0;
	}
	private OOPState GetOOPStateByPositionOffset(int diff)
	{
		switch(diff)
		{
			case 1:
				// if (role == ROLE_BLACKHATTER || role == ROLE_NANOMANCER || role == ROLE_ENGINEER || role == ROLE_SABOTEUR)
					return OOPState.Orange;
				// else return OOPState.Yellow;

			case 2:
				// if (role == ROLE_BLACKHATTER || role == ROLE_NANOMANCER || role == ROLE_ENGINEER || role == ROLE_SABOTEUR)
					return OOPState.Red;
				// else return OOPState.Orange;
	
			case 3: return OOPState.Red;
		}

		return OOPState.None;
	}
	private void PositionChanged()
	{
		Debug.Log("== PositionChanged ==");
		int diff = Mathf.Abs(_position - GetHomePositionByRole());
		Debug.Log("<color=white>Diff " + handle + " / " + diff + "</color>");
		this.oopState = GetOOPStateByPositionOffset(diff);
	}

	// public HealthVO ProcessHealthQueue()
	// {
	// 	HealthVO vo = null;

	// 	foreach (HealthVO h in healthQueue)
	// 	{

	// 	}

	// 	return vo;
	// }

	//This method is *required* by the IComparable interface. 
	// public int CompareTo(CharacterVO other)
	// {
	//     if (other == null)
	//     {
	//         return 1;
	//     }

	//     //Return the difference in power.
	//     return position - other.position;
	// }

	// getters & setters
	public int uid
    {
        get { return _uid; }
        set { _uid = value; }
    }

    public int position
    {
        get { return _position; }
        set 
		{ 
			_position = value;
			if (value > 0)
				PositionChanged();
		}
    }
	public OOPState oopState
	{
		get { return _oopState; }
		set
		{
			_oopState = value;
			EventParam eventParam = new EventParam();
			eventParam.name = "oopStateUpdated";
			// eventParam.data = ability;
			eventParam.value = uid; // required! value is source uid
			// eventParam.data = { oopState: value };
			EventManager.TriggerEvent("characterEvent", eventParam);
		}
	}

	public GSRequestData GetData()
	{
		GSRequestData data = new GSRequestData();
		data.AddNumber("uid", uid);
		data.AddNumber("pos", position);
		data.AddNumber("turn", turn);
		data.AddObject("actions", actions.GetData());

		return data;
	}
	// public int attack
	// {
	// 	get { return _attack; }
	// 	set { _attack = value; }
	// }
	// public int attackMax
	// {
	// 	get { return _attackMax; }
	// 	set { _attackMax = value; }
	// }
	// public int defense
	// {
	// 	get { return _defense; }
	// 	set { _defense = value; }
	// }
	// public int defenseMax
	// {
	// 	get { return _defenseMax; }
	// 	set { _defenseMax = value; }
	// }

	public string fullname
    {
        get { return _fullname; }
        set { _fullname = value; }
    }

    public string handle
    {
        get { return _handle; }
        set { _handle = value; }
    }

    public int status
    {
        get { return _status; }
        set { _status = value; }
    }
    public bool hasTurned
    {
        get { return _hasTurned; }
        set { _hasTurned = value; }
    }
    public int role
    {
        get { return _role; }
        set 
		{ 
			_role = value;
			switch(value)
			{
				case ROLE_CHROMER:
					_weaponAbility = 201;
					_gearAbility = 301;
				break;

				case ROLE_ANARCHIST:
					_weaponAbility = 202;
					_gearAbility = 302;
				break;

				case ROLE_NANOMANCER:
					_weaponAbility = 203;
					_gearAbility = 303;
				break;

				case ROLE_ENGINEER:
					_weaponAbility = 2005; // 204
					_gearAbility = 304;
				break;

				case ROLE_BLACKHATTER:
					_weaponAbility = 205;
					_gearAbility = 305;
				break;

				case ROLE_SABOTEUR:
					_weaponAbility = 206;
					_gearAbility = 306;
				break;

				case ROLE_MEDIC:
					_weaponAbility = 208;
					_gearAbility = 308;
				break;
				case ROLE_SHAMAN:
					_weaponAbility = 207;
					_gearAbility = 307;
				break;
			}
		}
    }
    public int owner
    {
        get { return _owner; }
        set { _owner = value; }
    }
	public int crewType
	{
		get { return _crewType; }
		set { _crewType = value; }
	}
	// public int healthMax
	// {
	// 	get { return _healthMax; }
	// 	set { _healthMax = value; }
	// }
	public HealthVO health
	{
		get { return _health; }
		set { _health = value; }
	}

    public float viewScale
    {
        get { return _viewScale; }
        set { _viewScale = value; }
    }

	public int weaponAbility {
		get { return _weaponAbility; }
		set 
		{
			// if not default, auto update abilitybar
			bool doUpdate = true;
			if (_weaponAbility == 0)
				doUpdate = false;

			_weaponAbility = value;

			if (doUpdate == true)
			{
				// dispatch event
				EventParam eventParam = new EventParam();
				eventParam.name = "abilityUpdated";
				eventParam.value = 1;
				eventParam.value2 = value;
				eventParam.character = this;
				EventManager.TriggerEvent("uiEvent", eventParam);
			}
		}
	}
	public int weaponModAbility
	{
		get { return _weaponModAbility; }
		set
		{
			// if not default, auto update abilitybar
			bool doUpdate = true;
			if (_weaponModAbility == 0)
				doUpdate = false;

			_weaponModAbility = value;

			if (doUpdate == true)
			{
				// dispatch event
				EventParam eventParam = new EventParam();
				eventParam.name = "abilityUpdated";
				eventParam.value = 2;
				eventParam.value2 = value;
				eventParam.character = this;
				EventManager.TriggerEvent("uiEvent", eventParam);
			}
		}
	}
	public int gearAbility
	{
		get { return _gearAbility; }
		set
		{
			// if not default, auto update abilitybar
			bool doUpdate = true;
			if (_gearAbility == 0)
				doUpdate = false;

			_gearAbility = value;

			if (doUpdate == true)
			{
				// dispatch event
				EventParam eventParam = new EventParam();
				eventParam.name = "abilityUpdated";
				eventParam.value = 4;
				eventParam.value2 = value;
				eventParam.character = this;
				EventManager.TriggerEvent("uiEvent", eventParam);
			}
		}
	}
	public int gearModAbility
	{
		get { return _gearModAbility; }
		set
		{
			// if not default, auto update abilitybar
			bool doUpdate = true;
			if (_gearModAbility == 0)
				doUpdate = false;

			_gearModAbility = value;

			if (doUpdate == true)
			{
				// dispatch event
				EventParam eventParam = new EventParam();
				eventParam.name = "abilityUpdated";
				eventParam.value = 3;
				eventParam.value2 = value;
				eventParam.character = this;
				EventManager.TriggerEvent("uiEvent", eventParam);
			}
		}
	}

}

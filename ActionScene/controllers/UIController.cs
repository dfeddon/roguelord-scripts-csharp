using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Roguelord 
{
	public class UIController : MonoBehaviour
	{
		// public GameObject canvasUI;
		public GameObject attackerHistory;
		// public GameObject attackerInfoView;
		public GameObject defenderHistory;
		// public GameObject defenderInfoView;
		public GameObject turnCounter;
		public GameObject turnCounterInfoText;
		public GameObject roundCounter;
		public GameObject healthbarsUI;
		public GameObject defendStack;
		
		public GameObject attackStack;
		public CharacterVO turnCharacter;
		public GameObject discardPileA;
		public GameObject discardPileD;
		public GameObject drawPileA;
		public GameObject drawPileD;
		public GameObject calloutPanel;
		private GameObject powerBank;
		private GameObject powerBankAGO;
		private GameObject powerBankDGO;
		private GameObject attackerPower;
		private int attackerPowerMax = 0;
		private int attackerPowerCurrent = 0;
		private GameObject defenderPower;
		private GameObject bankWithdraw;
		private int _powerBankA = 0;
		private int _powerBankD = 0;
		private GameObject canvasMenu;
		private GameObject canvasPanel;
		private GameObject menuScroller;
		private GameObject abilitybar;
		private GameObject abilityTooltip;
		private GameObject characterCapsulePanel;
		private GameObject characterActionPanel;
		// private Roguelord.CardController cardController;
		public AnimationCurve moveCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

		private UnityAction<EventParam> uiEventHandler;
		List<CharacterView> attackers;
		List<CharacterView> defenders;
		List<CharacterView> crews = new List<CharacterView>();
		private AudioSource audioSource;


		// Start is called before the first frame update
		void Awake()
		{
			// UI Canvas
			// canvasUI = GameObject.FindWithTag("CanvasUI");
			attackerHistory = GameObject.FindWithTag("AttackerHistory");
			// attackerInfoView = GameObject.FindWithTag("AttackerInfo");
			defenderHistory = GameObject.FindWithTag("DefenderHistory");
			// defenderInfoView = GameObject.FindWithTag("DefenderInfo");
			turnCounter = GameObject.FindWithTag("TurnCounter");
			turnCounterInfoText = GameObject.FindWithTag("TurnCounterInfoText");
			healthbarsUI = GameObject.FindWithTag("Healthbars");
			defendStack = GameObject.FindWithTag("DefendStack");
			attackStack = GameObject.FindWithTag("AttackStack");
			roundCounter = GameObject.FindWithTag("RoundCounter");
			calloutPanel = GameObject.FindWithTag("CalloutPanel");
			drawPileA = GameObject.FindWithTag("DrawPileA");
			discardPileA = GameObject.FindWithTag("DiscardPileA");
			drawPileD = GameObject.FindWithTag("DrawPileD");
			discardPileD = GameObject.FindWithTag("DiscardPileD");
			powerBank = GameObject.FindWithTag("PowerBank");
			powerBankAGO = powerBank.transform.GetChild(2).transform.gameObject;
			// powerBankDGO = powerBank.transform.GetChild(2).transform.gameObject;
			attackerPower = GameObject.FindWithTag("AttackerPower");
			defenderPower = GameObject.FindWithTag("DefenderPower");
			bankWithdraw = GameObject.FindWithTag("BankWithdraw");
			canvasMenu = GameObject.FindWithTag("CanvasMenu");
			menuScroller = canvasMenu.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
			canvasPanel = canvasMenu.transform.GetChild(0).gameObject;
			abilityTooltip = GameObject.FindWithTag("AbilityTooltip");
			abilitybar = GameObject.FindWithTag("AbilityBar");
			characterCapsulePanel = GameObject.FindWithTag("CharacterCapsulePanel");
			characterActionPanel = GameObject.FindWithTag("CharacterActionPanel");

			// by default, decativate all (but canvasui)
			foreach (Transform child in transform)
				child.gameObject.SetActive(false);

			// attackerInfoView.SetActive(false);
			// defenderInfoView.SetActive(false);

			// attackerHistory.SetActive(false);
			// attackerInfoView.SetActive(false);
			// defenderHistory.SetActive(false);
			// defenderInfoView.SetActive(false);

			// turnCounter.SetActive(false);
			// turnCounterInfoText.SetActive(false);
			// roundCounter.SetActive(false);
			// healthbarsUI.SetActive(false);
			uiEventHandler = new UnityAction<EventParam>(uiEventHandlerFunction);
			EventManager.StartListening("uiEvent", uiEventHandler);
		}

		void Start() 
		{
			// disable menu
			calloutPanel.SetActive(false);
			canvasMenu.SetActive(false);
			// abilityTooltip.SetActive(false);

			// powerBankAGO.GetComponent<TextMeshProUGUI>().text = powerBankA.ToString();
			// powerBankDGO.GetComponent<TextMeshProUGUI>().text = powerBankD.ToString();

			drawPileA.GetComponent<Button>().onClick.AddListener(() => ShowMenu("draw"));
			discardPileA.GetComponent<Button>().onClick.AddListener(() => ShowMenu("discard"));
			// bankWithdraw.GetComponent<Button>().onClick.AddListener(() => ShowMenu("withdraw"));

			canvasPanel.GetComponent<Button>().onClick.AddListener(() => CloseMenu());
			audioSource = gameObject.AddComponent<AudioSource>();
		}

		public void ShowAll(bool b = true)
		{
			foreach (Transform child in transform)
			{
				// omissions
				if (child.gameObject.name == "AttackerInfoView") continue;
				if (child.gameObject.name == "DefenderInfoView") continue;

				// show
				child.gameObject.SetActive(b);
			}
			// attackerInfoView.SetActive(false);
			// defenderInfoView.SetActive(false);
			
			// hide top panel
			characterCapsulePanel.gameObject.SetActive(b);
		}

		void uiEventHandlerFunction(EventParam eventParam)
		{
			switch(eventParam.name)
			{
				case "abilityUpdated":
					// update ability bar
					BuildAbilityBar(eventParam.character);
					// update capsule
					CharacterView c = ListHelper.getCharacterViewById(crews, eventParam.character.uid);
					UpdateCapsule(c);
				break;
			}
		}

		private void UpdateCapsule(CharacterView c)
		{
			// add profile image
			// CharacterView c = ListHelper.getCharacterViewById(crews, eventParam.character.uid);
			c.capsule.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = c.model.GetProfileSprite();
			// ... and tech sprite
			c.capsule.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<Image>().sprite = c.model.GetTechSprite();
			// ... weapon
			c.capsule.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = c.model.GetAbilitySpriteByUid(c.model.weaponAbility);
			// ... weapon mod
			c.capsule.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = c.model.GetAbilitySpriteByUid(c.model.weaponModAbility);
			// ... gear
			c.capsule.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = c.model.GetAbilitySpriteByUid(c.model.gearAbility);
			// ... gear mod
			c.capsule.transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = c.model.GetAbilitySpriteByUid(c.model.gearModAbility);
		}

		public void ShowHealthbars(bool b = true)
		{
			if (GameManager.instance.roundIsLocked == true) return;
			// if (b == true)
			//     canvasUI.SetActive(true);
			healthbarsUI.SetActive(b);
			// foreach (Transform child in healthbarsUI.transform)
				// child.gameObject.SetActive(false);
		}

		private void CloseMenu()
		{
			// clear contents
			foreach (Transform child in menuScroller.transform)
			{
				GameObject.Destroy(child.gameObject);
			}

			// close menu
			canvasMenu.SetActive(false);
		}

		private void ShowMenu(string from = "none")
		{
			Debug.Log("* Bank Withdraw Handler " + menuScroller);
			canvasMenu.SetActive(true);
			// CardView c;
			Roguelord.CardController cardController = GameManager.instance.cardController;
			List<CardStoreVO> list = null;

			switch(from)
			{
				case "discard":
					list = cardController.discardPile;
				break;

				case "draw":
					list = cardController.drawPile;
				break;

				case "inHand":
					list = cardController.inHand;
				break;

				case "withdraw":
					list = cardController.deck;
				break;
			}

			// for (int i = 0; i < 20; i++)
			CardVO vo;
			foreach(CardStoreVO cardStore in list)
			{
				vo = CardLibraryVO.GetCardByStore(cardStore);
				GameObject card = Instantiate(GameManager.instance.assetBundleCards.LoadAsset<GameObject>("CardPanel Variant")) as GameObject;
				// below might break?...
				vo.state = CardVO.CardState.Menu;
				card.GetComponent<CardView>().model = vo;
				card.transform.SetParent(menuScroller.transform);
			}
		}

		public void BuildAbilityBar(CharacterVO c)
		{
			Debug.Log("== UIController.BuildAbilityBar ==");
			ShowAll(true);

			// AbilityVO[] characterAbilities = AbilityMatrixVO.GetAbilitiesByClass(c.role);
			List<AbilityVO> characterAbilities = AbilityMatrixVO.GetCharacterAbilities(c);
			GameObject[] abilities = GameObject.FindGameObjectsWithTag("AbilityButtonView");
			AbilityImageView abilityImageView;
			Sprite sprite;
			Image image;
			foreach (GameObject ability in abilities)
			{
				abilityImageView = ability.GetComponent<AbilityImageView>();

				// ignore global, hard-coded Move ability (slot #0)
				if (abilityImageView.abilityNumber == 7) continue;
				// TODO: Temp fix below
				if ((abilityImageView.abilityNumber) > characterAbilities.Count) continue;
				// TODO: End fix
				Debug.Log("* " + (abilityImageView.abilityNumber - 1).ToString() + " / " + ability.name);
				abilityImageView.abilityVO = characterAbilities[abilityImageView.abilityNumber - 1];
				sprite = GameManager.instance.assetBundleCombat.LoadAsset<Sprite>(characterAbilities[abilityImageView.abilityNumber - 1].image);
				image = ability.transform.GetChild(0).GetComponent<Image>();
				image.sprite = sprite;
				// Debug.Log("<color=orange>AbilityImageView " + abilityImageView.abilityNumber + "</color>");

				// first, validate positioning
				bool isValidPosition = c.ValidatePositioning(abilityImageView.abilityVO.positionRequirement);
				abilityImageView.IsValidPosition(isValidPosition);
				// if out of position, disregard cooldowns
				if (isValidPosition == false) continue;

				// next, cooldowns
				foreach(AbilityCooldownVO cd in c.abilityCooldowns)
				{
					if (cd.uid == abilityImageView.abilityVO.uid)
					{
						if (cd.cooldownCounter > 0)
						{
							abilityImageView.IsOnCooldown(cd, true);
						}
						else
						{ 
							abilityImageView.IsOnCooldown(cd, false);
						}
					}
				}

			}
		}

		public AbilityVO updateAbilityBarBySlot(AbilityVO ability, int slot)
		{
			AbilityVO returnValue = null;



			return returnValue;
		}

		public IEnumerator BuildHealthbars(CrewView _crewAttackGroup, CrewView _crewDefendGroup, GameObject idleWindow)
		{
			if (attackers == null)
			{
				attackers = _crewAttackGroup.pool;
				defenders = _crewDefendGroup.pool;
				crews.AddRange(_crewAttackGroup.pool);
				crews.AddRange(_crewDefendGroup.pool);
			}
			yield return new WaitForSeconds(2.5f);
			BuildHealthbarsGo(_crewAttackGroup, _crewDefendGroup, idleWindow);
		}

		private void BuildHealthbarsGo(CrewView _crewAttackGroup, CrewView _crewDefendGroup, GameObject idleWindow)
		{
			// yield return new WaitForSeconds(2.5f);

			List<CharacterView> attackers = _crewAttackGroup.pool;//GetComponentsInChildren<CharacterView>();
			List<CharacterView> defenders = _crewDefendGroup.pool;//GetComponentsInChildren<CharacterView>();
			GameObject hbr = GameManager.instance.assetBundleCombat.LoadAsset<GameObject>("HealthbarComponent");
			GameObject ebr = GameManager.instance.assetBundleCombat.LoadAsset<GameObject>("EffectsbarComponent");
			// Vector3 capsulePanelPos = RectTransformUtility.PixelAdjustPoint(new Vector2(characterCapsulePanel.transform.position.x, characterCapsulePanel.transform.position.y));
			Vector2 offset = this.transform.root.GetComponent<RectTransform>().offsetMax;
			float offset2 = characterCapsulePanel.transform.GetComponent<RectTransform>().rect.height;
			float ystub = offset.y + offset2;// + 90; // TODO: <-- FIX THIS STUB
			Vector3 screenPos;
			GameObject container;
			GameObject capsule, actionpanel;
			int count = 0;
			foreach (CharacterView c in attackers)
			{
				screenPos = Camera.main.WorldToScreenPoint(new Vector3(c.prefab.transform.position.x, c.prefab.transform.position.y, 0));
				container = new GameObject("container");
				container.transform.SetParent(healthbarsUI.transform, false);
				container.transform.position = new Vector3(screenPos.x, healthbarsUI.transform.position.y, 0);
				c.healthbarContainer = container;

				// add and assign character capsule
				capsule = characterCapsulePanel.transform.GetChild(0).gameObject;
				c.capsule = capsule;
				// parent it
				capsule.transform.SetParent(container.transform, false);
				// and position
				capsule.transform.position = new Vector3(screenPos.x, ystub, 0);
				// add profile image
				UpdateCapsule(c);

				// actions
				actionpanel = characterActionPanel.transform.GetChild(0).gameObject;
				// actionpanel.GetComponent<ActionsViewController>().SetSource(c.model);
				c.actions = actionpanel;
				actionpanel.GetComponent<ActionsViewController>().character = c.model;
				actionpanel.transform.SetParent(container.transform, false);
				Vector3 head = c.prefab.GetComponent<CharController>().head.position;
				Vector3 t = Camera.main.WorldToScreenPoint(new Vector3(head.x, head.y + 0.5f, 0));
				t.y += 225;//75; // TODO: <-- ANOTHER Y STUB
				actionpanel.transform.position = t;

				// healthbar
				GameObject hbar = Instantiate(hbr) as GameObject;
				c.healthbar = hbar.GetComponent<HealthbarComponent>();
				c.healthbar.SetHealth(c.model.health);
				c.healthbar.UpdateAttack(c.model.attributes.attack, 0, true);
				c.healthbar.UpdateDefense(c.model.attributes.defense, 0, true);
				c.healthbar.isDefender = false;
				c.healthbar.transform.position = new Vector3(c.healthbar.transform.position.x, c.healthbar.transform.position.y - 25, c.healthbar.transform.position.y);
				hbar.transform.SetParent(container.transform, false);
				// hbar.transform.SetParent(healthbarsUI.transform, false);
				// hbar.transform.position = new Vector3(screenPos.x, healthbarsUI.transform.position.y, 0);

				// effects bar
				GameObject ebar = Instantiate(ebr) as GameObject;
				c.effectsbar = ebar.GetComponent<EffectsbarComponent>(); //effectsbarComponent;
				ebar.transform.SetParent(container.transform, false);
				ebar.transform.position = new Vector3(screenPos.x, healthbarsUI.transform.position.y + 15, 0);

				count++;
			}
			// int defenderCount = 0;
			foreach (CharacterView c in defenders)
			{
				screenPos = Camera.main.WorldToScreenPoint(new Vector3(c.prefab.transform.position.x, 0, 0));
				container = new GameObject("container");
				container.transform.SetParent(healthbarsUI.transform, false);
				container.transform.position = new Vector3(screenPos.x, healthbarsUI.transform.position.y, 0);
				c.healthbarContainer = container;

				// add character capsule
				capsule = characterCapsulePanel.transform.GetChild(0).gameObject;
				c.capsule = capsule;
				capsule.transform.SetParent(container.transform, false);
				capsule.transform.position = new Vector3(screenPos.x, ystub, 0);
				// capsule.transform.position = new Vector3(screenPos.x, characterCapsulePanel.transform.position.y, 0);

				// add profile image
				UpdateCapsule(c);

				/*/// actions
				actionpanel = characterActionPanel.transform.GetChild(0).gameObject;
				// actionpanel.GetComponent<ActionsViewController>().SetSource(c.model);
				c.actions = actionpanel;
				actionpanel.GetComponent<ActionsViewController>().character = c.model;
				actionpanel.transform.SetParent(container.transform, false);
				Vector3 head = c.prefab.GetComponent<CharController>().head.position;
				Vector3 t = Camera.main.WorldToScreenPoint(new Vector3(head.x, head.y + 0.5f, 0));
				t.y += 275; // TODO: <-- ANOTHER Y STUB
				actionpanel.transform.position = t;
				//*/

				GameObject hbar = Instantiate(hbr) as GameObject;
				c.healthbar = hbar.GetComponent<HealthbarComponent>();
				c.healthbar.SetHealth(c.model.health);
				c.healthbar.UpdateAttack(c.model.attributes.attack, 0, true);
				c.healthbar.UpdateDefense(c.model.attributes.defense, 0, true);
				screenPos = Camera.main.WorldToScreenPoint(new Vector3(c.prefab.transform.position.x, 0, 0));//c.prefab.transform.position);
				c.healthbar.transform.position = new Vector3(c.healthbar.transform.position.x, c.healthbar.transform.position.y - 25, c.healthbar.transform.position.y);
				hbar.transform.SetParent(container.transform, false);
				// hbar.transform.position = new Vector3(screenPos.x, healthbarsUI.transform.position.y, 0);

				// effects bar
				GameObject ebar = Instantiate(ebr) as GameObject;
				c.effectsbar = ebar.GetComponent<EffectsbarComponent>(); //effectsbarComponent;
				ebar.transform.SetParent(container.transform, false);
				ebar.transform.position = new Vector3(screenPos.x, healthbarsUI.transform.position.y + 15, 0);
				// Debug.Log("** screenPos " + screenPos.x);
				// VikingCrew.Tools.UI.SpeechBubbleManager.Instance.AddSpeechBubble(c.prefab.transform, "Hello world!");
				c.healthbar.isDefender = true;

				count++;
				// defenderCount++;
			}
			screenPos = Camera.main.WorldToScreenPoint(new Vector3(0, idleWindow.transform.position.y, 0));
			healthbarsUI.transform.position = new Vector3(healthbarsUI.transform.position.x, screenPos.y - 25, 0);
		}

		/*public void updateCharacterStatsUI(CharacterVO vo, int state)
		{
			// state: 1=enter, 0=exit
			// TODO currently disabled
			vo = null;

			if (vo == null) return;

			// update ability history panels
			if (vo.crewType == CharacterVO.CREW_TYPE_DEFENDER)
			{
				defenderHistory.GetComponent<AbilityHistoryView>().UpdateHistoryPanel(vo, state);
				// defenderInfoView.GetComponent<CharacterInfoView>().UpdateCharacterInfo(vo, state);
			}
			else 
			{
				attackerHistory.GetComponent<AbilityHistoryView>().UpdateHistoryPanel(vo, state);
				// attackerInfoView.GetComponent<CharacterInfoView>().UpdateCharacterInfo(vo, state);
			}

			// if (state == 1 && vo == null) return;
			// if (state == 0 && turnCharacter != null)
			// 	vo = turnCharacter;
			return;
			GameObject panel = null;
			switch (vo.crewType)
			{
				case CharacterVO.CREW_TYPE_ATTACKER:
					Debug.Log("* attacker " + vo.handle);
					// stats panel
					// panel = attackerInfoView.transform.Find("Panel").gameObject;
					// update stats
					panel.transform.Find("Handle").gameObject.GetComponent<TextMeshProUGUI>().text = vo.fullname;
					// handle.text = "Hello";

					// if mouse exit, default to active character
					if (state == 0)
					{
						
					}
					break;

				case CharacterVO.CREW_TYPE_DEFENDER:
					Debug.Log("* defender " + vo.handle);
					// stats panel
					// panel = defenderInfoView.transform.Find("Panel").gameObject;
					// update stats
					panel.transform.Find("Handle").gameObject.GetComponent<TextMeshProUGUI>().text = vo.fullname;

					// if mouse exit, default to active target (if set and single)
					if (state == 0)
					{

					}
					break;
			}
		}*/

		public void UnlockModHandler(AbilityVO ability)//int from)
		{
			Debug.Log("* Unlock mod " + ability.uid + " / " + ability.name);

			// remove listener
			// abilityTooltip.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Button>().onClick.RemoveListener(() => UnlockModHandler(ability));
			// validate available power versus cost of unlock (weapon mod cost = 4, gear mod cost = 3)
			Debug.Log("* avail power " + ability.owner.attributes.power);
			SoundVO soundFx = null;
			float volume = 1.0f;
			switch(ability.uid)
			{
				case 1001: // weapon mod (unlocked uid => 1003)
					// if (ability.owner.attributes.power < 4)
					// {
					// 	Debug.Log("* NOT enough power...");
					// 	return;
					// }
					// Debug.Log("* yes, enough power available for weapon mod purchase...");

					// reduce character power (event?)
					if (ability.owner.crewType == CharacterVO.CREW_TYPE_ATTACKER)
					{
						Debug.Log("* powerBankA " + powerBankA);
						if (powerBankA >= 4)
						{
							powerBankA -= 4;
							ability.owner.weaponModAbility = 1003;
							soundFx = new SoundVO(200, "Weapon Mod Locked", SoundVO.ABILITY_SELECT_UNLOCK);
							volume = 0.25f;
						}
						else
						{ 
							Debug.Log("...not enough crypto!");
							// modal window
							// sound fx
							soundFx = new SoundVO(200, "Unlock Fail", SoundVO.ABILITY_SELECT_UNLOCK_FAIL);
							volume = 0.25f;
						}
					}
					else
					{
						Debug.Log("* powerBankD " + powerBankD);
						if (powerBankD >= 4)
						{
							powerBankD -= 4;
							ability.owner.weaponModAbility = 1003;
							// soundFx = new SoundVO(200, "Weapon Mod Locked", SoundVO.ABILITY_SELECT_UNLOCK);
							// volume = 0.25f;
						}
						else
						{
							Debug.Log("...not enough crypto!");
							// soundFx = new SoundVO(200, "Unlock Fail", SoundVO.ABILITY_SELECT_UNLOCK_FAIL);
							// volume = 0.25f;
						}
					}

					// update characterVO.weaponModAbility
					// ability.owner.weaponModAbility = 1003;
				break;

				case 1002: // gear mod (unlocked uid => 1004)
					// if (ability.owner.attributes.power >= 3)
					// 	Debug.Log("* yes, enough power available for gear mod purchase...");
					// update characterVO.weaponModAbility
					// ability.owner.gearModAbility = 1004;

					if (ability.owner.crewType == CharacterVO.CREW_TYPE_ATTACKER)
					{
						if (powerBankA >= 3)
						{
							powerBankA -= 3;
							ability.owner.gearModAbility = 1004;
							soundFx = new SoundVO(200, "Gear Mod Locked", SoundVO.ABILITY_SELECT_UNLOCK);
							volume = 0.25f;
						}
						else
						{
							Debug.Log("...not enough crypto!");
							// modal window
							// sound fx
							// soundFx = new SoundVO(200, "Unlock Fail", SoundVO.ABILITY_SELECT_UNLOCK_FAIL);
							// volume = 0.25f;
						}
					}
					else
					{
						if (powerBankD >= 3)
						{
							powerBankD -= 3;
							ability.owner.gearModAbility = 1004;
							soundFx = new SoundVO(200, "Weapon Mod Locked", SoundVO.ABILITY_SELECT_UNLOCK);
							volume = 0.25f;
						}
						else
						{
							Debug.Log("...not enough crypto!");
							// soundFx = new SoundVO(200, "Unlock Fail", SoundVO.ABILITY_SELECT_UNLOCK_FAIL);
							// volume = 0.25f;
						}
					}


				break;
			}

			Debug.Log("a " + powerBankA + " / d " + powerBankD);
			if (soundFx != null)
				audioSource.PlayOneShot(soundFx.audioClip, volume);

			// refresh abilities
			// BuildAbilityBar(ability.owner);
			abilityTooltip.SetActive(false);
			// add fx?
		}
		public void ShowAbilityTooltip(AbilityVO a)
		{
			Debug.Log("* show tooltip " + a + " / " + abilityTooltip.activeSelf);
			if (abilityTooltip.activeSelf == true) 
			{
				abilityTooltip.SetActive(false);
				return;
			}
			Debug.Log("* uid " + a.uid);

			// GameObject go = null;
			abilityTooltip.SetActive(true);

			AbilityTooltipView view = abilityTooltip.GetComponent<AbilityTooltipView>();
			view.ability = a;
			if (view.uIController == null)
				view.uIController = this;// as UIController;
			/*
			switch(a.uid) {
				case 1000: // move
				break;

				case 1001: // locked weapon mod
					Debug.Log("case 1001");
					go = abilityTooltip.transform.GetChild(1).gameObject;
					go.SetActive(true);
					// upgrade button
					// go.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => UnlockModHandler(a));
				break;

				case 1002: // locked gear mod
					Debug.Log("case 1002");
					go = abilityTooltip.transform.GetChild(1).gameObject;
					go.SetActive(true);
					// upgrade button
					// go.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => UnlockModHandler(a));
				break;
			}*/
		}
		public IEnumerator DealCards()
		{
			Debug.Log("<color=yellow>== UIController.dealCards ==</color>");
			// if (turnCounter == null)
			// 	turnCounter = GameObject.FindWithTag("TurnCounter");

			yield return new WaitForSeconds(1.5f);
			// hide turn counter UI
			// turnCounter.SetActive(false);

			// activate cards
			// cardsUI.SetActive(true);
			List<GameObject> cards = new List<GameObject>();
			GameObject card1 = Instantiate(GameManager.instance.assetBundleCards.LoadAsset<GameObject>("CardPanel Variant")) as GameObject;
			GameObject card2 = Instantiate(GameManager.instance.assetBundleCards.LoadAsset<GameObject>("CardPanel Variant")) as GameObject;
			GameObject card3 = Instantiate(GameManager.instance.assetBundleCards.LoadAsset<GameObject>("CardPanel Variant")) as GameObject;
			cards.Add(card1);
			cards.Add(card2);
			cards.Add(card3);

			// card data
			CardVO card1data = CardLibraryVO.getChargedUpCard();
			card1data.owner = CardVO.CardOwner.Attacker;
			CardVO card2data = CardLibraryVO.getEvasiveManeuversCard();
			card2data.owner = CardVO.CardOwner.Attacker;
			CardVO card3data = CardLibraryVO.getExposedCard();
			card3data.owner = CardVO.CardOwner.Attacker;

			// card states
			card1.GetComponent<CardView>().model = card1data;
			card2.GetComponent<CardView>().model = card2data;
			card3.GetComponent<CardView>().model = card3data;
			// card1.GetComponent<CardView>().model.owner = CardVO.CardOwner.Attacker;
			// card2.GetComponent<CardView>().model.owner = CardVO.CardOwner.Attacker;
			// card3.GetComponent<CardView>().model.owner = CardVO.CardOwner.Attacker;

			// test defender stack
			GameObject card4 = Instantiate(GameManager.instance.assetBundleCards.LoadAsset<GameObject>("CardPanel Variant")) as GameObject;
			CardVO card4data = CardLibraryVO.getChargedUpCard();
			card4data.owner = CardVO.CardOwner.Defender;
			// card4.GetComponent<CardView>().model.state = CardVO.CardState.Dealer;
			card4.GetComponent<CardView>().model = card4data;
			cards.Add(card4);

			// CardSODisplay cod = card1.GetComponent<CardSODisplay>();
			// cod.card = GameManager.instance.assetBundleCards.LoadAsset<GameObject>("test1").GetComponent<CardSO>();
			// CardSO zcard = Object.Instantiate(GameManager.instance.assetBundleCards.LoadAsset<GameObject>("test1"));
			// ScriptableObject card2 = ScriptableObject.CreateInstance<CardSO>();
			// ScriptableObject card3 = ScriptableObject.CreateInstance<CardSO>();
			// Transform stackTransform;
			CardView cardView;
			foreach(GameObject card in cards)
			{
				cardView = card.GetComponent<CardView>();
				if (cardView.model.owner == CardVO.CardOwner.Attacker)
					card.transform.SetParent(attackStack.transform);
				else card.transform.SetParent(defendStack.transform);
			}
			// if (card1.GetComponent<CardView>().model.owner == CardVO.CardOwner.Defender)
			// 	stackTransform = defendStack.transform;
			// else stackTransform = attackStack.transform;
			// card1.transform.SetParent(stackTransform);
			// card2.transform.SetParent(stackTransform);
			// card3.transform.SetParent(stackTransform);

			//*
			// AddCards(card1, card2, card3);
			// card size
			RectTransform rt = (RectTransform)card1.transform;
			float attackCenter = ((Screen.width / 2) / 2) - (rt.rect.width / 2);
			float defendCenter = Screen.width - ((Screen.width / 3) / 2) - (rt.rect.width / 2);
			Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

			Vector3 pointA = card1.transform.position;
			// float baseZ = transform.forward + 1;
			pointA.x += 1.5f;
			Vector3 pointB = new Vector3(2 + 0.75f, 3f, 0f);
			Vector3 x = defendStack.transform.position;
			Vector3 cam = Camera.main.transform.position;

			Vector3 middle = screenCenter;// Vector3(Screen.width / 2, Screen.height / 2, 0);//cam.z);
			middle.y += (rt.rect.height / 2);
			Vector3 left = screenCenter;
			left.x -= rt.rect.width + 10;
			left.y += (rt.rect.height / 2);
			Vector3 right = screenCenter;
			right.x += rt.rect.width + 10;
			right.y += (rt.rect.height / 2);

			// time
			float seconds = 0.15f;
			yield return StartCoroutine(MoveObject(card1.transform, pointA, middle, seconds));

			// yield return new WaitForSeconds(0.15f);
			pointA = card2.transform.position;
			pointA.x += 1.5f;
			Vector3 pointC = new Vector3(0, 3f, -5f);//pointA.z);
			yield return StartCoroutine(MoveObject(card2.transform, pointA, left, seconds));

			// yield return new WaitForSeconds(0.15f);
			pointA = card3.transform.position;
			pointA.x += 1.5f;
			Vector3 pointD = new Vector3(0 - 0.75f, 3f, -5);//pointA.z);
			yield return StartCoroutine(MoveObject(card3.transform, pointA, right, seconds));
			//*/

			yield return null;
		}

		// public void ShowFooter(bool b)
		// {
		// 	GameObject.FindWithTag("CardsCanvas").gameObject.SetActive(b);
		// }

		public void updateAttackerPowerUI(CharacterAttributesVO vo)
		{
			Debug.Log("<color=white>== UIController.updateAttackPowerUI ==</color>");
			int max = attackerPowerMax = vo.powerMax;
			int current = attackerPowerCurrent = vo.power;
			// attackerPower.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = current.ToString() + "/" + max.ToString();
		}

		public void updateUIByCharacter(CharacterVO vo)
		{
			Debug.Log("<color=white>== UIController.updateUIByCharacter ==</color>");
			updateAttackerPowerUI(vo.attributes);
		}

		public void updateBankTotals(int a, int d)
		{
			powerBankA = a;
			powerBankD = d;
		}
		/*
		void AddCards(GameObject card1, GameObject card2, GameObject card3)
		{
			Debug.Log("<color=yellow>== SceneActionMain.addCards ==");

			// hide turn counter UI
			// turnCounter.SetActive(false);

			// card1 = Instantiate(cardBundle.LoadAsset<GameObject>("CardBase")) as GameObject;
			// card2 = Instantiate(cardBundle.LoadAsset<GameObject>("CardBase")) as GameObject;
			// card3 = Instantiate(cardBundle.LoadAsset<GameObject>("CardBase")) as GameObject;
			// card1.layer = 8;
			// card2.layer = 8;
			// card3.layer = 8;
			// card1.transform.SetParent(attackStack.transform);
			// card2.transform.SetParent(attackStack.transform);
			// card3.transform.SetParent(attackStack.transform);

			PlayingCardView cview1 = card3.GetComponent<PlayingCardView>();
			PlayingCardView cview2 = card2.GetComponent<PlayingCardView>();
			PlayingCardView cview3 = card3.GetComponent<PlayingCardView>();
			cview1.SetCardView(cardBundle, 0, 1);
			cview2.SetCardView(cardBundle, 1, 2);
			cview3.SetCardView(cardBundle, 2, 3);

			// CinemachineVirtualCamera cam = GameManager.instance.cinecamController.getCurrentCam();
			// float distanceFromCamera = cam.nearClipPlane;
			// Vector3 topLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distanceFromCamera));
			// card1.transform.localScale = new Vector3(card1.transform.localScale.x * 2, card1.transform.localScale.y * 2, card1.transform.localScale.z * 2);
			// float baseZ = transform.forward.z;
			Vector3 cam = Camera.main.transform.position;

			Vector3 screenBottomLeft = new Vector3(Screen.width / 2, Screen.height / 2, cam.z);//Camera.main.nearClipPlane);
			Vector3 inWorld = Camera.main.ScreenToWorldPoint(screenBottomLeft);
			card1.transform.position = screenBottomLeft;// inWorld;

			// card2.transform.localScale = new Vector3(card2.transform.localScale.x * 2, card2.transform.localScale.y * 2, card2.transform.localScale.z * 2);
			Vector3 screenBottomLeft1 = new Vector3((Screen.width / 2) - 5, Screen.height / 2, cam.z);//Camera.main.nearClipPlane);
			Vector3 inWorld2 = Camera.main.ScreenToWorldPoint(screenBottomLeft1);
			card2.transform.position = inWorld;//new Vector3(0, 2, cam.z);

			// card3.transform.localScale = new Vector3(card3.transform.localScale.x * 2, card3.transform.localScale.y * 2, card3.transform.localScale.z * 2);
			Vector3 screenBottomLeft2 = new Vector3((Screen.width / 2) - 25, Screen.height / 2, cam.z);//Camera.main.nearClipPlane);
			Vector3 inWorld3 = Camera.main.ScreenToWorldPoint(screenBottomLeft2);
			card3.transform.position = inWorld;//new Vector3(0, 2, cam.z);

			// card.layer = 8; // "Crew" layer
			// SpriteRenderer cardRend = card.GetComponent<SpriteRenderer>();
			// cardRend.sortingLayerName = "Cards";
			// Vector3 scale = new Vector3(0.05f, 0.05f, 1f);
			// cardRend.transform.localScale = scale;
			// card1.transform.SetParent(cardsUI.transform, false);
			// card2.transform.SetParent(cardsUI.transform, false);
			// card3.transform.SetParent(cardsUI.transform, false);
		}//*/
		IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
		{
			Debug.Log("== MoveObject ==");

			// AnimationCurve curve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
			thisTransform.DOMove(endPos, time)
				.ChangeStartValue(startPos)
				.SetEase(moveCurve);
			yield return null;

		}

		// getters & setters
		public int powerBankA
		{
			get { return _powerBankA; }
			set
			{
				_powerBankA = value;

				// update view
				powerBankAGO.GetComponent<TextMeshProUGUI>().text = value.ToString();
			}
		}
		public int powerBankD
		{
			get { return _powerBankD; }
			set
			{
				_powerBankD = value;

				// update view
				// powerBankDGO.GetComponent<TextMeshProUGUI>().text = value.ToString();
			}
		}
	}
}
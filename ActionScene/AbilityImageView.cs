using UnityEngine;
using System.Collections;
// using System.Reflection;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AbilityImageView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	// public Slider slider;
	public int abilityNumber;
	public AbilityVO abilityVO;
	// private Button button;
	private GameObject cooldownPanel;
	private GameObject cooldownBgOutline;
	private GameObject cooldownBg;
	private TextMeshProUGUI cooldownText;
	private GameObject positionPanel;
	private GameObject positionBg;
	private TextMeshProUGUI positionText;
	private AudioSource audioSource;

	void Awake()
	{
		// button = gameObject.GetComponent<Button>();
		cooldownPanel = transform.GetChild(2).gameObject;
		cooldownBgOutline = cooldownPanel.transform.GetChild(0).gameObject;
		cooldownBg = cooldownPanel.transform.GetChild(1).gameObject;
		cooldownText = cooldownPanel.transform.GetChild(2).transform.GetComponent<TextMeshProUGUI>();

		positionPanel = transform.GetChild(3).gameObject;
		positionBg = positionPanel.transform.GetChild(0).gameObject;
		positionText = positionPanel.transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>();
	}
	void Start()
	{
		// slider = gameObject.GetComponent<Slider>();
		// slider.wholeNumbers = true;
		// button = gameObject.GetComponent<Button>();
		cooldownBgOutline.SetActive(false);
		cooldownBg.SetActive(false);
		cooldownText.text = "";
		cooldownPanel.SetActive(false);

		positionPanel.SetActive(false);

		audioSource = this.gameObject.AddComponent<AudioSource>();
	}

	public void IsOnCooldown(AbilityCooldownVO cd, bool b)
	{
		cooldownPanel.SetActive(b);
		cooldownBg.SetActive(b);
		cooldownBgOutline.SetActive(b);
		cooldownText.text = cd.cooldownCounter.ToString();
	}

	public void IsValidPosition(bool b)
	{
		bool isActive = false;
		if (b == false)
			isActive = true;
		positionPanel.SetActive(isActive);
		positionBg.SetActive(isActive);
		positionText.text = (isActive == true) ? "P" : "";
	}

	public void OnPointerClick(PointerEventData e)
	{
		Debug.Log("OnPointerClick");
		// disallow clicks if cooldown panel not active
		if (cooldownPanel.activeSelf == true) return;

		// OnClick code goes here ...
		Debug.Log("I clicked ability #" + abilityNumber + " / " + abilityVO.name + " / " + abilityVO.uid);

		SoundVO soundFx = null;
		float volume = 1.0f;
		switch(abilityVO.uid)
		{
			case 1000: // move
				soundFx = new SoundVO(200, "Ability Move", SoundVO.ABILITY_SELECT_MOVE);
				volume = 0.25f;
			break;
			case 1001: // locked weapon mod
				soundFx = new SoundVO(200, "Weapon Mod Locked", SoundVO.ABILITY_SELECT_WEAPON_MOD_LOCKED);
				volume = 0.25f;
				break;
			case 1002: // locked gear mod
				soundFx = new SoundVO(200, "Gear Mod Locked", SoundVO.ABILITY_SELECT_GEAR_MOD_LOCKED);
				volume = 0.25f;
				break;
			case 1003: // empty weapon mod
				soundFx = new SoundVO(200, "Weapon Mod Empty", SoundVO.ABILITY_SELECT_WEAPON_MOD_EMPTY);
				volume = 0.5f;
				break;
			case 1004: // empty gear mod
				soundFx = new SoundVO(200, "Gear Mod Empty", SoundVO.ABILITY_SELECT_GEAR_MOD_EMPTY);
				volume = 0.5f;
				break;
			default:
				if (abilityVO.slot == 1)
				{
					soundFx = new SoundVO(200, "Ability Move", SoundVO.ABILITY_SELECT_DEFAULT_SLOT_1);
				}
				else
				{
					soundFx = new SoundVO(200, "Ability Select", SoundVO.ABILITY_SELECT);
					volume = 0.75f;
				}
			break;
		}

		audioSource.PlayOneShot(soundFx.audioClip, volume);


		// clone ability (so we don't retain extant values)
		EventParam eventParam = new EventParam();
		eventParam.value = abilityNumber;//characterView.model;
		eventParam.name = "abilitySelected";
		eventParam.data = new AbilityVO().getClone(abilityVO);//abilityVO;
		// eventParam.data = abilityVO;
		EventManager.TriggerEvent("combatEvent", eventParam);
	}
	public void OnPointerEnter(PointerEventData e)
	{
		Debug.Log("* Ability Enter Handler " + abilityVO.name);
		EventParam eventParam = new EventParam();
		eventParam.value = abilityNumber;//characterView.model;
		eventParam.name = "abilityEnter";
		eventParam.data = abilityVO;
		EventManager.TriggerEvent("combatEvent", eventParam);
	}

	public void OnPointerExit(PointerEventData e)
	{
		Debug.Log("* Ability Exit");
		EventParam eventParam = new EventParam();
		eventParam.value = abilityNumber;//characterView.model;
		eventParam.name = "abilityExit";
		eventParam.data = abilityVO;
		EventManager.TriggerEvent("combatEvent", eventParam);
	}

}
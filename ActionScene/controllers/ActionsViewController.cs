using UnityEngine;
using System.Collections;
// using System.Reflection;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class ActionsViewController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler//, IPointerClickHandler,
{
	// public Slider slider;
	private GameObject actionCards;
	private GameObject abilityTarget;
	private Image abilityImage;
	private Image targetImage;
	private Image removeImage;
	private Image card1Image;
	private Image card2Image;
	private Image card3Image;

	private TextMeshProUGUI positionText;
	public CharacterVO character;

	void Awake()
	{
		Debug.Log("<color=red>ActionsViewController.Awake</color>");

		// character.actions = new ActionVO();
	}
	void Start()
	{
		Debug.Log("<color=red>ActionsViewController.Start</color>");

		actionCards = transform.GetChild(0).gameObject;
		card1Image = actionCards.transform.GetChild(1).gameObject.GetComponent<Image>();
		card2Image = actionCards.transform.GetChild(0).gameObject.GetComponent<Image>();
		card3Image = actionCards.transform.GetChild(2).gameObject.GetComponent<Image>();

		abilityTarget = transform.GetChild(1).gameObject;
		removeImage = abilityTarget.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>();
		abilityImage = abilityTarget.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>();
		targetImage = abilityTarget.transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>();

		abilityImage.GetComponent<Button>().onClick.AddListener(() => ChangeAbility());
		targetImage.GetComponent<Button>().onClick.AddListener(() => ChangeTarget());
		removeImage.GetComponent<Button>().onClick.AddListener(() => RemoveAbility());

		// hide cards row
		// card1Image.transform.parent.gameObject.SetActive(false);
		actionCards.SetActive(false);
		// card1Image.color = new Color(card1Image.color.r, card1Image.color.g, card1Image.color.b, 0f);
		// hide abilitytarget row
		DoHide(abilityImage, true);
		DoHide(targetImage, true);
		DoHide(removeImage, true);
		abilityTarget.SetActive(false);

		// hide ability and target
	}

	public void AddAbility(AbilityVO a)
	{
		// Validate ability
		if(a.uid >= 1000) return;

		abilityTarget.SetActive(true);
		DoHide(abilityImage, false);
		abilityImage.sprite = GameManager.instance.assetBundleCombat.LoadAsset<Sprite>(a.image);

		character.actions.ability = a;

		// DoThrobImageFx(abilityImage);
		DoThrobGameObjectFx(abilityImage.transform.parent.gameObject);
	}

	private void ChangeAbility()
	{
		Debug.Log("* change ability");

		if (removeImage.color.a == 0)
		{
			DoHide(removeImage, false);
			// start auto-remove timer (5 sec)
			StartCoroutine("RemoveTimer");
		}
		else
		{ 
			DoHide(removeImage, true);
			StopCoroutine("RemoveTimer");
		}
		// set as active character
		EventParam eventParam = new EventParam();
		eventParam.name = "changeTurnCharacter";
		eventParam.character = character;//.actions.source;
		EventManager.TriggerEvent("combatEvent", eventParam);
	}

	private void AbilityOver()
	{
		Debug.Log("* ability over " + character.actions.ability.name);
	}

	IEnumerator RemoveTimer()
	{
		yield return new WaitForSeconds(5.0f);
		if (character.actions.ability != null)
			DoHide(removeImage, true);
	}

	public void AddTarget(CharacterVO vo)
	{
		Debug.Log("AddTarget " + vo.handle);
		// if (abilityImage.activeSelf == true)
		// {
			// abilityTarget.SetActive(true);
		// DoHide(targetImage, false);
		// }
		targetImage.sprite = vo.GetProfileSprite();
		DoHide(targetImage, false);
		character.actions.target = vo;

		// DoThrobImageFx(targetImage);
		DoThrobGameObjectFx(targetImage.transform.parent.gameObject);
	}

	private void ChangeTarget()
	{
		Debug.Log("* change target " + character.handle);
		EventParam eventParam = new EventParam();
		// eventParam.value = abilityNumber;//characterView.model;
		eventParam.name = "changeTarget";
		// eventParam.ability = character.actions.ability;
		eventParam.character = character;//.actions.source;
		// eventParam.gameObject = this.gameObject;
		EventManager.TriggerEvent("combatEvent", eventParam);
		// DoThrobGameObjectFx(targetImage.transform.parent.gameObject, false);
	}

	public void RemoveTarget()
	{
		character.actions.target = null;
		DoHide(targetImage, true);
	}

	public bool hasTarget()
	{
		if (character.actions.target != null)
			return true;
		return false;
	}
	public bool hasAbility()
	{
		if (character.actions.ability != null)
			return true;
		return false;
	}

	public void RemoveAbility(bool automated = false)
	{
		Debug.Log("* remove ability");

		// restore power cost
		if (automated == false)
			character.attributes.power += character.actions.ability.cost;

		// clear
		character.actions.ability = null;
		character.actions.target = null;

		// hide
		DoHide(abilityImage, true);
		DoHide(targetImage, true);
		DoHide(removeImage, true);
	}

	// public void SetSource(CharacterVO vo)
	// {
	// 	character.actions.source = vo;
	// }

	private void DoHide(Image i, bool doHide)
	{
		float opacity = 0f;
		if (doHide == false)
			opacity = 1.0f;
		i.color = new Color(i.color.r, i.color.g, i.color.b, opacity);
	}

	private void DoThrobImageFx(Image img, bool doThrob = true)
	{
		img.transform.DOScale(1.2f, 0.95f).SetLoops(-1, LoopType.Yoyo);
		// abilityImage.transform.DOScale(1.2f, 0.95f).SetLoops(-1, LoopType.Yoyo);
	}
	private void DoThrobGameObjectFx(GameObject go, bool doThrob = true)
	{
		Debug.Log("<color=purple>DoThrobGameObjectFx</color> " + doThrob);
		if (doThrob == true)// && DOTween.IsTweening(go.transform) == false)
		{
			if (DOTween.IsTweening(go.transform) == false)
				go.transform.DOScale(0.9f, 0.95f).SetLoops(-1, LoopType.Yoyo);
			else DOTween.Play(go.transform);
		}
		else DOTween.Pause(go.transform);
	}

	//*
		// public void OnPointerClick(PointerEventData e)
		// {
		// 	// OnClick code goes here ...
		// 	Debug.Log("PointerClick");
		// }
		public void OnPointerEnter(PointerEventData e)
		{
			Debug.Log("PointerEnter");
			switch(e.pointerEnter.name)
			{
				case "abilityImage":
					Debug.Log("Ability Image!");
				break;

				case "targetImage":
					Debug.Log("Target Image!");
				break;
			}
			// EventParam eventParam = new EventParam();
			// eventParam.value = abilityNumber;//characterView.model;
			// eventParam.name = "abilityEnter";
			// eventParam.data = abilityVO;
			// EventManager.TriggerEvent("combatEvent", eventParam);
		}

		public void OnPointerExit(PointerEventData e)
		{
			Debug.Log("* PointerExit");
			switch (e.pointerEnter.name)
			{
				case "abilityImage":
					Debug.Log("Ability Image!");
				break;

				case "targetImage":
					Debug.Log("Target Image!");
				break;
			}
		// EventParam eventParam = new EventParam();
		// eventParam.value = abilityNumber;//characterView.model;
		// eventParam.name = "abilityExit";
		// eventParam.data = abilityVO;
		// EventManager.TriggerEvent("combatEvent", eventParam);
	}
}
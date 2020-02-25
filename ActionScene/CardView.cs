using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

// namespace Roguelord {
public class CardView: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public AnimationCurve moveCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
	private CardVO _model;
	public int cardIndex = 0;
	public bool isLatest = false;
	public Vector3 relaxPosition;
	private Quaternion relaxRotation;
	private CardStackController controller;
	private Rigidbody rigidBody;
	public RectTransform rt;
	// views
	private Image cardbackImage;
	private GameObject imagePanel;
	private Image image;
	private GameObject costPanel;
	private Image costImage;
	public TextMeshProUGUI costText;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI descriptionText;
	public GameObject prefab;


	public CardView()
	{
		Debug.Log("== CardView ==");
	}
	void Awake() {
		// rigidBody = gameObject.AddComponent<Rigidbody>();
		rt = GetComponent<RectTransform>();
		prefab = transform.gameObject;
		// rt.sizeDelta = new Vector2(rt.rect.width/2, rt.rect.height/2);
		cardbackImage = transform.GetComponent<Image>();
		imagePanel = transform.GetChild(0).gameObject;
		image = imagePanel.transform.GetChild(0).transform.GetComponent<Image>();
		costPanel = transform.GetChild(1).gameObject;
		costText = costPanel.transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>();
		nameText = transform.GetChild(2).transform.GetComponent<TextMeshProUGUI>();
		descriptionText = transform.GetChild(3).transform.GetComponent<TextMeshProUGUI>();
	}
	void Start()
	{
		
	}
	public void OnPointerClick(PointerEventData pointerEventData)
	{
		Debug.Log("click " + name + " / " + model.state);
		controller = transform.parent.GetComponent<CardStackController>();
		EventParam eventParam;
		switch(model.state)
		{
			case CardVO.CardState.Dealer:
				model.state = CardVO.CardState.InHand;
				isLatest = true;
				// controller.NewCardAdded(this);
				eventParam = new EventParam();
				eventParam.name = "dealerSelected";
				eventParam.value = this.cardIndex;
				eventParam.card = this;
				EventManager.TriggerEvent("cardEvent", eventParam);
				break;

			case CardVO.CardState.InHand:
				Debug.Log("stowed!");
				eventParam = new EventParam();
				eventParam.name = "stowedCardActivated";
				eventParam.value = this.cardIndex;
				eventParam.card = this;
				EventManager.TriggerEvent("cardEvent", eventParam);
			break;
		}
	}
	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		Debug.Log("enter " + name);
		EventParam eventParam = new EventParam();
		eventParam.name = "mouseEnter";
		eventParam.value = cardIndex;
		eventParam.owner = model.owner;
		eventParam.card = this;
		EventManager.TriggerEvent("cardEvent", eventParam);
	}
	public void OnPointerExit(PointerEventData pointerEventData)
	{
		Debug.Log("exit " + name);
		EventParam eventParam = new EventParam();
		eventParam.name = "mouseExit";
		eventParam.value = cardIndex;
		eventParam.owner = model.owner;
		eventParam.card = this;
		EventManager.TriggerEvent("cardEvent", eventParam);
	}

	public void doRelax(float rot = 0)
	{
		// if (rot != 0)
			// relaxRotation.x = rot;

		Debug.Log("* doRelax " + rot + " / " + cardIndex);
		// transform.position = relaxPosition;
		// transform.rotation = relaxRotation;

		if (model.state != CardVO.CardState.InHand) 
		{
			Debug.Log("*** Not InHand... " + model.state);
			return;
		}

		MoveObject(relaxPosition, rot, 0.25f);
	}
	public void setRelax(Vector3 position)
	{
		Debug.Log("<color=yellow>== CardView.setRelax ==</color>");
		Debug.Log(cardIndex + " / " + position);
		if (position != null)
			relaxPosition = position;
		else relaxPosition = transform.position;
		
		relaxRotation = transform.rotation; // Quaternion.identity

		MoveObject(relaxPosition, relaxRotation.x, 0.25f);
	}
	// public IEnumerator CardActionOnPlayer(CharacterView character)
	// {
	// 	// TODO fix this! state update logic handled by action
	// 	model.state = CardVO.CardState.DiscardPile;

	// 	// get positions/sizes
	// 	Vector3 characterPos = Camera.main.WorldToScreenPoint(character.prefab.transform.position);
	// 	Vector3 characterSize = Camera.main.WorldToScreenPoint(character.prefab.GetComponent<BoxCollider>().bounds.size);
	// 	RectTransform cardRect = (RectTransform)transform;

	// 	// move card over character        
	// 	Vector3 toPos = characterPos;
	// 	toPos.y += (cardRect.rect.height / 2) + (characterSize.y / 2);
	// 	StartCoroutine(MoveObject(toPos, 0f, 0.35f));

	// 	yield return new WaitForSeconds(1.5f);

	// 	// update character (apply card action by level)
	// 	character.model.AssignedCard(model, model.levelData[model.level - 1]);

	// 	// discard
	// 	StartCoroutine(Discard());
	// }
	/*public IEnumerator StartAction2()
	{
		Debug.Log("<color=yellow>== CardView.StartAction2 ==</color>");
		// get level data
		CardLevelDataVO leveldata = model.levelData[model.level - 1];
		// get action type 2
		switch(leveldata.actionType2)
		{
			case CardLevelDataVO.CardActionType.Draw:
			break;
		} 

		yield return null;
	}*/
	
	public IEnumerator Discard()//, Vector3 endPos, float time)
	{
		Debug.Log("== DiscardObject ==");

		model.state = CardVO.CardState.DiscardPile;

		float time = 0.5f;

		Vector3 toPos;
		if (model.owner == CardVO.CardOwner.Attacker)
			toPos = GameObject.FindWithTag("DiscardPileA").transform.position;
		else toPos = GameObject.FindWithTag("DiscardPileD").transform.position;

		Sequence s = DOTween.Sequence();

		s.Append(transform.DOMove(toPos, time)
			// .ChangeStartValue(startPos)
			.SetEase(moveCurve)
		);
		s.Join(transform.DOScale(0.25f, time));
		// s.Join(transform.GetComponent<SpriteRenderer>().DOFade(0f, "color", time));
		s.Join(transform.DORotate(toPos, time, RotateMode.Fast));
		
		yield return new WaitForSeconds(time);
		
		// unparent card from stack
		transform.SetParent(null);
		// disable
		transform.gameObject.SetActive(false);
		// update counts
		EventParam eventParam = new EventParam();
		eventParam.name = "updatePileCounts";
		// eventParam.value = this.cardIndex;
		// eventParam.card = this;
		EventManager.TriggerEvent("cardEvent", eventParam);
	}
	// private void MoveCallback(bool isRelaxing = false)//CardView card, CardVO.CardState state)
	// {
	// 	if (isRelaxing == true)
	// 	{
	// 		EventParam eventParam = new EventParam();
	// 		eventParam.name = "cardMoved";
	// 		eventParam.value = this.cardIndex;
	// 		eventParam.card = this;
	// 		EventManager.TriggerEvent("cardEvent", eventParam);
	// 	}
	// }
	public void MoveObject(Vector3 toPos, float toRot = 0, float duration = 0.25f)//, bool isRelaxing = false)
	{
		Debug.Log("== CardView.MoveObject ==");

		Sequence s = DOTween.Sequence();
		s.Append(transform
			.DOMove(toPos, duration)
			.SetEase(moveCurve)
			// .OnComplete(() => MoveCallback(isRelaxing))
		);
		s.Join(transform
			.DORotate(new Vector3(0f, 0f, toRot), duration)
		);

		// transform.DOMove(endPos, time)
		// 	.ChangeStartValue(transform.position)
		// 	.SetEase(moveCurve);
		// transform.DOMove(endRot, time)
		// 	.ChangeStartValue(transform.rotation)
		// 	.SetEase(moveCurve);
		// yield return null;
	}

	public CardVO model
	{
		get { return _model; }
		set
		{
			_model = value;

			// update view
			costText.text = value.cost.ToString();
			nameText.text = value.name;
			// update description text with level data
			string revised = value.description.Replace(":levelstring1:", value.levelData[value.level - 1].text1);
			if (value.levelItemsTotal == 2)
				revised = revised.Replace(":levelstring2:", value.levelData[value.level - 1].text2);
			descriptionText.text = revised;//value.description;

			// cardback
			if (value.owner == CardVO.CardOwner.Defender)
			{
				cardbackImage.sprite = GameManager.instance.assetBundleCards.LoadAsset<Sprite>("cardback-yellow") as Sprite;
				imagePanel.SetActive(false);
				// costPanel.SetActive(false);
				nameText.text = "";
				descriptionText.text = "";
			}
			else
			{
				imagePanel.SetActive(true);
				image.sprite = GameManager.instance.assetBundleCards.LoadAsset<Sprite>(value.image) as Sprite;
			}
		}
	}
}
// }
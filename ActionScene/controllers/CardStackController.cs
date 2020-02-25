using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using Roguelord;

public class CardStackController: MonoBehaviour
{
	// private UnityAction<EventParam> stowedEventHandler;
	public CardVO.CardOwner stackType;
	private UnityAction<EventParam> cardEventHandler;
	private bool isFanned = false;
	// private float attackCenter = ((Screen.width / 2) / 2) - (rt.rect.width / 2);
	// private float defendCenter = Screen.width - ((Screen.width / 3) / 2) - (rt.rect.width / 2);
	private Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
	public AnimationCurve moveCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
	private float gutterWidth;
	private float gutterX;
	private Vector2 gutterMid;
	private float midX;
	private RectTransform stackRect;

	void Awake()
	{
		Debug.Log("<color=blue>== CardStackController.Awake ==</color>");
		// stowedEventHandler = new UnityAction<EventParam>(stowedEventHandlerFunction);
		// EventManager.StartListening("stowedEvent", stowedEventHandler);
		cardEventHandler = new UnityAction<EventParam>(cardEventHandlerFunction);
		EventManager.StartListening("cardEvent", cardEventHandler);
	}
	void Start()
	{
		Debug.Log("<color=blue>== CardStackController.Start ==</color>");

		stackRect = (RectTransform) transform;
		// GameObject abilityBar = GameObject.FindWithTag("AbilityBar");
		// RectTransform abarRect = (RectTransform) abilityBar.transform;

		// GameObject canvasUI = GameObject.FindWithTag("CanvasUI");
		if (stackType == CardVO.CardOwner.Attacker)
		{
			// get distance from left edge of screen to right edge of ability bar
			// gutterX = stackRect.rect.xMin;
			// gutterWidth = gutterX + stackRect.rect.width;
			// gutterMid = gutterWidth / 2;
			gutterMid = stackRect.rect.center;
		}
		else 
		{
			// get distance from right edge of screen to left edge of ability bar
			// gutterX = abarRect.rect.xMax;// + abarRect.rect.width;
			// gutterWidth = stackRect.rect.width - gutterX;
			gutterMid = stackRect.rect.center;// gutterWidth / 2;
		}
		Debug.Log("done");
	}

	// void OnMouseEnter()
	// {
	// 	Debug.Log("IN!");
	// }
	// void OnMouseExit()
	// {
	// 	Debug.Log("OUT!");
	// }

	void cardEventHandlerFunction(EventParam eventParam)
	{
		Debug.Log("<color=lightblue>== CardStackController.cardEventHandlerFunction ==</color>");
		Debug.Log("== "+ eventParam.name + " / " + stackType + " / " + eventParam.owner);
		
		if (stackType != eventParam.owner) return;

		switch(eventParam.name)
		{
			case "mouseEnter":
				if (eventParam.card.model.state == CardVO.CardState.InHand)
					FanStack(eventParam.card.cardIndex);
			break;
			case "mouseExit":
				Debug.Log("# " + eventParam.card.model.state);
				if (eventParam.card.model.state == CardVO.CardState.InHand)
					ResetStack();
			break;

			case "cardMoved":
				// ResetStack();
			break;

			case "stowedCardActivated":
				
			break;

			case "updatePileCounts":
				ReindexStack();
			break;
		}
	}

	public void DealHand(List<CardStoreVO> hand)
	{
		Debug.Log("<color=orange>== CardStackController.DealHand ==</color>");
		Debug.Log(hand.Count);
		CardVO vo;
		CardView view;

		foreach(CardStoreVO store in hand)
		{
			Debug.Log("### " + store.uid + " / " + store.level);
		}
	}
	void FanStack(int selectedIndex)
	{
		Debug.Log("<color=orange>== CardStackController.FanStack ==</color>");
		Debug.Log("<color=orange> * card index " + selectedIndex + "</color>");
		// CardView card;
		int total = transform.childCount;
		int count = 0;
		float baseX = total * 15;
		Vector3 newPos = new Vector3();
		float newRot = 0, rotVal, xVal;
		// get middle index
		// foreach (Transform child in transform)
		// for (int i = 0; i < GetCountInHand(); i++)
		foreach(CardView card in GetCardsInHand())
		{
			// card = transform.GetChild(i).gameObject.GetComponent<CardView>();
			// card = child.GetComponent<CardView>();
			// if (card.model.state != CardVO.CardState.InHand) continue;
			Debug.Log("* cardIndex " + card.cardIndex + " / " + selectedIndex + " / " + GetCountInHand());// card.model.state);			
			// float rotVal, xVal, 
			float yVal;
			//*
			if (!card) continue;
			if (card.cardIndex < selectedIndex)
			{
				newRot = -6f;
				xVal = 75f;//0.45f;
				yVal = 25f;//0.25f;
				// card.transform.Rotate(0f, 0f, rotVal);
				newPos = new Vector3(card.relaxPosition.x + xVal, card.relaxPosition.y + yVal, card.relaxPosition.z);
				// Debug.Log("* negative " + card.cardIndex + " / " + rotVal);
			}
			else if (card.cardIndex > selectedIndex)
			{
				newRot = 6f;
				xVal = -75f;//-0.35f;
				yVal = 25f;//0.25f;
				// card.transform.Rotate(0f, 0f, rotVal);
				newPos = new Vector3(card.relaxPosition.x + xVal, card.relaxPosition.y + yVal, card.relaxPosition.z);
				// Debug.Log("* positive " + card.cardIndex + " / " + rotVal);
			}
			else if (card.cardIndex == selectedIndex) // selected card
			{
				// raise it higher than the rest
				yVal = 125;//0.35f;
				// yval => card.height
				yVal = card.rt.rect.height - stackRect.rect.height;
				newPos = new Vector3(card.relaxPosition.x, yVal, 10);
				newRot = 0f;//new Quaternion();
	
				// reindex selected and 2 closest neighbors
				if (card.cardIndex > 0)
					GetCardByCardIndex(card.cardIndex - 1).transform.SetAsLastSibling();
				if (card.cardIndex < (GetCountInHand() - 1))
					GetCardByCardIndex(card.cardIndex + 1).transform.SetAsLastSibling();
				card.transform.SetAsLastSibling();
				// StartCoroutine(card.MoveObject(newPos, newRot, 0.25f));
			}
			card.MoveObject(newPos, newRot, 0.25f);

			count += 1;
		}
	}

	private int GetCountInHand()
	{
		int count = 0;
		foreach (Transform child in transform)
		{
			// card = transform.GetChild(count).gameObject.GetComponent<PlayingCardView>();
			if (child.GetComponent<CardView>().model.state == CardVO.CardState.InHand)
				count += 1;
		}

		return count;
	}

	public List<CardView> GetCardsInHand(bool reverseList = false)
	{
		List<CardView> list = new List<CardView>();
		CardView card;
		foreach (Transform child in transform)
		{
			card = child.GetComponent<CardView>();
			if (card.model.state == CardVO.CardState.InHand)
				list.Add(card);
		}

		list.Sort((p1, p2) => p1.cardIndex.CompareTo(p2.cardIndex));

		if (reverseList == true)
			list.Reverse();

		return list;
	}

	private CardView GetCardByCardIndex(int index)
	{
		foreach(CardView card in GetCardsInHand())
		{
			if (card.cardIndex == index) 
				return card;
		}
		return null;
	}

	public void ReindexStack()
	{
		Debug.Log("<color=orange>== CardStackController.ReindexStack ==</color>");

		// PlayingCardView card;
		// CardView card;
		// int mid = GetCountInHand() / 2; //transform.childCount / 2;

		int count = 0;
		foreach (CardView card in GetCardsInHand())// child in transform)
		{
			// card = transform.GetChild(count).gameObject.GetComponent<PlayingCardView>();
			// card = child.GetComponent<CardView>();
			// if (card.model.state == CardVO.CardState.InHand)//if (card.isStowed == true)
			// {
				Debug.Log("* " + count);
				card.cardIndex = count;
				count += 1;
			// }
			// count += 1;
		}

		ResetStack();
	}

	private void ResetStack()
	{
		Debug.Log("<color=orange>== CardStackController.ResetStack ==</color>");

		// CardView card;
		// int count = 0;
		int mid = GetCountInHand() / 2;
		Debug.Log("* mid = " + mid);
		float rotTo = 0;
		int total = GetCountInHand();
		int count = 0;

		if (total == 0) 
		{
			Debug.Log("* no card to reset");
			return;
		}


		float center = Mathf.Floor(GetCountInHand() / 2);
		if (center < 1) center = 1;
		Transform centerCard = transform.GetChild(Mathf.RoundToInt(center) - 1);
		// centerCard.x - mid
		float offset = centerCard.gameObject.transform.position.x - gutterMid.x;// midX;
		float centerPosX = centerCard.position.x - (5 * total);

		// foreach (Transform child in transform)
		foreach(CardView card in GetCardsInHand(true))
		{
			// if (transform.GetChild(count).gameObject.activeInHierarchy == false) continue;

			// reposition by index
			// move extant cards to the left
			// if (transform.childCount > 1 && card.isLatest == false)// && count != 1)
			// {
			
			//*
			Debug.Log("* move it " + card.transform.position.z);
			// card.transform.position = new Vector3(card.transform.position.x - (5 * total), card.transform.position.y, card.transform.position.z);
			card.transform.SetSiblingIndex(transform.GetChild(count).GetSiblingIndex());
			// get X center point

			// update relaxed position
			// float newx = stackRect.rect.width - (125 * count);
			float newx = gutterMid.x + 700 + (125 * count); // TODO: Fix this!! 700 stub!
			// newx = 0 + (125 * count);
			card.setRelax(new Vector3(newx, card.relaxPosition.y, card.relaxPosition.z));
			//*/

			// }
			// card.isLatest = false;

			// card = transform.GetChild(count).GetComponent<CardView>();
			// card = child.GetComponent<CardView>();
			Debug.Log("## " + card.model.state + " / " + card.cardIndex);
			// if (card.model.state != CardVO.CardState.InHand) continue;
			//
			rotTo = 0;
			if (card.cardIndex > mid)
				rotTo = 10;
			else if (card.cardIndex < mid)
				rotTo = -10;
			card.doRelax(rotTo);
			// if (card.trans)
			// child.rotation = Quaternion.identity;
			/*if (card.rotVal != 0)
			{
				Debug.Log("* resetting " + card.cardIndex);
				child.Rotate(0f, 0f, card.rotVal * -1);
				child.position = new Vector3(child.position.x + (card.xVal * -1), child.position.y + (card.yVal * -1), child.position.z);
				card.rotVal = 0;
				card.xVal = 0;
				card.yVal = 0;
			}
			else // selected card
			{
				Debug.Log("* NOT resetting " + card.cardIndex);
				child.position = new Vector3(child.position.x, child.position.y + (card.yVal * -1), child.position.z);
				card.yVal = 0;
			}*/
			// count += 1;

			card.transform.SetAsLastSibling();
			count += 1;
		}

		// z order
		/*CardView c;
		for (int i = 0; i < GetCountInHand(); i++)
		{
			c = transform.GetChild(i).GetComponent<CardView>();
			Debug.Log("z order " + c.cardIndex);
			c.transform.SetAsLastSibling();
		}*/
		// yield return null;
	}

	public void NewCardAdded(CardView newCard)
	{
		Debug.Log("<color=yellow>==CardStackController.NewCardAdded == " + transform.childCount + "</color>");
		newCard.isLatest = true;

		// first, remove dealer cards
		CardView card;
		foreach (Transform child in transform)
		{
			card = child.GetComponent<CardView>();

			// remove unselected dealer cards
			if (card.model.state == CardVO.CardState.Dealer)
			{
				child.gameObject.transform.parent = null;
				child.gameObject.SetActive(false);
				// Destroy(child.gameObject);
			}
		}

		// move extant cards back
		foreach(CardView existingCard in GetCardsInHand())
		{
			existingCard.cardIndex += 1;
		}
		// move new card to front
		newCard.cardIndex = 0;

		// update stack
		MoveCardToStack(newCard);
	}

	public void MoveCardToStack(CardView newCard)
	{
		Debug.Log("<color=yellow>== CardStackController.MoveCardToStack ==</color>");
		// move new card into stack
		// CardView card;
		Vector3 newPos = new Vector3(newCard.gameObject.transform.position.x, newCard.gameObject.transform.position.y, newCard.gameObject.transform.position.z);
		int total = transform.childCount;
		RectTransform rt = GetComponent<RectTransform>();
		float cardwidth = newCard.rt.rect.width;
		float midX = gutterMid.x;// rt.rect.width / 2;
		float gapVal = rt.rect.width / total;
		if (gapVal > (cardwidth/ 4)) gapVal = cardwidth;
		float gap = (total * gapVal);
		Debug.Log("<color=purple> #gap " + gap + " / cardwidth " + cardwidth + " type " + newCard.model.name + "</color>");
		newPos.x = gap + (rt.rect.width / 2 + (newCard.rt.rect.width / 2));
		newPos.y = rt.rect.y;// + (rt.rect.height / 2);

		float center = Mathf.Floor(total / 2);
		if (center < 1) center = 1;
		Transform centerCard = transform.GetChild(Mathf.RoundToInt(center) - 1);
		// centerCard.x - mid
		float offset = centerCard.gameObject.transform.position.x - midX;
		Debug.Log("<color=green>offset " + offset + "</color>");

		// if (stackType == CardVO.CardOwner.Attacker)
		// newPos = new Vector3((Screen.width / 4), 0, 0);
		// else newPos = new Vector3((Screen.width / 1.25f), 0, 0);
		// newPos = new Vector3(0, 0, 0);
		// newCard.setRelax(newPos);
		// StartCoroutine(MoveObject(newCard.transform, newCard.transform.position, newPos, 5.5f));

		// yield return new WaitForSeconds(30.5f);

		// assimilate into stack
		// float zi = 1;//0.01f;
		int count = 0;
		// foreach (Transform child in transform)
		foreach(CardView card in GetCardsInHand())
		{
			// card = transform.GetChild(count).gameObject.GetComponent<CardView>();
			// card = child.GetComponent<CardView>();
			
			// ignore dealer cards
			// if (card.model.state != CardVO.CardState.InHand) continue;
			
			card.cardIndex = count; // update card index
			Debug.Log("* cardIndex " + card.cardIndex + " / " + card.model.name + " / " + card.isLatest + " / " + transform.position.z);
			
			// update positioning...
			card.transform.position = new Vector3(card.transform.position.x, card.transform.position.y, card.transform.position.z);

			// move extant cards to the left
			if (transform.childCount > 1 && card.isLatest == false)// && count != 1)
			{
				Debug.Log("* move it " + card.transform.position.z);
				card.transform.position = new Vector3(card.transform.position.x - (5 * total), card.transform.position.y, card.transform.position.z);// + count);//zi);
				card.transform.SetSiblingIndex(transform.GetChild(count).GetSiblingIndex());
				// update relaxed position
				card.setRelax(card.transform.position);//newPos);
			}
			card.isLatest = false;

			// card.setRelax(child.transform.position);

			count += 1;
			Debug.Log("x " + card.transform.position.z);

		}

		// ReindexStack();

		// newPos = new Vector3(0, 0, 0);
		newPos.x -= (125 * total);//125f;
		// Debug.Log("======================== " + newPos.x);
		newCard.setRelax(newPos);
		StartCoroutine(MoveObject(newCard, newCard.transform.position, newPos, 0.15f, CardVO.CardState.InHand));

		// yield return null;
	}
	// public void NewCardAddedOff()
	// {
	// 	Debug.Log("<color=yellow>==CardStackController.NewCardAdded == " + transform.childCount + "</color>");

	// 	// assimilate into stack
	// 	PlayingCardView card;
	// 	int count = 0;
	// 	float zi = 0.01f; 
	// 	foreach (Transform child in transform)
	// 	{
	// 		card = transform.GetChild(count).gameObject.GetComponent<PlayingCardView>();
	// 		card.cardIndex = count; // update card index
	// 		Debug.Log("* cardIndex " + card.cardIndex + " / " + card.model.name);
	// 		card.transform.position = new Vector3(card.transform.position.x, card.transform.position.y, card.transform.position.z - zi);

	// 		// move extant cards to the left
	// 		if (transform.childCount > 1 && card.isLatest == false)// && count != 1)
	// 		{
	// 			child.transform.position = new Vector3(child.transform.position.x + 0.25f, child.transform.position.y, child.transform.position.z);
	// 		}
	// 		else card.isLatest = false;

	// 		count += 1;
	// 	}
	// }

	private void MoveCallback(CardView card, CardVO.CardState state)
	{
		// only update state if valid state is passed in
		if (state != CardVO.CardState.None)
		{
			card.model.state = state;
			ReindexStack();
		}
	}
	IEnumerator MoveObject(CardView cardView, Vector3 startPos, Vector3 endPos, float time, CardVO.CardState state = CardVO.CardState.None)
	{
		Transform thisTransform = cardView.gameObject.transform;
		Debug.Log("== CardStackController.MoveObject == " + endPos.x);

		Sequence s = DOTween.Sequence();
		s.Append(thisTransform.DOMove(endPos, time)
			.ChangeStartValue(startPos)
			.SetEase(moveCurve)
			.OnComplete(()=>MoveCallback(cardView, state))
		);
		s.Join(thisTransform.DOScale(1.0f, time));
		// s.Join(transform.GetComponent<SpriteRenderer>().DOFade(0f, "color", time));
		// s.Join(transform.DORotate(toPos, time, RotateMode.Fast));


		// thisTransform.DOMove(endPos, time)
		// 	.ChangeStartValue(startPos)
		// 	.SetEase(moveCurve);

		yield return null;
	}

}

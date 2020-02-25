using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Roguelord;

namespace Roguelord {
	public class CardController: MonoBehaviour
	{
		private UnityAction<EventParam> cardEventHandler;
		private List<GameObject> dealerCards;
		public List<CardStoreVO> inHand = new List<CardStoreVO>();
		public List<CardStoreVO> drawPile = new List<CardStoreVO>();
		public List<CardStoreVO> discardPile = new List<CardStoreVO>();
		public List<CardStoreVO> exhaustPile = new List<CardStoreVO>();
		public List<CardStoreVO> deck = new List<CardStoreVO>();
		private GameObject drawPileA;
		private GameObject discardPileA;
		private GameObject drawPileD;
		private GameObject discardPileD;
		// List<CardStoreVO> drawPileD = new List<CardStoreVO>();
		// List<CardStoreVO> discardPileD = new List<CardStoreVO>();
		// List<CardStoreVO> exhaustPileD = new List<CardStoreVO>();
		private List<GameObject> pool = new List<GameObject>();
		private int poolTotal = 10;
		public GameObject defendStack;
		public GameObject attackStack;
		private Vector3 originalPosition, originalScale;
		private Quaternion originalRotation;
		public AnimationCurve moveCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

		void Awake()
		{
		}

		void Start()
		{
			GameManager.instance.cardController = this;//cardController
			defendStack = GameObject.FindWithTag("DefendStack");
			attackStack = GameObject.FindWithTag("AttackStack");
			drawPileA = GameObject.FindWithTag("DrawPileA");
			discardPileA = GameObject.FindWithTag("DiscardPileA");
			drawPileD = GameObject.FindWithTag("DrawPileD");
			discardPileD = GameObject.FindWithTag("DiscardPileD");

			cardEventHandler = new UnityAction<EventParam>(cardEventHandlerFunction);
			EventManager.StartListening("cardEvent", cardEventHandler);

			InitPool();

			GetStarterDeck();

			// Deal Cards
			// EventParam eventParam = new EventParam();
			// eventParam.name = "dealCards";
			// EventManager.TriggerEvent("cardEvent", eventParam);

		}

		public void InitPool()
		{
			GameObject card;
			for (int i = 0; i < poolTotal; i++)
			{
				card = Instantiate(GameManager.instance.assetBundleCards.LoadAsset<GameObject>("CardPanel Variant")) as GameObject;
				if (i == 0)
				{
					originalPosition = card.transform.position;
					originalRotation = card.transform.rotation;
					originalScale = card.transform.localScale;
				}
				card.SetActive(false);
				pool.Add(card);
			}
		}
		public GameObject GetNewCard()
		{
			foreach (GameObject card in pool)
			{
				if (card.activeInHierarchy == false)
				{
					card.SetActive(true);
					card.transform.position = originalPosition;
					card.transform.rotation = originalRotation;
					card.transform.localScale = originalScale;
					return card;
				}
			}

			return null;
		}
		void cardEventHandlerFunction(EventParam eventParam)
		{
			Debug.Log("<color=red>== CardController.cardEventHandlerFunction ==</color>");
			Debug.Log("== " + eventParam.name);

			// if (stackType != eventParam.owner) return;

			switch (eventParam.name)
			{
				// case "mouseEnter":
				// 	if (eventParam.card.model.state == CardVO.CardState.InHand)
				// 		FanStack(eventParam.card.cardIndex);
				// 	break;
				// case "mouseExit":
				// 	if (eventParam.card.model.state == CardVO.CardState.InHand)
				// 		ResetStack();
				// 	break;
				case "updatePileCounts":
					UpdatePileCounts();
				break;

				case "dealerSelected":
					CardView cardView;
					CardStackController stack = null;
					CardVO.CardOwner owner = CardVO.CardOwner.Attacker;

					foreach (GameObject card in dealerCards)
					{
						cardView = card.GetComponent<CardView>();
						if (cardView.model.state != CardVO.CardState.InHand)
							card.transform.SetParent(null);//SetParent(attackStack.transform);
						else
						{
							if (cardView.model.owner == CardVO.CardOwner.Attacker)
							{
								stack = attackStack.GetComponent<CardStackController>();
							}
							else
							{
								owner = CardVO.CardOwner.Defender;
								stack = defendStack.GetComponent<CardStackController>();
							}
							cardView.isLatest = true;
							stack.NewCardAdded(cardView);
						}
						// else card.transform.SetParent(defendStack.transform);
					}
					// add card store to hand
					inHand.Insert(0, new CardStoreVO(eventParam.card.model.uid, eventParam.card.model.level, true));
					// deal hand
					// DrawPileToHand(stack, owner);
				break;

				case "stowedCardActivated":
					discardPile.Insert(0, new CardStoreVO(eventParam.card.model.uid, eventParam.card.model.level, false));
					// UpdatePileCounts();
				break;
			}
		}

		private void InHandToDiscardPile()
		{
			Debug.Log("<color=blue>### " + drawPile.Count + " / " + inHand.Count + " / " + discardPile.Count + "</color>");

			CardStackController stack = attackStack.GetComponent<CardStackController>();

			int inHandTotal = stack.GetCardsInHand().Count;
			// add inHand to dicard pile
			foreach(CardView card in stack.GetCardsInHand())
			{
				discardPile.Insert(0, new CardStoreVO(card.model.uid, card.model.level));
				StartCoroutine(card.Discard());
			}
			// remove from inHand
			inHand.RemoveRange(0, inHandTotal);
			Debug.Log("<color=blue>### " + drawPile.Count + " / " + inHand.Count + " / " + discardPile.Count + "</color>");
		}

		private void DrawPileToHand(CardStackController toStack, CardVO.CardOwner owner)
		{
			Debug.Log("<color=orange>== CardController.DrawPileToHand ==</color>");
			Debug.Log("* " + toStack + " / " + owner);
			// if not enough cards in draw pile...
			if (drawPile.Count < 5)
			{
				// cull *all* cards from discard
				foreach(CardStoreVO discard in discardPile)
				{
					drawPile.Add(discard);
				}
				// remove all from discard
				discardPile.Clear();
			}
			UpdatePileCounts();
			
			// take top-most 4 (or more) cards (not including dealer card which is already added)
			List<CardStoreVO> draw = drawPile.GetRange(0, 4);
			inHand.AddRange(draw);
			drawPile.RemoveRange(0, 4);
			List<CardVO> displayCards = new List<CardVO>();
			CardVO card = null;
			foreach(CardStoreVO store in inHand)
			{
				Debug.Log(store.uid + " / " + store.level.ToString());
				// don't include dealer cards (already in stack)
				if (store.isDealer == false)
				{
					card = CardLibraryVO.GetCardByStore(store);
					card.owner = owner;
					displayCards.Add(card);
				}
			}
			Debug.Log("done!");
			List<CardView> cardViews = new List<CardView>();
			// CardView cardView2;
			foreach(CardVO vo in displayCards)
			{
				// CardView cardView2 = CardVOToView(vo, owner);
				cardViews.Add(CardVOToView(vo, owner));
			}
			// toStack.NewCardAdded(cardView);
			Vector3 drawPilePos;// = drawPileA.transform.position;
			// GameObject prefab;
			// Vector3 container;
			// Vector3 screenPos = new Vector3();
			foreach (CardView view in cardViews)
			{
				Debug.Log("# dealer card " + view.transform.position.x);
				// cardView = go.GetComponent<CardView>();
				view.model.state = CardVO.CardState.InHand;
				if (view.model.owner == CardVO.CardOwner.Attacker)
				{
					view.gameObject.transform.SetParent(attackStack.transform);
					drawPilePos = drawPileA.transform.GetChild(0).transform.position;
					// drawPilePos.x -= (view.rt.rect.width / 2);
					drawPilePos.y += 25;//(view.rt.rect.height / 2);
					view.gameObject.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
					// Vector3 temp = view.gameObject.transform.rotation.eulerAngles;
					// temp.x = -45.0f;
					// view.gameObject.transform.rotation = Quaternion.Euler(temp);
				}
				else
				{ 
					view.gameObject.transform.SetParent(defendStack.transform);
					drawPilePos = drawPileD.transform.position;
				}
				view.gameObject.transform.position = drawPilePos;
				view.gameObject.SetActive(true);
				view.model.state = CardVO.CardState.DrawPile;
				toStack.MoveCardToStack(view);
				// toStack.NewCardAdded(view);
			}
			UpdatePileCounts();
		}

		private CardView CardVOToView(CardVO vo, CardVO.CardOwner owner)
		{
			GameObject go = GetNewCard();
			vo.owner = owner;
			CardView view = go.GetComponent<CardView>();
			view.model = vo;

			return view;
		}

		public void GetStarterDeck()
		{
			CardVO card;
			CardStoreVO store;
			for(var i = 0; i < 9; i++)
			{
				store = null;
				if (i < 4) {
					card = CardLibraryVO.getChargedUpCard();
					store = new CardStoreVO(card.uid, 1);
				}
				else if (i < 8)
				{
					card = CardLibraryVO.getEvasiveManeuversCard();
					store = new CardStoreVO(card.uid, 1);
				}
				else
				{
					// random
					card = CardLibraryVO.getExposedCard();
					store = new CardStoreVO(card.uid, 1);
				}

				if (store != null)
				{
					deck.Add(store);
					drawPile.Add(store);
				}

			}

			Debug.Log("deck len " + deck.Count);
			UpdatePileCounts();
			ShufflePile(drawPile);
			DrawPileToHand(attackStack.GetComponent<CardStackController>(), CardVO.CardOwner.Attacker);
		}

		private void ShufflePile(List<CardStoreVO> pile)
		{
			pile = ListHelper.ShuffleCardStoreList(pile);
			Debug.Log("* shuffled");
		}

		private void UpdatePileCounts()
		{
			drawPileA.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = drawPile.Count.ToString();
			discardPileA.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = discardPile.Count.ToString();
		}
		public IEnumerator DealCards(bool firstRun = false)
		{
			Debug.Log("<color=yellow>== CardController.dealCards ==</color>");
			// if (turnCounter == null)
			// 	turnCounter = GameObject.FindWithTag("TurnCounter");

			// if (firstRun == true)
			// {
			// 	yield return new WaitForSeconds(2f);
			// 	DrawPileToHand(attackStack.GetComponent<CardStackController>(), CardVO.CardOwner.Attacker);
			// 	yield return new WaitForSeconds(3f);
			// }
			// yield return new WaitForSeconds(1.5f);
			// hide turn counter UI
			// turnCounter.SetActive(false);

			// activate dealerCards
			// cardsUI.SetActive(true);
			dealerCards = new List<GameObject>();
			GameObject card1 = GetNewCard();
			GameObject card2 = GetNewCard();
			GameObject card3 = GetNewCard();
			// Instantiate(GameManager.instance.assetBundleCards.LoadAsset<GameObject>("CardPanel Variant")) as GameObject;
			// GameObject card2 = Instantiate(GameManager.instance.assetBundleCards.LoadAsset<GameObject>("CardPanel Variant")) as GameObject;
			// GameObject card3 = Instantiate(GameManager.instance.assetBundleCards.LoadAsset<GameObject>("CardPanel Variant")) as GameObject;
			dealerCards.Add(card1);
			dealerCards.Add(card2);
			dealerCards.Add(card3);

			// card data and owner
			CardVO card1data = CardLibraryVO.getChargedUpCard();
			card1data.owner = CardVO.CardOwner.Attacker;
			CardVO card2data = CardLibraryVO.getEvasiveManeuversCard();
			card2data.owner = CardVO.CardOwner.Attacker;
			CardVO card3data = CardLibraryVO.getExposedCard();
			card3data.owner = CardVO.CardOwner.Attacker;

			// card states
			CardView c = card1.GetComponent<CardView>();
			Debug.Log("* c " + c);
			card1.GetComponent<CardView>().model = card1data;
			card2.GetComponent<CardView>().model = card2data;
			card3.GetComponent<CardView>().model = card3data;
			// card1.GetComponent<CardView>().model.owner = CardVO.CardOwner.Attacker;
			// card2.GetComponent<CardView>().model.owner = CardVO.CardOwner.Attacker;
			// card3.GetComponent<CardView>().model.owner = CardVO.CardOwner.Attacker;

			// test defender stack
			/*GameObject card4 = GetNewCard();// Instantiate(GameManager.instance.assetBundleCards.LoadAsset<GameObject>("CardPanel Variant")) as GameObject;
			CardVO card4data = CardLibraryVO.getChargedUpCard();
			card4data.owner = CardVO.CardOwner.Defender;
			// card4.GetComponent<CardView>().model.state = CardVO.CardState.Dealer;
			card4.GetComponent<CardView>().model = card4data;
			dealerCards.Add(card4);*/

			// CardSODisplay cod = card1.GetComponent<CardSODisplay>();
			// cod.card = GameManager.instance.assetBundleCards.LoadAsset<GameObject>("test1").GetComponent<CardSO>();
			// CardSO zcard = Object.Instantiate(GameManager.instance.assetBundleCards.LoadAsset<GameObject>("test1"));
			// ScriptableObject card2 = ScriptableObject.CreateInstance<CardSO>();
			// ScriptableObject card3 = ScriptableObject.CreateInstance<CardSO>();
			// Transform stackTransform;
			CardView cardView;
			foreach (GameObject card in dealerCards)
			{
				Debug.Log("* dealer card " + attackStack);
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
			// Vector3 pointB = new Vector3(2 + 0.75f, 3f, 0f);
			Vector3 x = defendStack.transform.position;
			Vector3 cam = Camera.main.transform.position;

			Vector3 middle = screenCenter;// Vector3(Screen.width / 2, Screen.height / 2, 0);//cam.z);
			middle.y += (rt.rect.height / 2) + (card1.GetComponent<CardView>().rt.rect.height / 3);
			Vector3 left = screenCenter;
			left.x -= rt.rect.width + 10;
			left.y += (rt.rect.height / 2) + (card1.GetComponent<CardView>().rt.rect.height / 3);
			Vector3 right = screenCenter;
			right.x += rt.rect.width + 10;
			right.y += (rt.rect.height / 2) + (card1.GetComponent<CardView>().rt.rect.height / 3);

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

			// deal new hands to stacks
			// attackStack.GetComponent<CardStackController>().DealHand(inHand);
			// if (firstRun == false)
			// 	RoundComplete();
				// DrawPileToHand(attackStack.GetComponent<CardStackController>(), CardVO.CardOwner.Attacker);
			// DrawPileToHand(defendStack.GetComponent<CardStackController>(), CardVO.CardOwner.Defender);

			yield return null;
		}

		public IEnumerator RoundComplete()
		{
			Debug.Log("<color=lightblue>CardController.RoundComplete</color>");
			// move inhand to discard pile
			InHandToDiscardPile();

			yield return new WaitForSeconds(2.0f);
			// if draw pile empty, replenish
			// move 5 (+ additional) from draw pile to inhand
			DrawPileToHand(attackStack.GetComponent<CardStackController>(), CardVO.CardOwner.Attacker);
			yield return null;
		}

		public IEnumerator CardActionOnPlayer(CardView card, CharacterView character)
		{
			Debug.Log("<color=lightblue>CardController.CardActionOnPlayer</color>");

			// TODO fix this! state update logic handled by action
			card.model.state = CardVO.CardState.DiscardPile;

			// get positions/sizes
			Vector3 characterPos = Camera.main.WorldToScreenPoint(character.prefab.transform.position);
			Vector3 characterSize = Camera.main.WorldToScreenPoint(character.prefab.GetComponent<BoxCollider>().bounds.size);
			RectTransform cardRect = (RectTransform)card.transform;

			// move card over character        
			Vector3 toPos = characterPos;
			toPos.y += (cardRect.rect.height / 2) + (characterSize.y / 2);
			card.MoveObject(toPos, 0f, 0.35f);

			yield return new WaitForSeconds(0.75f);
			StartCoroutine(CardAction2(card, character));
			yield return new WaitForSeconds(0.75f);

			// update character (apply card action by level)
			character.model.AssignedCard(card.model, card.model.levelData[card.model.level - 1]);

			// discard (or exhaust?)
			StartCoroutine(card.Discard());
		}

		private IEnumerator CardAction2(CardView card, CharacterView character = null)
		{
			Debug.Log("<color=yellow>== CardController.CardAction2 ==</color>");

			// get level data
			CardLevelDataVO leveldata = card.model.levelData[card.model.level - 1];
			// get action type 2
			switch (leveldata.actionType2)
			{
				/*
					ADD RANDOM
				*/
				case CardLevelDataVO.CardActionType.AddRandom:
				
					CardStackController stack = attackStack.GetComponent<CardStackController>();
					if (card.model.owner == CardVO.CardOwner.Defender)
						stack = defendStack.GetComponent<CardStackController>();
					// get number of draw cards
					int toDraw = leveldata.value2;
					Debug.Log("* drawing " + toDraw + " cards...");
					// slide card(s) out from behind card
					// GameObject newcard;
					// CardView newcardview;
					// CardVO carddata;
					List<CardView> holding = new List<CardView>();
					for (var i = 0; i < toDraw; i++)
					{
						// get (random?) card
						CardVO carddata = CardLibraryVO.getExposedCard();
						// set owner
						carddata.owner = CardVO.CardOwner.Attacker;
						// get new card from pool
						GameObject newcard = GetNewCard();
						// add to stack
						newcard.transform.SetParent(stack.transform);
						CardView newcardview = newcard.GetComponent<CardView>();
						holding.Add(newcardview);
						// assign data to card
						newcardview.model = carddata;//CardLibraryVO.getExposedCard();
						// move it
						Vector3 tran = new Vector3(card.transform.position.x, card.transform.position.y, card.transform.position.x);
						newcardview.transform.position = tran;//new Vector3(tran.x + card.rt.rect.width + 10, tran.y, tran.z);
						newcardview.MoveObject(new Vector3(tran.x + (card.rt.rect.width * (i + 1) + 10), tran.y, tran.z), 0, 0.25f);
						// yield return new WaitForSeconds(1f);
					}

					yield return new WaitForSeconds((float)toDraw);

					foreach(CardView cview in holding)
					{
						// add to inHand
						cview.model.state = CardVO.CardState.InHand;
						stack.NewCardAdded(cview);
						// pause
						yield return new WaitForSeconds(1f);
					}

				break;
			}

			yield return null;
		}

		IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
		{
			Debug.Log("== MoveObject ==");

			// AnimationCurve curve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
			thisTransform.DOMove(endPos, time)
				.ChangeStartValue(startPos)
				.SetEase(moveCurve);
			yield return null;

		}

	}
}
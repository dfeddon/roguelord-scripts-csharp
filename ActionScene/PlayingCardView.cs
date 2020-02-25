using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayingCardView: MonoBehaviour//, IPointerEnterHandler
{
    public CardVO model;
    public bool cardFlipped = false;
    private bool cardFlipping = false;
	public bool isStowed = false;
    public bool isLatest = true;
    public bool isFocused = false;
    public bool isActive = false;
    private float rotateSpeed = 5f;
    private bool isMouseOver = false;
    private Vector3 screenPoint;
    private GameObject cardStock;
    private GameObject cardUI;
    private Button selectButton;
    public int cardIndex = -1;
	public float rotVal = 0;
    public float xVal = 0;
    public float yVal = 0;
    public GameObject bottomView;
	private AssetBundle cardBundle;

	// GameObject getTarget;
	// bool isMouseDragging;
	// Vector3 offsetValue;
	// Vector3 positionOfScreen;


	void Awake()
    {
		// this.onClick.AddListener((UnityEngine.Events.UnityAction)this.OnClick);
		screenPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        Debug.Log("* screenPoint " + screenPoint.x + " / " + screenPoint.y + " / " + screenPoint.z);

		cardStock = this.gameObject.transform.GetChild(0).gameObject;
        cardUI = this.gameObject.transform.GetChild(1).gameObject;
		selectButton = cardUI.transform.GetChild(0).GetComponent<Button>();
        selectButton.onClick.AddListener(SelectButtonHandler);
		cardUI.SetActive(false);
	}

    void Start()
    {
        Debug.Log("== PlayingCardView.Start ==");
        cardFlipped = false;

		// cardBundle = GameManager.instance.LoadAssetsBundle("cards");
		// StartCoroutine("flip");
    }

    void Update()
    {
		// //Mouse Button Press Down
		// if (Input.GetMouseButtonDown(0) && cardFlipped == true)
		// {
		// 	RaycastHit hitInfo;
		// 	getTarget = ReturnClickedObject(out hitInfo);
		// 	if (getTarget != null)
		// 	{
		// 		isMouseDragging = true;
		// 		//Converting world position to screen position.
		// 		positionOfScreen = Camera.main.WorldToScreenPoint(getTarget.transform.position);
		// 		offsetValue = getTarget.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z));
		// 	}
		// }

		// //Mouse Button Up
		// if (Input.GetMouseButtonUp(0))
		// {
		// 	isMouseDragging = false;
		// }

		// //Is mouse Moving
		// if (isMouseDragging)
		// {
		// 	//tracking mouse position.
		// 	Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z);

		// 	//converting screen position to world position with offset changes.
		// 	Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offsetValue;

		// 	//It will update target gameobject's current postion.
		// 	getTarget.transform.position = currentPosition;
		// }


	}

    void OnMouseDown()
    {
        Debug.Log("* down");
        if (isStowed)
        {
            isStowed = false;
            isActive = true;
            // reset index
            cardIndex = -1;
			// validate that card is stowed
			EventParam eventParam = new EventParam();
			// eventParam.data = "data";
			eventParam.name = "stowedCardActivated";
			eventParam.value = this.cardIndex;
            // eventParam.card = this;
			EventManager.TriggerEvent("cardEvent", eventParam);
		}
        else if (isActive == true)
        {
            Debug.Log("active button pushed!");
			EventParam eventParam = new EventParam();
			// eventParam.data = "data";
			eventParam.name = "activeCardStowed";
			eventParam.value = this.cardIndex;
			// eventParam.card = this;
			EventManager.TriggerEvent("cardEvent", eventParam);
		}
        else if (!cardFlipped && !cardFlipping)
        {
            // cardFlipped = true;
            Debug.Log("flipping...");
            cardFlipping = true;
		    StartCoroutine("flip");
        }
        else if (!cardFlipped)
        {
            Debug.Log("spinning...");
        }
        else
        {
            Debug.Log("dragging");
			SelectButtonHandler();
		}
    }

    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     Debug.Log("derek");
    // }
    void OnMouseEnter()
    {
        Debug.Log("enter! " + cardIndex);
        if (cardFlipped == true && isStowed == false)
        {
            Debug.Log("setactive! ");// + cardUI.GetChild);
            cardUI.SetActive(true);
        }
        else if (isStowed == true && isFocused == false)
        {
            Debug.Log("isstowed!");
            isFocused = true;
            transform.position = new Vector3(transform.position.x + 0.10f, transform.position.y + 0.35f, transform.position.z - 0.15f);

            // notify controller
			EventParam eventParam = new EventParam();
			eventParam.data = this;
			eventParam.name = "mouseEnter";
			EventManager.TriggerEvent("stowedEvent", eventParam);
		}
	}

    void OnMouseExit()
    {
        Debug.Log("exit!");
        if (isStowed == false && cardFlipped == true)
        {
		    cardUI.SetActive(false);
        }
        if (isStowed == true && isFocused == true)
        {
			isFocused = false;
			transform.position = new Vector3(transform.position.x - 0.10f, transform.position.y - 0.35f, transform.position.z + 0.15f);

            // notify controller
			EventParam eventParam = new EventParam();
			eventParam.data = this;
			eventParam.name = "mouseExit";
			EventManager.TriggerEvent("stowedEvent", eventParam);
		}
        isMouseOver = false;
	}
    void OnMouseOver()
    {
        if (isMouseOver != true)
            isMouseOver = true;
    }

    public void SetStowed(bool b)
    {
        isStowed = b;
        Debug.Log("Bottom " + bottomView);

        if (b == true)
            bottomView.SetActive(false);
    }
    public void SetCardView(AssetBundle asset, int type, int cardNumber)
    {
        this.cardIndex = cardNumber;
		Debug.Log("card num index " + cardNumber + " / " + this.cardIndex);

		MeshRenderer mesh = cardStock.GetComponent<MeshRenderer>();
		Material[] mat = mesh.materials;
        switch(type)
        {
            case 0:
                mat[0] = asset.LoadAsset<Material>("corrupt");
                break;

            case 1: 
                mat[0] = asset.LoadAsset<Material>("double_damage");
                break;

			case 2:
				mat[0] = asset.LoadAsset<Material>("crits");
				break;
		}
        mesh.materials = mat;
    }

    void SelectButtonHandler()
    {
        Debug.Log("* selection made! from card #" + this.cardIndex);
		EventParam eventParam = new EventParam();
		// eventParam.type
		eventParam.name = "dealerCardSelected";
        eventParam.value = this.cardIndex;
		EventManager.TriggerEvent("cardEvent", eventParam);

	}

	//Method to Return Clicked Object
	// GameObject ReturnClickedObject(out RaycastHit hit)
	// {
	// 	GameObject target = null;
	// 	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	// 	if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
	// 	{
	// 		target = hit.collider.gameObject;
	// 	}
	// 	return target;
	// }

    IEnumerator flip()//Transform startRotation, Transform endRotation, float timeCount)
    {
		float lastZ = 0f;
		while (cardFlipping)
        {
            cardStock.transform.rotation = Quaternion.Slerp(cardStock.transform.rotation, Quaternion.Euler(90, 0, 180), Time.deltaTime * rotateSpeed);
            // Debug.Log(cardStock.transform.rotation.z + " / " + lastZ.ToString());
            if (cardStock.transform.rotation.z == lastZ)
            {
                // Debug.Log("* card flipped!");
                cardFlipped = true;
                cardFlipping = false;

                if (isMouseOver)
                    cardUI.SetActive(true);
            }
            lastZ = cardStock.transform.rotation.z;
			yield return null;
        }
    }
}

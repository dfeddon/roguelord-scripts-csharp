using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Roguelord {
public class CameraController : MonoBehaviour
{
    // public GameObject player;
    private Vector3 offset;
    private bool moveCamera = false;
	public float panSpeed = 20f;
	public float panBorderThickness = 10f;
	public Vector2 panLimit;
	public float scrollSpeed = 20f;
	public float minY = 20f;
	public float maxY = 120f;
	private Vector3 moveToGrid;
	private float dist;
	public float dragSpeed = 5f;
	private Vector3 dragOrigin;	// private Vector3 MouseStart, MouseMove;
	// private Vector3 derp;
    // Start is called before the first frame update
    void Start()
    {
        // offset = transform.position - player.transform.position;
		dist = transform.position.z;

		// InvokeRepeating("doMove", 0.0f, 3.0f);
    }
    public void doMove(Vector3 grid)
    {
		Debug.Log("== doMove ==" + grid);
		// hide sphere beneath map
		grid.y = -6f;
		moveToGrid = grid;
        moveCamera = true;
    }
    // Update is called once per frame
    void Update()
    {
		// Vector3 pos = transform.position;
		// player.transform.position = new Vector3(player.transform.position.x + 0.1f, player.transform.position.y, player.transform.position.z);
		// Debug.Log(Time.realtimeSinceStartup);
		// if (Input.GetMouseButtonDown(1))
		// {
		// 	Debug.Log("* mousebuttondown");
		// 	MouseStart = new Vector3(Input.mousePosition.x, Input.mousePosition.z, dist);
		// }
		// else if (Input.GetMouseButton(1))
		// {
		// 	Debug.Log("* mousebutton 1");
		// 	MouseMove = new Vector3(Input.mousePosition.x - MouseStart.x, Input.mousePosition.z - MouseStart.z, dist);
		// 	// MouseStart = new Vector3(Input.mousePosition.x, Input.mousePosition.z, dist);
		// 	transform.position = new Vector3(transform.position.x + MouseMove.x * Time.deltaTime, transform.position.z + MouseMove.z * Time.deltaTime, dist);
		// }
		// if (moveCamera == true)
		// {
		// 	moveCamera = false;
		// 	StartCoroutine("MoveCamera");
		// }
		/*
		if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
		{
			pos.z += panSpeed * Time.deltaTime;
		}
		if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
		{
			pos.z -= panSpeed * Time.deltaTime;
		}
		if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
		{
			pos.x -= panSpeed * Time.deltaTime;
		}
		if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
		{
			pos.x += panSpeed * Time.deltaTime;
		}
		*/
		if (Input.GetAxis("Mouse ScrollWheel") != 0 && !EventSystem.current.IsPointerOverGameObject()) {
			Vector3 pos = transform.position;
			Debug.Log("y " + pos.y);

			float scroll = Input.GetAxis("Mouse ScrollWheel");
			pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

			if (pos.y < 15) pos.y = 15;
			else if (pos.y > 200) pos.y = 200;

			transform.position = pos;
		}

		// pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
		// pos.y = Mathf.Clamp(pos.y, minY, maxY);
		// pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

		// transform.position = pos;

	}

    // LateUpdate is called once per frame (after all items in Update have been processed)
    void LateUpdate()
    {
		// if pointer on UI get out!
		// if (IsPointerOverUIObject()) return;
		if (EventSystem.current.IsPointerOverGameObject()) return;

		// if (Input.GetMouseButtonDown(0))
		// transform.position = player.transform.position + offset;
		if (Input.GetMouseButtonDown(0))
		{
			dragOrigin = Input.mousePosition;
			return;
		}

		if (!Input.GetMouseButton(0)) return;

		Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
		Vector3 move = new Vector3(-pos.x * dragSpeed, 0, -pos.y * dragSpeed);

		transform.Translate(move, Space.World);
	}

	/*IEnumerator MoveCamera()
    {
		// var p = player.transform.position;
		// Debug.Log("MoveCamera " + player.transform.position.x + "/" + moveToGrid.x);
		///////////////////////////////////
		// move
		///////////////////////////////////
		// float seconds = 0.5f;
		// yield return StartCoroutine(MoveObject(player.transform, player.transform.position, moveToGrid, seconds));
	
        // var targ = new Vector3(player.transform.position.x + 500.0f, player.transform.position.y, player.transform.position.z);
		// player.transform.position = Vector3.Lerp(player.transform.position, targ, Time.deltaTime);
		// pos += lerpSpeed;
	}*/
	IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
	{
		Debug.Log("== MoveObject ==");

		var i = 0.0f;
		var rate = 1.0f / time;
		while (i < 1.0f)
		{
			i += Time.deltaTime * rate;
			thisTransform.position = Vector3.Slerp(startPos, endPos, i);
			yield return null;
		}
	}

	// private bool IsPointerOverUIObject()
	// {
	// 	PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
	// 	eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
	// 	List<RaycastResult> results = new List<RaycastResult>();
	// 	EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
	// 	return results.Count > 0;
	// }
}
}
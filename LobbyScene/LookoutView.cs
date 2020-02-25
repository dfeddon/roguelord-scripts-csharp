using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// namespace PINS {
public class LookoutView : MonoBehaviour
{
	private float rotateSpeed = 0.5f;
	private bool waiting = false;
	private GameObject cube;
	public Vector3 nexus;
	public Vector3 pos;
	public Color c1 = Color.yellow;
	public Color c2 = Color.red;
	public int lengthOfLineRenderer = 2;

	// public void Construct(Vector3 pos)
	// {
	// 	Debug.Log("LookoutView.constructor ");
	// 	this.pos = pos;
	// }

	void Start()
	{
		Debug.Log("* lookout start");
		// transform.Rotate(transform.up, Random.Range(0f, 360f));
		// StartCoroutine(Spin(KeyCode.Space));
		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
		lineRenderer.widthMultiplier = 0.15f;
		// lineRenderer.positionCount = lengthOfLineRenderer;

		// A simple 2 color gradient with a fixed alpha of 1.0f.
		float alpha = 0.25f;
		Gradient gradient = new Gradient();
		gradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(0.5f, 1.0f) }
		);
		lineRenderer.colorGradient = gradient;
	}

	// Update is called once per frame
	void Update()
	{
		if(pos != null)
		{
			// Debug.Log("pos " + pos);
			LineRenderer lineRenderer = GetComponent<LineRenderer>();
			// var points = new Vector3[lengthOfLineRenderer];
			// var t = Time.time;
			// points[0] = new Vector3(pos.x, pos.y, pos.z);
			// points[1] = new Vector3(pos.x, 10, pos.z);
			// points[2] = new Vector3(pos.x, 20, pos.z);
			// for (int i = 0; i < lengthOfLineRenderer; i++)
			// {
			// 	// lineRenderer.SetPosition(i, new Vector3(i * 0.5f, Mathf.Sin(i + t), 0.0f));
			// 	points[i] = new Vector3(i * 0.5f, Mathf.Sin(i + t), 20.0f);
			// }
			// lineRenderer.SetPositions(points);
			// Debug.Log(points);
			// Spin the object around the world origin at 20 degrees/second.
			// transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);
			// if (nexus != null)
			// {
			// 	// Debug.Log("* rotate...");
			// 	transform.RotateAround(nexus, Vector3.up, 50 * Time.deltaTime);
			// }
			lineRenderer.SetPosition(0, new Vector3(pos.x, pos.y, pos.z));
			lineRenderer.SetPosition(1, new Vector3(pos.x, 20, pos.z));		}
	}

	public void doSpin()
	{
		cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.transform.localScale = new Vector3(5, 5, 0.15f);
		cube.transform.position = new Vector3(pos.x, 20, pos.z);

		waiting = true;
		StartCoroutine(Spin(KeyCode.Space));
	}

	private IEnumerator Spin(KeyCode button)
	{
		bool waiting = true;
		while (waiting)
		{
			//Spin the object around the world origin at 20 degrees/second.
			cube.transform.Rotate(transform.up, 360 * rotateSpeed * Time.deltaTime);
			if (Input.GetKeyDown(button))
			{
				waiting = false;
			}
			yield return new WaitForFixedUpdate();
		}
	}
}
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneObject : MonoBehaviour
{
	private float rotateSpeed = 1.0f;
	private bool waiting = false;
	public Vector3 nexus;
	// Start is called before the first frame update
	void Start()
	{
		Debug.Log("* drone start");
		// transform.Rotate(transform.up, Random.Range(0f, 360f));
		// StartCoroutine(Spin(KeyCode.Space));
	}

	public void doSpin()
	{
		waiting = true;
		// StartCoroutine(Spin(KeyCode.Space));
	}

	public void setNexus(Vector3 nex) {
		nex.x += 2;
		nexus = nex;
	}

	// Update is called once per frame
	void Update()
	{
		// Spin the object around the world origin at 20 degrees/second.
		// transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);
		if (nexus != null) {
			// Debug.Log("* rotate...");
			transform.RotateAround(nexus, Vector3.up, 50 * Time.deltaTime);
		}
	}

	private IEnumerator Spin(KeyCode button)
	{
		bool waiting = true;
		while (waiting)
		{
			//Spin the object around the world origin at 20 degrees/second.
			transform.Rotate(transform.up, 360 * rotateSpeed * Time.deltaTime);
			if (Input.GetKeyDown(button))
			{
				waiting = false;
			}
			yield return new WaitForFixedUpdate();
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowObject : MonoBehaviour
{
    private float rotateSpeed = 1.0f;
    private bool waiting = false;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("* arrow start");
        // transform.Rotate(transform.up, Random.Range(0f, 360f));
        // StartCoroutine(Spin(KeyCode.Space));
    }

    public void doSpin()
    {
        waiting = true;
        StartCoroutine(Spin(KeyCode.Space));
    }

    // Update is called once per frame
    void Update()
    {
	}

    private IEnumerator Spin(KeyCode button)
    {
        bool waiting = true;
        while(waiting)
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

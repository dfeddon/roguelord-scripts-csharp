using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
	Rigidbody m_Rigidbody;
	float m_Speed;
    // Start is called before the first frame update
    void Start()
    {
		//Fetch the Rigidbody component you attach from your GameObject
		m_Rigidbody = GetComponent<Rigidbody>();
		//Set the speed of the GameObject
		m_Speed = 10.0f;        
    }

    // Update is called once per frame
    void Update()
    {
		m_Rigidbody.velocity = ( transform.forward * -1) * m_Speed;   
    }
}

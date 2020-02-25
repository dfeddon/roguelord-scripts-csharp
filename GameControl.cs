using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Singleton Pattern
public class GameControl : MonoBehaviour {

	public static GameControl control;

	// init
	void Awake () {
		if (control == null) {
			DontDestroyOnLoad(gameObject);
			control = this;
		} else if (control != this) {
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

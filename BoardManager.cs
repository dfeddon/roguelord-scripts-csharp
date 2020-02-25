using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	[Serializable]
	public class Count {
		public int minimum;
		public int maximum;

		public Count (int min, int max) {
			minimum = min;
			maximum = max;
		}
	}

	public int columns = 8;
	public int rows = 8;
	public Count wallCount = new Count (5, 9);
	public Count foodCount = new Count (1, 5);

	private Transform boardHolder;

	// Use this for initialization
	void Start () {
		Debug.Log("== BoardManager.Start ==");
	}
	
	// Update is called once per frame
	void Update () {	
	}
}

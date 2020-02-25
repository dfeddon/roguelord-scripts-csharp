using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// [System.Serializable]
public class EffectItemVO
{
	public float timeStart;
	public float timeEnd;
	public GameObject fx = null;
	public GameObject asset = null;
	public bool autoDelete = true;
	// public int numLoops = 0;
}

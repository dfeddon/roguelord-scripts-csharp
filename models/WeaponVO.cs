using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//In order to use a collection's Sort() method, this class needs to implement the IComparable interface.
[System.Serializable]
public class WeaponVO// : IComparable<CharacterVO>
{
	public GameObject prefab;
	public Transform endpoint;
}
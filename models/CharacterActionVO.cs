using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// [System.Serializable]
public class CharacterActionVO
{
	public int weapon = 0;
	public int weaponSlot = 0;
	public int gear = 0;
	public int gearslot = 0;
	public int source = 0;
	public int target = 0;
	public List<int> cards = new List<int>();

	public CharacterActionVO()
	{

	}

	public void Reset()
	{
	}

}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AbilityCooldownVO
{
	// public BSON.ObjectId id;
	// public string _id;
	public int slot;
	public int uid;
	public bool isCoolingDown = false;
	public int cooldownCounter = 1;

	public AbilityCooldownVO(AbilityVO vo)
	{
		if (vo != null)
		{
			uid = vo.uid;
			slot = vo.slot;
			cooldownCounter = vo.cooldownTotal + 1; // <- +1 because cooldown decrements after addition
		}
	}
}

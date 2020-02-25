using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public class CharacterResultsVO
{
	public int uid;
	public int healthModifier = 0; // 1 = damage, 2 = heal, 3 = other
	public int healthValue = 0;
	public int cardPlayed;
	public EffectVO effect;// = new EffectVO();
						   // public int effectModifier; // 0 = none, 1 = condition, 2 = cc, 3 = boon
	public int effectUid; // poison, burning, fear, etc.
	public int effectValue = 0;
	public int effectDuration;
	public bool effectResisted;
	public bool isCrit;

	public CharacterResultsVO()
	{
		effect = new EffectVO();
	}
}

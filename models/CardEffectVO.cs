using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class CardEffectVO
{
	// verb (add), noun (burn), target (discard pile)
	public enum Action { Draw, Discard, GainDefense, GainAttack, GainEnergy, ApplyHeal, ApplyPoison, ApplyBurn };
	public enum Target { Source, Target, Opponents, Colleagues, DrawPile, DiscardPile };
	public Action action;
	public Target target;
	public int value;
	public int duration;
	public bool isExhaustable = false;
	public int level;
	public int[] levelMultipliers;
}
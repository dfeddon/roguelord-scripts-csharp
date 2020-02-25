using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Core;


[System.Serializable]
public class ActionVO
{
	// verb (add), noun (burn), target (discard pile)
	public enum AbilityType { Draw, Discard, GainDefense, GainAttack, GainEnergy, ApplyHeal, ApplyPoison, ApplyBurn };
	public enum TargetType { Source, Target, Opponents, Colleagues, DrawPile, DiscardPile };

	public CharacterVO target;
	public CharacterVO source;
	public AbilityVO ability;
	public List<CardVO> cardsList = new List<CardVO>();

	public ActionVO() {}

	void Awake()
	{

	}

	private void Start() 
	{
		
	}

	public GSRequestData GetData()
	{
		GSRequestData gsData = new GSRequestData();
		gsData.AddString("source", "abc");
		gsData.AddString("target", "123");
		gsData.AddString("ability", "a1b2c3");

		return gsData;
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public class RoundCompleteResultsVO
{
	// public const int EFFECT_MODIFIER_CONDITION = 1;
	// public const int EFFECT_MODIFIER_CC = 2;
	// public const int EFFECT_MODIFIER_BOON = 3;

	public const int HEALTH_MODIFIER_DAMAGE = 1;
	public const int HEALTH_MODIFIER_HEAL = 2;

	public CharacterResultsVO source;
	public List<CharacterResultsVO> targets = new List<CharacterResultsVO>();

	public RoundCompleteResultsVO()
	{

	}

	public RoundCompleteResultsVO GetStubResults()
	{
		RoundCompleteResultsVO vo = new RoundCompleteResultsVO();

		return vo;
	}


}
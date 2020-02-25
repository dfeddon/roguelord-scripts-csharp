using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public class CombatResultsVO
{
	// public const int EFFECT_MODIFIER_CONDITION = 1;
	// public const int EFFECT_MODIFIER_CC = 2;
	// public const int EFFECT_MODIFIER_BOON = 3;

	public const int HEALTH_MODIFIER_DAMAGE = 1;
	public const int HEALTH_MODIFIER_HEAL = 2;
	public const int HEALTH_MODIFIER_OTHER = 3;
	public int abilityId;
	public AbilityVO ability;
	private AbilityLibraryVO lib;
	public CharacterResultsVO source;
	public List<CharacterResultsVO> targets = new List<CharacterResultsVO>();
	public CharacterNextTurnResultsVO next;

	public CombatResultsVO()
	{
		// lib = new AbilityMatrixVO();// AbilityLibraryVO();
		ability = new AbilityVO(); // <- avoid null checks
	}

	public CombatResultsVO GetStubResults(int sourceId, List<int> targetIds, int abilityId, int nextTurnCharacterId)
	{
		CombatResultsVO vo = new CombatResultsVO();

		////////////////////////////////////
		// ability
		////////////////////////////////////
		vo.abilityId = abilityId;

		// vo.ability = lib.GetAbilityById(abilityId);
		vo.ability = AbilityMatrixVO.GetAbilityById(abilityId);

		////////////////////////////////////
		// source
		////////////////////////////////////
		CharacterResultsVO s1 = new CharacterResultsVO();
		s1.uid = sourceId;
		
		s1.healthModifier = 0;
		s1.healthValue = 0;
		// s1.effectModifier = 0;
		s1.effect = vo.ability.effect;
		s1.effectValue = 0;
		s1.cardPlayed = 0;
		s1.isCrit = false;

		vo.source = s1;

		////////////////////////////////////
		// targets
		////////////////////////////////////
		CharacterResultsVO t;
		CharacterVO c;
		CharacterVO s = CharacterVO.getCharacterById(sourceId);
		foreach(int targetId in targetIds)
		{
			c = CharacterVO.getCharacterById(targetId);
			// Debug.Log("# isPrimaryTarget " + c.isPrimaryTarget);

			t = new CharacterResultsVO();
			t.uid = targetId;
			// healthModifier based on Ability
			t.healthModifier = vo.ability.healthModifier; //CombatResultsVO.HEALTH_MODIFIER_DAMAGE;
			if (t.healthModifier == HealthVO.HEALTH_MODIFIER_DAMAGE || t.healthModifier == HealthVO.HEALTH_MODIFIER_HEAL)
			{
				// t.healthValue = vo.ability.healthRoll.RollTheDice();// Random.Range(1, 20);
				int d = c.attributes.defense;
				int diff = d - s.attributes.attack;
				// if damaged
				if (diff < 0)
				{
					t.healthValue += Mathf.Abs(diff);
				}
				// reduce defense
				c.attributes.defense = d - s.attributes.attack;
				// reduce attack
				s.attributes.attack -= d;
			}
			t.effect = vo.ability.effect;// EffectVO.EFFECT_MODIFIER_CONDITION;
			if (t.effect.effectModifier == EffectVO.EFFECT_MODIFIER_CONDITION)
				t.effectValue = vo.ability.effectRoll.RollTheDice();// Random.Range(1, 20);
			t.cardPlayed = 0;
			// is crit!?
			if (t.healthValue > 15)
				t.isCrit = true;
			else t.isCrit = false;

			vo.targets.Add(t);
		}
		////////////////////////////////////
		// next turn character (effect/health mod)
		// TODO: generate this value also on RoundBegin
		////////////////////////////////////
		// if nextTurnCharacterId is 0, this is
		// last turn of round
		next = new CharacterNextTurnResultsVO();
		next.uid = nextTurnCharacterId;
		next.lastTurnOfRound = (nextTurnCharacterId == 0) ? true : false;
		next.effectValue = 5;
		vo.next = next;
		// next = nextTurnCharacterId;// CharacterVO.getCharacterById(nextTurnCharacterId);

		return vo;
	}


}
// public class CharacterResultsVO
// {
// 	public int uid;
// 	public int healthModifier = 0; // 1 = damage, 2 = heal, 3 = other
// 	public int healthValue = 0;
// 	public int cardPlayed;
// 	public EffectVO effect;// = new EffectVO();
// 	// public int effectModifier; // 0 = none, 1 = condition, 2 = cc, 3 = boon
// 	public int effectUid; // poison, burning, fear, etc.
// 	public int effectValue = 0;
// 	public int effectDuration;
// 	public bool effectResisted;
// 	public bool isCrit;

// 	public CharacterResultsVO()
// 	{
// 		effect = new EffectVO();
// 	}
// }

//*
// public class CharacterNextTurnResultsVO
// {
// 	public int uid;
// 	public EffectVO effect;
// 	public int effectValue;
// 	public bool lastTurnOfRound;
// }
//*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public static class EffectMatrixVO
{
	//////////////////////////////////////
	// Conditions
	//////////////////////////////////////
	public static EffectVO GetPoisonEffect(EffectVO vo)
	{
		vo.uid = 100;
		vo.name = "Poison";
		vo.label = "Poisoned";
		vo.image = EffectVO.EFFECT_IMAGE_POISON;
		vo.effectModifier = EffectVO.EFFECT_MODIFIER_CONDITION;
		vo.isStackable = true;
		vo.stackType = EffectVO.EFFECT_STACK_TYPE_DURATION;
		vo.stacksTotal = 3;
		vo.duration = 3;
		vo.intensity = 1;
		vo.isExhaustible = false;

		return vo;
	}
	public static EffectVO GetBleedEffect(EffectVO vo)
	{
		vo.uid = 101;
		vo.name = "Bleed";
		vo.label = "Bleeding";
		vo.image = EffectVO.EFFECT_IMAGE_DROP_OF_BLOOD;
		vo.effectModifier = EffectVO.EFFECT_MODIFIER_CONDITION;
		vo.isStackable = true;
		vo.stackType = EffectVO.EFFECT_STACK_TYPE_DURATION;
		vo.stacksTotal = 3;
		vo.duration = 3;
		vo.intensity = 1;
		vo.isExhaustible = false;

		return vo;
	}
	public static EffectVO GetBurnEffect(EffectVO vo)
	{
		vo.uid = 102;
		vo.name = "Burn";
		vo.label = "Burning";
		vo.image = EffectVO.EFFECT_IMAGE_FIRE;
		vo.effectModifier = EffectVO.EFFECT_MODIFIER_CONDITION;
		vo.isStackable = true;
		vo.stackType = EffectVO.EFFECT_STACK_TYPE_INTENSITY;
		vo.stacksTotal = 3;
		vo.duration = 3;
		vo.intensity = 3;
		vo.isExhaustible = false;

		return vo;
	}
	public static EffectVO GetConfusionEffect(EffectVO vo)
	{
		vo.uid = 103;
		vo.name = "Confusion";
		vo.label = "Confused";
		vo.image = EffectVO.EFFECT_IMAGE_CONFETTI;
		vo.effectModifier = EffectVO.EFFECT_MODIFIER_CONDITION;
		vo.isStackable = false;
		vo.stackType = EffectVO.EFFECT_STACK_TYPE_DURATION;
		vo.stacksTotal = 1;
		vo.duration = 1;
		vo.intensity = 1;
		vo.isExhaustible = false;

		return vo;
	}

	//////////////////////////////////////
	// Crowd Control
	//////////////////////////////////////
	public static EffectVO GetChillEffect(EffectVO vo)
	{
		vo.uid = 200;
		vo.name = "Chill";
		vo.label = "Chilled";
		vo.image = EffectVO.EFFECT_IMAGE_SNOWFLAKE;
		vo.effectModifier = EffectVO.EFFECT_MODIFIER_CC;
		vo.isStackable = false;
		vo.stackType = EffectVO.EFFECT_STACK_TYPE_DURATION;
		vo.stacksTotal = 1;
		vo.duration = 3;
		vo.intensity = 1;
		vo.isExhaustible = false;

		return vo;
	}
	public static EffectVO GetBlindEffect(EffectVO vo)
	{
		vo.uid = 201;
		vo.name = "Blind";
		vo.label = "Blinded";
		vo.image = EffectVO.EFFECT_IMAGE_BLIND;
		vo.effectModifier = EffectVO.EFFECT_MODIFIER_CC;
		vo.isStackable = false;
		vo.stackType = EffectVO.EFFECT_STACK_TYPE_DURATION;
		vo.stacksTotal = 1;
		vo.duration = 1;
		vo.intensity = 1;
		vo.isExhaustible = false;

		return vo;
	}
	public static EffectVO GetFearEffect(EffectVO vo)
	{
		vo.uid = 202;
		vo.name = "Fear";
		vo.label = "Fearful";
		vo.image = EffectVO.EFFECT_IMAGE_SCREAM;
		vo.effectModifier = EffectVO.EFFECT_MODIFIER_CC;
		vo.isStackable = false;
		vo.stackType = EffectVO.EFFECT_STACK_TYPE_DURATION;
		vo.stacksTotal = 1;
		vo.duration = 1;
		vo.intensity = 1;
		vo.isExhaustible = false;

		return vo;
	}
	public static EffectVO GetWeaknessEffect(EffectVO vo)
	{
		vo.uid = 200;
		vo.name = "Weakness";
		vo.label = "Weakened";
		vo.image = EffectVO.EFFECT_IMAGE_PULSE;
		vo.effectModifier = EffectVO.EFFECT_MODIFIER_CC;
		vo.isStackable = false;
		vo.stackType = EffectVO.EFFECT_STACK_TYPE_INTENSITY;
		vo.stacksTotal = 1;
		vo.duration = 3;
		vo.intensity = 3;
		vo.isExhaustible = false;

		return vo;
	}
	//////////////////////////////////////
	// Boons
	//////////////////////////////////////
	public static EffectVO GetAegisEffect(EffectVO vo)
	{
		vo.uid = 300;
		vo.name = "Aegis";
		vo.label = "Aegis";
		vo.image = EffectVO.EFFECT_IMAGE_SHIELD;
		vo.effectModifier = EffectVO.EFFECT_MODIFIER_BOON;
		vo.isStackable = false;
		vo.stackType = EffectVO.EFFECT_STACK_TYPE_DURATION;
		vo.stacksTotal = 1;
		vo.duration = 3;
		vo.intensity = 1;
		vo.isExhaustible = true;

		return vo;
	}
	public static EffectVO GetStabilityEffect(EffectVO vo)
	{
		vo.uid = 301;
		vo.name = "Stability";
		vo.label = "Stability";
		vo.image = EffectVO.EFFECT_IMAGE_LEAVING_GEO_FENCE_LOCATION;
		vo.effectModifier = EffectVO.EFFECT_MODIFIER_BOON;
		vo.isStackable = true;
		vo.stackType = EffectVO.EFFECT_STACK_TYPE_DURATION;
		vo.stacksTotal = 3;
		vo.duration = 3;
		vo.intensity = 1;
		vo.isExhaustible = false;

		return vo;
	}
	public static EffectVO GetMightEffect(EffectVO vo)
	{
		vo.uid = 302;
		vo.name = "Might";
		vo.label = "Might";
		vo.image = EffectVO.EFFECT_IMAGE_CLENCHED_FIST;
		vo.effectModifier = EffectVO.EFFECT_MODIFIER_BOON;
		vo.isStackable = true;
		vo.stackType = EffectVO.EFFECT_STACK_TYPE_INTENSITY;
		vo.stacksTotal = 1;
		vo.duration = 5;
		vo.intensity = 3;
		vo.isExhaustible = false;

		return vo;
	}
	public static EffectVO GetRegenerationEffect(EffectVO vo)
	{
		vo.uid = 303;
		vo.name = "Regeneration";
		vo.label = "Regen";
		vo.image = EffectVO.EFFECT_IMAGE_NATURAL_FOOD;
		vo.effectModifier = EffectVO.EFFECT_MODIFIER_BOON;
		vo.isStackable = true;
		vo.stackType = EffectVO.EFFECT_STACK_TYPE_INTENSITY;
		vo.stacksTotal = 3;
		vo.duration = 3;
		vo.intensity = 3;
		vo.isExhaustible = false;

		return vo;
	}
	public static EffectVO GetAlacrityEffect(EffectVO vo)
	{
		vo.uid = 304;
		vo.name = "Alacrity";
		vo.label = "Alacrity";
		vo.image = EffectVO.EFFECT_IMAGE_EXERCISE;
		vo.effectModifier = EffectVO.EFFECT_MODIFIER_BOON;
		vo.isStackable = true;
		vo.stackType = EffectVO.EFFECT_STACK_TYPE_INTENSITY;
		vo.stacksTotal = 3;
		vo.duration = 3;
		vo.intensity = 3;
		vo.isExhaustible = false;

		return vo;
	}
	public static EffectVO GetTurretEffect(EffectVO vo)
	{
		vo.uid = 305;
		vo.name = "Turret";
		vo.label = "Turret";
		vo.image = EffectVO.EFFECT_IMAGE_MORTAR;
		vo.effectModifier = EffectVO.EFFECT_MODIFIER_BOON;
		vo.isStackable = false;
		vo.stackType = EffectVO.EFFECT_STACK_TYPE_DURATION;
		vo.stacksTotal = 1;
		vo.duration = 3;
		vo.intensity = 3;
		vo.isExhaustible = false;

		return vo;
	}
	public static EffectVO GetTotemEffect(EffectVO vo)
	{
		vo.uid = 305;
		vo.name = "Totem";
		vo.label = "Totem";
		vo.image = EffectVO.EFFECT_IMAGE_BARBER_POLE;
		vo.effectModifier = EffectVO.EFFECT_MODIFIER_BOON;
		vo.isStackable = false;
		vo.stackType = EffectVO.EFFECT_STACK_TYPE_DURATION;
		vo.stacksTotal = 1;
		vo.duration = 3;
		vo.intensity = 3;
		vo.isExhaustible = false;

		return vo;
	}
}

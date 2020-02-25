using UnityEngine;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EffectVO
{
	public const int EFFECT_MODIFIER_NONE = 0;
	public const int EFFECT_MODIFIER_CONDITION = 1;
	public const int EFFECT_MODIFIER_CC = 2;
	public const int EFFECT_MODIFIER_BOON = 3;

	public const string EFFECT_IMAGE_ACCURACY = "icons8-accuracy-48";
	public const string EFFECT_IMAGE_ANGRY = "icons8-angry-48";
	public const string EFFECT_IMAGE_BIOHAZARD = "icons8-biohazard-48";
	public const string EFFECT_IMAGE_BLIND = "icons8-blind-48";
	public const string EFFECT_IMAGE_CLINIC = "icons8-clinic-48";
	public const string EFFECT_IMAGE_CONFUSED = "icons8-confused-48";
	public const string EFFECT_IMAGE_COOLING = "icons8-cooling-48";
	public const string EFFECT_IMAGE_DEFENSE = "icons8-defense-48";
	public const string EFFECT_IMAGE_DELETE_SHIELD = "icons8-delete-shield-48";
	public const string EFFECT_IMAGE_DROP_OF_BLOOD = "icons8-drop-of-blood-48";
	public const string EFFECT_IMAGE_EXERCISE = "icons8-exercise-48";
	public const string EFFECT_IMAGE_FIRE = "icons8-fire-48";
	public const string EFFECT_IMAGE_FIST = "icons8-fist-48";
	public const string EFFECT_IMAGE_FOOT = "icons8-foot-48";
	public const string EFFECT_IMAGE_HUNT = "icons8-hunt-48";
	public const string EFFECT_IMAGE_ICY = "icons8-icy-48";
	public const string EFFECT_IMAGE_LEAVING_GEO_FENCE_LOCATION = "icons8-leaving-geo-fence-location-48";
	public const string EFFECT_IMAGE_LOW_CONNECTION = "icons8-low-connection-48";
	public const string EFFECT_IMAGE_MEDICAL_BAG = "icons8-medical-bag-48";
	public const string EFFECT_IMAGE_MUSCLE = "icons8-muscle-48";
	public const string EFFECT_IMAGE_NAIL = "icons8-nail-48";
	public const string EFFECT_IMAGE_NATURAL_FOOD = "icons8-natural-food-48";
	public const string EFFECT_IMAGE_POISON = "icons8-poison-48";
	public const string EFFECT_IMAGE_PULSE = "icons8-pulse-48";
	public const string EFFECT_IMAGE_QUICK_MODE_ON = "icons8-quick-mode-on-48";
	public const string EFFECT_IMAGE_RADAR = "icons8-radar-48";
	public const string EFFECT_IMAGE_REFLECTION = "icons8-reflection-48";
	public const string EFFECT_IMAGE_REFRESH_SHIELD = "icons8-refresh-shield-48";
	public const string EFFECT_IMAGE_RUNNING = "icons8-running-48";
	public const string EFFECT_IMAGE_SCREAM = "icons8-scream-48";
	public const string EFFECT_IMAGE_SHIELD = "icons8-shield-48";
	public const string EFFECT_IMAGE_SKULL = "icons8-skull-48";
	public const string EFFECT_IMAGE_SNOWFLAKE = "icons8-snowflake-48";
	public const string EFFECT_IMAGE_THE_FLASH_SIGN = "icons8-the-flash-sign-48";
	public const string EFFECT_IMAGE_USER_SHIELD = "icons8-user-shield-48";
	public const string EFFECT_IMAGE_WALKING_STICK = "icons8-walking-stick-48";
	public const string EFFECT_IMAGE_WASH_YOUR_HANDS = "icons8-wash-your-hands-48";
	public const string EFFECT_IMAGE_WINTER = "icons8-winter-48";
	public const string EFFECT_IMAGE_CONFETTI = "icons8-confetti-48";
	public const string EFFECT_IMAGE_CLENCHED_FIST = "icons8-clenched-fist-48";
	public const string EFFECT_IMAGE_GATLING_GUN = "icons8-gatling-gun-48";
	public const string EFFECT_IMAGE_MORTAR = "icons8-mortar-48";
	public const string EFFECT_IMAGE_BARBER_POLE = "icons8-barber-pole-48";

	public const int EFFECT_STACK_TYPE_DURATION = 1;
	public const int EFFECT_STACK_TYPE_INTENSITY = 2;

	public const int EFFECT_UPDATE_TYPE_ADD = 1;
	public const int EFFECT_UPDATE_TYPE_REMOVE = 2;
	public const int EFFECT_UPDATE_TYPE_UPDATE = 3;


	// public BSON.ObjectId id;
	public int uid;
	public string name;
	public string label;
	public int effectModifier = EFFECT_MODIFIER_NONE;
	// public int cooldownTotal;
	// public int cooldownCurrent;
	public string image;
	public bool isStackable = false;
	public int stackType = 0;
	public int stacksTotal = 1;
	public List<EffectStackVO> effectStacks = new List<EffectStackVO>();
	public int duration = 1;
	public int intensity = 1;
	public bool isExhaustible = false; // remove when employed
	public int roundAdded;

	public EffectVO()
	{

	}

	public int TurnBegin()
	{
		// bool doUpdate = false;
		// int stacksRemaining = -1; // -1 equals no change
		int stacksRemaining;

		// update cooldowns
		foreach(EffectStackVO stack in effectStacks)
		{
			// do NOT process if roundAdded == currentPhaseTotal;
			if (roundAdded != GameManager.instance.currentPhaseTotal)
			{
				if (stack.cooldownCurrent > 0)
				{
					// reduce by turn
					stack.cooldownCurrent -= 1;

					if (stack.cooldownCurrent == 0)
						stacksRemaining = 0;
					else stacksRemaining = stack.cooldownCurrent;

					// if (stack.cooldownCurrent > 0)
					// 	stacksRemaining += stack.cooldownCurrent;
				}
				else
				{ 
					Debug.Log("<color=red>Cooldown already expired!</color>");
				}
			}
			else {
				Debug.Log("<color=red>Ability was added *this* round</color>");
				// already processed, don't double-dip
				return -1;
			}
		}

		// now, return total turns in all stacks remaining
		return GetStackTurnsRemaining();// GetNumberOfActiveStacks();// stacksRemaining;
	}

	public EffectVO ProcessEffect()
	{
		return this;
	}

	public int GetNumberOfActiveStacks(bool ignoreRoundAdded = false)
	{
		int totalActiveStacks = 0;

		switch(stackType)
		{
			case EffectVO.EFFECT_STACK_TYPE_DURATION:
				foreach (EffectStackVO stack in effectStacks)
				{
					if (stack.cooldownCurrent > 0)
						totalActiveStacks += 1;
				}
			break;

			case EffectVO.EFFECT_STACK_TYPE_INTENSITY:
				if (effectStacks[0].cooldownCurrent == 1)
				{
					totalActiveStacks = 0;
					effectStacks[0].intensityCurrent = 1; // <- reset
				}
				else totalActiveStacks = effectStacks[0].intensityCurrent;
			break;
		}

		// if effect added this round, ignore
		if (ignoreRoundAdded == false && roundAdded == GameManager.instance.currentPhaseTotal)
			totalActiveStacks = -1;

		return totalActiveStacks;
	}

	public int GetStackTurnsRemaining()
	{
		int totalDuration = 0;

		// switch (stackType)
		// {
		// 	case EffectVO.EFFECT_STACK_TYPE_DURATION:
		foreach (EffectStackVO stack in effectStacks)
		{
			// if (stack.cooldownCurrent > 0)
			totalDuration += stack.cooldownCurrent;
		}
		// break;

		// 	case EffectVO.EFFECT_STACK_TYPE_INTENSITY:
		// 		totalDuration = effectStacks[0].intensityCurrent;
		// 		break;
		// }

		return totalDuration;

	}

	public void resetStack()
	{
		foreach(EffectStackVO stack in effectStacks)
			stack.cooldownCurrent = 0;
	}

	public EffectStackVO AddStack()//EffectStackVO vo)
	{
		if (isStackable != true)
			return null;

		EffectStackVO vo = null;

		// new slots available?
		if (effectStacks.Count == 0 || effectStacks.Count < stacksTotal)
		{
			vo = new EffectStackVO();
			vo.cooldownTotal = duration;
			vo.cooldownCurrent = duration;
			vo.intensityTotal = intensity;
			vo.intensityCurrent = 1;

			effectStacks.Add(vo);
		}

		switch(stackType)
		{
			case EffectVO.EFFECT_STACK_TYPE_DURATION:
				if (effectStacks.Count == stacksTotal && vo == null) // vo == null means the final stack wasn't just added above
				{
					// find eldest stack
					EffectStackVO laststack = null;
					EffectStackVO eldest = null;
					foreach(EffectStackVO stack in effectStacks)
					{
						if (laststack != null && stack.cooldownCurrent < laststack.cooldownCurrent)
							eldest = stack;
						laststack = stack;
					}
					if (eldest == null)
						eldest = laststack;

					// reset eldest cooldown
					eldest.cooldownCurrent = duration;//cooldownTotal;
					vo = eldest;
				}
				else
				{
					
				}
			break;

			case EffectVO.EFFECT_STACK_TYPE_INTENSITY:
				if (effectStacks.Count == stacksTotal && vo == null) // vo == null means the final stack wasn't just added above
				{
					// intensity stacks have only 1
					vo = effectStacks[0];

					// update intensity (if applicable)
					if (vo.intensityCurrent < vo.intensityTotal)
						vo.intensityCurrent += 1;
				}
			break;
		}

		return vo;
	}
}

public class EffectStackVO
{
	public int cooldownTotal;
	public int cooldownCurrent = 0;
	public int intensityTotal;
	public int intensityCurrent = 1;
}

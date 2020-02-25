using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class AbilityLibraryVO
{
	private List<AbilityVO> pool = new List<AbilityVO>();

	public AbilityLibraryVO()
	{
		Debug.Log("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
		// pool.Add(CatClaws());
		// pool.Add(SkullBash());
		// pool.Add(Empower());
		// pool.Add(RazerSlash());
		// pool.Add(RifleSpray());

		// pool.Add(GroupHeal());
	}

	// public AbilityVO GetAbilityById(int id)
	// {
	// 	Debug.Log("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% id");
	// 	AbilityVO returnVal = null;

	// 	foreach(AbilityVO a in pool)
	// 	{
	// 		if (id == a.uid)
	// 		{
	// 			returnVal = a;
	// 			break;
	// 		}
	// 	}

	// 	return returnVal;
	// }
	public static AbilityVO CatClaws()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 101;
		a.slot = 1;
		a.image = "yellow (16)";
		a.name = "Cat Claws";
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_MELEE;
		a.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		a.positionRequirement = CharacterVO.POSITION_TYPE_FORE; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.effect = EffectMatrixVO.GetBleedEffect(new EffectVO());
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.healthRoll = new RngRoller(RngRoller.RNG_DICE_8, 5);
		a.effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 2);
		a.positionOffset = new Vector3(1, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		a.hitFx = "Hit 2 Variant";
		return a;
	}
	public static AbilityVO DefaultKnife()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 201;
		a.slot = 1;
		a.image = "Assassinskill_21";
		a.name = "Default Knife";
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_MELEE;
		a.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		a.positionRequirement = CharacterVO.POSITION_TYPE_FORE; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.cost = 2;
		a.effect = EffectMatrixVO.GetBleedEffect(new EffectVO());
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.healthRoll = new RngRoller(RngRoller.RNG_DICE_8, 5);
		a.effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 2);
		a.positionOffset = new Vector3(1, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		a.hitFx = "Hit 2 Variant";
		return a;
	}
	public static AbilityVO DefaultKatana()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 202;
		a.slot = 1;
		a.image = "7";
		a.name = "Katana";
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_MELEE;
		a.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		a.positionRequirement = CharacterVO.POSITION_TYPE_FORE; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.effect = EffectMatrixVO.GetBleedEffect(new EffectVO());
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.healthRoll = new RngRoller(RngRoller.RNG_DICE_8, 5);
		a.effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 2);
		a.positionOffset = new Vector3(1, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		a.soundOnBlockShieldPhysical = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_BLOCK_SHIELD_PHYSICAL);
		a.hitFx = "Hit 2 Variant";
		return a;
	}
	public static AbilityVO DefaultWeapon3()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 203;
		a.slot = 1;
		a.image = "Engineerskill_13";
		a.name = "Weapon 3";
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		a.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		a.positionRequirement = CharacterVO.POSITION_TYPE_MID_FORE; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.effect = EffectMatrixVO.GetBleedEffect(new EffectVO());
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.healthRoll = new RngRoller(RngRoller.RNG_DICE_8, 5);
		a.effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 2);
		a.positionOffset = new Vector3(3, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		a.soundOnBlockShieldPhysical = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_BLOCK_SHIELD_PHYSICAL);
		a.hitFx = "Hit 2 Variant";
		return a;
	}

	public static AbilityVO DefaultWeapon4()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 204;
		a.slot = 1;
		a.image = "Engineerskill_09";
		a.name = "Weapon 4";
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		a.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		a.positionRequirement = CharacterVO.POSITION_TYPE_ANY; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.effect = EffectMatrixVO.GetBleedEffect(new EffectVO());
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.healthRoll = new RngRoller(RngRoller.RNG_DICE_8, 5);
		a.effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 2);
		a.positionOffset = new Vector3(1, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		a.soundOnBlockShieldPhysical = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_BLOCK_SHIELD_PHYSICAL);
		a.hitFx = "Hit 2 Variant";
		return a;
	}

	public static AbilityVO DefaultWeapon5()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 205;
		a.slot = 1;
		a.image = "red (10)";
		a.name = "Weapon 5";
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_RANGED;
		a.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		a.positionRequirement = CharacterVO.POSITION_TYPE_MID_REAR; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.effect = EffectMatrixVO.GetBleedEffect(new EffectVO());
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.healthRoll = new RngRoller(RngRoller.RNG_DICE_8, 5);
		a.effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 2);
		a.positionOffset = new Vector3(1, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		a.soundOnBlockShieldPhysical = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_BLOCK_SHIELD_PHYSICAL);
		a.hitFx = "Hit 2 Variant";
		a.filterType = "matrix";
		return a;
	}
	public static AbilityVO DefaultWeapon6()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 206;
		a.slot = 1;
		a.image = "Engineerskill_07";
		a.name = "Weapon 6";
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		a.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		a.positionRequirement = CharacterVO.POSITION_TYPE_MID_FORE; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.effect = EffectMatrixVO.GetBleedEffect(new EffectVO());
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.healthRoll = new RngRoller(RngRoller.RNG_DICE_8, 5);
		a.effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 2);
		a.positionOffset = new Vector3(1, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		a.soundOnBlockShieldPhysical = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_BLOCK_SHIELD_PHYSICAL);
		a.hitFx = "Hit 2 Variant";
		return a;
	}
	public static AbilityVO DefaultWeapon7()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 207;
		a.slot = 1;
		a.image = "2-10 (32)";
		a.name = "Weapon 7";
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		a.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		a.positionRequirement = CharacterVO.POSITION_TYPE_REAR; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.effect = EffectMatrixVO.GetBleedEffect(new EffectVO());
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.healthRoll = new RngRoller(RngRoller.RNG_DICE_8, 5);
		a.effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 2);
		a.positionOffset = new Vector3(1, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		a.soundOnBlockShieldPhysical = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_BLOCK_SHIELD_PHYSICAL);
		a.hitFx = "Hit 2 Variant";
		return a;
	}

	public static AbilityVO DefaultWeapon8()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 208;
		a.slot = 1;
		a.image = "yellow (3)";
		a.name = "Weapon 8";
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_RANGED;
		a.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		a.positionRequirement = CharacterVO.POSITION_TYPE_REAR; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.effect = EffectMatrixVO.GetBleedEffect(new EffectVO());
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.healthRoll = new RngRoller(RngRoller.RNG_DICE_8, 5);
		a.effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 2);
		a.positionOffset = new Vector3(1, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		a.soundOnBlockShieldPhysical = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_BLOCK_SHIELD_PHYSICAL);
		a.hitFx = "Hit 2 Variant";
		return a;
	}

	public static AbilityVO DefaultGear()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 301;
		a.slot = 1;
		a.image = "Shamanskill_20";
		a.name = "Cat Claws";
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SELF;
		a.targetType = AbilityVO.TARGET_TYPE_COLLEAGUE;
		a.positionRequirement = CharacterVO.POSITION_TYPE_FORE; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.effect = EffectMatrixVO.GetBleedEffect(new EffectVO());
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.healthRoll = new RngRoller(RngRoller.RNG_DICE_8, 5);
		a.effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 2);
		a.positionOffset = new Vector3(1, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		a.hitFx = "Hit 2 Variant";
		return a;
	}
	public static AbilityVO DefaultGear2()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 302;
		a.slot = 1;
		a.image = "2-10 (4)";
		a.name = "Default Gear 2";
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SELF;
		a.targetType = AbilityVO.TARGET_TYPE_COLLEAGUE;
		a.positionRequirement = CharacterVO.POSITION_TYPE_FORE; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.effect = EffectMatrixVO.GetBleedEffect(new EffectVO());
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.healthRoll = new RngRoller(RngRoller.RNG_DICE_8, 5);
		a.effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 2);
		a.positionOffset = new Vector3(1, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		a.hitFx = "Hit 2 Variant";
		return a;
	}
	public static AbilityVO DefaultGear3()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 303;
		a.slot = 1;
		a.image = "Paladinskill_03";
		a.name = "Default Gear 3";
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SELF;
		a.targetType = AbilityVO.TARGET_TYPE_COLLEAGUE;
		a.positionRequirement = CharacterVO.POSITION_TYPE_MID_FORE; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.effect = EffectMatrixVO.GetBleedEffect(new EffectVO());
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.healthRoll = new RngRoller(RngRoller.RNG_DICE_8, 5);
		a.effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 2);
		a.positionOffset = new Vector3(1, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		a.hitFx = "Hit 2 Variant";
		return a;
	}
	public static AbilityVO DefaultGear4()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 304;
		a.slot = 1;
		a.image = "Priestskill_46";
		a.name = "Default Gear 4";
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SELF;
		a.targetType = AbilityVO.TARGET_TYPE_COLLEAGUE;
		a.positionRequirement = CharacterVO.POSITION_TYPE_MID_FORE; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.effect = EffectMatrixVO.GetBleedEffect(new EffectVO());
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.healthRoll = new RngRoller(RngRoller.RNG_DICE_8, 5);
		a.effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 2);
		a.positionOffset = new Vector3(1, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		a.hitFx = "Hit 2 Variant";
		return a;
	}

	public static AbilityVO DefaultGear5()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 305;
		a.slot = 1;
		a.image = "59";
		a.name = "Default Gear 5";
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SELF;
		a.targetType = AbilityVO.TARGET_TYPE_COLLEAGUE;
		a.positionRequirement = CharacterVO.POSITION_TYPE_MID_REAR; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.effect = EffectMatrixVO.GetBleedEffect(new EffectVO());
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.healthRoll = new RngRoller(RngRoller.RNG_DICE_8, 5);
		a.effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 2);
		a.positionOffset = new Vector3(1, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		a.hitFx = "Hit 2 Variant";
		return a;
	}
	public static AbilityVO DefaultGear6()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 306;
		a.slot = 1;
		a.image = "Shamanskill_09";
		a.name = "Default Gear 6";
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SELF;
		a.targetType = AbilityVO.TARGET_TYPE_COLLEAGUE;
		a.positionRequirement = CharacterVO.POSITION_TYPE_MID_REAR; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.effect = EffectMatrixVO.GetBleedEffect(new EffectVO());
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.healthRoll = new RngRoller(RngRoller.RNG_DICE_8, 5);
		a.effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 2);
		a.positionOffset = new Vector3(1, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		a.hitFx = "Hit 2 Variant";
		return a;
	}
	public static AbilityVO DefaultGear7()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 307;
		a.slot = 1;
		a.image = "2-10 (95)";
		a.name = "Default Gear 7";
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SELF;
		a.targetType = AbilityVO.TARGET_TYPE_COLLEAGUE;
		a.positionRequirement = CharacterVO.POSITION_TYPE_REAR; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.effect = EffectMatrixVO.GetBleedEffect(new EffectVO());
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.healthRoll = new RngRoller(RngRoller.RNG_DICE_8, 5);
		a.effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 2);
		a.positionOffset = new Vector3(1, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		a.hitFx = "Hit 2 Variant";
		return a;
	}
	public static AbilityVO DefaultGear8()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 308;
		a.slot = 1;
		a.image = "2-10 (50)";
		a.name = "Default Gear 8";
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SELF;
		a.targetType = AbilityVO.TARGET_TYPE_COLLEAGUE;
		a.positionRequirement = CharacterVO.POSITION_TYPE_REAR; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.effect = EffectMatrixVO.GetBleedEffect(new EffectVO());
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.healthRoll = new RngRoller(RngRoller.RNG_DICE_8, 5);
		a.effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 2);
		a.positionOffset = new Vector3(1, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		a.hitFx = "Hit 2 Variant";
		return a;
	}
	public static AbilityVO LockedWeaponMod()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 1001;
		a.slot = 1;
		a.image = "lock_64_l_circle";//"lock_64";
		a.name = "Weapon Mod Locked";
		return a;
	}
	public static AbilityVO LockedGearMod()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 1002;
		a.slot = 1;
		a.image = "lock_64_l_circle";//"lock_64";
		a.name = "Gear Mod Locked";
		return a;
	}
	public static AbilityVO EmptyWeaponMod()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 1003;
		a.slot = 1;
		a.image = "unlock_64_l_circle";//"lock_64";
		a.name = "Empty Weapon Mod";
		return a;
	}
	public static AbilityVO EmptyGearMod()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 1004;
		a.slot = 1;
		a.image = "unlock_64_l_circle";//"lock_64";
		a.name = "Empty Gear Mod";
		return a;
	}

	public static AbilityVO SkullBash()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 102;
		a.slot = 2;
		a.image = "2-10 (42)";
		a.name = "Skull Bash";
		a.positionRequirement = CharacterVO.POSITION_TYPE_ANY; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.positionOffset = new Vector3(3, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		a.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		return a;
	}

	public static AbilityVO Empower()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 103;
		a.slot = 3;
		a.image = "Engineerskill_07";//"red (1)";
		a.name = "Empower";
		a.floaterText = "Empowered!";
		a.positionRequirement = CharacterVO.POSITION_TYPE_ANY; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "Teleport2 Variant";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.healthModifier = HealthVO.HEALTH_MODIFIER_NONE;
		// // a.effectModifier = EffectVO.EFFECT_MODIFIER_BOON;
		a.effect = EffectMatrixVO.GetMightEffect(a.effect);
		a.positionOffset = new Vector3(0.5f, 3.5f, -0.5f);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(2, 2, 0);
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SELF;
		a.targetType = AbilityVO.TARGET_TYPE_COLLEAGUE;
		return a;
	}

	public static AbilityVO RazerSlash()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 104;
		a.slot = 4;
		a.image = "53";
		a.name = "Razer Slash";
		a.positionRequirement = CharacterVO.POSITION_TYPE_ANY; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.positionOffset = new Vector3(3, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		a.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		return a;
	}

	public static AbilityVO RifleSpray()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 105;
		a.slot = 5;
		a.image = "67";
		a.name = "Rifle Spray";
		a.positionRequirement = CharacterVO.POSITION_TYPE_ANY; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.positionOffset = new Vector3(3, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		a.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		return a;
	}

	public static AbilityVO Medic1()
	{
		AbilityVO a = new AbilityVO();
		a.uid= 401;
		a.slot = 1;
		a.image = "yellow (3)";
		a.name = "Medic 1";
		a.positionRequirement = CharacterVO.POSITION_TYPE_ANY; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.positionOffset = new Vector3(3, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_MELEE;
		a.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		return a;
	}

	public static AbilityVO Medic2()
	{
		AbilityVO a = new AbilityVO();
		a.uid= 402;
		a.slot = 2;
		a.image = "2";
		a.name = "Medic 2";
		a.positionRequirement = CharacterVO.POSITION_TYPE_ANY; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.positionOffset = new Vector3(3, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		a.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		return a;
	}
	public static AbilityVO Medic3()
	{
		AbilityVO a = new AbilityVO();
		a.uid= 403;
		a.slot = 3;
		a.image = "3";
		a.name = "Medic 3";
		a.positionRequirement = CharacterVO.POSITION_TYPE_ANY; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.positionOffset = new Vector3(3, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		a.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		return a;
	}

	public static AbilityVO GroupHeal()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 404;
		a.slot = 4;
		a.image = "2-10 (50)";
		a.name = "Group Heal";
		a.positionRequirement = CharacterVO.POSITION_TYPE_ANY; // <-- 0 = any position
		a.targets = 4;
		a.prefabId = "Heal Variant";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.healthModifier = HealthVO.HEALTH_MODIFIER_HEAL;
		a.healthRoll = new RngRoller(RngRoller.RNG_DICE_8, 2);
		// a.effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 2);
		a.positionOffset = new Vector3(3, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_CREW;
		a.targetType = AbilityVO.TARGET_TYPE_COLLEAGUE;
		return a;
	}

	public static AbilityVO Medic5()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 405;
		a.slot = 4;
		a.image = "2-10 (50)";
		a.name = "Group Heal";
		a.positionRequirement = CharacterVO.POSITION_TYPE_ANY; // <-- 0 = any position
		a.targets = 4;
		a.prefabId = "Heal Variant";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.positionOffset = new Vector3(3, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_CREW;
		a.targetType = AbilityVO.TARGET_TYPE_COLLEAGUE;
		return a;
	}

	public static AbilityVO MistyTurret()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 803;
		a.slot = 4;
		a.image = "green (14)";
		a.name = "Mists Turret";
		a.isSidecar = true;
		a.floaterText = "Mists Turret!";
		a.positionRequirement = CharacterVO.POSITION_TYPE_ANY; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "Teleport2 Variant";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.healthModifier = HealthVO.HEALTH_MODIFIER_NONE;
		// // a.effectModifier = EffectVO.EFFECT_MODIFIER_BOON;
		a.effect = EffectMatrixVO.GetTurretEffect(a.effect);
		a.positionOffset = new Vector3(0.0f, 3f, -0.5f);
		a.rotationOffset = new Vector3(-90f, 90, 0);
		a.localScaleOffset = new Vector3(0.65f, 0.65f, 0.65f);
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SIDECAR;
		a.targetType = AbilityVO.TARGET_TYPE_COLLEAGUE;
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset("Quest_Game_Award_Positive_Star_3_Dark");
		a.hitFx = "Magic circle 13 Variant";
		return a;
	}

	public static AbilityVO Chromer1()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 501;
		a.slot = 1;
		a.image = "Warriorskill_13";//"red (31)";
		a.name = "Searing Blades";
		a.positionRequirement = CharacterVO.POSITION_TYPE_ANY; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.effect = EffectMatrixVO.GetBurnEffect(new EffectVO());
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.healthRoll = new RngRoller(RngRoller.RNG_DICE_8, 5);
		a.effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 2);
		a.positionOffset = new Vector3(1, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_MELEE;
		a.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset(SoundVO.ASSET_IMPACT_LASER_LIGHT_1);
		a.hitFx = "Hit 15 Variant";
		return a;
	}
	public static AbilityVO MistsTotem()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 703;
		a.slot = 4;
		a.image = "blue (9)";
		a.name = "Mists Totem";
		a.isSidecar = true;
		a.floaterText = "Mists Totem!";
		a.positionRequirement = CharacterVO.POSITION_TYPE_ANY; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "Teleport2 Variant";
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.healthModifier = HealthVO.HEALTH_MODIFIER_NONE;
		// // a.effectModifier = EffectVO.EFFECT_MODIFIER_BOON;
		a.effect = EffectMatrixVO.GetTotemEffect(a.effect);
		a.positionOffset = new Vector3(0.0f, 3f, -0.5f);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(1.0f, 1.0f, 1.0f);
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SIDECAR;
		a.targetType = AbilityVO.TARGET_TYPE_COLLEAGUE;
		a.soundOnActivate = SoundMatrixVO.GetSoundByAsset("Quest_Game_Tribal_Ancient_Relic_Perc_Drone_Tone_1");
		a.soundOnHit = SoundMatrixVO.GetSoundByAsset("Quest_Game_Award_Positive_Star_3_Dark");
		a.hitFx = "Magic circle 13 Variant";
		return a;
	}
	public static AbilityVO Grappler()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 6003;
		a.slot = 3;
		a.image = "79";
		a.name = "Whiplash";
		a.positionRequirement = 0; // <-- 0 = any position
		a.targets = 2;
		a.prefabId = "REP4_Effect1";
		a.effect = EffectMatrixVO.GetBleedEffect(new EffectVO());
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.positionOffset = new Vector3(3, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_RANGED;
		a.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		return a;
	}

	public static AbilityVO SlamDancer()
	{
		AbilityVO a = new AbilityVO();
		a.uid= 701;
		a.slot = 1;
		a.image = "2-10 (11)";
		a.name = "Grappler";
		// a.name = "Vaporizer";
		a.positionRequirement = 0; // <-- 0 = any position
		a.targets = 1;
		a.prefabId = "";
		a.effect = EffectMatrixVO.GetBleedEffect(new EffectVO());
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.positionOffset = new Vector3(3, 1, 0);
		a.rotationOffset = new Vector3(0, 90, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		a.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		return a;
	}

	public static AbilityVO Portal()
	{
		AbilityVO a = new AbilityVO();
		a.uid = 2005;
		a.slot = 4;
		a.image = "91";
		a.positionRequirement = 0; // <-- 0 = any position
		a.targets = 2;
		a.name = "Portal";
		a.prefabId = "REP4_Effect5";
		a.effect = EffectMatrixVO.GetBleedEffect(new EffectVO());
		a.cooldownTotal = 2;
		a.cooldown = 0;
		a.healthModifier = HealthVO.HEALTH_MODIFIER_DAMAGE;
		a.healthRoll = new RngRoller(RngRoller.RNG_DICE_8, 5);
		a.effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 2);
		a.positionXRelativeToPrimaryTarget = true;
		a.positionOffset = new Vector3(0.0f, 3.5f, 1.0f);
		a.rotationOffset = new Vector3(-45, -85, 0);
		a.localScaleOffset = new Vector3(0, 0, 0);
		a.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_RANGED;
		a.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		a.isFloorTriggered = true;
		return a;
	}

}
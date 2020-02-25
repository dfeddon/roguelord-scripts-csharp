using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class AbilityVO
{
	public const int ABILITY_ACTIONTYPE_OFFENSE_RANGED = 1; // ranged attack targets
	public const int ABILITY_ACTIONTYPE_OFFENSE_MELEE = 2; // melee attack targets
	public const int ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE = 3; // projectile
	public const int ABILITY_ACTIONTYPE_DEFENSE_SELF = 4; // self ability
	public const int ABILITY_ACTIONTYPE_DEFENSE_CREW = 5; // party heals, party buffs, party clenses, etc
	public const int ACTION_TYPE_REPOSITIONING = 6; // character reposition
	public const int ABILITY_ACTIONTYPE_DEFENSE_SIDECAR = 7;
	public const int TARGET_TYPE_OPPONENT = 1;
	public const int TARGET_TYPE_COLLEAGUE = 2;
	public const int WEAPON_TYPE_RIFLE = 1;
	public const int WEAPON_TYPE_PISTOL = 2;
	public const int WEAPON_TYPE_PUNCH = 3;
	public const int WEAPON_TYPE_KICK = 4;
	public const int WEAPON_TYPE_FIRE = 5;
	public const int WEAPON_TYPE_EXPLOSION = 6;
	public const int WEAPON_TYPE_POISON = 7;
	public const int WEAPON_TYPE_ACID = 8;

	public int uid;
	public int id;
	public int targets;
	public int cooldown = 0;
	public int cooldownTotal;
	public int positionRequirement;
	public string name;
	public string floaterText;
	public string prefabId;
	// private GameObject prefab;
	public bool isLocked;
	public int level;
	public int slot; // 1: weapon, 2: weapon mod, 3: gear mod, 4: gear
	public string image;
	public int cooldownLength;
	public int cooldownValue;
	public string animationName;
	public int actionType;
	public bool isSidecar = false;
	public int targetType = TARGET_TYPE_OPPONENT;
	public int weaponType;
	public int healthModifier = HealthVO.HEALTH_MODIFIER_NONE;
	public RngRoller healthRoll = new RngRoller(RngRoller.RNG_DICE_6, 1);
	// public int effectModifier = EffectVO.EFFECT_MODIFIER_NONE;
	public int effectId;
	public EffectVO effect;// = new EffectVO();
	public RngRoller effectRoll = new RngRoller(RngRoller.RNG_DICE_4, 1);
	public Vector3 positionOffset;
	public Vector3 rotationOffset;
	public Vector3 localScaleOffset;
	public bool positionXRelativeToPrimaryTarget = false;
	public bool hasFXSettings = false;
	public bool isFloorTriggered = false;
	public Dictionary<string, string> effectSettings;
	public SoundVO soundOnHit;
	public SoundVO soundOnBlockShieldPhysical;
	public SoundVO soundOnBlockShieldCyber;
	public SoundVO soundOnActivate;
	public string hitFx = "Hit 1 Variant";
	public CharacterVO owner;
	public string filterType = null;
	public int cost = 0;

	public AbilityVO(int _slot = 0, string _name = "", string _prefabId = "")
	{
		this.slot = _slot;
		this.name = _name;
		this.prefabId = _prefabId;
		this.effect = new EffectVO();
		// this.soundOnActivate = new SoundVO();
		// this.soundOnHit = new SoundVO();
	}

	public AbilityVO getClone(AbilityVO _vo)
	{
		AbilityVO vo = new AbilityVO();
		vo.uid = _vo.uid;
		// vo.id = _vo.id;
		vo.targets = _vo.targets;
		vo.cooldown = _vo.cooldown;
		vo.cooldownTotal = _vo.cooldownTotal;
		vo.positionRequirement = _vo.positionRequirement;
		vo.name = _vo.name;
		vo.floaterText = _vo.floaterText;
		vo.prefabId = _vo.prefabId;
		vo.isLocked = _vo.isLocked;
		vo.level = _vo.level;
		vo.slot = _vo.slot;
		vo.image = _vo.image;
		vo.cooldownLength = _vo.cooldownLength;
		vo.cooldownValue = _vo.cooldownValue;
		vo.animationName = _vo.animationName;
		vo.actionType = _vo.actionType;
		vo.targetType = _vo.targetType;
		vo.weaponType = _vo.weaponType;
		vo.positionOffset = _vo.positionOffset;
		vo.rotationOffset = _vo.rotationOffset;
		vo.localScaleOffset = _vo.localScaleOffset;
		vo.positionXRelativeToPrimaryTarget = _vo.positionXRelativeToPrimaryTarget;
		vo.hasFXSettings = _vo.hasFXSettings;
		vo.isFloorTriggered = _vo.isFloorTriggered;
		vo.effectSettings = _vo.effectSettings;
		vo.healthModifier = _vo.healthModifier;
		vo.healthRoll = _vo.healthRoll;
		// vo.effectModifier = _vo.effectModifier;
		vo.effect = _vo.effect;
		vo.effectRoll = _vo.effectRoll;
		vo.soundOnHit = _vo.soundOnHit;
		vo.soundOnActivate = _vo.soundOnActivate;
		vo.soundOnBlockShieldPhysical = _vo.soundOnBlockShieldPhysical;
		vo.hitFx = _vo.hitFx;
		vo.owner = _vo.owner;
		vo.filterType = _vo.filterType;
		vo.cost = _vo.cost;

		return vo;
	}
}
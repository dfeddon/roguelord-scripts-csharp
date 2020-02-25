using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public static class AbilityMatrixVO
{
	// public AbilityMatrixVO()
	// {
	// }

	public static AbilityVO[] GetAbilitiesByClass(int role, int slot = 0)
	{
		// AbilityVO[] a;

		switch(role)
		{
			case CharacterVO.ROLE_ANARCHIST 	:		return GetAnarchistAbilities(slot);
			case CharacterVO.ROLE_CHROMER 		:		return GetChromerAbilities(slot);
			case CharacterVO.ROLE_SHAMAN 		:		return GetChemistAbilities(slot);
			case CharacterVO.ROLE_MEDIC 		:		return GetMedicAbilities(slot);
			case CharacterVO.ROLE_ENGINEER 		:		return GetEngineerAbilities(slot);
			case CharacterVO.ROLE_SABOTEUR 		:		return GetSaboteurAbilities(slot);
			case CharacterVO.ROLE_NANOMANCER 	:		return GetNanomancerAbilities(slot);
			case CharacterVO.ROLE_BLACKHATTER 	:		return GetBlackhatterAbilities(slot);
		}

		return null;
	}

	private static List<AbilityVO> PoolAbilities()
	{
		List<AbilityVO> list = new List<AbilityVO>();
		
		list.AddRange(GetAnarchistAbilities(0));
		list.AddRange(GetChromerAbilities(0));
		list.AddRange(GetChemistAbilities(0));
		list.AddRange(GetMedicAbilities(0));
		list.AddRange(GetEngineerAbilities(0));
		list.AddRange(GetSaboteurAbilities(0));
		list.AddRange(GetNanomancerAbilities(0));
		list.AddRange(GetBlackhatterAbilities(0));

		// add general abilities
		list.Add(AbilityLibraryVO.LockedGearMod());
		list.Add(AbilityLibraryVO.LockedWeaponMod());
		list.Add(AbilityLibraryVO.EmptyWeaponMod());
		list.Add(AbilityLibraryVO.EmptyGearMod());

		return list;
	}

	public static List<AbilityVO> GetCharacterAbilities(CharacterVO c)
	{
		Debug.Log("* character " + c.handle);
		List<AbilityVO> list = new List<AbilityVO>();
		AbilityVO a1 = AbilityMatrixVO.GetAbilityById(c.weaponAbility);
		// a1.owner = c;
		AbilityVO a2 = AbilityMatrixVO.GetAbilityById(c.weaponModAbility);
		// a2.owner = c;
		AbilityVO a3 = AbilityMatrixVO.GetAbilityById(c.gearModAbility);
		// a3.owner = c;
		AbilityVO a4 = AbilityMatrixVO.GetAbilityById(c.gearAbility);
		// a4.owner = c;

		if (a2.owner == null)
		{
			a1.owner = c;
			a2.owner = c;
			a3.owner = c;
			a4.owner = c;
		}

		list.Add(a1);
		list.Add(a2);
		list.Add(a3);
		list.Add(a4);

		return list;
	}

	public static AbilityVO GetAbilityById(int id)
	{
		Debug.Log("* get ability by id " + id);

		List<AbilityVO> list = PoolAbilities();
		foreach(AbilityVO a in list)
		{
			if (a.uid == id)
			{
				return a;
			}
		}
		Debug.Log("did not find id " + id);
		return null;
	}

	public static AbilityVO[] GetAnarchistAbilities(int slot)
	{
		// default weapon
		AbilityVO weapon = AbilityLibraryVO.DefaultKatana();
		weapon.slot = 1;
		weapon.cooldown = 0;

		// weapon mod locked
		AbilityVO weaponMod = AbilityLibraryVO.LockedWeaponMod();
		weaponMod.slot = 2;
		weaponMod.cooldown = 0;

		// gear mod locked
		AbilityVO gearMod = AbilityLibraryVO.LockedGearMod();
		gearMod.slot = 3;
		gearMod.cooldown = 0;

		// default gear
		AbilityVO gear = AbilityLibraryVO.DefaultGear2();
		gear.slot = 4;
		gear.cooldown = 0;

		// AbilityVO ability5 = AbilityLibraryVO.RifleSpray();
		// ability5.slot = 5;
		// ability5.cooldown = 0;

		AbilityVO[] array = { weapon, weaponMod, gearMod, gear };//, ability5 };

		return array;
	}

	public static AbilityVO[] GetChromerAbilities(int slot)
	{
		// default weapon
		AbilityVO weapon = AbilityLibraryVO.DefaultKnife();
		weapon.slot = 1;
		weapon.cooldown = 0;
		Debug.Log("chromer weapon " + weapon.uid);

		// weapon mod locked
		AbilityVO weaponMod = AbilityLibraryVO.LockedWeaponMod();
		weaponMod.slot = 2;
		weaponMod.cooldown = 0;

		// gear mod locked
		AbilityVO gearMod = AbilityLibraryVO.LockedGearMod();
		gearMod.slot = 3;
		gearMod.cooldown = 0;

		// default gear
		AbilityVO gear = AbilityLibraryVO.DefaultGear();
		gear.slot = 4;
		gear.cooldown = 0;

		// AbilityVO ability5 = AbilityLibraryVO.RifleSpray();
		// ability5.slot = 5;
		// ability5.cooldown = 0;

		AbilityVO[] array = { weapon, weaponMod, gearMod, gear };//, ability5 };

		return array;
	}

	public static AbilityVO[] GetChemistAbilities(int slot)
	{
		AbilityVO weapon = AbilityLibraryVO.DefaultWeapon7();
		weapon.slot = 1;
		weapon.cooldown = 0;

		// weapon mod locked
		AbilityVO weaponMod = AbilityLibraryVO.LockedWeaponMod();
		weaponMod.slot = 2;
		weaponMod.cooldown = 0;

		// gear mod locked
		AbilityVO gearMod = AbilityLibraryVO.LockedGearMod();
		gearMod.slot = 3;
		gearMod.cooldown = 0;


		AbilityVO gear = AbilityLibraryVO.DefaultGear7();
		gear.slot = 4;
		gear.cooldown = 0;

		AbilityVO[] array = { weapon, weaponMod, gearMod, gear };

		return array;
		/*
		AbilityVO ability1 = new AbilityVO();
		ability1.uid= 701;
		ability1.slot = 1;
		ability1.image = "2-10 (11)";
		ability1.name = "Grappler";
		ability1.name = "Vaporizer";
		ability1.positionRequirement = 0; // <-- 0 = any position
		ability1.targets = 1;
		ability1.prefabId = "";
		ability1.cooldownTotal = 2;
		ability1.cooldown = 0;
		ability1.positionOffset = new Vector3(3, 1, 0);
		ability1.rotationOffset = new Vector3(0, 90, 0);
		ability1.localScaleOffset = new Vector3(0, 0, 0);
		ability1.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		ability1.targetType = AbilityVO.TARGET_TYPE_OPPONENT;

		AbilityVO ability2 = new AbilityVO();
		ability2.uid= 002;
		ability2.slot = 2;
		ability2.image = "2-10 (78)";
		ability2.name = "Purge";
		ability2.name = "Vaporizer";
		ability2.positionRequirement = 0; // <-- 0 = any position
		ability2.targets = 1;
		ability2.prefabId = "";
		ability2.cooldownTotal = 2;
		ability2.cooldown = 0;
		ability2.positionOffset = new Vector3(3, 1, 0);
		ability2.rotationOffset = new Vector3(0, 90, 0);
		ability2.localScaleOffset = new Vector3(0, 0, 0);
		ability2.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		ability2.targetType = AbilityVO.TARGET_TYPE_OPPONENT;

		AbilityVO ability3 = new AbilityVO();
		ability3.uid= 003;
		ability3.slot = 3;
		ability3.image = "79";
		ability3.name = "Whiplash";
		ability3.positionRequirement = 0; // <-- 0 = any position
		ability3.targets = 2;
		ability3.prefabId = "REP4_Effect1";
		ability3.cooldownTotal = 2;
		ability3.cooldown = 0;
		ability3.positionOffset = new Vector3(3, 1, 0);
		ability3.rotationOffset = new Vector3(0, 90, 0);
		ability3.localScaleOffset = new Vector3(0, 0, 0);
		ability3.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_RANGED;
		ability3.targetType = AbilityVO.TARGET_TYPE_OPPONENT;


		AbilityVO ability4 = AbilityLibraryVO.MistsTotem();
		ability4.slot = 4;
		ability4.cooldown = 3;

		AbilityVO ability5 = new AbilityVO();
		ability5.uid= 005;
		ability5.slot = 5;
		ability5.image = "91";
		ability5.positionRequirement = 0; // <-- 0 = any position
		ability5.targets = 2;
		ability5.name = "Portal";
		ability5.prefabId = "REP4_Effect5";
		ability5.cooldownTotal = 2;
		ability5.cooldown = 0;
		ability5.positionXRelativeToPrimaryTarget = true;
		ability5.positionOffset = new Vector3(3.0f, 3, 1.0f);
		ability5.rotationOffset = new Vector3(-45, -65, 0);
		ability5.localScaleOffset = new Vector3(0, 0, 0);
		ability5.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_RANGED;
		ability5.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		ability5.isFloorTriggered = true;

		AbilityVO[] array = { ability1, ability4, ability3, ability2, ability5 };

		return array;
		*/
	}

	public static AbilityVO[] GetMedicAbilities(int slot)
	{
		AbilityVO weapon = AbilityLibraryVO.DefaultWeapon8();//Medic1();
		weapon.slot = 1;
		weapon.cooldown = 0;

		// weapon mod locked
		AbilityVO weaponMod = AbilityLibraryVO.LockedWeaponMod();
		weaponMod.slot = 2;
		weaponMod.cooldown = 0;

		// gear mod locked
		AbilityVO gearMod = AbilityLibraryVO.LockedGearMod();
		gearMod.slot = 3;
		gearMod.cooldown = 0;


		AbilityVO gear = AbilityLibraryVO.DefaultGear8();
		gear.slot = 4;
		gear.cooldown = 0;

		AbilityVO[] array = { weapon, weaponMod, gearMod, gear };

		return array;
	}

	public static AbilityVO[] GetEngineerAbilities(int slot)
	{
		// default weapon
		AbilityVO weapon = AbilityLibraryVO.Portal();//DefaultWeapon4();
		weapon.slot = 1;
		weapon.cooldown = 0;
		weapon.positionRequirement = CharacterVO.POSITION_TYPE_MID_FORE;

		// weapon mod locked
		AbilityVO weaponMod = AbilityLibraryVO.LockedWeaponMod();
		weaponMod.slot = 2;
		weaponMod.cooldown = 0;

		// gear mod locked
		AbilityVO gearMod = AbilityLibraryVO.LockedGearMod();
		gearMod.slot = 3;
		gearMod.cooldown = 0;

		// default gear
		AbilityVO gear = AbilityLibraryVO.DefaultGear4();
		gear.slot = 4;
		gear.cooldown = 0;
		gear.positionRequirement = CharacterVO.POSITION_TYPE_MID_FORE;

		/*AbilityVO ability1 = new AbilityVO();
		ability1.uid= 501;
		ability1.slot = 1;
		ability1.image = "Engineerskill_36";
		ability1.name = "Vaporizer";
		ability1.positionRequirement = 0; // <-- 0 = any position
		ability1.targets = 1;
		ability1.prefabId = "";
		ability1.cooldownTotal = 2;
		ability1.cooldown = 0;
		ability1.positionOffset = new Vector3(3, 1, 0);
		ability1.rotationOffset = new Vector3(0, 90, 0);
		ability1.localScaleOffset = new Vector3(0, 0, 0);
		ability1.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		ability1.targetType = AbilityVO.TARGET_TYPE_OPPONENT;

		AbilityVO ability2 = new AbilityVO();
		ability2.uid= 502;
		ability2.slot = 2;
		ability2.image = "2";
		ability2.name = "Vaporizer";
		ability2.positionRequirement = 0; // <-- 0 = any position
		ability2.targets = 1;
		ability2.prefabId = "";
		ability2.cooldownTotal = 2;
		ability2.cooldown = 0;
		ability2.positionOffset = new Vector3(3, 1, 0);
		ability2.rotationOffset = new Vector3(0, 90, 0);
		ability2.localScaleOffset = new Vector3(0, 0, 0);
		ability2.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		ability2.targetType = AbilityVO.TARGET_TYPE_OPPONENT;

		AbilityVO ability3 = new AbilityVO();
		ability3.uid= 503;
		ability3.slot = 3;
		ability3.image = "3";
		ability3.name = "Vaporizer";
		ability3.positionRequirement = 0; // <-- 0 = any position
		ability3.targets = 1;
		ability3.prefabId = "";
		ability3.cooldownTotal = 2;
		ability3.cooldown = 0;
		ability3.positionOffset = new Vector3(3, 1, 0);
		ability3.rotationOffset = new Vector3(0, 90, 0);
		ability3.localScaleOffset = new Vector3(0, 0, 0);
		ability3.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		ability3.targetType = AbilityVO.TARGET_TYPE_OPPONENT;

		AbilityVO ability4 = new AbilityVO();
		ability4.uid= 504;
		ability4.slot = 4;
		ability4.image = "4";
		ability4.name = "Vaporizer";
		ability4.positionRequirement = 0; // <-- 0 = any position
		ability4.targets = 1;
		ability4.prefabId = "";
		ability4.cooldownTotal = 2;
		ability4.cooldown = 0;
		ability4.positionOffset = new Vector3(3, 1, 0);
		ability4.rotationOffset = new Vector3(0, 90, 0);
		ability4.localScaleOffset = new Vector3(0, 0, 0);
		ability4.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		ability4.targetType = AbilityVO.TARGET_TYPE_OPPONENT;

		AbilityVO ability5 = AbilityLibraryVO.MistyTurret();
		ability5.slot = 5;
		ability5.cooldown = 3;*/

		AbilityVO[] array = { weapon, weaponMod, gearMod, gear };

		return array;
	}

	public static AbilityVO[] GetSaboteurAbilities(int slot)
	{
		// default weapon
		AbilityVO weapon = AbilityLibraryVO.DefaultWeapon6();
		weapon.slot = 1;
		weapon.cooldown = 0;

		// weapon mod locked
		AbilityVO weaponMod = AbilityLibraryVO.LockedWeaponMod();
		weaponMod.slot = 2;
		weaponMod.cooldown = 0;

		// gear mod locked
		AbilityVO gearMod = AbilityLibraryVO.LockedGearMod();
		gearMod.slot = 3;
		gearMod.cooldown = 0;

		// default gear
		AbilityVO gear = AbilityLibraryVO.DefaultGear6();
		gear.slot = 4;
		gear.cooldown = 0;

		AbilityVO[] array = { weapon, weaponMod, gearMod, gear };

		return array;
		/*
		AbilityVO ability1 = new AbilityVO();
		ability1.uid= 601;
		ability1.slot = 1;
		ability1.image = "1";
		ability1.name = "Vaporizer";
		ability1.positionRequirement = 0; // <-- 0 = any position
		ability1.targets = 1;
		ability1.prefabId = "";
		ability1.cooldownTotal = 2;
		ability1.cooldown = 0;
		ability1.positionOffset = new Vector3(3, 1, 0);
		ability1.rotationOffset = new Vector3(0, 90, 0);
		ability1.localScaleOffset = new Vector3(0, 0, 0);
		ability1.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		ability1.targetType = AbilityVO.TARGET_TYPE_OPPONENT;

		AbilityVO ability2 = new AbilityVO();
		ability2.uid= 602;
		ability2.slot = 2;
		ability2.image = "2";
		ability2.name = "Vaporizer";
		ability2.positionRequirement = 0; // <-- 0 = any position
		ability2.targets = 1;
		ability2.prefabId = "";
		ability2.cooldownTotal = 2;
		ability2.cooldown = 0;
		ability2.positionOffset = new Vector3(3, 1, 0);
		ability2.rotationOffset = new Vector3(0, 90, 0);
		ability2.localScaleOffset = new Vector3(0, 0, 0);
		ability2.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		ability2.targetType = AbilityVO.TARGET_TYPE_OPPONENT;

		AbilityVO ability3 = new AbilityVO();
		ability3.uid= 603;
		ability3.slot = 3;
		ability3.image = "3";
		ability3.name = "Vaporizer";
		ability3.positionRequirement = 0; // <-- 0 = any position
		ability3.targets = 1;
		ability3.prefabId = "";
		ability3.cooldownTotal = 2;
		ability3.cooldown = 0;
		ability3.positionOffset = new Vector3(3, 1, 0);
		ability3.rotationOffset = new Vector3(0, 90, 0);
		ability3.localScaleOffset = new Vector3(0, 0, 0);
		ability3.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		ability3.targetType = AbilityVO.TARGET_TYPE_OPPONENT;

		AbilityVO ability4 = new AbilityVO();
		ability4.uid= 604;
		ability4.slot = 4;
		ability4.image = "4";
		ability4.name = "Vaporizer";
		ability4.positionRequirement = 0; // <-- 0 = any position
		ability4.targets = 1;
		ability4.prefabId = "";
		ability4.cooldownTotal = 2;
		ability4.cooldown = 0;
		ability4.positionOffset = new Vector3(3, 1, 0);
		ability4.rotationOffset = new Vector3(0, 90, 0);
		ability4.localScaleOffset = new Vector3(0, 0, 0);
		ability4.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		ability4.targetType = AbilityVO.TARGET_TYPE_OPPONENT;

		AbilityVO ability5 = new AbilityVO();
		ability5.uid= 605;
		ability5.slot = 5;
		ability5.image = "5";
		ability5.name = "Vaporizer";
		ability5.positionRequirement = 0; // <-- 0 = any position
		ability5.targets = 1;
		ability5.prefabId = "";
		ability5.cooldownTotal = 2;
		ability5.cooldown = 0;
		ability5.positionOffset = new Vector3(3, 1, 0);
		ability5.rotationOffset = new Vector3(0, 90, 0);
		ability5.localScaleOffset = new Vector3(0, 0, 0);
		ability5.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		ability5.targetType = AbilityVO.TARGET_TYPE_OPPONENT;

		AbilityVO[] array = { ability1, ability2, ability3, ability4, ability5 };

		return array;
		*/
	}

	public static AbilityVO[] GetNanomancerAbilities(int slot)
	{
		// default weapon
		AbilityVO weapon = AbilityLibraryVO.DefaultWeapon3();
		weapon.slot = 1;
		weapon.cooldown = 0;

		// weapon mod locked
		AbilityVO weaponMod = AbilityLibraryVO.LockedWeaponMod();
		weaponMod.slot = 2;
		weaponMod.cooldown = 0;

		// gear mod locked
		AbilityVO gearMod = AbilityLibraryVO.LockedGearMod();
		gearMod.slot = 3;
		gearMod.cooldown = 0;

		// default gear
		AbilityVO gear = AbilityLibraryVO.DefaultGear3();
		gear.slot = 4;
		gear.cooldown = 0;

		/*
		AbilityVO ability2 = new AbilityVO();
		ability2.uid= 702;
		ability2.slot = 2;
		ability2.image = "2";
		ability2.name = "Boon X";
		ability2.positionRequirement = 0; // <-- 0 = any position
		ability2.targets = 1;
		ability2.prefabId = "Teleport2 Variant";
		ability2.cooldownTotal = 2;
		ability2.cooldown = 0;
		ability2.positionOffset = new Vector3(0, 0, 0);
		ability2.rotationOffset = new Vector3(0, 0, 0);
		ability2.localScaleOffset = new Vector3(0, 0, 0);
		ability2.actionType = AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_CREW; 
		ability2.targetType = AbilityVO.TARGET_TYPE_COLLEAGUE;

		AbilityVO ability3 = new AbilityVO();
		ability3.uid= 703;
		ability3.slot = 3;
		ability3.image = "3";
		ability3.name = "Vaporizer";
		ability3.positionRequirement = 0; // <-- 0 = any position
		ability3.targets = 1;
		ability3.prefabId = "";
		ability3.cooldownTotal = 2;
		ability3.cooldown = 0;
		ability3.positionOffset = new Vector3(3, 1, 0);
		ability3.rotationOffset = new Vector3(0, 90, 0);
		ability3.localScaleOffset = new Vector3(0, 0, 0);
		ability3.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		ability3.targetType = AbilityVO.TARGET_TYPE_OPPONENT;

		AbilityVO ability4 = new AbilityVO();
		ability4.uid= 704;
		ability4.slot = 4;
		ability4.image = "4";
		ability4.name = "Vaporizer";
		ability4.positionRequirement = 0; // <-- 0 = any position
		ability4.targets = 1;
		ability4.prefabId = "";
		ability4.cooldownTotal = 2;
		ability4.cooldown = 0;
		ability4.positionOffset = new Vector3(3, 1, 0);
		ability4.rotationOffset = new Vector3(0, 90, 0);
		ability4.localScaleOffset = new Vector3(0, 0, 0);
		ability4.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		ability4.targetType = AbilityVO.TARGET_TYPE_OPPONENT;

		AbilityVO ability5 = new AbilityVO();
		ability5.uid= 705;
		ability5.slot = 5;
		ability5.image = "5";
		ability5.name = "Vaporizer";
		ability5.positionRequirement = 0; // <-- 0 = any position
		ability5.targets = 1;
		ability5.prefabId = "";
		ability5.cooldownTotal = 2;
		ability5.cooldown = 0;
		ability5.positionOffset = new Vector3(3, 1, 0);
		ability5.rotationOffset = new Vector3(0, 90, 0);
		ability5.localScaleOffset = new Vector3(0, 0, 0);
		ability5.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		ability5.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		*/

		AbilityVO[] array = { weapon, weaponMod, gearMod, gear };

		return array;
	}

	public static AbilityVO[] GetBlackhatterAbilities(int slot)
	{
		// default weapon
		AbilityVO weapon = AbilityLibraryVO.DefaultWeapon5();
		weapon.slot = 1;
		weapon.cooldown = 0;

		// weapon mod locked
		AbilityVO weaponMod = AbilityLibraryVO.LockedWeaponMod();
		weaponMod.slot = 2;
		weaponMod.cooldown = 0;

		// gear mod locked
		AbilityVO gearMod = AbilityLibraryVO.LockedGearMod();
		gearMod.slot = 3;
		gearMod.cooldown = 0;

		// default gear
		AbilityVO gear = AbilityLibraryVO.DefaultGear5();
		gear.slot = 4;
		gear.cooldown = 0;
		/*AbilityVO ability1 = new AbilityVO();
		ability1.uid= 801;
		ability1.slot = 1;
		ability1.image = "yellow (10)";
		ability1.name = "Vaporizer";
		ability1.positionRequirement = 0; // <-- 0 = any position
		ability1.targets = 1;
		ability1.prefabId = "";
		ability1.cooldownTotal = 2;
		ability1.cooldown = 0;
		ability1.positionOffset = new Vector3(1, 1, 0);
		ability1.rotationOffset = new Vector3(0, 0, 0);
		ability1.localScaleOffset = new Vector3(0, 0, 0);
		ability1.hasFXSettings = true;
		ability1.effectSettings = new Dictionary<string, string>
		{
			["EffectRadius"] = "0.1",
			["test2"] = "val2"
		};
		ability1.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		ability1.targetType = AbilityVO.TARGET_TYPE_OPPONENT;

		AbilityVO ability2 = new AbilityVO();
		ability2.uid= 802;
		ability2.slot = 2;
		ability2.image = "2";
		ability2.name = "Vaporizer";
		ability2.positionRequirement = 0; // <-- 0 = any position
		ability2.targets = 1;
		ability2.prefabId = "";
		ability2.cooldownTotal = 2;
		ability2.cooldown = 0;
		ability2.positionOffset = new Vector3(3, 1, 0);
		ability2.rotationOffset = new Vector3(0, 90, 0);
		ability2.localScaleOffset = new Vector3(0, 0, 0);
		ability2.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		ability2.targetType = AbilityVO.TARGET_TYPE_OPPONENT;

		AbilityVO ability3 = new AbilityVO();
		ability3.uid= 803;
		ability3.slot = 3;
		ability3.image = "yellow (14)";
		ability3.name = "Fear Mirage";
		ability3.positionRequirement = 0; // <-- 0 = any position
		ability3.targets = 1;
		ability3.prefabId = "";
		ability3.cooldownTotal = 2;
		ability3.cooldown = 0;
		ability3.positionOffset = new Vector3(2, 1, 0.5f);
		ability3.rotationOffset = new Vector3(0, 90, 0);
		ability3.localScaleOffset = new Vector3(0, 0, 0);
		ability3.isFloorTriggered = true;
		// ability1.hasFXSettings = true;
		// ability1.effectSettings = new Dictionary<string, string>
		// {
		// 	["EffectRadius"] = "0.1",
		// 	["test2"] = "val2"
		// };
		ability3.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_RANGED;
		ability3.targetType = AbilityVO.TARGET_TYPE_OPPONENT;

		AbilityVO ability4 = new AbilityVO();
		ability4.uid= 804;
		ability4.slot = 4;
		ability4.image = "yellow (15)";
		ability4.name = "Portal";
		ability4.positionRequirement = 0; // <-- 0 = any position
		ability4.targets = 2;
		ability4.prefabId = "REP4_Effect5";
		ability4.cooldownTotal = 2;
		ability4.cooldown = 0;
		ability4.positionXRelativeToPrimaryTarget = true;
		ability4.positionOffset = new Vector3(3.0f, 3, 1.0f);
		ability4.rotationOffset = new Vector3(-45, -65, 0);
		ability4.localScaleOffset = new Vector3(0, 0, 0);
		ability4.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_RANGED;
		ability4.targetType = AbilityVO.TARGET_TYPE_OPPONENT;
		ability4.isFloorTriggered = true;



		AbilityVO ability5 = new AbilityVO();
		ability5.uid= 805;
		ability5.slot = 5;
		ability5.image = "5";
		ability5.name = "Vaporizer";
		ability5.positionRequirement = 0; // <-- 0 = any position
		ability5.targets = 1;
		ability5.prefabId = "";
		ability5.cooldownTotal = 2;
		ability5.cooldown = 0;
		ability5.positionOffset = new Vector3(3, 1, 0);
		ability5.rotationOffset = new Vector3(0, 90, 0);
		ability5.localScaleOffset = new Vector3(0, 0, 0);
		ability5.actionType = AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE;
		ability5.targetType = AbilityVO.TARGET_TYPE_OPPONENT;

		AbilityVO[] array = { ability1, ability2, ability3, ability4, ability5 };*/

		AbilityVO[] array = { weapon, weaponMod, gearMod, gear };

		return array;
	}

}
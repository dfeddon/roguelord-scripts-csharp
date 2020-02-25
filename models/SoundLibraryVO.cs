using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SoundLibraryVO
{
	// public const string ASSET_
	private List<SoundVO> pool = new List<SoundVO>();

	SoundLibraryVO() {}

	// public static SoundVO GetSoundByAsset(string a)
	// {
	// 	foreach(SoundVO s in pool)
	// 	{
	// 		if (s.asset == a)
	// 			return s;
	// 	}

	// 	return null;
	// }

	public static List<SoundVO> CatClaws()
	{
		List<SoundVO> list = new List<SoundVO>();
		SoundVO activate = new SoundVO(100, "Whip Sound 3", SoundVO.ASSET_ACTIVATE_MELEE_SWING);
		SoundVO hit = new SoundVO(101, "Laser Impact Light 1", SoundVO.ASSET_IMPACT_BULLET_8);
		SoundVO block_shield_physical = new SoundVO(102, "Shield Block Physical", SoundVO.ASSET_ACTIVATE_BLOCK_SHIELD_PHYSICAL);
		list.Add(activate);
		list.Add(hit);
		list.Add(block_shield_physical);

		return list;
	}

	public static SoundVO GetCrit(int crewType)
	{
		if (crewType == CharacterVO.CREW_TYPE_DEFENDER)
		{
			// yes!
			int rng = UnityEngine.Random.Range(0, 3);
			Debug.Log("<color=purple>*** " + rng + "</color>");
			switch(rng)
			{
				case 0: return new SoundVO(1000, "Crit!", "BarbarianVP_YesHard");
				case 1: return new SoundVO(1000, "Crit!", "BarbarianVP_YesHard");
				case 2: return new SoundVO(1001, "Crit2!", "BarbarianVP_NoMercySoft");
			}
		}
		else
		{
			// no!
			int rng = UnityEngine.Random.Range(0, 3);
			Debug.Log("*** " + rng);
			switch (rng)
			{
				case 0: return new SoundVO(1002, "No!", "BarbarianVP_No(Long)Medium");
				case 1: return new SoundVO(1003, "No2!", "BarbarianVP_NoHard2");
				case 2: return new SoundVO(1005, "Ahh!", "BarbarianVP_AaahHard");
			}
		}
		return null;
	}
	public static SoundVO GetBleed(int crewType)
	{
		if (crewType == CharacterVO.CREW_TYPE_DEFENDER)
		{
			// yes!
			int rng = UnityEngine.Random.Range(0, 3);
			switch (rng)
			{
				case 0: return new SoundVO(1000, "Bleed Soft", "BarbarianVP_BleedSoft");
				case 1: return new SoundVO(1000, "Bleed Soft", "BarbarianVP_BleedSoft");
				case 2: return new SoundVO(1001, "Bleed Medium", "BarbarianVP_BleedMedium");
			}
		}
		else
		{
			// no!
			int rng = UnityEngine.Random.Range(0, 4);
			// Debug.Log("*** " + rng);
			switch (rng)
			{
				case 0: return new SoundVO(1000, "Bleed Soft", "BarbarianVP_BleedSoft");
				case 1: return new SoundVO(1000, "Bleed Soft", "BarbarianVP_BleedSoft");
				case 2: return new SoundVO(1004, "Help Me", "BarbarianVP_HelpMeSoft");
				case 3: return new SoundVO(1005, "Help Me", "Undead_Dialogue_Vocal_Help_Me");
			}
		}
		return null;
	}

	public static SoundVO GetBurn(int crewType)
	{
		if (crewType == CharacterVO.CREW_TYPE_DEFENDER)
		{
			// yes!
			int rng = UnityEngine.Random.Range(0, 3);
			switch (rng)
			{
				case 0: return new SoundVO(1000, "Burn Soft", "BarbarianVP_BurnSoft");
				case 1: return new SoundVO(1000, "Burn Soft", "BarbarianVP_BurnSoft");
				case 2: return new SoundVO(1001, "Burn Hard", "BarbarianVP_BurnHard");
			}
		}
		else
		{
			// no!
			int rng = UnityEngine.Random.Range(0, 3);
			Debug.Log("*** " + rng);
			switch (rng)
			{
				case 0: return new SoundVO(1000, "Burn Soft", "BarbarianVP_BurnSoft");
				case 1: return new SoundVO(1001, "Burn Soft", "BarbarianVP_BurnSoft");
				case 2: return new SoundVO(1004, "Help Me", "BarbarianVP_HelpMeSoft");
			}
		}
		return null;
	}

	public static SoundVO GetMusic()
	{
		return new SoundVO(1002, "Vengeance", "Vengeance (Loop)");
	}

	public static SoundVO GetUIFX(string type)
	{
		switch(type)
		{
			case "attack-increase": return new SoundVO(1000, "Attack Increase", SoundVO.ASSET_UI_ATTACK_INCREASE);
			case "defense-increase": return new SoundVO(1001, "Defense Increase", SoundVO.ASSET_UI_DEFENSE_INCREASE);
			// case "attack-decrease": return new SoundVO(1002, "Attack Decrease", )
		}

		return null;
	}
}
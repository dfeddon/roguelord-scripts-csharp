using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public static class SoundMatrixVO
{
	// private static List<SoundVO> pool;

	public static SoundVO GetSoundById(int id)
	{
		foreach(SoundVO s in PoolSounds())
		{
			if (s.uid == id)
				return s;
		}

		return null;
	}

	public static SoundVO GetSoundByAsset(string a)
	{
		foreach(SoundVO s in PoolSounds())
		{
			if (s.asset == a)
				return s;
		}

		return null;
	}

	private static List<SoundVO> PoolSounds()
	{
		List<SoundVO> list = new List<SoundVO>();

		list.AddRange(GetAnarchistSounds());
		// list.AddRange(GetChromerAbilities(0));
		// list.AddRange(GetShamanAbilities(0));
		// list.AddRange(GetMedicAbilities(0));
		// list.AddRange(GetEngineerAbilities(0));
		// list.AddRange(GetSaboteurAbilities(0));
		// list.AddRange(GetNanomancerAbilities(0));
		// list.AddRange(GetBlackhatterAbilities(0));

		return list;
	}

	public static SoundVO[] GetAnarchistSounds()
	{
		// Cat Claws (Melee)
		List<SoundVO> ability1 = SoundLibraryVO.CatClaws();

		SoundVO[] array = { ability1[0], ability1[1], ability1[2] };

		return array;
	}

	public static SoundVO[] GetShamanSounds()
	{
		return null;
	}

}
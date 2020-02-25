using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System;

//In order to use a collection's Sort() method, this class needs to implement the IComparable interface.
public class RngRoller
{
	public const int RNG_BOOL = 1;
	public const int RNG_DICE_2 = 2;
	public const int RNG_DICE_4 = 3;
	public const int RNG_DICE_6 = 4;
	public const int RNG_DICE_8 = 5;
	public const int RNG_DICE_12 = 6;
	public const int RNG_DICE_20 = 7;

	private int dice;
	private int rolls;

	public RngRoller(int _dice, int _rolls = 1)
	{
		dice = _dice;
		rolls = _rolls;
	}

	public int RollTheDice()//int dice, int rolls = 1)
	{
		int result = 0;

		switch(dice)
		{
			case RNG_BOOL:
			case RNG_DICE_2:
				for (var i = 0; i < rolls; i++)
					result += Random.Range(1, 2);
			break;

			case RNG_DICE_4:
				for (var i = 0; i < rolls; i++)
					result += Random.Range(1, 4);
			break;

			case RNG_DICE_6:
				for (var i = 0; i < rolls; i++)
					result += Random.Range(1, 6);
			break;

			case RNG_DICE_8:
				for (var i = 0; i < rolls; i++)
					result += Random.Range(1, 8);
			break;

			case RNG_DICE_12:
				for (var i = 0; i < rolls; i++)
					result += Random.Range(1, 12);
			break;

			case RNG_DICE_20:
				for (var i = 0; i < rolls; i++)
					result += Random.Range(1, 20);
			break;
		}

		return result;
	}
}
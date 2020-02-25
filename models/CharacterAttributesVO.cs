using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CharacterAttributesVO
{
	public int uid; // <- uid is same as parent (CharacterVO.uid)

	public int reflexes;
	public int talent;
	public int grit;
	public int precision;
	public int detox;

	public int powerMax; // maximum
	public int powerPool; // true value
	public int powerMod; // modification
	private int _power; // printable
	public int attackMax;
	public int attackPool;
	public int attackMod;
	private int _attack;
	public int defenseMax;
	public int defensePool;
	public int defendMod;
	private int _defense;

	public CharacterAttributesVO(int _uid, int _reflexes = 0, int _talent = 0, int _grit = 0, int _detox = 0)
	{
		uid = _uid;

		if (_reflexes > 0)
		{
			reflexes = _reflexes;
		}
		else reflexes = GetRandom();

		if (_talent > 0)
		{
			talent = _talent;
		}
		else talent = GetRandom();
		attackPool = talent * 2;
		attack = attackPool;
		attackMax = talent * 4;

		if (_grit > 0)
		{
			grit = _grit;
		}
		else grit = GetRandom();
		defensePool = grit * 2;
		defense = defensePool;
		defenseMax = grit * 4;

		if (_detox > 0)
		{
			detox = _detox;
		}
		else detox = GetRandom();

		// power
		powerMax = 3;
		powerPool = 3;
		_power = 3;
	}

	private int GetRandom(int a = 1, int b = 10)
	{
		return Random.Range(a, b);
	}

	private void Randomize()
	{
		uid = Random.Range(1, 10);
		reflexes = Random.Range(1, 10);
		talent = Random.Range(1, 10);
		grit = Random.Range(1, 10);
		precision = Random.Range(1, 10);
		detox = Random.Range(1, 10);
	}

	// Getters & Setters
	public int attack
	{
		get { return _attack; }
		set
		{
			Debug.Log("* attack has changed to " + value);
			
			if (_attack == 0 && value <= 0) return;

			// get change val
			int diff = 0;
			if (value > _attack)
				diff = value - _attack;
			else diff = -value - _attack;

			// value cannot go below 0
			if (value < 0)
				value = 0;
			_attack = value;
			EventParam eventParam = new EventParam();
			eventParam.name = "attackUpdate";
			eventParam.value = uid; // value is source uid
			eventParam.value2 = value;
			eventParam.value3 = diff;
			EventManager.TriggerEvent("characterEvent", eventParam);
		}
	}
	public int defense
	{
		get { return _defense; }
		set
		{
			Debug.Log("* defense has changed to " + value);

			if (_defense == 0 && value <= 0) return;

			// get change val
			var diff = 0;
			if (value > _defense)
				diff = value - _defense;
			else diff = -value - _defense;

			// value cannot go below 0
			if (value < 0)
				value = 0;
			_defense = value;
			EventParam eventParam = new EventParam();
			eventParam.name = "defenseUpdate";
			eventParam.value = uid; // value is source uid
			eventParam.value2 = value;
			eventParam.value3 = diff;
			EventManager.TriggerEvent("characterEvent", eventParam);
		}
	}

	public int power
	{
		get { return _power; }
		set 
		{
			Debug.Log("* power has changed to " + value);
			// get change val
			var diff = 0;
			if (value > _power)
				diff = value - _power;
			else diff = -value - _power;
			// value cannot exceed player power maximum
			// value cannot go below 0
			if (value < 0)
				value = 0;
			_power = value;

			EventParam eventParam = new EventParam();
			eventParam.name = "powerUpdate";
			eventParam.value = uid; // value is source uid
			eventParam.value2 = value;
			eventParam.value3 = diff;
			EventManager.TriggerEvent("characterEvent", eventParam);


		}
	}

}

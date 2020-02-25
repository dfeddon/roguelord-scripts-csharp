using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CardVO
{
	public enum CardState { None, Dealer, InHand, Activated, DrawPile, DiscardPile, ExhaustPile, Menu };
	public enum CardOwner { Attacker, Defender };
	public enum CardType { None, Glitch, Virus, Sprawl, Tactics, Weapon, Gear, Mod, Archetype }
	public enum CardTech { None, ElectroChemical, Nanotech, Augmentation, Hacking }
	public enum CardTechType { None, Offense, Defense, Attack, Wild }
	public enum TargetType { None, Protos, Cards, Character, Pile, Ability }
	public enum TargetSource { None, Echem, Nanotech, Augmentation, Hacking, Opponent, Self, Colleague, DrawPile, DiscardPile, ExhaustPile }


	public int uid;
	public string name;
	public string label;
	public string description;
	public string image;
	public int cost;
	public int level = 1;
	public int levelsTotal = 3;
	public int levelItemsTotal = 1;
	public List<CardLevelDataVO> levelData = new List<CardLevelDataVO>(); // up to three levels
	public CardTech tech = CardTech.None;
	public CardTechType techType = CardTechType.None;
	public List<CardEffectVO> cardEffects = new List<CardEffectVO>();
	public CardState state = CardState.Dealer;
	public CardOwner owner;
	public CardType type;
	public TargetType targetType = TargetType.None;
	public TargetSource targetSource = TargetSource.None;
	public List<CardConditionalVO> conditions;// = new List<CardConditionalVO>();
	// public int cardIndex = -1;

	public CardVO()
	{

	}

}

public class CardLevelDataVO
{
	public enum CardActionType { None, Attack, Defense, Draw, Discard, Exhaust, Duplicate, AddRandom }
	public CardActionType actionType1 = CardActionType.None;
	public CardActionType actionType2 = CardActionType.None;
	public int value1 = 0;
	public int value2 = 0;
	public string text1 = "";
	public string text2 = "";

	public CardLevelDataVO(CardActionType _actionType1, int _value1, string _text1, CardActionType _actionType2, int _value2 = 0, string _text2 = "")
	{
		actionType1 = _actionType1;
		value1 = _value1;
		text1 = _text1;
		if (_value2 != 0)
		{
			actionType2 = _actionType2;
			value2 = _value2;
			text2 = _text2;
		}
	}
}

public class CardConditionalVO
{
	public enum ConditionCheck { None, Activation, Trigger, EveryTurn, EveryRound }
	// if pre-check fails, card will NOT activate
	// post-check can fail silently
	public enum ConditionActionLocation { None, PreAction, PostAction }
	public enum ConditionTrigger { None, Effect, Health, Stats, Ability, Card }
	public enum ConditionSubject { None, Protos, Card, Character, Pile, Ability, Effect }
	public enum ConditionCharacterType { None, AllOpponents, AnyOpponent, AllColleagues, AnyColleague, Source, Target }
	public enum ConditionEval { None, HasEffect, LosesHealth, DeliversKillshot, All, EqualTo, GreaterThan, LessThan, Gains }
	public enum ConditionObject { None, Ability, Effect }

	public ConditionCheck conditionCheck = ConditionCheck.None;
	public ConditionTrigger conditionTrigger = ConditionTrigger.None;
	public ConditionActionLocation conditionActionLocation = ConditionActionLocation.None;
	public ConditionSubject conditionSubject = ConditionSubject.None;
	public ConditionCharacterType conditionCharacterType = ConditionCharacterType.None;
	public ConditionEval conditionEval = ConditionEval.None;
	public ConditionObject conditionObject = ConditionObject.None;
	public int conditionObjectUid;

	public CardConditionalVO()
	{

	}

	public bool Validation()
	{
		bool returnValue = false;



		return returnValue;
	}

}

public class CardStoreVO
{
	public int uid = 0;
	public int level = 1;
	public bool isDealer = false;

	public CardStoreVO(int _uid = 0, int _level = 1, bool _isDealer = false)
	{
		uid = _uid;
		level = _level;
		isDealer = _isDealer;
	}
}

// public class GenericTarget<T>
// {
// 	T item;

// 	public void SetItem(T newItem) {
// 		{
// 			item = newItem;
// 		}
// 	}
// }

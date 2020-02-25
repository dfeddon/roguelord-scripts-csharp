using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CardLibraryVO
{
	public const string COLOR_CARDTYPE = "#063B00";
	public const string COLOR_DAMAGE = "#8B0000";

	public static CardVO GetCardByStore(CardStoreVO store)
	{
		CardVO card = null;
		switch(store.uid)
		{
			case 100: card = getEvasiveManeuversCard(); break;
			case 101: card = getChargedUpCard(); break;
			case 102: card = getExposedCard(); break;
			case 103: card = getTotemicFirebrand(); break;
		}
		if (card != null)
			card.level = store.level;
		return card;
	}
	public static CardVO getEvasiveManeuversCard()
	{
		CardVO card = new CardVO();
		card.uid = 100;
		card.state = CardVO.CardState.Dealer;
		card.type = CardVO.CardType.Sprawl;
		card.tech = CardVO.CardTech.ElectroChemical;
		card.targetType = CardVO.TargetType.Character;
		card.targetSource = CardVO.TargetSource.Self;
		card.cost = 1;
		card.level = 1;
		card.levelsTotal = 3;
		card.levelItemsTotal = 2;
		card.name = "Evasive Maneuvers";
		card.description = "Gain :levelstring1:.\n:levelstring2:.";
		card.image = "violet (3) 1";
		card.levelData = new List<CardLevelDataVO>(new[]
		{
			new CardLevelDataVO(
				CardLevelDataVO.CardActionType.Defense, 
				5, 
				"<color=#063B00><b>5 DEFENSE</b></color>", 
				CardLevelDataVO.CardActionType.AddRandom, 
				1, 
				"<color=#003366><b>DRAW</b> 1</color> Card"
			),
			new CardLevelDataVO(
				CardLevelDataVO.CardActionType.Defense, 
				10, 
				"<color=#063B00><b>10 DEFENSE</b></color>", 
				CardLevelDataVO.CardActionType.AddRandom,
				2, 
				"<color=#003366><b>DRAW</b> 1</color> Card"
			),
			new CardLevelDataVO(
				CardLevelDataVO.CardActionType.Defense, 
				15, 
				"<color=#063B00><b>15 DEFENSE</b></color>", 
				CardLevelDataVO.CardActionType.AddRandom,
				3, 
				"<color=#003366><b>DRAW</b> 1</color> Card"
			)
		});
		return card;
	}
	public static CardVO getChargedUpCard()
	{
		CardVO card = new CardVO();
		card.uid = 101;
		card.state = CardVO.CardState.Dealer;
		card.type = CardVO.CardType.Sprawl;
		card.tech = CardVO.CardTech.ElectroChemical;
		card.targetType = CardVO.TargetType.Character;
		card.targetSource = CardVO.TargetSource.Self;
		card.cost = 1;
		card.level = 1;
		card.levelsTotal = 3;
		card.levelItemsTotal = 2;
		card.name = "Charged Up";
		card.description = "Gain :levelstring1:.\n:levelstring2:.";
		card.image = "Archerskill_07";
		card.levelData = new List<CardLevelDataVO>(new[]
		{
			new CardLevelDataVO(
				CardLevelDataVO.CardActionType.Attack,
				5, 
				"<color=#8B0000><b>5 ATTACK</b></color>",
				CardLevelDataVO.CardActionType.AddRandom,
				1, 
				"<color=#003366><b>DRAW</b> 1</color> Card"
			),
			new CardLevelDataVO(
				CardLevelDataVO.CardActionType.Attack,
				10, 
				"<color=#8B0000><b>10 ATTACK</b></color>",
				CardLevelDataVO.CardActionType.AddRandom,
				2, 
				"<color=#003366><b>DRAW</b> 2</color> Cards"
			),
			new CardLevelDataVO(
				CardLevelDataVO.CardActionType.Attack,
				15, 
				"<color=#8B0000><b>15 ATTACK</b></color>",
				CardLevelDataVO.CardActionType.AddRandom,
				3, 
				"<color=#003366><b>DRAW</b> 3</color> Cards"
			)
		});

		return card;
	}
	public static CardVO getExposedCard()
	{
		CardVO card = new CardVO();
		card.uid = 102;
		card.state = CardVO.CardState.Dealer;
		card.type = CardVO.CardType.Sprawl;
		card.tech = CardVO.CardTech.ElectroChemical;
		card.targetType = CardVO.TargetType.Character;
		card.targetSource = CardVO.TargetSource.Self;
		card.cost = 2;
		card.level = 1;
		card.levelsTotal = 3;
		card.levelItemsTotal = 2;
		card.name = "Surveillance";
		card.description = "Gain :levelstring1:.\n:levelstring2:.";
		card.image = "Druideskill_48";
		card.levelData = new List<CardLevelDataVO>(new[]
		{
			new CardLevelDataVO(
				CardLevelDataVO.CardActionType.Attack,
				5,
				"<color=#8B0000><b>5 ATTACK</b></color>",
				CardLevelDataVO.CardActionType.AddRandom,
				1,
				"<color=#003366><b>DRAW</b> 1</color> Card"
			),
			new CardLevelDataVO(
				CardLevelDataVO.CardActionType.Attack,
				10,
				"<color=#8B0000><b>10 ATTACK</b></color>",
				CardLevelDataVO.CardActionType.AddRandom,
				2,
				"<color=#003366><b>DRAW</b> 2</color> Cards"
			),
			new CardLevelDataVO(
				CardLevelDataVO.CardActionType.Attack,
				15,
				"<color=#8B0000><b>15 ATTACK</b></color>",
				CardLevelDataVO.CardActionType.AddRandom,
				3,
				"<color=#003366><b>DRAW</b> 3</color> Cards"
			)
		});

		return card;
	}
	public static CardVO getTotemicFirebrand()
	{
		CardVO card = new CardVO();
		card.uid = 103;
		card.state = CardVO.CardState.Dealer;
		card.type = CardVO.CardType.Sprawl;
		card.tech = CardVO.CardTech.ElectroChemical;
		card.targetType = CardVO.TargetType.Character;
		card.targetSource = CardVO.TargetSource.Self;
		card.cost = 2;
		card.level = 1;
		card.levelsTotal = 2;
		card.name = "Totemic Firebrand";
		card.description = "Can only be played if every card in your hand is an <color=#063b00><b>Attack</b></color>. Deal <color=#8b0000><b>14</b> Damage</color>.";
		card.image = "Shamanskill_15";
		card.conditions = new List<CardConditionalVO>
		{
			new CardConditionalVO {
				conditionCheck = CardConditionalVO.ConditionCheck.Activation, // on cast
				conditionTrigger = CardConditionalVO.ConditionTrigger.None, // no triggers
				conditionActionLocation = CardConditionalVO.ConditionActionLocation.PreAction, // before cast check
				conditionSubject = CardConditionalVO.ConditionSubject.Character, // evaluating character
				conditionCharacterType = CardConditionalVO.ConditionCharacterType.Source, // caster
				conditionObject = CardConditionalVO.ConditionObject.Effect, // has effect
				conditionEval = CardConditionalVO.ConditionEval.EqualTo, // equal to
				conditionObjectUid = 305 // <- totem id
			},
			new CardConditionalVO {
				conditionCheck = CardConditionalVO.ConditionCheck.Activation, // on cast
				conditionTrigger = CardConditionalVO.ConditionTrigger.None, // no triggers
				conditionActionLocation = CardConditionalVO.ConditionActionLocation.PostAction, // after cast check
				conditionSubject = CardConditionalVO.ConditionSubject.Character, // evaluating character
				conditionCharacterType = CardConditionalVO.ConditionCharacterType.AnyOpponent, // any opponent
				conditionObject = CardConditionalVO.ConditionObject.Effect, // has effect
				conditionEval = CardConditionalVO.ConditionEval.EqualTo, // equal to
				conditionObjectUid = 102 // <- burning
			}
		};

		return card;
	}
	public static CardVO getDoublePlayCard()
	{
		CardVO card = new CardVO();
		card.uid = 104;
		card.state = CardVO.CardState.Dealer;
		card.type = CardVO.CardType.Sprawl;
		card.tech = CardVO.CardTech.None;
		card.targetType = CardVO.TargetType.None;
		card.targetSource = CardVO.TargetSource.None;
		card.cost = 2;
		card.level = 1;
		card.levelsTotal = 3;
		card.levelItemsTotal = 2;
		card.name = "Double Play";
		card.description = "Draw :levelstring1:.\n:levelstring2:.";
		card.image = "Druideskill_48";
		card.levelData = new List<CardLevelDataVO>(new[]
		{
			new CardLevelDataVO(
				CardLevelDataVO.CardActionType.Attack,
				5,
				"<color=#8B0000><b>5 ATTACK</b></color>",
				CardLevelDataVO.CardActionType.AddRandom,
				1,
				"<color=#003366><b>DRAW</b> 1</color> Card"
			),
			new CardLevelDataVO(
				CardLevelDataVO.CardActionType.Attack,
				10,
				"<color=#8B0000><b>10 ATTACK</b></color>",
				CardLevelDataVO.CardActionType.AddRandom,
				2,
				"<color=#003366><b>DRAW</b> 2</color> Cards"
			),
			new CardLevelDataVO(
				CardLevelDataVO.CardActionType.Attack,
				15,
				"<color=#8B0000><b>15 ATTACK</b></color>",
				CardLevelDataVO.CardActionType.AddRandom,
				3,
				"<color=#003366><b>DRAW</b> 3</color> Cards"
			)
		});

		return card;
	}
	public static CardVO getWeaponKnifeCard()
	{
		CardVO card = new CardVO();
		card.uid = 200;
		card.state = CardVO.CardState.Dealer;
		card.type = CardVO.CardType.Sprawl;
		card.tech = CardVO.CardTech.ElectroChemical;
		card.targetType = CardVO.TargetType.Character;
		card.targetSource = CardVO.TargetSource.Self;
		card.cost = 2;
		card.level = 1;
		card.levelsTotal = 3;
		card.levelItemsTotal = 2;
		card.name = "Surveillance";
		card.description = "Gain :levelstring1:.\n:levelstring2:.";
		card.image = "Druideskill_48";
		card.levelData = new List<CardLevelDataVO>(new[]
		{
			new CardLevelDataVO(
				CardLevelDataVO.CardActionType.Attack,
				5,
				"<color=#8B0000><b>5 ATTACK</b></color>",
				CardLevelDataVO.CardActionType.AddRandom,
				1,
				"<color=#003366><b>DRAW</b> 1</color> Card"
			),
			new CardLevelDataVO(
				CardLevelDataVO.CardActionType.Attack,
				10,
				"<color=#8B0000><b>10 ATTACK</b></color>",
				CardLevelDataVO.CardActionType.AddRandom,
				2,
				"<color=#003366><b>DRAW</b> 2</color> Cards"
			),
			new CardLevelDataVO(
				CardLevelDataVO.CardActionType.Attack,
				15,
				"<color=#8B0000><b>15 ATTACK</b></color>",
				CardLevelDataVO.CardActionType.AddRandom,
				3,
				"<color=#003366><b>DRAW</b> 3</color> Cards"
			)
		});

		return card;
	}

}
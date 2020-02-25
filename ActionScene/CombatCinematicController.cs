using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatCinematicController//: MonoBehaviour
{
	private CharacterView source;
	private List<CharacterView> targets;
	private int actionType;
	private AbilityVO actionValue;
	private GameObject ability;

	// void Awake()
	// {
	// 	GameManager.instance.combatCinematicController = this;
	// }

	// void Start()
	// {
	// 	GameManager.instance.combatCinematicController = this;
	// }

	public void addActors(CharacterView _source, List<CharacterView> _targets, AbilityVO _actionValue)
	{
		Debug.Log("<color=yellow>== CombatCinematicController.addActors ==</color>");
		Debug.Log("* source " + _source.model.handle);
		Debug.Log("* targets " + _targets);
		// Debug.Log("* actionType " + _actionType);
		Debug.Log("* actionValue " + _actionValue);

		GameManager.instance.combatCinematicController = this;

		source = _source;
		targets = _targets; // if no targets, then target is self?
		// actionType = _actionType; // 1 = ability type (ranged, melee, CC), 2 = movement
		actionValue = _actionValue; // ability # or movement direction

		// get ability go
		AbilityVO ability = _actionValue;// AbilityVO.getAbilityByCharacterRole(source.model.role, actionValue);
		Debug.Log(ability + " / " + ability.actionType);

		// assign ability to characters
		source.activeAbility = ability;
		foreach (CharacterView i in targets)
		{
			// TODO: if floor dictates character hits, set targets refs to floorcontroller?
			i.activeAbility = ability;
		}

		switch(ability.actionType)
		{
			case AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_RANGED:
				Debug.Log("== action type is offense...");
				// // get ability go
				// AbilityVO ability = AbilityVO.getAbilityByCharacterRole(source.model.role, 3);
				// source.charController.ActivateEffect(actionValue - 1);
				Debug.Log("* ability = " + ability.prefabId);
				// get sequence (ienumerator)
				EnumeratorOffenseRanged offenseRanged = new GameObject().AddComponent<EnumeratorOffenseRanged>();
				offenseRanged.Go(source, targets, ability);//.prefabId);
			break;

			case AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_PROJECTILE:
			case AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_MELEE:
				Debug.Log("== action type is offense projectile OR melee " + ability.actionType);
				// get sequence (ienumerator)
				EnumeratorOffenseRanged offenseProjectile = new GameObject().AddComponent<EnumeratorOffenseRanged>();
				offenseProjectile.Go(source, targets, ability);
			break;

			case AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_CREW:
				Debug.Log("== action type is defense crew...");
				// get ability go based on character class => actionValue
				EnumeratorDefenseCrew defenseCrew = new GameObject().AddComponent<EnumeratorDefenseCrew>();
				defenseCrew.Go(source, targets, ability);
			break;
			
			case AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SELF:
			case AbilityVO.ABILITY_ACTIONTYPE_DEFENSE_SIDECAR:
				Debug.Log("== action type is defense self...");
				EnumeratorDefenseCrew defenseSelf = new GameObject().AddComponent<EnumeratorDefenseCrew>();
				defenseSelf.Go(source, targets, ability);
			break;

			case AbilityVO.ACTION_TYPE_REPOSITIONING:
				Debug.Log("== action type is repositioning...");
			break;
		}
	}

	public void FloorCollisionHandler()
	{
		foreach (CharacterView i in targets)
		{
			// TODO: if floor dictates character hits, set targets refs to floorcontroller?
			// i.activeAbility = ability;
			i.isHit();
		}
	}
}
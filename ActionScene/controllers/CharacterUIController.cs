using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Roguelord
{
	public class CharacterUIController : MonoBehaviour
	{
		private GameObject canvas;
		private GameObject container;
		private GameObject weaponSlot;
		private GameObject weaponModSlot;
		private GameObject gearSlot;
		private GameObject gearModSlot;

		private void Awake() 
		{
			Debug.Log("CharacterUIController Awake");

			canvas = GameObject.FindWithTag("CharacterUI");
			// container = canvas.transform.GetChild(0).transform.GetChild(0).gameObject;
			// weaponSlot = container.transform.GetChild(0).gameObject;
			// weaponModSlot = container.transform.GetChild(1).gameObject;
			// gearSlot = container.transform.GetChild(2).gameObject;
			// gearModSlot = container.transform.GetChild(3).gameObject;
		}

		private void Start() {
			Debug.Log("CharacterUIController Start");
		}
		public void BuildAbilityBar(CharacterVO c)
		{
			Debug.Log("== CharacterUIController.BuildAbilityBar ==");
			// ShowAll(true);

			// AbilityVO[] characterAbilities = AbilityMatrixVO.GetAbilitiesByClass(c.role);
			List<AbilityVO> characterAbilities = AbilityMatrixVO.GetCharacterAbilities(c);
			GameObject[] abilities = GameObject.FindGameObjectsWithTag("AbilityButtonView");
			AbilityImageView abilityImageView;
			Sprite sprite;
			Image image;
			foreach (GameObject ability in abilities)
			{
				abilityImageView = ability.GetComponent<AbilityImageView>();

				// ignore global, hard-coded Move ability (slot #0)
				if (abilityImageView.abilityNumber == 7) continue;
				// TODO: Temp fix below
				if ((abilityImageView.abilityNumber) > characterAbilities.Count) continue;
				// TODO: End fix
				Debug.Log("* " + (abilityImageView.abilityNumber - 1).ToString() + " / " + ability.name);
				abilityImageView.abilityVO = characterAbilities[abilityImageView.abilityNumber - 1];
				sprite = GameManager.instance.assetBundleCombat.LoadAsset<Sprite>(characterAbilities[abilityImageView.abilityNumber - 1].image);
				image = ability.transform.GetChild(0).GetComponent<Image>();
				image.sprite = sprite;
				// Debug.Log("<color=orange>AbilityImageView " + abilityImageView.abilityNumber + "</color>");

				// first, validate positioning
				bool isValidPosition = c.ValidatePositioning(abilityImageView.abilityVO.positionRequirement);
				abilityImageView.IsValidPosition(isValidPosition);
				// if out of position, disregard cooldowns
				if (isValidPosition == false) continue;

				// next, cooldowns
				foreach (AbilityCooldownVO cd in c.abilityCooldowns)
				{
					if (cd.uid == abilityImageView.abilityVO.uid)
					{
						if (cd.cooldownCounter > 0)
						{
							abilityImageView.IsOnCooldown(cd, true);
						}
						else
						{
							abilityImageView.IsOnCooldown(cd, false);
						}
					}
				}

			}
		}

	}
}
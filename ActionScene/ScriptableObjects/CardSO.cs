using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardSO: ScriptableObject
{
	public new string name = "";
	public string description;
	public Sprite image;
	public int cost;
	public int attack;
	public int defense;
}
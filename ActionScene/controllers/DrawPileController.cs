using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DrawPileController : MonoBehaviour {
	
	public enum DrawPileOwner { Attacker, Defender }

	public DrawPileOwner owner;
	private Image drawPileImage;

	public DrawPileController() {}

	private void Awake() 
	{
		// if (owner == DrawPileOwner.Attacker)
		drawPileImage = transform.GetChild(0).GetComponent<Image>();
	}
}
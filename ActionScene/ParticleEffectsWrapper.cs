using UnityEngine;
using System;
// using System.Collections;
// using System.Collections.Generic;

public class ParticleEffectsWrapper: MonoBehaviour
{
	public bool doDestroy = true;
	public GameObject wrapper;
	private ParticleSystem ps;
	public delegate void TestDelegate();
	public EffectItemVO effectItem;
	public TestDelegate methodToCall;
	// private UnityEngine.ParticleSystem.MainModule main;

	void Start()
	{
		Debug.Log("<color=blue>========== Start ===========</color>");
		ps = GetComponent<ParticleSystem>();
		ps.Clear();
		ps.Play();
		var main = ps.main;
		main.stopAction = ParticleSystemStopAction.Callback;
		if (effectItem.autoDelete == true)
			main.loop = false;
	}

	public void AddItem(EffectItemVO item)
	{
		effectItem = item;
		effectItem.timeStart = Time.fixedTime;
		// var main = ps.main;
		// if (effectItem.autoDelete == true)
		// 	main.loop = false;
	}
	public void SetDelegateFunc(TestDelegate handler)
	{
		methodToCall = handler;
	}
	public void OnParticleSystemStopped()
		{
			Debug.Log("<color=blue>========== Stop ===========</color>");
			effectItem.timeEnd = Time.fixedTime;
			// if (doDestroy == true)
			if (effectItem.autoDelete == true)
			{
				methodToCall();
				Destroy(this);
			}

			// transform.parent = null;
			// particleSystem.main.stopAction = ParticleSystemStopAction.Destroy;
			// Destroy(particleSystem);
			// Destroy(GetComponent<ParticleSystem>().main);
		}
}
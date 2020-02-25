using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Roguelord;
// namespace System
// {
// 	public delegate void UnityAction();
// }
public class EventManager : MonoBehaviour
{

	private Dictionary<string, UnityAction<EventParam>> eventDictionary;

	private static EventManager eventManager;

	public static EventManager instance
	{
		get
		{
			if (!eventManager)
			{
				eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

				if (!eventManager)
				{
					Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
				}
				else
				{
					eventManager.Init();
				}
			}
			return eventManager;
		}
	}

	void Init()
	{
		if (eventDictionary == null)
		{
			eventDictionary = new Dictionary<string, UnityAction<EventParam>>();
		}
	}

	public static void StartListening(string eventName, UnityAction<EventParam> listener)
	{
		UnityAction<EventParam> thisEvent;
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
		{
			//Add more event to the existing one
			thisEvent += listener;

			//Update the Dictionary
			instance.eventDictionary[eventName] = thisEvent;
		}
		else
		{
			//Add event to the Dictionary for the first time
			thisEvent += listener;
			instance.eventDictionary.Add(eventName, thisEvent);
		}
	}

	public static void StopListening(string eventName, UnityAction<EventParam> listener)
	{
		if (eventManager == null) return;
		UnityAction<EventParam> thisEvent;
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
		{
			//Remove event from the existing one
			thisEvent -= listener;

			//Update the Dictionary
			instance.eventDictionary[eventName] = thisEvent;
		}
	}

	public static void TriggerEvent(string eventName, EventParam eventParam)
	{
		UnityAction<EventParam> thisEvent = null;
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
		{
			thisEvent.Invoke(eventParam);
			// OR USE  instance.eventDictionary[eventName](eventParam);
		}
	}
}

//Re-usable structure/ Can be a class to. Add all parameters you need inside it
public struct EventParam
{
	public object data;
	public int value;
	public int value2;
	public int value3;
	public string name;
	public string message;
	public CardView card;
	public CardVO.CardOwner owner;
	public CharacterVO character;
	public AbilityVO ability;
	public List<EffectVO> effectsList;
	public GameObject gameObject;
	public object[] array;
	// public string param1;
	// public int param2;
	// public float param3;
	// public bool param4;
}
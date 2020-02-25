using UnityEngine;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Driver;

[System.Serializable]
public class CrewVO
{
	// public ObjectId id;
	public object role;
	public object position;
	public bool isCombat;
	// public TacticsVO[] tactics;

	// public CharacterVO character;
}

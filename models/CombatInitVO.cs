using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson;

[System.Serializable]
public class CombatInitVO
{
	// public CrewsVO[] crews;
	public List<CrewsVO> crews = new List<CrewsVO>();
	public DealerCardVO dealer;
	// public string name;
	// public int lives;
	// public float health;

	public static CombatInitVO CreateFromJSON(string jsonString)
	{
		Debug.Log(jsonString);
		// string str = JsonUtility.FromJson<CombatInitVO>(jsonString);
		// Debug.Log(js);
		return JsonUtility.FromJson<CombatInitVO>(jsonString);
	}

	// public CombatInitVO(Bson response)
	// {
	// 	Debug.Log(response);
	// 	for (int i = 0; i < response.crews.Length; i++)
	// 		crews.Add(response[i] as CrewVO);
	// 	Debug.Log("HI");
	// }

	// Given JSON input:
	// {"name":"Dr Charles","lives":3,"health":0.8}
	// this example will return a PlayerInfo object with
	// name == "Dr Charles", lives == 3, and health == 0.8f.
}

/*[System.Serializable]
public struct MyObject
{
   [System.Serializable]
   public struct ArrayEntry
   {
      public string name;
      public string place;
      public string description;
   }
 
   public ArrayEntry[] object;
} */

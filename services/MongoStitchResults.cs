using UnityEngine;

[System.Serializable]
public class MongoStitchResults
{
	// public object[] crews;
	public string data;
	// public string name;
	// public int lives;
	// public float health;

	public static CombatInitVO CreateFromJSON(string jsonString)
	{
		return JsonUtility.FromJson<CombatInitVO>(jsonString);
	}

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

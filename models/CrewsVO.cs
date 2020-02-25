using UnityEngine;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Driver;

[System.Serializable]
public class CrewsVO
{
	// JsonConvert();
	// public string id;
	// public object id;
	private ObjectId oid;
	public string id
	{
		get { return id; }
		set 
		{
			// Debug.Log("setter"); 
			value = id;
			// value = id["_id"].GetString("$oid");
			// SetId(id);
		}
	}
	private void SetId(string id)
	{
		// oid = (ObjectId)id as ObjectId;
	}
	// }
	public CrewVO[] crew;
	public TacticsVO[] tactics;

	// private string OidByteArrayToString(byte[] oid)
	// {
	// 	// StringBuilder retVal = new StringBuilder();

	// 	for (int i = 0; i < oid.Length; i++)
	// 	{
	// 		if (i == 0)
	// 		{
	// 			int b = oid[0] % 40;
	// 			int a = (oid[0] - b) / 40;
	// 			retVal.AppendFormat("{0}.{1}", a, b);
	// 		}
	// 		else
	// 		{
	// 			if (oid[i] < 128)
	// 				retVal.AppendFormat(".{0}", oid[i]);
	// 			else
	// 			{
	// 				retVal.AppendFormat(".{0}",
	// 				   ((oid[i] - 128) * 128) + oid[i + 1]);
	// 				i++;
	// 			}
	// 		}
	// 	}

	// 	return retVal.ToString();
	// }
}
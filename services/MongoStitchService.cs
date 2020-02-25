using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class MongoStitchService : MonoBehaviour
{
	public const string URL_COMBATINIT = "https://***";

	string combatInitResponse;
	public MongoStitchService()
	{
		Debug.Log("== MongoStitchService.constructor ==");	
	}
	void Start()
	{
		Debug.Log("== MongoStitchService.Start ==");
		// yield return StartCoroutine(GetText());
	}

	void CombatInitHandler()
	{
		Debug.Log("== MongoStitchService.CombatInitHandler ==" + combatInitResponse);

	}

	// public IEnumerator CombatInit()
	// {
	// 	object returnValue;
	// 	yield return StartCoroutine(GetText());//CombatInitHandler));
	// }

	public IEnumerator CombatInit(string id1, string id2, System.Action<string> callback)
	{
		Debug.Log("<color=yellow>== MongoStitchService.CombatInit ==</color>");
		Debug.Log("* id1 " + id1);
		Debug.Log("* id2 " + id2);

		using (UnityWebRequest request = UnityWebRequest.Get(URL_COMBATINIT + "?id1=" + id1 + "&id2=" + id2))
		{
			yield return request.SendWebRequest();

			if (request.isNetworkError) // Error
			{
				Debug.Log(request.error);
			}
			else // Success
			{
				Debug.Log(request.downloadHandler.text);
				// yield return request.downloadHandler.text;
				// this.combatInitResponse = request.downloadHandler.text;
				// successCallback();//request.downloadHandler.text);
				// object result = new object();// {};
				// result.data = request.downloadHandler.text;

				yield return null;
				callback (request.downloadHandler.text);


				// string response = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
				// Debug.Log(response);
				// string json = JsonUtility.FromJson<object>(response);
				// string json = JsonUtility.ToJson(response, true);
				// Debug.Log("* response " + json);//response.crews);
				// CombatInit player = JsonConvert.DeserializeObject<CombatInit>(response);
				// CombatInitVO resp;
				// Debug.Log(CombatInitVO.CreateFromJSON(response).crews);
			}
		}
	}
}
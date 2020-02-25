using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Api.Responses;
using GameSparks.Core;

public class GSAuthVO
{
	public string userId;
	public string authToken;
	public string displayName;
	public bool? newPlayer;
	public GSData scriptData;
	public AuthenticationResponse._Player switchSummary;

	public GSAuthVO(AuthenticationResponse response)
	{
		authToken = response.AuthToken;
		displayName = response.DisplayName;
		newPlayer = response.NewPlayer;
		scriptData = response.ScriptData;
		switchSummary = response.SwitchSummary;
		string userId = response.UserId;
		Debug.Log("authentication success! " + response);
	}
}
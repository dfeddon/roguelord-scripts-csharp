using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Api;
using GameSparks.Core;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Api.Messages;

public class GameSparksService: MonoBehaviour
{
	private const string AUTH_USERNAME = "***";
	private const string AUTH_PASSWORD = "***";
	public GSAuthVO authVO;
	public ChallengeTurnTakenMessage._Challenge challenge;

	private void Awake() {
		Debug.Log("<color=blue>== GameSparksService.Awake ==</color>");
		ChallengeStartedMessage.Listener += ChallengeStartedMessageHandler;
		ChallengeAcceptedMessage.Listener += ChallengeAcceptedMessageHandler;
		ChallengeTurnTakenMessage.Listener += ChallengeTurnTakenMessageHandler;
	}
	private void Start()
	{
		Debug.Log("<color=blue>== GameSparksService.Start ==</color>");
	}
	private void ChallengeStartedMessageHandler(ChallengeStartedMessage message)
	{
		Debug.Log("<color=blue>== GameSparksService.ChallengeStartedMessageHandler ==</color>");
		Debug.Log("* got challenge started message " + message);
	}
	private void ChallengeAcceptedMessageHandler(ChallengeAcceptedMessage message)
	{
		Debug.Log("<color=blue>== GameSparksService.ChallengeAcceptedMessageHandler ==</color>");
		Debug.Log("* got challenge accepted message " + message);
	}
	private void ChallengeTurnTakenMessageHandler(ChallengeTurnTakenMessage message)
	{
		Debug.Log("<color=blue>== GameSparksService.ChallengeTurnTakenMessageHandler ==</color>");
		Debug.Log("* got challenge turn taken message " + message);
		// ChallengeTurnTakenMessage._Challenge challenge = message.Challenge;
		challenge = message.Challenge;
		string messageId = message.MessageId;
		bool? notification = message.Notification;
		GSData scriptData = message.ScriptData;
		Debug.Log("* scriptData " + challenge.ScriptData.JSON);
		// GSEnumerable<GSData> scriptData = message.ScriptData;
		string subTitle = message.SubTitle;
		string summary = message.Summary;
		string title = message.Title;
		string who = message.Who;
	}
	public void Authenticate(System.Action<AuthenticationResponse> callback)
	{
		Debug.Log("* authenticating " + AUTH_USERNAME + " / " + AUTH_PASSWORD);

		new AuthenticationRequest()
		.SetUserName(AUTH_USERNAME)
		.SetPassword(AUTH_PASSWORD)
		.Send((response) =>
		{
			// if (response.HasErrors)
			// {
			// 	Debug.Log("authentication error: " + response.Errors.JSON);
			// }
			// else
			// {
			authVO = new GSAuthVO(response);
			Debug.Log("<color=green>authToken " + authVO.authToken + "</color>");
			Debug.Log("<color=green>displayName " + authVO.displayName + "</color>");
			Debug.Log("<color=green>scriptData " + authVO.scriptData + "</color>");
			callback(response);
				// string authToken = response.AuthToken;
				// string displayName = response.DisplayName;
				// bool? newPlayer = response.NewPlayer;
				// GSData scriptData = response.ScriptData;
				// AuthenticationResponse._Player switchSummary = response.SwitchSummary;
				// string userId = response.UserId;
				// Debug.Log("authentication success! " + response);
			// }
		});

		// yield return null;
	}

	public void SendActions(System.Action<LogChallengeEventResponse> callback)
	{
		Debug.Log("<color=green>== GameSparksService.SendActions ==</color>");

		/*
			array:
				character: id, turn, position, action { source, target, ability, card1, card2, card3 }
		*/

		// get attacking character actions
		// List<GSData> list = new List<GSData>();
		GSRequestData req = new GSRequestData();
		req.AddBoolean("isChallenger", false);
		req.AddNumber("phase", GameManager.instance.currentPhaseTotal);
		foreach (CharacterVO c in GameManager.instance.allcharacters)
		{
			if (c.crewType == CharacterVO.CREW_TYPE_DEFENDER) continue;

			// GSRequestData cd = new GSRequestData();
			// cd.AddObjectList()
			// list.Add(cd);
			// cd.AddS
			// list.Add(c.actions.GetData());
			req.AddObject("char", c.GetData());
			// ActionVO vo = new ActionVO();
			// GSRequestData sd = vo.GetData();
		}
		string challengeId = "5d9f6bc561f7e305207b72d7";
		// return;
		// GSRequestData sd = new GSRequestData(data);
		new LogChallengeEventRequest()
			.SetChallengeInstanceId(challengeId)//challenge.ChallengeId)// challengeInstanceId)
			.SetEventKey("ActionStore")
			// .SetScriptData(sd)
			.SetEventAttribute("ActionData", req)//sd)
			.Send((response) =>
			{
				GSData scriptData = response.ScriptData;
			});

	}
}
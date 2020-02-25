using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Lobby {
public class CombatController : MonoBehaviour
{
public Button StartMapButton;

	void Awake()
	{

	}
	// Use this for initialization
	void Start()
	{
		Debug.Log("CombatController Start");

		// StartMapButton = GameObject.FindWithTag("ShootButton") as Button;
		StartMapButton.onClick.AddListener(() =>
		{
			StartMapHandler();
		});

	}

	// Update is called once per frame
	void Update()
	{

	}

	void StartMapHandler()
	{
		Debug.Log("* start map...");
		StartCoroutine(LoadNewScene());
	}

	IEnumerator LoadNewScene()
	{
		AsyncOperation async = SceneManager.LoadSceneAsync("Sprawl Map", LoadSceneMode.Single);
		while (!async.isDone)
		{
			Debug.Log("loading...");
			yield return null;
		}
	}
}}
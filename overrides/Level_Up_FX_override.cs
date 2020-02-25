using UnityEngine;
using Unity;
using System.Collections;

public class Level_Up_FX_override : Level_Up_FX
{
	// public GameObject levelUpFX_A;
	// public GameObject levelUpFX_B;
	// public GameObject spiralMesh;
	// public Light levelup_Light;

	// private float fadeStart = 5.0f;
	// private float fadeEnd = 0;
	// private float fadeTime = 2.0f;
	// private float t = 0.0f;

	// private bool levelUpActive = false;
	private bool levelUpActive2 = false;

	// private float offset = 0;
	// private Vector2 offsetVector;
	// private Renderer spiralMeshRenderer;

	// void Start()
	// {

	// 	levelUpFX_A.SetActive(false);
	// 	levelUpFX_B.SetActive(false);

	// 	spiralMeshRenderer = spiralMesh.GetComponent<Renderer>();

	// }


	void Update()
	{

		if (Input.GetButtonDown("Fire1"))
		{

			if (levelUpActive2 == false)
			{
				levelUpActive2 = true;
				StartCoroutine("LevelUp");

			}

		}

	}




	// IEnumerator SpiralMagic()
	// {

	// 	offset = 0;

	// 	while (offset < 1.0f)
	// 	{

	// 		offset += (Time.deltaTime * 0.4f);
	// 		Vector2 offsetVector = new Vector2(0, -offset);
	// 		spiralMeshRenderer.material.SetTextureOffset("_MainTex", offsetVector);
	// 		// print(offset);

	// 		yield return 0;

	// 	}

	// }



	// IEnumerator FadeLight()
	// {

	// 	while (t < fadeTime)
	// 	{
	// 		t += Time.deltaTime;
	// 		levelup_Light.intensity = Mathf.Lerp(fadeStart, fadeEnd, t / fadeTime);
	// 		yield return 0;
	// 	}

	// 	t = 0;

	// }


}

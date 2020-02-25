using UnityEngine;

public class TimeManager : MonoBehaviour
{

	public float slowdownFactor = 0.05f;
	public float slowdownLength = 2f;
	public float returnSpeed = 0.05f; // 0.025

	void Update()
	{
		Time.timeScale += (returnSpeed / slowdownLength) * Time.unscaledDeltaTime;
		Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
	}

	public void DoSlowmotion()
	{
		Debug.Log("== TimeManager.DoSlowmotion ==");
		Time.timeScale = slowdownFactor;
		Time.fixedDeltaTime = Time.timeScale * .02f;
	}

}
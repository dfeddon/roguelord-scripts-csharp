using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TimerClass : MonoBehaviour//, IPointerClickHandler
{
	public const int TIMER_MAX = 60;
    public float time;
    private float _timer;
    // public float targetTime = 60.0f;
    private bool isRunning = false;
    private Image timerImage;
    private TextMeshProUGUI textMeshPro;
	int interval = 1;
	float nextTime = 0;
    float targetTime;
    float endTime;
    bool midPointReached = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("<color=yellow>== TimerClass.Start ==</color>");
        timerImage = gameObject.GetComponent<Image>();
        textMeshPro = GameObject.FindWithTag("TurnCounterText").GetComponent<TextMeshProUGUI>();
        // targetTime = turnTimer;
        // StartTimer(TIMER_MAX);
    }

    public void StartTimer(float t = TIMER_MAX)
    {
        targetTime = t;
        endTime = Time.time + t;
		isRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        // only check in 1 second intervals
        if (isRunning && Time.time >= nextTime)
        {
            if (Time.time >= endTime)
            {
                isRunning = false;
                timerEnded();
            }
            else if (midPointReached == false && Time.time >= (endTime / 2))
            {
                midPointReached = true;
                hitMidPoint();

            }
			timerImage.fillAmount = (Mathf.Round(endTime - Time.time) / targetTime);
			textMeshPro.text = Mathf.Round(endTime - Time.time).ToString();
			nextTime += interval;
        }
    }

    void timerEnded()
    {
        Debug.Log("* timer ended!");
        midPointReached = false;
		EventParam eventParam = new EventParam();
		// eventParam.data = characterView.model;
		eventParam.name = "turnCounterComplete";
		EventManager.TriggerEvent("combatEvent", eventParam);

		// for now, just restart timer
		// StartTimer(TIMER_MAX);
    }

    void hitMidPoint()
    {
        Debug.Log("* midpoint reached");
		EventParam eventParam = new EventParam();
		eventParam.name = "turnCounterMidpoint";
		EventManager.TriggerEvent("combatEvent", eventParam);
	}

    // public void OnPointerClick(PointerEventData e)
    // {
    //     Debug.Log("click!");
    //     if (isRunning == true)
    //         isRunning = false;
    //     else isRunning = true; 
    // }
}

using UnityEngine;
using System.Collections;

// [System.Serializable]
public class CharacterMovementVO
{
		public int forwardMax = 1;
		public int backwardMax = 1;
		public int forwardCurrent = 0;
		public int backwardCurrent = 0;

	public CharacterMovementVO()
	{

	}

	public void Reset()
	{
		forwardCurrent = 0;
		backwardCurrent = 0;
	}

	public bool MoveForward(int uid)
	{
		bool isValid = false;

		if (forwardCurrent < forwardMax)
		{
			forwardCurrent++;
			isValid = true;
			backwardCurrent--;
		}

		Debug.Log("<color=blue>MoveForward # " + forwardCurrent + "</color>");
		
		if (isValid == true)
		{
			Debug.Log("<color=blue>Triggering Event</color>");
			EventParam eventParam = new EventParam();
			eventParam.name = "positionUpdateForward";
			eventParam.value = uid; // value is source uid
			eventParam.value2 = forwardCurrent;
			EventManager.TriggerEvent("charEvent", eventParam);
		}
		return isValid;
	}

	public bool MoveBackward(int uid)
	{
		bool isValid = false;

		if (backwardCurrent < backwardMax)
		{
			backwardCurrent++;
			isValid = true;
			forwardCurrent--;
		}

		Debug.Log("# " + backwardCurrent);

		if (isValid == true)
		{
			Debug.Log("<color=blue>Triggering Event</color>");
			EventParam eventParam = new EventParam();
			eventParam.name = "positionUpdateBackward";
			eventParam.value = uid;
			eventParam.value2 = forwardCurrent; // value is source uid
			EventManager.TriggerEvent("charEvent", eventParam);
		}

		return isValid;
	}
}
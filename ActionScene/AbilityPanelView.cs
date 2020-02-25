using UnityEngine;
using System.Collections;
// using System.Reflection;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AbilityPanelView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

	public void OnPointerEnter(PointerEventData e)
	{
		Debug.Log("* AbilityPanel Enter Handler");// + abilityVO.name);
		// EventParam eventParam = new EventParam();
		// eventParam.value = abilityNumber;//characterView.model;
		// eventParam.name = "abilityEnter";
		// eventParam.data = abilityVO;
		// EventManager.TriggerEvent("combatEvent", eventParam);
	}

	public void OnPointerExit(PointerEventData e)
	{
		Debug.Log("* AbilityPanel Exit");
        if (e.pointerEnter.name == "Image")
            Debug.Log("IMAGE!");
        else Debug.Log("OUT?");
		// EventParam eventParam = new EventParam();
		// eventParam.value = abilityNumber;//characterView.model;
		// eventParam.name = "abilityExit";
		// eventParam.data = abilityVO;
		// EventManager.TriggerEvent("combatEvent", eventParam);
	}

}

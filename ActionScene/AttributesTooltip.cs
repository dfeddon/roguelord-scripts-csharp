using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class AttributesTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public enum TooltipSourceType { Attack, Defense };
    public TooltipSourceType sourceType;

	private GameObject tooltip;
    // Start is called before the first frame update
    void Start()
    {
        // if object pooling, get out...
        if (this.transform.parent == null) return;
            tooltip = this.transform.parent.transform.GetChild(2).gameObject;
        tooltip.SetActive(false);
    }

	public void OnPointerEnter(PointerEventData eventData)
	{
        tooltip.SetActive(true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
        tooltip.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}

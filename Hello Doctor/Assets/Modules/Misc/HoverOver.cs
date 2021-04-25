using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverOver : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string message;

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Entered " + gameObject.name);
        HoverOverDisplay.ShowTooltip(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Exited " + gameObject.name);
        HoverOverDisplay.HideTooltip(this);
    }
}

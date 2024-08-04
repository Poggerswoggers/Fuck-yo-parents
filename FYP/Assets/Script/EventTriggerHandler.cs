using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerHandler : MonoBehaviour
{
    public void PointerEnter(BaseEventData eventData)
    {
        PointerEventData pointerData = eventData as PointerEventData;
        Vector3 scale = pointerData.pointerEnter.transform.localScale;
        LeanTween.scale(pointerData.pointerEnter, scale*1.1f, 0.2f); ;
    }

    public void PointerExit(BaseEventData eventData)
    {
        PointerEventData pointerData = eventData as PointerEventData;
        float facing = pointerData.pointerEnter.transform.localScale.x;
        LeanTween.scale(pointerData.pointerEnter, new Vector3(Mathf.Sign(facing), 1, 1), 0.2f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemScript : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {

        if(transform.childCount == 0)
        {
            eventData.pointerDrag.GetComponent<InventorySlotScript>().originalParent = transform;
        }

    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
 
    private Image image;

    [SerializeField]
    private RectTransform canvasTransform;

    private Vector3 originalPosition;

    [HideInInspector] 
    public Transform originalParent;


    private void Awake()
    {
        image = GetComponent<Image>();  
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
        originalParent = transform.parent;
        transform.SetParent(transform.root);  
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pos);

        transform.localPosition = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {


        transform.SetParent(originalParent);


        image.raycastTarget = true;

    }


}

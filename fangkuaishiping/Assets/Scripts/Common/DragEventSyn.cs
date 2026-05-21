using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragEventSyn : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler
{
    public ScrollRect parentScrollRect;

    void Start()
    {
        if (parentScrollRect == null)
        {
            //GameObject parentScrollRectObj = GameObject.Find("Canvas-category/Scroll View-content");
            //Debug.Log(parentScrollRectObj);
            //parentScrollRect = parentScrollRectObj.GetComponent<ScrollRect>();
        }
    }

    public void InitScrollRect(ScrollRect _scrollRect)
    {
        parentScrollRect = _scrollRect;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (parentScrollRect)
        {
            parentScrollRect.OnEndDrag(eventData);
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (parentScrollRect)
        {
            parentScrollRect.OnBeginDrag(eventData);
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (parentScrollRect)
        {
            parentScrollRect.OnDrag(eventData);
        }

    }
}
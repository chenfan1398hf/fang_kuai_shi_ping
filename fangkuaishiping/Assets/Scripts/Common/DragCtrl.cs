using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragCtrl : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, ICanvasRaycastFilter
{
    public Camera mCamera;
    private Transform nowparent;
    private bool isRaycastLocationValid = true;//默认射线不能穿透物体
    public Transform IconList;
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return isRaycastLocationValid;
    }
    /// <summary>
    /// 开始拖动
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        OClickBtn();
        this.transform.SetAsLastSibling();//将被拖拽的物体设置为最后渲染
        nowparent = this.transform.parent;//保存最初始的位置
        isRaycastLocationValid = false;
        transform.SetParent(IconList);//将拖拽的物体放在公共父节点下
        transform.Find("Select").gameObject.SetActive(true);
    }

    /// <summary>
    /// 拖动中
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 v2 = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(IconList.GetComponent<RectTransform>(), Input.mousePosition,mCamera, out v2);
        transform.localPosition = v2;
    }

    /// <summary>
    /// 拖动结束
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnEndDrag(PointerEventData eventData)
    {
        //transform.Find("Select").gameObject.SetActive(false);
        
        GameObject go = eventData.pointerCurrentRaycast.gameObject;
        if(go != null)
        {
                transform.SetParent(nowparent);
                transform.position = nowparent.position;
        }
        else
        {
            transform.SetParent(nowparent);
            transform.position = nowparent.position;
        }
        isRaycastLocationValid = true;//射线不可以穿透物体
    }


    public void OClickBtn()
    {

    }
}

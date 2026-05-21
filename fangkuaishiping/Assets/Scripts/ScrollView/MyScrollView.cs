using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyScrollView : MonoBehaviour
{
    public float rowDistance;               //行距
    public float columnDistance;            //列距
    /// <summary>
    /// 调用设置scroll view内的所有元素的显示与位置的进入方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="setElement"></param>
    public void SetParam<T>(List<T> list, ScrollViewSetter<T>.SetItemData setElement, int columnCount,float scale)
    {
        ScrollViewSetter<T> baseList = new ScrollViewSetter<T>();
        baseList.SetParam(list, setElement, transform, rowDistance, columnDistance, columnCount, scale);
    }
}

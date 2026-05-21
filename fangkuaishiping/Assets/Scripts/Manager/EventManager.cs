using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 事件中心
/// 观察者模式
/// </summary>
public class EventManager : BaseManager<EventManager>
{
    //kye事件名
    //value 监听这个事件对应的委托函数们
    private Dictionary<string, UnityAction<object>> eventDic = new Dictionary<string, UnityAction<object>>();

    /// <summary>
    /// 添加事件监听
    /// </summary>
    public void AddEventListener(string _name, UnityAction<object> _action)
    {
        if (eventDic.ContainsKey(_name))
        {
            eventDic[_name] += _action;
        }
        else
        {
            eventDic.Add(_name, _action);
        }
    }
    /// <summary>
    /// 移除事件监听
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_action"></param>
    public void RemoveEventListener(string _name, UnityAction<object> _action)
    {
        if (eventDic.ContainsKey(_name))
        {
            eventDic[_name] -= _action;
        }
    }

    /// <summary>
    /// 事件触发
    /// </summary>
    public void EventTrigger(string _name, object _obj)
    {
        if (eventDic.ContainsKey(_name))
        {
            eventDic[_name].Invoke(_obj);
        }
    }
    /// <summary>
    /// 清空
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
}

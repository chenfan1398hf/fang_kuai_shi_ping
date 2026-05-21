using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyButton : Button
{
    //构造函数
    protected MyButton()
    {

    }

    //长按
    public ButtonClickedEvent my_onLongPress;
    public ButtonClickedEvent OnLongPress
    {
        get { return my_onLongPress; }
        set { my_onLongPress = value; }
    }


    //长按参数
    private bool my_IsStartPress = false;//是否开始长按
    private float my_currPointDownTime = 0.0f;//当前按下的时间
    private float my_longPressTiem = 0.5f;//长按的触发时间
    private bool my_longPressTrigger = false;//是否正在触发

    /// <summary>
    /// CheckIsLongPress() 检查长按是否触发
    /// </summary>
    private void CheckIsLongPress()
    {
        if (my_IsStartPress && !my_longPressTrigger)
        {
            if (Time.time > my_currPointDownTime + my_longPressTiem)
            {
                my_longPressTrigger=true;
                my_IsStartPress = false;
                if (my_onLongPress != null)
                {
                    my_onLongPress.Invoke();
                }
            }
        }

    }


    public void Update()
    {
        CheckIsLongPress();
    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        my_currPointDownTime = Time.time;
        my_IsStartPress = true;
        my_longPressTrigger = false;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        my_IsStartPress=false;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        my_IsStartPress = false;
    }
}

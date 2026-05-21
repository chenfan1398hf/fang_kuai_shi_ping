using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWebRequest : MonoBehaviour
{
    #region SendPost()数据请求---建立推送
    public void SendPost(string m_url)
    {
        WWWForm form = new WWWForm();
       // form.AddField("rid", roomID);//表单
        UnityWebRequestPostSimpleWrapper postSimpleWrapper = new UnityWebRequestPostSimpleWrapper(this);
        postSimpleWrapper.StartUnityWebRequestFormPost(m_url,
            form,
            "",
            (isNotError, responseStr) =>
            {
                if (isNotError)
                {
                    //Debug.Log( responseStr);
                }
                else
                {
                    try
                    {
                        Debug.Log(GetType() + "/Test()/ error : " + responseStr);
                        Debug.Log(GetType() + "/Test()/ 针对报错，检查处理 ... ");
                    }
                    catch (Exception e)
                    {
                        Debug.Log(GetType() + "/Test()/ error : " + e);
                        Debug.Log(GetType() + "/Test()/ 针对报错，检查处理 ... ");
                    }
                }
            });

    }
    #endregion
}

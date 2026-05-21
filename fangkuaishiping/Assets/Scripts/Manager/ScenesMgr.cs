using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景切换模块用到
/// 场景异步加载
/// 协程
/// 委托
/// </summary>
public class ScenesMgr : BaseManager<ScenesMgr>
{
    /// <summary>
    /// 场景同步加载
    /// </summary>
    /// <param name="_name">场景名</param>
    /// <param name="_fun">回调函数</param>
    public void LoadScene(string _name, UnityAction _fun)
    {
        SceneManager.LoadScene(_name);
        _fun.Invoke();
    }
    /// <summary>
    /// 异步加载携程
    /// </summary>
    /// <param name="_name">场景名</param>
    /// <param name="_fun">回调函数</param>
    public void LoadSceneAsyn(string _name, UnityAction _fun)
    {
        GameManager.instance.StartCoroutine(ReallyLoadSceneAsyn(_name, _fun));
    }

    private IEnumerator ReallyLoadSceneAsyn(string _name, UnityAction _fun)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(_name);
        while (!ao.isDone)
        {
            //事件中心向外分发进度情况，外部可使用用来更新进度条
            //EventManager.GetInstance().EventTrigger("进度条更新", ao.progress);

            yield return ao.progress;
        }
        _fun.Invoke();
    }
}

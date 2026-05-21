using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ResourcesLoad :Singleton<ResourcesLoad>
{
    [Tooltip("预制体哈希表")]
    private Hashtable m_PrefabTable;

    #region ResourcesLoad 构造函数
    /// <summary>
    /// 构造函数
    /// </summary>
    public ResourcesLoad()
    {
        m_PrefabTable = new Hashtable();
    }
    #endregion

    #region Load 预制体动态加载方法
    /// <summary>
    /// Load 预制体动态加载方法
    /// </summary>
    /// <param name="type"> 预制体类型</param>
    /// <param name="path">预制体名称（路径）</param>
    /// <param name="cache">是否有缓存</param>
    /// <returns>预制体实体</returns>
    public T Load<T>(string AllPath, bool cache = false)where T : Object
    {
        StringBuilder m_Builder = new StringBuilder();
        T prefab = null;// default(T);
        if (m_PrefabTable.ContainsKey(AllPath))
        {
            // Debug.Log(path + "该资源来自缓存");
            prefab = m_PrefabTable[AllPath] as T;
            cache = false;
        }
        else
        {
            m_Builder.Append(AllPath);
            prefab = Resources.Load<T>(m_Builder.ToString());
            //if (prefab is GameObject)
            //{
            //    prefab = GameObject.Instantiate(prefab);
            //}
            if (!cache)
            {
                m_PrefabTable.Add(AllPath, prefab);
            }
        }
        if (prefab == null)
        {
            Debug.Log("Load ===>" + AllPath);
        }
        return  prefab;
    }
    #endregion

    #region Dispose() 释放资源
    /// <summary>
    /// Dispose() 释放资源
    /// </summary>
    public void Dispose()
    {
        m_PrefabTable.Clear();
        Resources.UnloadUnusedAssets();
    }
    #endregion
}

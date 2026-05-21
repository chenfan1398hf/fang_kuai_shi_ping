using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjPath
{
    string objPath { get; set; }
}

/// <summary>对象池</summary>
public class ObjPool : Singleton<ObjPool>
{
    //public ResManager resManager { get { return GameManager.instance.resManager; } }
    public class PoolItem
    {
        /// <summary>未使用 队列
        /// </summary>
        public readonly Queue<GameObject> queueUnused = new Queue<GameObject>();
        /// <summary>已使用 列表
        /// </summary>
        public readonly List<GameObject> listUsed = new List<GameObject>();
    }
    private Dictionary<string, GameObject> prefabCacheDic = new Dictionary<string, GameObject>();
    private readonly Dictionary<string, PoolItem> dicObjPool = new Dictionary<string, PoolItem>();

    public Dictionary<string, PoolItem> DicObjPool { get { return dicObjPool; } }

    //添加预制体到prefabCacheDic中
    public void addPrefabPool(GameObject prefabObj)
    {
        if (prefabObj == null) return;
        string key = prefabObj.name;
        if (prefabCacheDic.ContainsKey(key))
        {
            prefabCacheDic[key] = prefabObj;
        }
        else
        {
            prefabCacheDic.Add(key, prefabObj);
        }
    }

    /// <summary>预产
    /// </summary>
    /// <param name="prefabObj"></param>
    /// <param name="objParent"></param>
    internal void PreCreatioin(GameObject prefabObj, Transform objParent)
    {
        if (prefabObj == null) return;
        GameObject objItem = GameObject.Instantiate(prefabObj);
        objItem.SetActive(false);
        objItem.transform.SetParent(objParent);

        string key = prefabObj.name;
        AddPoolItem(key);
        DicObjPool[key].queueUnused.Enqueue(objItem);
    }
    
    private void AddPoolItem(string key)
    {
        if (DicObjPool.ContainsKey(key) == false) DicObjPool.Add(key, new PoolItem());
    }

    /// <summary>加载预制件</summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private GameObject LoadPrefab(string path)
    {

        // GameObject result = Resources.Load<GameObject>(path);
        GameObject result = ResourcesLoad.instance.Load<GameObject>(path);
        return result;
    }

    /// <summary>Get Item
    /// </summary>
    public GameObject GetObj(string path, Transform parent_, bool isSetAsLastSibling = true)
    {
        this.AddPoolItem(path);
        if (DicObjPool[path].listUsed.Count > byte.MaxValue) this.RemoveNullUsed(path);

        GameObject item = null;
        if (DicObjPool[path].queueUnused.Count > 0)
        {
            item = DicObjPool[path].queueUnused.Dequeue();

            if (item == null)
            {
                return GetObj(path, parent_, isSetAsLastSibling);
            }
            item.transform.SetParent(parent_, false);
            DicObjPool[path].listUsed.Add(item);
            item.SetActive(true);
            return item;
        }

        GameObject _prefab = this.LoadPrefab(path);
        if (_prefab == null)
        {
            Debug.LogError("你想实例化到对象为空。 请检查路径！" + path);
            return null;
        }
        item = UnityEngine.GameObject.Instantiate(_prefab);
        item.transform.SetParent(parent_, false);
        if (isSetAsLastSibling) item.transform.SetAsLastSibling();

        DicObjPool[path].listUsed.Add(item);//
        return item;
    }

    public GameObject GetObj(GameObject prefabObj, Transform parent_, bool isSetAsLastSibling = true)
    {
        if (prefabObj == null) return null;
        string path = prefabObj.name;

        this.AddPoolItem(path);
        if (DicObjPool[path].listUsed.Count > byte.MaxValue) this.RemoveNullUsed(path);

        GameObject item = null;
        if (DicObjPool[path].queueUnused.Count > 0)
        {
            item = DicObjPool[path].queueUnused.Dequeue();

            if (item == null)
            {
                return GetObj(prefabObj, parent_, isSetAsLastSibling);
            }
            item.transform.SetParent(parent_, false);
            DicObjPool[path].listUsed.Add(item);//
            return item;
        }

        GameObject _prefab = prefabObj; //

        item = UnityEngine.GameObject.Instantiate(_prefab);
        item.transform.SetParent(parent_, false);
        if (isSetAsLastSibling) item.transform.SetAsLastSibling();

        DicObjPool[path].listUsed.Add(item);//
        return item;
    }



    /// <summary>如果,对象实例化到某子物体层级，被跟着销毁的时候，已经为Null占用了列表长度空间。
    /// 防止已使用缓存列表存储过多的空值
    /// </summary>
    /// <param name="key"></param>
    public void RemoveNullUsed(string key)
    {
        var listObj = DicObjPool[key].listUsed;
        for (int i = 0; i < listObj.Count; i++)
        {
            if (listObj[i] == null)
            {
                listObj.RemoveAt(i);
                i = 0;
            }
        }
    }

    /// <summary>这个方法建议在 跳转到其他场景的时候调用， 清理全部键值对应的 已使用列表(listUsed) 空间
    /// </summary>
    /// <param name="key"></param>
    public void RemoveAllNullUsed()
    {
        foreach (var item in DicObjPool)
        {
            RemoveNullUsed(item.Key);
        }
    }
    
    /// <summary>回收</summary>
    /// <param name="path"></param>
    /// <param name="obj"></param>
    public void Recycle(string key, GameObject obj)
    {
        if (DicObjPool.ContainsKey(key)) DicObjPool[key].listUsed.Remove(obj);
        if (obj == null)
        {
            return;//防止有些在某父对象下被跟着销毁报空处理
        }
        if (DicObjPool.ContainsKey(key))
        {
            Queue<GameObject> queObj = DicObjPool[key].queueUnused;
            obj.SetActive(false);
            if (queObj.Contains(obj) == false) queObj.Enqueue(obj);
        }
    }

    /// <summary>回收</summary>
    /// <param name="path"></param>
    /// <param name="obj"></param>
    public void Recycle(GameObject prefabObj, GameObject obj)
    {
        if (prefabObj == null) return;
        Recycle(prefabObj.name, obj);
    }
    /// <summary>回收</summary>
    /// <param name="path"></param>
    /// <param name="obj"></param>
    public void Recycle(GameObject prefabObj, GameObject obj, string path)
    {
        if (prefabObj == null) return;
        //Recycle("Prefab/"+prefabObj.name, obj);
        string[] splits = prefabObj.name.Split('(');
        if (splits.Length > 0)
        {
            Recycle(path + splits[0], obj);
        }
        else
        {
            Recycle(path + prefabObj.name, obj);
        }
    }
    /// <summary>回收某类型的全部</summary>
    /// <param name="path"></param>
    public void RecycleTypeAll(string key)
    {
        if (DicObjPool.ContainsKey(key))
        {
            Queue<GameObject> queObj = DicObjPool[key].queueUnused;
            List<GameObject> listCache = DicObjPool[key].listUsed;//

            if (listCache != null && listCache.Count > 0)
            {
                for (int i = 0; i < listCache.Count; i++)
                {
                    GameObject obj = listCache[i];
                    if (obj != null)
                    {
                        queObj.Enqueue(obj);
                        obj.SetActive(false);
                    }
                }
            }

            DicObjPool[key].listUsed.Clear();
        }
    }

    /// <summary>回收某类型的全部</summary>
    /// <param name="path"></param>
    public void RecycleTypeAll(GameObject prefabObj)
    {
        if (prefabObj == null) return;
        RecycleTypeAll(prefabObj.name);
    }
    public PoolItem GetPoolItem(string key)
    {
        if (DicObjPool.ContainsKey(key))
        {

            return DicObjPool[key];
        }
        return null;
    }

    //清理所有对象
    public void clearPoolMap()
    {
        prefabCacheDic.Clear();
        dicObjPool.Clear();
    }
}


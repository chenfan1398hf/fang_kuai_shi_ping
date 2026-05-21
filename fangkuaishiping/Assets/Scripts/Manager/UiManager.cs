using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI界面管理类
public class UiManager : MonoSingleton<UiManager>
{

    public List<GameObject> UiObj = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //创建一个UI界面
    public GameObject CreatUi(string name)
    {
        string path = "UI/" + name;
        CloseUiByName(name);
        GameObject obj = GameManager.instance.AddPrefab(path, this.transform);
        UiObj.Add(obj);
        TipsMaxUp();
        return obj;
    }
    //关闭最上层UI界面
    public void CloseUiUp()
    {
        if (UiObj.Count <= 0)
        {
            Debug.Log("没有UI了");
            return;
        }
        //HallManager.instance.DestroyPrefab(UiObj[UiObj.Count - 1], UiObj[UiObj.Count - 1], "UI/");
        Destroy(UiObj[UiObj.Count - 1]);
        UiObj.Remove(UiObj[UiObj.Count - 1]);
        return;
    }
    //根据名称找UI界面
    public GameObject GetUiByName(string name)
    {
        name += "(Clone)";
        foreach (var item in UiObj)
        {
            if (item.name == name)
            {
                return item;
            }
        }
        return null;
    }
    //根据名称关闭UI
    public void CloseUiByName(string name)
    {
        foreach (var item in UiObj)
        {
            if (item.name == name+"(Clone)")
            {
                //HallManager.instance.DestroyPrefab(item, item, "UI/");
                Destroy(item);
                UiObj.Remove(item);
                return;
            }
        }
        return;
    }
    //创建一个定时Tips
    public void CreatTipsUi(string _msg, float _time)
    {
        var obj = GetUiByName("Panel_Tips");
        if (obj != null)
        {
            var awardObj = GameManager.instance.AddPrefab("UI/Tips", obj.transform.Find("Scroll View").Find("Viewport").Find("Content"));
            //awardObj.GetComponent<TipsAward>().InitTips(_msg, _time);
        }
        return;
    }
    //创建一个定时奖励Tips
    public void CreatTipsAwardUi(int _awardId, int _number, float _time)
    {
        var obj = UiManager.instance.GetUiByName("Panel_Tips");
        if (obj != null)
        {
            var awardObj = GameManager.instance.AddPrefab("UI/TipsAward", obj.transform.Find("Scroll View").Find("Viewport").Find("Content"));
            //awardObj.GetComponent<TipsAward>().InitTipsAward(_awardId, _number, _time);
        }
        return;
    }
    //把TiPS界面设置为最上层
    public void TipsMaxUp()
    {
        var obj = GetUiByName("Panel_Tips");
        if (obj != null)
        {
            obj.GetComponent<RectTransform>().SetAsLastSibling();
        }
        return;
    }
    //隐藏显示UI界面
    public void ShowRulersUi(string _name, bool _isBool)
    {
        GameObject obj = GetUiByName(_name);
        if (obj != null)
        {
            obj.SetActive(_isBool);
        }
        return;
    }
    //关闭除大厅UI的所有界面
    public void CloseAllUi()
    {
        for (int i = 0; i < UiObj.Count; i++)
        {
            if (UiObj[i].gameObject.name != "Panel_hall_ui(Clone)"&& UiObj[i].gameObject.name != "Panel_Tips(Clone)")
            {
               Destroy(UiObj[i]);
               UiObj.Remove(UiObj[i]);
               i--;
            }
        }
        return;
    }
}

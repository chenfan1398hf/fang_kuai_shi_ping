using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Person
{
    public string name;
    public int level;
    public Person(string name, int level)
    {
        this.name = name;
        this.level = level;
    }
}

public class SVController : MonoBehaviour
{
    /// <summary>
    /// scroll view自身
    /// </summary>
    public MyScrollView myScrollView;
    /// <summary>
    /// 分为几列
    /// </summary>
    public int columnCount;
    public float mScale=1;
    void Start()
    {
        //测试代码
        List<Person> personList = new List<Person>();
        for (int i = 0; i < 100; i++)
        {
            personList.Add(new Person("张三" + i, i));
        }
        myScrollView = GetComponent<MyScrollView>();
        myScrollView.SetParam(personList, SetEnement, columnCount,mScale);
    }

    /// <summary>
    /// 设置每个元素的属性
    /// </summary>
    /// <param name="item">scroll view内的元素对象</param>
    /// <param name="itemData">对应的数据</param>
    /// <returns></returns>
    public Transform SetEnement(Transform item, Person itemData)
    {
        item.Find("Name").GetComponent<Text>().text = itemData.name;
        item.Find("Level").GetComponent<Text>().text = itemData.level + "";
        return item.transform;
    }
}
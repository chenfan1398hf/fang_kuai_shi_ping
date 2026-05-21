using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewSetter<T>
{
    /// <summary>
    /// 委托
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="itemData"></param>
    /// <returns></returns>
    public delegate Transform SetItemData(Transform transform, T itemData);
    /// <summary>
    /// 显示列数
    /// </summary>
    public int columnCount = 1;
    /// <summary>
    /// 行距
    /// </summary>
    public float rowDistance;
    /// <summary>
    /// 列距
    /// </summary>
    public float columnDistance;
    /// <summary>
    /// 可显示行数
    /// </summary>
    private int rowCount;
    /// <summary>
    /// List的基本组件
    /// </summary>
    private Transform content;
    /// <summary>
    /// List的宽度
    /// </summary>
    private float parentWidth = 0;
    /// <summary>
    /// List的高度
    /// </summary>
    private float parentHeight = 0;
    /// <summary>
    /// 显示的ITem
    /// </summary>
    private Transform itemTransform;
    /// <summary>
    /// 子物体宽度
    /// </summary>
    private float itemWidth = 0;
    /// <summary>
    /// 子物体高度
    /// </summary>
    private float itemHeight = 0;
    /// <summary>
    /// 显示的子物体数量
    /// </summary>
    private int showCount = 0;
    /// <summary>
    /// 可视范围
    /// </summary>
    private float viewScope = 0;
    /// <summary>
    /// 总长度
    /// </summary>
    private float overAllLength = 0;
    /// <summary>
    /// 开始位置
    /// </summary>
    private float contentStartPos = 0;

    /// <summary>
    /// ScrollView自身
    /// </summary>
    private Transform rootTransform = null;

    /// <summary>
    /// 数据列表
    /// </summary>
    private List<T> dataList;
    /// <summary>
    /// 委托
    /// </summary>
    private SetItemData setItemData;

    //private Stack<Transform> itemStack; //搞个栈存储Item，方便存取
    /// <summary>
    /// 存储显示的元素，每次滑动时进行修改
    /// </summary>
    private Dictionary<int, Transform> itemDictionary = new Dictionary<int, Transform>();


    /// <summary>
    /// 设置基础数据
    /// </summary>
    /// <param name="list">所有显示的内容</param>
    /// <param name="setElement">设置单独每个元素内容的方法</param>
    /// <param name="root">scroll view本体</param>
    /// <param name="rowDistance">行距</param>
    /// <param name="columnDistance">列距</param>
    /// <param name="columnCount">显示列数</param>
    public void SetParam(List<T> list, SetItemData setElement, Transform root, float rowDistance, float columnDistance, int columnCount,float scale)
    {
        this.rowDistance = rowDistance;
        this.columnCount = columnCount;
        this.columnDistance = columnDistance;
        rootTransform = root;
        dataList = list;
        this.setItemData = setElement;
        InitParameter();
        SetData(scale);
    }


    /// <summary>
    /// 参数初始化
    /// </summary>
    private void InitParameter()
    {
        content = rootTransform.Find("Viewport/Content");                           //获取Content
        contentStartPos = content.localPosition.y;                                  //得到开始位置
        viewScope = rootTransform.GetComponent<RectTransform>().rect.height;      //得到scroll view的可见区域高度

        itemTransform = rootTransform.Find("Viewport/Item").transform;              //显示的元素本体
        itemTransform.gameObject.SetActive(false);                                  //将原始的关闭显示
        Rect itemRect = itemTransform.GetComponent<RectTransform>().rect;           //得到元素的宽高
        itemWidth = itemRect.width;
        itemHeight = itemRect.height;

        parentWidth = rootTransform.GetComponent<RectTransform>().rect.width;
        parentHeight = rootTransform.GetComponent<RectTransform>().rect.height;

        int maxcolumnCount = (int)(parentWidth / (itemWidth + columnDistance));                        //得到最多可显示的列数，若少于设置的列数，则将列数修改为最大列数
        if (maxcolumnCount < (columnCount))
            columnCount = maxcolumnCount;

        rowCount = (int)((parentHeight - (rowDistance + itemHeight / 2)) / (rowDistance + itemHeight) + 2);     //得到可显示的行数
        if (columnCount == 0) columnCount = 1;
        showCount = columnCount * rowCount;                                                                     //得到界面内最多同时显示的元素数量
    }

    /// <summary>
    /// 设置元素
    /// </summary>
    /// <param name="list"></param>
    public void SetData(float scale)
    {
        int maxCount = dataList.Count;
        int index = 0;
        overAllLength = (itemHeight + rowDistance) * maxCount / columnCount;        //计算得到要显示所有数据的高度，用于设置下拉的滑动限制
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, overAllLength + itemHeight);  //重新计算Content的宽高用来装Item,但是这个x轴的宽度很神奇填0是ok的但是不规范坐等大神解决（自行调试发现问题我暂时没有解决）
        ScrollRect scrollRect = rootTransform.GetComponent<ScrollRect>();
        if (scrollRect != null)
        {
            scrollRect.onValueChanged.AddListener(OnValueChange);                                       //监听滑动值的改变，传入的是位置信息 
        }

        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                if (index >= maxCount)
                {
                    return;
                }
                Transform itemTrans = setItemData(GameObject.Instantiate(itemTransform.gameObject).transform, dataList[index]);
                itemTrans.parent = content.transform;
                itemTrans.gameObject.SetActive(true);
                itemTrans.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);                  //设置元素锚点
                itemTrans.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);

                float x = (column * (itemWidth + columnDistance) + itemWidth / 2 + columnDistance);
                //前面物体的距离=行数*（物体高度+物体间离）
                //自己再往后退半个+间距
                float y = -(row * (itemHeight + rowDistance) + itemHeight / 2 + rowDistance);
                itemTrans.localPosition = new Vector2(x, y);   //设置位置
                itemTrans.localScale = Vector3.one*scale;
                itemDictionary.Add(index, itemTrans);
                index++;
            }
            if (index >= maxCount)
            {
                return;
            }
        }
    }


    /// <summary>
    /// 监听List滑动
    /// </summary>
    /// <param name="vector"></param>
    public void OnValueChange(Vector2 vector)
    {
        int startIndex = GetShowIndex();

        //不能小于第一个，不能大到剩下的item不足以容下完整一页
        if (startIndex < 1) startIndex = 1;
        //else if (startIndex - (dataList.Count - showCount) < 0) startIndex = (dataList.Count - showCount);

        float overallProp = viewScope / overAllLength;    //可视比例
        float maxY = content.transform.InverseTransformPoint(new Vector3(0, vector.y * overAllLength, 0)).y;              //最大高度
        float minY = content.transform.InverseTransformPoint(new Vector3(0, vector.y * overallProp * overAllLength, 0)).y;                //最小高度

        int index = startIndex - 1; //虽然找到了开始位置，但是数组下标是从0开始的

        int endIndex = index + showCount;

        List<int> uplist = new List<int>();
        List<int> downList = new List<int>();
        //清空不在范围内的数据存到队列中
        foreach (int key in itemDictionary.Keys)
        {
            //当前物体在可视范围之上
            if (key < index && key + showCount < dataList.Count)
            {
                uplist.Add(key);
            }

            //当前物体在可视范围之下
            if (key >= endIndex/* + (columnCount * (rowCount + 1)) - columnCount - 1*/)
            {
                downList.Add(key);
            }
        }
        //删除上面的表示物体往下滑了，
        //我们要填充的是该位置往下拉可视范围数量
        foreach (int cursor in uplist)
        {

            Transform trans;
            if (itemDictionary.TryGetValue(cursor, out trans))
            {
                itemDictionary.Remove(cursor);
                int row = cursor / columnCount + rowCount;  //拉到第几行
                int pos = cursor + showCount;
                float colum = -(row * (itemHeight + rowDistance) + itemHeight / 2 + rowDistance);   //计算出该行位置
                if (showCount + cursor < dataList.Count)
                {
                    trans = setItemData(trans, dataList[showCount + cursor]);
                    trans.localPosition = new Vector2(trans.localPosition.x, colum);
                    itemDictionary.Add(pos, trans);
                }

            }
        }

        //删除下面的表示物体往上滑了，
        //我们要填充的是该位置往上滑可视范围数量
        foreach (int cursor in downList)
        {
            Transform trans;
            if (itemDictionary.TryGetValue(cursor, out trans))
            {
                itemDictionary.Remove(cursor);
                int row = cursor / columnCount - rowCount;  //拉到第几行
                int pos = cursor - showCount;
                float colum = -(row * (itemHeight + rowDistance) + itemHeight / 2 + rowDistance);  //计算出该行位置
                trans = setItemData(trans, dataList[cursor - showCount]);
                trans.localPosition = new Vector2(trans.localPosition.x, colum);
                itemDictionary.Add(pos, trans);
            }
        }

    }

    /// <summary>
    /// 获取到要从第几个位置开始显示
    /// 这里的做法就是最开始的位置减去当前位置
    /// 往下滑值越低，往上滑值越高。
    /// (初始位置-当前位置)/(item垂直距离+item高度)+1 = 从第几行开始显示
    /// 行数*3+1
    /// </summary>
    public int GetShowIndex()
    {
        float startPos = contentStartPos;
        float currentPos = content.localPosition.y;
        int line = ((int)((currentPos - startPos) / (itemHeight + rowDistance)) + 1);
        int startIndex = line * columnCount - columnCount + 1;
        return startIndex;
    }
}

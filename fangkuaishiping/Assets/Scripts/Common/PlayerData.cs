using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

//玩家数据
public class PlayerData
{
    public PlayerData()
    {
        isSave = true;
        playerLocalizeState = 1;
        palyerUpdateTime = Convert.ToDateTime("2021-9-25");
        playerLogoutTime =DateTime.Now;

        playerSyntheticBtnset = 0;
        
    }
    public static bool isSave;                          //是否存档
    public DateTime palyerUpdateTime;                   //每日状态重置时间 
    public DateTime palyerUpdateWeekTime;               //每周状态重置时间 
    public int playerLocalizeState;                     //1中文2英文
    public int playerSyntheticBtnset;                   //0-未选择  1-选择
    public DateTime playerTlTime;                       //上次恢复体力时间
    public int playerPower;                             //玩家合成小游戏的体力

    public DateTime playerLoginTime;                    //玩家登录游戏的时间
    public DateTime playerLogoutTime;                   //玩家登出游戏时间

    public bool playerAudio1;                           //音效开关
    public bool playerAudio2;                           //音乐开关


    public Dictionary<String, TaskInfo> palyerTaskDic = new Dictionary<String, TaskInfo>();                         //任务数据




    //读取本地玩家数据
    public static PlayerData GetLocalData()
    {
        PlayerData newData = null;
        //读取本地数据
        if (PlayerPrefs.HasKey("PlayerData"))
        {
            string data = PlayerPrefs.GetString("PlayerData").ToString();
            newData = JsonMapper.ToObject<PlayerData>(data);
            if (GameManager.isDbugLog)
                Debug.Log(data);
        }
        else
        {
            if (GameManager.isDbugLog)
                Debug.Log("未找到本地玩家数据------>新建");
            newData = new PlayerData();
        }

        return newData;
    }

    public static PlayerData GetCloudData(string receiveData)
    {
        var newData = JsonMapper.ToObject<PlayerData>(receiveData);
        return newData;
    }
    public  void UpdateLoginTime()
    {
        playerLoginTime = DateTime.Now;
    }

    public static void ClearLocalData()
    {
        isSave = false;
        if (PlayerPrefs.HasKey("PlayerData"))
        {
            PlayerPrefs.DeleteKey("PlayerData");
            PlayerPrefs.Save();
            if (GameManager.isDbugLog)
                Debug.Log("清除本地存档");
        }
    }
    public void InitData()
    {
        //foreach (var item in GameManager.instance.configMag.TaskInfoCfg)
        //{
        //    TaskInfo taskInfo1 = new TaskInfo();
        //    if (!palyerTaskDic.TryGetValue(item.ID.ToString(), out taskInfo1))
        //    {
        //        TaskInfo taskInfo = new TaskInfo();
        //        taskInfo.taskId = item.ID;
        //        taskInfo.tasktype = item.type;
        //        taskInfo.taskPrepositionTaskID = item.prepositionTaskID;
        //        if (item.needUnlock == 0)
        //        {
        //            taskInfo.isUnlock = true;
        //            taskInfo.taskState = 0;
        //        }
        //        else
        //        {
        //            taskInfo.isUnlock = false;
        //            taskInfo.taskState = -2;
        //        }
        //        palyerTaskDic.Add(item.ID.ToString(), taskInfo);
        //    }
        //}


    }


    //保存本地玩家数据
    public void Save()
    {
        if (isSave)
        {
            string data = JsonMapper.ToJson(this);
            PlayerPrefs.SetString("PlayerData", data);
            PlayerPrefs.Save();
            if (GameManager.isDbugLog)
                Debug.Log("本地存档");
        }
    }
    //保存玩家数据到本地
    public void SaveLocal()
    {
        //string data = JsonMapper.ToJson(this);
        //PlayerPrefs.SetString("PlayerData", data);
        //PlayerPrefs.Save();
        //string data = JsonMapper.ToJson(this);
        //if (GameManager.instance.userInfo.isGoogle)
        //{
        //    PottingMobile._SavedGame(data);
        //}
        //else
        //{
        //    PlayerPrefs.SetString("PlayerData", data);
        //    PlayerPrefs.Save();
        //    Debug.Log("本地存档");
        //}
    }
    //开关音效
    public void PlayerOpenAudio()
    {
        //AudioManager.SwitchSound(playerAudio1);
        //AudioManager.SwitchBgm(playerAudio2);
        return;
    }
    //发奖
    public void AwardGive(string award, bool isTipsAwardUi = true)
    {
        string strSkills = award;
        string[] list = strSkills.Split(new char[] { ';' });
        for (int i = 0; i < list.Length; i++)
        {
            string[] listGrade = list[i].Split(new char[] { ':' });
            if (listGrade.Length != 2)
            {
                if (GameManager.isDbugLog)
                    Debug.Log("奖励异常");
                return;
            }
            AwardToPlayer(int.Parse(listGrade[0]), (int)float.Parse(listGrade[1]), isTipsAwardUi);
        }
    }
    public string AwardGive(string award, float number, bool isTipsAwardUi = true)
    {
        string strSkills = award;
        string str = string.Empty;
        string[] list = strSkills.Split(new char[] { ';' });
        for (int i = 0; i < list.Length; i++)
        {
            string[] listGrade = list[i].Split(new char[] { ':' });
            if (listGrade.Length != 2)
            {
                if (GameManager.isDbugLog)
                    Debug.Log("奖励异常");
                return null;
            }
            int adNumber = (int)(float.Parse(listGrade[1]) * number);
            AwardToPlayer(int.Parse(listGrade[0]), adNumber, isTipsAwardUi);
            str += listGrade[0] + ":" + adNumber.ToString() + ";";
        }
        if (str != string.Empty)
        {
            str = str.Substring(0, str.Length - 1);
        }
        return str;
    }
    //扣除货币
    public void DeductProp(string str)
    {
        string strSkills = str;
        string[] list = strSkills.Split(new char[] { ';' });
        for (int i = 0; i < list.Length; i++)
        {
            string[] listGrade = list[i].Split(new char[] { ':' });
            if (listGrade.Length != 2)
            {
                if (GameManager.isDbugLog)
                    Debug.Log("奖励异常");
                return;
            }
            AwardToPlayer(int.Parse(listGrade[0]), -(int)float.Parse(listGrade[1]));
        }
    }
    //发放货币扣除货币
    public void AwardToPlayer(int id, int number,bool isTipsAwardUi = true)
    {
        //if (id == (int)PropId.Jb)
        //{
        //    playerJb += number;
        //}
        //else if (id == (int)PropId.Zs)
        //{
        //    playerZs += number;
        //}
        //else if (id == (int)PropId.Tl)
        //{
        //    playerPower += number;
        //    if (playerPower > 999)
        //    {
        //        playerPower = 999;
        //    }
        //}
 


        return;
    }
    public void AwardToPlayer(int id, long number, bool isTipsAwardUi = true)
    {
        //if (id == (int)PropId.Jb)
        //{
        //    playerJb += number;
        //}

     
        return;
    }
    //检查玩家道具数量
    public bool CheckPlayerPropNumber(int id, long number)
    {
        //if (id == (int)PropId.Jb)
        //{
        //    if (playerJb >= number)
        //    {
        //        return true;
        //    }
        //}
        //else if (id == (int)PropId.Zs)
        //{
        //    if (playerZs >= number)
        //    {
        //        return true;
        //    }
        //}
        //else if (id == (int)PropId.Tl)
        //{
        //    if (playerPower >= number)
        //    {
        //        return true;
        //    }
        //}

        return false;
    }
    //检查玩家道具数量
    public bool CheckPlayerPropNumber(string str)
    {
        string[] list = str.Split(new char[] { ';' });
        for (int i = 0; i < list.Length; i++)
        {
            string[] listGrade = list[i].Split(new char[] { ':' });
            if (listGrade.Length != 2)
            {
                if (GameManager.isDbugLog)
                    Debug.Log("奖励异常");
                return false;
            }
            bool state = CheckPlayerPropNumber(int.Parse(listGrade[0]), int.Parse(listGrade[1]));
            if (state == false)
            {
                return false;
            }
        }
        return true;
    }
    //检查重置各种活动状态
    public void CheckUpdataDay()
    {
        DateTime dt = DateTime.Now;
        TimeSpan span = dt.Subtract(palyerUpdateWeekTime);
        if (dt.Day != palyerUpdateTime.Day || dt.Month != palyerUpdateTime.Month || dt.Year != palyerUpdateTime.Year)
        {
            RestPlayerDayState(dt);
        }
        //每周日重置
        if (dt.DayOfWeek == DayOfWeek.Monday && span.Days > 1)
        {
            RestPlayerWeekState(dt);
        }
    }
    //检查体力回复
    public void CheckPlayerTl()
    {
        if (playerPower >= GameManager.TI_LI_MAX_NUMBER)
        {
            playerTlTime = DateTime.Now;
            return;
        }
        int number = (int)(DateTimeUtil.DateTimeToTimeStamp(DateTime.Now) - DateTimeUtil.DateTimeToTimeStamp(playerTlTime)) / GameManager.TI_LI_CD_NUMBER;
        int maxNumber = GameManager.TI_LI_MAX_NUMBER - playerPower;
        if (number > maxNumber)
        {
            number = maxNumber;
        }
        if (number > 0)
        {
            AwardToPlayer((int)PropId.Tl, number, false);
            long time = DateTimeUtil.DateTimeToTimeStamp(playerTlTime) + (number * GameManager.TI_LI_CD_NUMBER);
            playerTlTime = DateTimeUtil.TimeStampToDateTime(time);
        }
        return;
    }
    //每日状态重置
    public void RestPlayerDayState(DateTime dt)
    {
        palyerUpdateTime = dt;
        //重置每日任务状态
        foreach (var item in palyerTaskDic)
        {
            if (item.Value.tasktype == 1)
            {
                item.Value.taskState = 0;
                item.Value.taskTargetNumber = 0;
            }
        }
       


    }
    //每周状态重置
    public void RestPlayerWeekState(DateTime dt)
    {
        palyerUpdateWeekTime = dt;

        //重置玩家的七日签到数据
        
    }
   
    //完成单次任务
    public void TaskOnceRecord(string taskId, long number)
    {
        TaskInfo taskInfo;
        if (!palyerTaskDic.TryGetValue(taskId, out taskInfo))
        {
            return;
        }
        if (taskInfo.taskState == -1 || taskInfo.taskState == 1 || taskInfo.taskState == -2)
        {
            return;
        }
        if (taskInfo.taskTargetNumber < number)
        {
            taskInfo.taskTargetNumber = number;
        }

        TaskUpdateState(taskId, taskInfo);

        return;
    }
    //完成累计任务
    public void TaskAddRecord(string taskId, long number)
    {
        TaskInfo taskInfo;
        if (!palyerTaskDic.TryGetValue(taskId, out taskInfo))
        {
            return;
        }
        if (taskInfo.taskState == -1 || taskInfo.taskState == 1 || taskInfo.taskState == -2)
        {
            return;
        }
        //检查前置任务
        //if (taskInfo.taskPrepositionTaskID != 0)
        //{
        //    TaskInfo taskInfo1;
        //    if (!palyerTaskDic.TryGetValue(taskInfo.taskPrepositionTaskID.ToString(), out taskInfo1))
        //    {
        //        return;
        //    }
        //    if (taskInfo1.taskState != -1)
        //    {
        //        return;
        //    }
        //}

        taskInfo.taskTargetNumber += number;
        TaskUpdateState(taskId, taskInfo);

        return;
    }
    //更新玩家任务状态
    public void TaskUpdateState(string taskId, TaskInfo playerTaskInfo)
    {
        TaskInfo taskInfo;
        if (!palyerTaskDic.TryGetValue(taskId, out taskInfo))
        {
            return;
        }
        if (taskInfo.taskState != 0)
        {
            return;
        }
        long targetNumber = 0;
        foreach (var item in GameManager.instance.configMag.TaskInfoCfg)
        {
            if (item.ID.ToString() == taskId)
            {
                targetNumber = item.targetNumber;
            }
        }
        if (targetNumber == 0)
        {
            return;
        }
        long Number = targetNumber;
        if (playerTaskInfo.taskTargetNumber >= Number)
        {
            playerTaskInfo.taskState = 1;
        }

        return;
    }
    //获取玩家完成任务状态
    public bool GetTaskState(string taskId)
    {
        TaskInfo taskInfo;
        if (palyerTaskDic.TryGetValue(taskId, out taskInfo))
        {
            if (taskInfo.taskState == 1 || taskInfo.taskState == -1)
            {
                return true;
            }
        }
        return false;
    }
    //任务红点检查
    public bool CheckTaskRedDot()
    {
        foreach (var item in palyerTaskDic)
        {
            if (item.Value.taskPrepositionTaskID == 0)
            {
                if (item.Value.taskState == 1)
                {
                    return true;
                }
            }
            else
            {
                //检查前置任务
                TaskInfo taskInfo = new TaskInfo();
                if (palyerTaskDic.TryGetValue(item.Value.taskPrepositionTaskID.ToString(), out taskInfo))
                {
                    if (taskInfo.taskState == -1)
                    {
                        if (item.Value.taskState == 1)
                        {
                            return true;
                        }
                    }
                }
            }
          
        }
        return false;
    }
}


public class Bait
{
    public Sprite sprite;
    public int id;
    public int price;
}

/// <summary>
/// profit1 增加当前设施钓鱼收益
/// profit2 增加钓鱼速度
/// profit3 增加离线收益
/// profit4 增加离线时长
/// profit5 缩短顾客来访间隔时长
/// profit6 增加鱼塘鱼量投放上限
/// profit7 每人收入增加（11.22新增）
/// profit8 增加鱼饵收益（百分比增加 1.14新增）
/// profit9 增加所有基础钓位收益（百分比增加 1.14新增）
/// 返回值都是int 具体数值怎么算问策划
/// </summary>
public class FacilitiesProfit
{
    public int profit1 = 0;     
    public int profit2 = 0;
    public int profit3 = 0;
    public int profit4 = 0;
    public int profit5 = 0;
    public int profit6 = 0;
    public int profit7 = 0;
    public int profit8 = 0;
    public int profit9 = 0;
}


//用具数据
public class CatPropData : IComparable<CatPropData>
{
    public int id = 0;
    private int state = 0;   //0未解锁1已解锁-1已购买-2已使用
    public int State
    {
        set
        {
            state = value;
        }
        get
        {
            return state;
        }
    }
    public int sort = 0;
    public int useNumber = 0;
    public int useCatId = 0;        //正在被哪只猫占用

    public int type = 1;            //用具类型1普通用具2猫碗
    public int catFoodId = 0;       //放入的猫饭猫粮id
    public int catFoodNumber = 0;   //剩余食用次数

    public int CompareTo(CatPropData other)
    {
        //return other.sort.CompareTo(this.sort);
        if (other.State > this.State)
        {
            return 1;
        }
        else if (other.State == this.State)
        {
            return -other.sort.CompareTo(this.sort);
        }
        else
        {
            return -1;
        }
    }
}

//设施数据
public class FacilitiesData
{
    public int id = 0;              //id
    public int unlockState = 0;     //解锁状态(0未解锁1已解锁2已购买3使用中)
    public int grade = 0;           //等级
    public int typeId;              //类型id（同类型设施只会采用最高等级的）
}
//设施类型数据
public class FacilitiesTypeData
{
    public int typeId = 0;              //设施类型id
    public int state = 0;               //设施类型状态（0未解锁1已解锁2已购买）
    public int showFacilitiesId = 0;        //外观展示设施id
}
//设施玩家数据
public class FacilitiesPlayerData
{
    public Dictionary<String, FacilitiesData> facilitiesDataDic = new Dictionary<String, FacilitiesData>();                     //设施数据
    public Dictionary<String, FacilitiesTypeData> facilitiesTypeDataDic = new Dictionary<String, FacilitiesTypeData>();         //设施类型数据
}
//任务数据
public class TaskInfo : IComparable<TaskInfo>
{
    public TaskInfo()
    {
        taskId = 0;
        taskState = 0;
        taskTargetNumber = 0;
        tasktype = 0;
        taskPrepositionTaskID = 0;
        isUnlock = false;
    }
    public int taskId;                     //任务id
    public int taskState;                  //完成状态（0未完成1已完成-1已领奖-2未解锁）
    public long taskTargetNumber;          //已完成目标任务
    public int tasktype;                   //任务类型（1日常2周长3成长4线索）
    public int taskPrepositionTaskID;      //前置任务ID
    public bool isUnlock;                  //是否解锁

    public int CompareTo(TaskInfo other)
    {
        //return other.taskState.CompareTo(this.taskState);
        if (other.taskState > this.taskState)
        {
            return 1;
        }
        else if (other.taskState == this.taskState)
        {
            return -other.taskId.CompareTo(this.taskId);
        }
        else
        {
            return -1;
        }
    }
}

//员工数据 
public class StaffData
{
    public int id = 0;                //员工ID
    public int is_unlock = 0;         //解锁状态(0未解锁1已解锁2已购买3使用中)
    public int level = 1;             //员工等级（默认等级1级等级上限为10级）
    public int Sw=0;                  //声望
}

// 员工玩家数据
public class StaffPlayerData
{
    public Dictionary<string, StaffData> staffDataDic = new Dictionary<string, StaffData>();//员工数据 
}

// 七日签到数据
public class SignInData
{
    public int data;                   //1-7  一周七天
    public int is_unlock;              //解锁状态(0未解锁1已解锁)
    public int is_double;              //是否双倍领取(0未领取1领取1倍 2领取两倍)
    public bool isReward;              //是否领取奖励
}

//签到记录
public class SignInRecord
{
    public int getRewardTime;           //玩家已经领取了几天的奖励
    public int currTime;                //当天的奖励天数
    public int isRush;                  // 当天的奖励是否领取
}

//七日签到玩家数据
public class SignInPlayerData
{
    public Dictionary<string, SignInData> signInDataDic = new Dictionary<string, SignInData>();
}

//商店看广告刷新物品的玩家数据
public class ShopItem_adv
{
    public int id;//商品ID
    public int advTime;//看广告刷新的次数
    public int isBuy;//是否被购买
}

//商店消耗货币购买的物品的玩家数据
public class ShopItem_other
{
    public int id;//商品ID
    public int isBuy;//是否被购买
}

//商店当日的物品
public class ShopItemCurrRushItem
{
    public int pos;
    public int id;
}

//商店的玩家数据
public class ShopItemData
{
    public Dictionary<string,ShopItem_adv> shopItem_advDataDic=new Dictionary<string, ShopItem_adv>();
    public Dictionary<string, ShopItem_other> shopItem_otherDataDic = new Dictionary<string, ShopItem_other>();
}
//玩家拥有的道具
public class Player_Materials
{
    public int id;
    public int num;
}
//玩家拥有的道具数据表
public class Player_MaterialsData
{
    public Dictionary<string, Player_Materials> player_MaterialsDataDic = new Dictionary<string, Player_Materials>();
}

//玩家拥有的道具
public class PlayerShopItem
{
    public int id;//道具ID 
    public long num;//道具数量
}
//玩家具有的道具数据
public class PlayerShopItemData
{
  public  Dictionary<string, PlayerShopItem> playerShopItemData = new Dictionary<string, PlayerShopItem>();
}
//鱼——解锁升级
public class Fish_Item
{
    public int id;//id 
    public int unlock;//是否解锁0--未解锁 1--解锁
    public int currGrade; //当前鱼的等级
    public int Sw=0;//声望
    public int qidiaoNum = 0;
    public int siyangNum = 0;   //饲养数量  累计记录
    public int toufangNum = 0;  //投放数量  累计记录
}
//鱼解锁升级--玩家数据
public class Fish_ItemPlayerData
{
    public Dictionary<string, Fish_Item> fish_ItemPlayerDataDic = new Dictionary<string, Fish_Item>();
}
//鱼——鱼苗
public class FishItem_fry
{
    public int id;//id 
    public int currGrade; //当前鱼的等级
    public int number;//鱼苗的数量
}
//鱼 ---鱼苗玩家数据
public class FishItem_fryPlayerData
{
    public Dictionary<string, FishItem_fry> fishItem_fryPlayerDataDic = new Dictionary<string, FishItem_fry>();
}

//鱼——可以投放的鱼
public class FishItem_big
{
    public int id;//id 
    public int currGrade; //当前鱼的等级
    public int number;//成品鱼的数量
}





// 渔场等级数据
public class GradeData
{
    public int currGrade;//玩家当前等级
    public int currGrade_id;//玩家当前等级的ID
    public int nextGrade_id;//下一等级的ID
}

//五个养鱼池的状态
public class FishFarmingStateData
{
    public int id;//养鱼池的编号 1,2号默认解锁  3,4,5看广告解锁
    public bool isUnlock;//是否解锁
    public int fish_id;//没有鱼ID为0
    public int fish_num;//鱼的数量
    public DateTime CloseTime;// 离开鱼池的时间
}
/// <summary>
/// AdultFishData 成鱼数据
/// </summary>
public class AdultFishData
{
    public int id;//编号 
    public int number;//鱼的数量
}

/// <summary>
/// 钓鱼结算类
/// </summary>
public class FishSettlementData
{
    public int id  = 0;             //编号 
    public int smallNumber = 0;     //鱼的数量
    public int bigNumber = 0;       //鱼的数量
}
/// <summary>
/// 游戏时间类
/// </summary>
public class GameTime
{
    public float allTime=160;//总的在线时间1440秒=1月  480秒=一天   20秒=一小时
    public int month=1;//月
    public int day=1;//日
    public int hour=8;//小时
    public int minute=0;//分钟
    public int week = 1;
    public DayType type = DayType.daytime;
}
//今日鱼情
public class ToDayFishInfo
{
    public int fish_1_id=0;
    public int fish_2_id;
    public int fish_3_id;
    public int fish_4_id;
}

public enum PropId : int
{
    Jb = 8000,                        //金币
    Zs = 8001,                        //钻石
    Tl = 8002,                          //声望
    Cl1 = 8003,                          //声望

    JP = 2026,                           //员工奖牌
    YG2027 = 2027,                       //超级鱼竿
    YX2028 = 2028,                       //超级鱼线
    YG2029 = 2029,                       //超级鱼钩
    YE2030 = 2030,                       //超级鱼饵
    HL2031 = 2031,                       //超级滑轮
    //设施的id 从10000开始到11000 你们不要重复了
    //员工id 从1001开始到1999
    XiaoRong =1001,                    //小荣
    GuZai =1002,                      //古仔
    HuaQiang =1003,                   //华强
    OuYangXiaoXu =1004,               //欧阳晓栩
    DengGang =1005,                   //邓岗
    LaoZhang =1006,                   //老张
    ChunTao =1007,                    //春桃
    XiaHe =1008,                      //夏荷
}
public enum TaskId
{
    Task_30001 = 30001,                           //
    Task_30002 = 30002,                           //
    Task_30003 = 30003,                           //
    Task_30004 = 30004,                           //
    Task_30005 = 30005,                           //
    Task_30006 = 30006,                           //
    Task_30007 = 30007,                           //
    Task_30008 = 30008,                           //
    Task_30009 = 30009,                           //
    Task_40001 = 40001,                           //
    Task_40002 = 40002,                           //
    Task_40003 = 40003,                           //
    Task_40004 = 40004,                           //
    Task_40005 = 40005,                           //
    Task_40006 = 40006,                           //
    Task_40007 = 40007,                           //
    Task_40008 = 40008,                           //
    Task_40009 = 40009,                           //
    Task_40010 = 40010,                           //
    Task_40011 = 40011,                           //
    Task_40012 = 40012,                           //
    Task_40013 = 40013,                           //
    Task_40014 = 40014,                           //
    Task_40015 = 40015,                           //
    Task_40016 = 40016,                           //
    Task_40017 = 40017,                           //
    Task_40018 = 40018,                           //
    Task_40019 = 40019,                           //
    Task_40020 = 40020,                           //
    Task_40021 = 40021,                           //
    Task_40022 = 40022,                           //
    Task_40023 = 40023,                           //
    Task_40024 = 40024,                           //
    Task_40025 = 40025,                           //
    Task_40026 = 40026,                           //
    Task_40027 = 40027,                           //
    Task_40028 = 40028,                           //
    Task_40029 = 40029,                           //
    Task_40030 = 40030,                           //
    Task_40031 = 40031,                           //
    Task_40032 = 40032,                           //
    Task_40033 = 40033,                           //
    Task_40034 = 40034,                           //    
    Task_40035 = 40035,                           //
    Task_40036 = 40036,                           //
    Task_40037 = 40037,                           //
    Task_40038 = 40038,                           //
    Task_40039 = 40039,                           //
    Task_40040 = 40040,                           //
}
//设施类型ID枚举
public enum FacilitiesTypeId : int
{
    FacilitiesType_1 = 1,               //1号钓鱼台
    FacilitiesType_2 = 2,               //2号钓鱼台
    FacilitiesType_3 = 3,               //3号钓鱼台
    FacilitiesType_4 = 4,               //4号钓鱼台
    FacilitiesType_5 = 5,               //5号钓鱼台
    FacilitiesType_6 = 6,               //6号钓鱼台
    FacilitiesType_7 = 7,               //路面
    FacilitiesType_8 = 8,               //绿化树木
    FacilitiesType_9 = 9,               //花丛
    FacilitiesType_10 = 10,             //路灯
    FacilitiesType_11 = 11,             //大门
    FacilitiesType_12 = 12,             //墙面
    FacilitiesType_13 = 13,             //牌匾
    FacilitiesType_14 = 14,             //广告牌
    FacilitiesType_15 = 15,             //氧气机
    FacilitiesType_16 = 16,             //动物
    FacilitiesType_17 = 17,             //水上植物
    FacilitiesType_18 = 18,             //货柜
    FacilitiesType_19 = 19,             //默认钓位
}

//白天黑夜
public enum DayType
{
    daytime,
    night,
}

public class SyntheticPos
{
    public int id;
    public int pos;
}

//祈福数据
public class QiFuData
{
    public int day = 0;
}
//拾取文字记录记录
public class PropPickTextData
{
    public int PropID = 0;//道具ID
    public int TextID = 0 ;//文字ID
    public int PropNum = 0;//道具数量
    public string TimeStr = "";//时间
}
//拾取记录
public class PropPickData
{
    public int id;//道具ID 
    public int num = 0;//道具数量
    public int ascription=0;      //拾取方向 1、合成小游戏拾取 2、货币拾取（体力、钻石、货币）3、道具拾取
}
//拾取列表
public class PropPickList
{
    //文字记录列表
    public  List<PropPickTextData> propPickTextList = new List<PropPickTextData>();
    //道具记录列表
    public  List<PropPickData> propPickList = new List<PropPickData>();
    public bool AddPropPick(int PropID, int PropNum,int ascription, int TextID,string TimeStr = "")
    {
        if (propPickList.Count < 10)
        {
            PropPickData propPickData = propPickList.Find(c => c.id == PropID);
            if (propPickData != null)
            {
                propPickData.num += PropNum;
                if (propPickData.ascription == 0)
                {
                    propPickData.ascription = ascription;
                }
                AddPropPickText(PropID, PropNum, TextID,TimeStr);
                return true;
            }
            else
            {
                propPickData = new PropPickData();
                if (propPickData != null)
                {
                    propPickData.id = PropID;
                    propPickData.ascription = ascription;
                    propPickData.num = PropNum;
                }
                propPickList.Add(propPickData);
                AddPropPickText(PropID, PropNum, TextID, TimeStr);
                return true;
            }

        }
        else
        {
            PropPickData propPickData =  propPickList.Find(c => c.id == PropID);
            if (propPickData != null )
            {
                propPickData.num += PropNum;
                if (propPickData.ascription == 0)
                {
                    propPickData.ascription = ascription;
                }
                AddPropPickText(PropID, PropNum, TextID, TimeStr);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    private void AddPropPickText(int PropID, int PropNum, int TextID, string TimeStr = "")
    {
        if (propPickTextList.Count < 20)
        {
            PropPickTextData propPickData = new PropPickTextData();
            propPickData.PropID = PropID;
            propPickData.PropNum = PropNum;
            propPickData.TextID = TextID;
            propPickData.TimeStr = TimeStr;
            propPickTextList.Add(propPickData);
        }
        else
        {
            PropPickTextData propPickData = propPickTextList[0];
            propPickData.PropID = PropID;
            propPickData.PropNum = PropNum;
            propPickData.TextID = TextID;
            propPickData.TimeStr = TimeStr;
            propPickTextList.RemoveAt(0);
            propPickTextList.Add(propPickData);
        }
    }
}

//自动合成数据
public class AutoBuffData
{
    public float AutoBuffTime = 0;                           //自动合成buff的时间
    public long SartTime = 0;                             //开始时间
    public float currTime = 0;
}

//换装数据
public class HomeChangeData
{
    public int FurnitureTypeId=0;       //换装道具类型(换装设施)
    public int PropId = 0;              //换装道具id（皮肤id）
    public int Floor = 0;               //楼层
    public int state = 0;               //0未解锁 1已解锁- 1已购买-2已使用
}
//换装数据
public class HomeChangeDataList
{
    public List<HomeChangeData> homeChangeDataList = new List<HomeChangeData>();

    //根据楼层获取楼层中的换装数据
    public List<HomeChangeData> GetHomeChangeDataListByFloor(int Floor)
    {
        return homeChangeDataList.FindAll(c => c.Floor == Floor);
    }
    //根据换装道具类型(换装设施) 获取换装数据
    public List<HomeChangeData> GetHomeChangeDataByFurnituresTypeId(int FurnitureTypeId)
    {
        return homeChangeDataList.FindAll(c => c.FurnitureTypeId == FurnitureTypeId);
    }
    //根据换装道具类型(换装设施) 获取当前使用换装数据
    public HomeChangeData GetHomeChangeDataByFurnitureTypeId(int FurnitureTypeId)
    {
        return homeChangeDataList.Find(c => c.FurnitureTypeId == FurnitureTypeId&& c.state == 2);
    }
    //根据换装道具ID(换装设施) 获取换装数据
    public HomeChangeData GetHomeChangeDataByPropId(int PropId)
    {
        return homeChangeDataList.Find(c => c.PropId == PropId);
    }
    //换装购买
    public void HomeChangeBuy(int FurnitureTypeId, int PropId, int Floor)
    {
        HomeChangeData homeChangeData;
        bool ischange = false;
        for (int i = 0; i < homeChangeDataList.Count; i++)
        {
            homeChangeData = homeChangeDataList[i];
            if (homeChangeData.PropId == PropId)  // if (homeChangeData.FurnitureTypeId == FurnitureTypeId)//&& homeChangeData.Floor == Floor
            {
                ischange = true;
                homeChangeData.state = 2;
            }
        }
        if (ischange == false)
        {
            AddHomeChangeData(FurnitureTypeId, PropId, Floor, 2);
        }
        UpdateHomeChangeData(FurnitureTypeId, PropId);

    }

    //刷新换装数据
    public void UpdateHomeChangeData(int FurnitureTypeId, int PropId)
    {
        HomeChangeData homeChangeData;
        for (int i = 0; i < homeChangeDataList.Count; i++)
        {
            homeChangeData = homeChangeDataList[i];
            if (homeChangeData.FurnitureTypeId ==  FurnitureTypeId && homeChangeData.PropId == PropId)
            {
                homeChangeData.state = 2;
            }
            else if(homeChangeData.FurnitureTypeId == FurnitureTypeId)
            {
                homeChangeData.state = 1;
            }

        }       
    }
    //新增换装数据
    private void AddHomeChangeData(int FurnitureTypeId, int PropId, int Floor, int state)
    {
        HomeChangeData homeChangeData = new HomeChangeData();
        homeChangeData.FurnitureTypeId = FurnitureTypeId;
        homeChangeData.PropId = PropId;
        homeChangeData.Floor = Floor;
        homeChangeData.state = state;
        homeChangeDataList.Add(homeChangeData);
    }
}
//鱼饵消消乐玩家数据
public class SheepGameData
{
    public int currLevelID = 1;//当前关卡
    public int gameTime = 3;//游戏次数
    public int rushTime = 1;//刷新次数
    public int returnTime = 1;//删除三个次数
    public int withdrawTime = 1;//返回上一步操作
    public int reviewTime = 1;//复活次数
}

public class GuideData
{
    public int guidetype;
    public bool guidetype_isTrue = false;//该阶段的新手引导是否完成 true -完成 FALSE-未完成
    public bool isExecution = false;//是否执行过
}
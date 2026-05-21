using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 游戏内语言文字
/// </summary>
public class LanguageCfg
{
    public string key="";                             //索引值
    public string content = "";                       //文字内容
}

/// <summary>
/// 任务配置表
/// </summary>
public class TaskInfoCfg
{
    public int ID;						    //ID
    public string system;					//所属系统
    public string describe;				    //描述
    public long targetNumber;			    //目标数
    public int prepositionTaskID;		    //前置任务ID
    public string award;					//奖励
    public int type;					    //类型（1日常2周长3成长4线索）
    public int open;					    //开关（0关闭1开启）
    public int jump;					    //跳转目的（0没有跳转）
    public int iocn;					    //图标
    public int gameID;					    //对应的游戏id
    public int rank;					    //排序
    public int frame;					    //外框
    public int numberType;				    //任务累计类型（1限制类2累积类型）
    public int taskType;				    //任务类型客户端分类用
    public string awardVip;					//战令奖励
    public int needUnlock;                  //是否需要解锁（0不需要1需要）
    public int vitality;                    //活跃度展示
}


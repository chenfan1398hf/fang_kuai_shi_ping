using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFSMMgr
{
    public CharacterAnimationCtrl charactorCtrl=null;//角色控制器
    public RoleState currStateEnum { get; private set; }//当前角色的状态枚举类型
    public CharacterStateAbstract currStateAbstract = null;//当前的角色动画状态
    private Dictionary<RoleState, CharacterStateAbstract> mStateDic;//角色动画字典

    public CharacterFSMMgr(CharacterAnimationCtrl  _Ctrl)
    {
        charactorCtrl = _Ctrl;
        mStateDic = new Dictionary<RoleState, CharacterStateAbstract>();
        //注册状态保存在字典里
        mStateDic[RoleState.Idle] = new CharacterStateIdle(this);
        mStateDic[RoleState.Attack] = new CharacterStateAttack(this);
        mStateDic[RoleState.Run] = new CharacterStateRun(this);
        mStateDic[RoleState.Death] = new CharacterStateDeath(this);
        mStateDic[RoleState.Hurt] = new CharacterStateHurt(this);

        //初始化当前状态
        if (mStateDic.ContainsKey(currStateEnum))
        {
            currStateAbstract = mStateDic[currStateEnum];
        }
    }

    #region OnUpdate() 保证动画每帧执行
    public void OnUpdate()
    {
        if (currStateAbstract != null)
        {
            currStateAbstract.OnUpdate();
        }
    }
    #endregion

    #region  ChangeState(RoleState newState) 改变角色状态
    /// <summary>
    /// ChangeState(RoleState newState) 改变角色状态
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(RoleState newState)
    {
        if (currStateAbstract != null)//当前状态未结束调用结束
        {
            currStateAbstract.OnLeave();
        }
        currStateEnum = newState;//更新当前的状态枚举
        currStateAbstract = mStateDic[newState];//更换新的动作
        currStateAbstract.OnEnter();//开始执行新的动作
        //Debug.Log("角色__"+roleCtrl.jobType+"__执行新的动作：" + newState.ToString());
    }
    public void ChangeState(RoleState newState, RoleAnimatorName name)
    {
        if (currStateAbstract != null)//当前状态未结束调用结束
        {
            currStateAbstract.OnLeave();
        }
        currStateEnum = newState;//更新当前的状态枚举
        currStateAbstract = mStateDic[newState];//更换新的动作
        currStateAbstract.OnEnter(name);//开始执行新的动作
        //Debug.Log("执行新的动作：" + newState.ToString());
    }
    #endregion
}

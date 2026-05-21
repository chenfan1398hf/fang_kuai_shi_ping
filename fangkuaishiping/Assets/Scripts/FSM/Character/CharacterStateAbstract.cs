using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateAbstract 
{
    public CharacterFSMMgr currFSMMgr { get; private set; } //当前角色的状态机管理
    public AnimatorStateInfo currAnimatorStateInfo { get; set; }//当前动画状态信息
    //构造函数
    public CharacterStateAbstract(CharacterFSMMgr currFSMMgr)
    {
        this.currFSMMgr = currFSMMgr;
    }
    public virtual void OnEnter() { }//进入状态

    public virtual void OnEnter(RoleAnimatorName name) { }//进入状态

    public virtual void OnUpdate() { }//执行状态

    public virtual void OnLeave() { }//离开状态
}

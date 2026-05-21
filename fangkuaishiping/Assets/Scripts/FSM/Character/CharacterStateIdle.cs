using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateIdle : CharacterStateAbstract
{

    public CharacterStateIdle(CharacterFSMMgr currFSMMgr) : base(currFSMMgr) { }

    public override void OnEnter()
    {
        base.OnEnter();
        currFSMMgr.charactorCtrl.mAnimator.SetBool(ToAnimatorCondition.ToIdle.ToString(), true);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        currAnimatorStateInfo = currFSMMgr.charactorCtrl.mAnimator.GetCurrentAnimatorStateInfo(0);//获取当前的动画状态

        if (currAnimatorStateInfo.IsName(RoleAnimatorName.Idle.ToString()))
        {
            currFSMMgr.charactorCtrl.mAnimator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleState.Idle);
        }
    }

    public override void OnLeave()
    {
        base.OnLeave();
        currFSMMgr.charactorCtrl.mAnimator.SetBool(ToAnimatorCondition.ToIdle.ToString(), false);
    }
}

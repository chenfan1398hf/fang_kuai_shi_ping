using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateRun : CharacterStateAbstract
{
    public CharacterStateRun(CharacterFSMMgr currFSMMgr) : base(currFSMMgr) { }

    public override void OnEnter()
    {
        base.OnEnter();
        currFSMMgr.charactorCtrl.mAnimator.SetBool(ToAnimatorCondition.ToRun.ToString(), true);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        currAnimatorStateInfo = currFSMMgr.charactorCtrl.mAnimator.GetCurrentAnimatorStateInfo(0);//获取当前的动画状态

        if (currAnimatorStateInfo.IsName(RoleAnimatorName.Run.ToString()))
        {
            currFSMMgr.charactorCtrl.mAnimator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleState.Run);
        }
    }

    public override void OnLeave()
    {
        base.OnLeave();
        currFSMMgr.charactorCtrl.mAnimator.SetBool(ToAnimatorCondition.ToRun.ToString(), false);
    }
}

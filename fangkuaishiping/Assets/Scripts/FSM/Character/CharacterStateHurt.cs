using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateHurt : CharacterStateAbstract
{
    public CharacterStateHurt(CharacterFSMMgr currFSMMgr) : base(currFSMMgr) { }

    public override void OnEnter()
    {
        base.OnEnter();
        currFSMMgr.charactorCtrl.mAnimator.SetBool(ToAnimatorCondition.ToHurt.ToString(), true);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        currAnimatorStateInfo = currFSMMgr.charactorCtrl.mAnimator.GetCurrentAnimatorStateInfo(0);//获取当前的动画状态

        if (currAnimatorStateInfo.IsName(RoleAnimatorName.Hurt.ToString()))
        {
            currFSMMgr.charactorCtrl.mAnimator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleState.Hurt);
        }
    }

    public override void OnLeave()
    {
        base.OnLeave();
        currFSMMgr.charactorCtrl.mAnimator.SetBool(ToAnimatorCondition.ToHurt.ToString(), false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateDeath : CharacterStateAbstract
{
    public CharacterStateDeath(CharacterFSMMgr characterFSMMgr) : base(characterFSMMgr) {}

    public override void OnEnter()
    {
        base.OnEnter();
        currFSMMgr.charactorCtrl.mAnimator.SetBool(ToAnimatorCondition.ToDeath.ToString(), true);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        currAnimatorStateInfo = currFSMMgr.charactorCtrl.mAnimator.GetCurrentAnimatorStateInfo(0);//获取当前的动画状态

        if (currAnimatorStateInfo.IsName(RoleAnimatorName.Death.ToString()))
        {
            currFSMMgr.charactorCtrl.mAnimator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleState.Death);
        }
    }

    public override void OnLeave()
    {
        base.OnLeave();
        currFSMMgr.charactorCtrl.mAnimator.SetBool(ToAnimatorCondition.ToDeath.ToString(), false);
    }
}

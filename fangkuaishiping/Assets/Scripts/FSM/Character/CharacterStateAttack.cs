using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateAttack :CharacterStateAbstract
{
    public CharacterStateAttack(CharacterFSMMgr currFSMMgr) : base(currFSMMgr) { }
    public override void OnEnter()
    {
        base.OnEnter();
        currFSMMgr.charactorCtrl.mAnimator.SetBool(ToAnimatorCondition.ToAttack.ToString(), true);
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        currAnimatorStateInfo = currFSMMgr.charactorCtrl.mAnimator.GetCurrentAnimatorStateInfo(0);
        if (currAnimatorStateInfo.IsName(RoleAnimatorName.Attack.ToString()))
        {
            currFSMMgr.charactorCtrl.mAnimator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleState.Attack);
            if (currAnimatorStateInfo.normalizedTime > 1)
            {
                currFSMMgr.charactorCtrl.ToIdle();
            }
        }

    }

    public override void OnLeave()
    {
        base.OnLeave();
        currFSMMgr.charactorCtrl.mAnimator.SetBool(ToAnimatorCondition.ToAttack.ToString(), false);

    }

}

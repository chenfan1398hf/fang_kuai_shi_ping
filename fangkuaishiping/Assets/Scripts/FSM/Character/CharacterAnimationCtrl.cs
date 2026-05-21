using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationCtrl : MonoBehaviour
{
    public Animator mAnimator;
    private CharacterFSMMgr currFSMMgr;
    public void Init()
    {
        mAnimator = GetComponent<Animator>();
        currFSMMgr = new CharacterFSMMgr(this);
    }
    void Update()
    {
        currFSMMgr.OnUpdate();
    }

    #region ToIdle()
    public void ToIdle()
    {
        currFSMMgr.ChangeState(RoleState.Idle);
    }
    #endregion

    #region ToRun()
    public void ToRun()
    {
        currFSMMgr.ChangeState(RoleState.Run);
    }
    #endregion

    #region ToAttack()
    public void ToAttack()
    {
        currFSMMgr.ChangeState(RoleState.Attack);
    }
    #endregion

    #region ToDie()
    public void ToDeath()
    {
        currFSMMgr.ChangeState(RoleState.Death);
    }
    #endregion

    #region ToHurt()
    public void ToHurt()
    {
        currFSMMgr.ChangeState(RoleState.Hurt);
    }
    #endregion

}


#region RoleState角色动作状态
public enum RoleState
{
    None = 0,//未设置
    Idle = 1,//待机
    Run=2,//跑
    Attack = 3,//攻击
    Hurt = 4,//受伤
    Death = 5//死亡
}
#endregion

#region RoleAnimatorName角色状态名称
public enum RoleAnimatorName
{
    Idle,
    Attack,
    Hurt,
    Death,
    Run,
}
#endregion

#region RoleAnimatorChangeName切换角色动画名称
public enum ToAnimatorCondition
{
    ToIdle,
    ToAttack,
    ToDeath,
    ToHurt,
    ToRun,
    CurrState
}
#endregion

#region 角色
//职业类型
public enum RoleJobType
{
    ignis=1001,//火系
    ice=1002,//冰系
    thunder=1003,//雷系

}
//位置
public enum RolePosType
{
    left=1,//左侧
    center=2,//中间
    right=3,//右侧
}
#endregion


#region MonsterState动作状态
public enum MonsterState
{
    None = 0,//未设置
    Idle = 1,//待机
    Walk = 2,//走
    Hurt = 3,//受伤
    Attack = 4,//攻击
    Die = 5//死亡
}
#endregion

#region MonsterAnimatorName状态名称
public enum MonsterAnimatorName
{
    Idle,
    Walk,
    Attack,
    Hurt,
    Die,
}
#endregion

#region MonsterToAnimatorCondition切换动画名称
public enum MonsterToAnimatorCondition
{
    ToIdle,
    ToAttack,
    ToWalk,
    ToHurt,
    ToDie,
    CurrState
}
#endregion
using UnityEngine;
using System.Collections;

/// <summary>类 单例模板
/// </summary>
/// <typeparam name="TYPE"></typeparam>
public class Singleton<TYPE> where TYPE : new()
{
    #region Instance Define
    static TYPE inst_ = default(TYPE);
    static public TYPE instance
    {
        get
        {
            if (inst_ == null)
            {
                inst_ = new TYPE();
            }
            return inst_;
        }
        protected set { inst_ = value; }
    }
    #endregion

    protected Singleton() { }
    private Singleton(ref Singleton<TYPE> instance) { }
    private Singleton(Singleton<TYPE> instance) { }


    public virtual void Init() { }
}


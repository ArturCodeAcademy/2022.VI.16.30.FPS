using System;
using UnityEngine;

public class PoolElement : MonoBehaviour
{
    protected PoolBase<PoolElement> _pool;

    private Action _onRelease;

    public virtual void OnCreateElement(Action onRelease)
    {
        _onRelease = onRelease;
    }

    public virtual void OnGetElement()
    {

    }

    public virtual void OnReturnElement()
    {
       
    }

    public virtual void OnDestroyElement()
    {

    }

    protected void Release()
    {
        _onRelease?.Invoke();
    }
}

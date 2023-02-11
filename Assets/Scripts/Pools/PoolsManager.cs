using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolsManager : MonoBehaviour
{
    public static PoolsManager Instance { get; private set; }

    private List<PoolBase<PoolElement>> _pools;

    private void Awake()
    {
        Instance = this;
        _pools = new List<PoolBase<PoolElement>>(GetComponents<PoolBase<PoolElement>>());
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public T GetPool<T>(Func<T, bool> predicate = null) where T : PoolBase<PoolElement>
    {
        foreach (var p in _pools)
            if (p is T pool && (predicate?.Invoke(pool) ?? true))
                return pool;
        return null;
    }

    public bool TryGetPool<T>(out T poolRes, Func<T, bool> predicate = null) where T : PoolBase<PoolElement>
    {
        foreach (var p in _pools)
            if (p is T pool && (predicate?.Invoke(pool) ?? true))
            {
                poolRes = pool;
                return true;
            }
        poolRes = null;
        return false;
    }

    public PoolElement GetElement<T>(Func<T, bool> predicate = null) where T : PoolBase<PoolElement>
    {
        foreach (var p in _pools)
            if (p is T pool && (predicate?.Invoke(pool) ?? true))
                return pool.GetElement();
        return null;
    }

    public bool TryGetElement<T>(out PoolElement element, Func<T, bool> predicate = null) where T : PoolBase<PoolElement>
    {
        element = default;
        foreach (var p in _pools)
            if (p is T pool && (predicate?.Invoke(pool) ?? true))
            {
                element = pool.GetElement();
                return true;
            }
        return false;
    }
}

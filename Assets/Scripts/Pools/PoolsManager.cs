using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolsManager : MonoBehaviour
{
    public static PoolsManager Instance { get; private set; }

    [SerializeField] List<PoolBase<IPoolElement>> _pools;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _pools = new List<PoolBase<IPoolElement>>(GetComponents<PoolBase<IPoolElement>>());
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public IPoolElement GetElement<T>() where T : PoolBase<IPoolElement>
    {
        foreach (var pool in _pools)
            if (pool is T poolT)
                return poolT.GetElement();
        return null;
    }
}

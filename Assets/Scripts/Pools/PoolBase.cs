using UnityEngine;
using UnityEngine.Pool;

public abstract class PoolBase<T> : MonoBehaviour where T : PoolElement
{
    [field: SerializeField] public string Name { get; private set; }
    [SerializeField] protected MonoBehaviour[] _prefabs;

    protected ObjectPool<T> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<T>(CreateNewElement, OnGetElement,
            OnReturnElement, OnDestroyElement);
    }

    protected virtual T CreateNewElement()
    { 
        MonoBehaviour mb = _prefabs[Random.Range(0, _prefabs.Length)];
        T element = Instantiate(mb.GetComponent<T>());
        element.OnCreateElement(() => ReleaseElement(element));
        return element;
    }

    protected virtual void OnGetElement(T element)
    {
        element.OnGetElement();
    }

    protected virtual void OnReturnElement(T element)
    {
        element.OnReturnElement();
    }

    protected virtual void OnDestroyElement(T element)
    {
        element.OnDestroyElement();
    }

    public T GetElement()
    {
        return _pool.Get();
    }

    private void ReleaseElement(T element)
    {
        _pool.Release(element);
    }
}

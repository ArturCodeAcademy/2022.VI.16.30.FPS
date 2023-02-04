using System.Collections.Generic;
using UnityEngine;

public abstract class PoolBase<T> : MonoBehaviour where T : class, IPoolElement
{
    [SerializeField] protected MonoBehaviour[] _prefabs;

    protected Queue<T> _queue;

    protected virtual T CreateNewElement()
    { 
        MonoBehaviour mb = _prefabs[Random.Range(0, _prefabs.Length)] as MonoBehaviour;
        //return Instantiate(mb.GetComponent<T>());
        return null;
    }

    public T GetElement()
    {
        if (_queue.Count == 0)
            _queue.Enqueue(CreateNewElement());

        T element = _queue.Dequeue();
        element.ResetElement();
        element.OnElementUsed = () =>
        {
            _queue.Enqueue(element);
        };
        return element;
    }
}

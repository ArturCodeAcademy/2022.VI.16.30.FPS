using System;
using System.Linq;
using UnityEngine;

using static UnityEngine.Random;

public class Dropper : MonoBehaviour
{
    [SerializeField] private Drop[] _drops;
    [SerializeField, Min(0)] private float _dropForce;
    [SerializeField] private bool _destroyAfterUse;
    [SerializeField, Min(1)] private int _dropCount;
    [SerializeField] private Vector3 _dropLocalPos;

    private float _probabilitySum;

    private void Start()
    {
        _probabilitySum = _drops.Sum(x => x.Probability);
    }

    public void DropObject()
    {
        float p = Range(0, _probabilitySum);
        foreach (Drop drop in _drops)
        {
            if (drop.Probability < p)
            {
                p -= drop.Probability;
                continue;
            }

            if (drop.Prefab == null)
                break;

            GameObject newDrop = Instantiate(drop.Prefab,
                transform.position + _dropLocalPos, Quaternion.identity);

            if (newDrop.TryGetComponent(out Rigidbody rb))
            {
                Vector3 dir = insideUnitSphere;
                dir.y = Mathf.Abs(dir.y);
                rb.AddForce(dir * _dropForce, ForceMode.Impulse);
            }

            break;
        }

        if (_destroyAfterUse)
        {
            _dropCount--;
            if (_dropCount == 0)
                Destroy(gameObject);
        }
    }

    [Serializable]
    public class Drop
    {
        public GameObject Prefab;
        [Range(0, 1)] public float Probability;
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        const float SIZE = 0.1f;
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + _dropLocalPos, SIZE);
    }

#endif
}

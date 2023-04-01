using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

using static UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public UnityEvent OnAllObjectsSpawned;
    public UnityEvent OnObjectSpawned;

    [SerializeField] private GameObject _prefab;
    [SerializeField, Min(0)] private float _spawnPause;
    [SerializeField, Min(1)] private int _spawnCount;
    [SerializeField] private bool _spawnRandomly;
    [SerializeField] private PosRot[] _spawnPositions;

    private Coroutine _coroutine;

	private void Awake()
	{
		OnAllObjectsSpawned ??= new UnityEvent();
        OnObjectSpawned ??= new UnityEvent();
	}

    public void StartSpawning()
    {
        StopSponing();
        _coroutine = StartCoroutine(Spawn());
    }

	public void StopSponing()
	{
		if (_coroutine != null)
            StopCoroutine(_coroutine);
	}

    private IEnumerator Spawn()
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnPause);

        for (int i = _spawnCount; i > 0; i--)
        {
            yield return wait;
            PosRot pr = _spawnRandomly
                ? _spawnPositions[Range(0, _spawnPositions.Length)]
                : _spawnPositions[(_spawnCount - 1) % _spawnPositions.Length];
            Instantiate(_prefab, pr.Position, pr.Rotation);
            OnObjectSpawned?.Invoke();
        }

        OnAllObjectsSpawned?.Invoke();
    }

	[Serializable]
    private class PosRot
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }

#if UNITY_EDITOR

    private const float SIZE = 0.2f;

	private void OnDrawGizmosSelected()
	{
		if (_spawnPositions == null)
            return;

        Gizmos.color = Color.red;
        foreach (var pos in _spawnPositions)
        {
            Gizmos.DrawSphere(pos.Position, SIZE);
            Gizmos.DrawLine(pos.Position,
                pos.Position + pos.Rotation * Vector3.forward);
        }
	}

#endif
}

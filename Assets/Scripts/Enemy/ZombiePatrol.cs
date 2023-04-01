using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class ZombiePatrol : MonoBehaviour
{
    [SerializeField] List<Vector3> _patrolPointsInWorldSpace;
    [SerializeField, Min(0)] private float _pauseOnStopPoints;
    [SerializeField, Min(0)] private float _speed;

    private NavMeshAgent _agent;
    private Animator _animator;

    private Coroutine _coroutine;
    private int _pointIndex;

    private const float MIN_DISTANCE = 1f;
    private const string ANIM_NAME = "AnimNr";

	private void Awake()
	{
        _agent = GetComponent<NavMeshAgent>();
		_animator = GetComponent<Animator>();
        _pointIndex = 0;
	}

	public void Begin()
    {
        _coroutine = StartCoroutine(Patrol());
        _agent.speed = _speed;
        _agent.isStopped = false;
    }

	public void Stop()
	{
        if (_coroutine != null)
		    StopCoroutine(_coroutine);
		_agent.isStopped = true;
	}

	private IEnumerator Patrol()
	{
        if (_patrolPointsInWorldSpace.Count == 0)
            yield break;

		Vector3 target = _patrolPointsInWorldSpace[_pointIndex];
        WaitForSeconds waitSeconds = new WaitForSeconds(_pauseOnStopPoints);
        WaitUntil waitUntil = new WaitUntil(
            () => Vector3.Distance(target, transform.position) < MIN_DISTANCE);
        while (true)
        {
            _agent.SetDestination(target);
            _animator.SetInteger(ANIM_NAME, Random.Range(1, 3));
            yield return waitUntil;
			_animator.SetInteger(ANIM_NAME, 0);
            yield return waitSeconds;
            _pointIndex++;
            if (_pointIndex >= _patrolPointsInWorldSpace.Count)
                _pointIndex = 0;
            target = _patrolPointsInWorldSpace[_pointIndex];
		}
	}

#if UNITY_EDITOR

	private void OnDrawGizmosSelected()
	{
        if (_patrolPointsInWorldSpace?.Count < 2)
            return;

        Gizmos.color = Color.blue;
        const float SIZE = 0.2f;

        for (int i = 0; i < _patrolPointsInWorldSpace.Count - 1; i++)
        {
            Gizmos.DrawSphere(_patrolPointsInWorldSpace[i], SIZE);
            Gizmos.DrawLine(_patrolPointsInWorldSpace[i],
                _patrolPointsInWorldSpace[i + 1]);
        }
		Gizmos.DrawSphere(_patrolPointsInWorldSpace[^1], SIZE);
		Gizmos.DrawLine(_patrolPointsInWorldSpace[0], _patrolPointsInWorldSpace[^1]);
	}

#endif
}

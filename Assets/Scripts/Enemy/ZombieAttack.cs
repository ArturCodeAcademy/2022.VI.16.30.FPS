using System.Collections;
using UnityEngine;
using UnityEngine.AI;

using static UnityEngine.Vector3;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class ZombieAttack : MonoBehaviour
{
	[SerializeField, Min(0)] private float _damage;
	[SerializeField, Min(0)] private int _attackCount = 2;
	[SerializeField, Min(0)] private float _attackDuration = 0.3f;
	[SerializeField, Min(0)] private float _attackCooldown = 0.3f;
	[SerializeField, Min(0)] private float _runSpeed;

	private NavMeshAgent _agent;
	private Animator _animator;
	private Coroutine _coroutine;

	private const float MIN_ATTACK_DISTANCE = 1f;
	private const string ANIM_NAME = "AnimNr";

	private void Awake()
	{
		_agent = GetComponent<NavMeshAgent>();
		_animator = GetComponent<Animator>();
	}

	public void Begin()
	{
		_coroutine = StartCoroutine(Attack());
		_agent.speed = _runSpeed;
		_agent.isStopped = false;
	}

	public void Stop()
	{
		if (_coroutine != null)
			StopCoroutine(_coroutine);
		_agent.isStopped = true;
	}

	private IEnumerator Attack()
	{
		Transform target = Player.Instance.transform;
		Health playerHealth = target.GetComponent<Health>();
		WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
		WaitForSeconds waitAD = new WaitForSeconds(_attackDuration);
		WaitForSeconds waitAC = new WaitForSeconds(_attackCooldown);

		while (true)
		{
			_agent.isStopped = false;
			_agent.SetDestination(target.position);
			_animator.SetInteger(ANIM_NAME, 3);
			yield return waitForEndOfFrame;

			if (Distance(transform.position, target.position) <= MIN_ATTACK_DISTANCE)
			{
				_agent.isStopped = true;
				_animator.SetInteger(ANIM_NAME, 4);
				for (int i = 0; i < _attackCount; i++)
				{
					yield return waitAD;
					if (Distance(transform.position, target.position) <= MIN_ATTACK_DISTANCE)
						playerHealth.Hit(_damage);
				}

				_animator.SetInteger(ANIM_NAME, 0);
				yield return waitAC;
			}
		}
	}
}

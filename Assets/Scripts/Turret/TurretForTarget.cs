using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretForTarget : MonoBehaviour
{
	[SerializeField] private Quaternion _defaultRotation;
	[SerializeField] private CharacterType.Type _targetType;
	[SerializeField, Range(0, 50)] private float _rotationSpeed;
	[SerializeField, Min(0)] private float _viewDistance;

	private RotateToTarget _rotateToTarget;
	private ShootTarget _shoot;
	private Coroutine _coroutine;
	private HashSet<Transform> _targets;

	private const float RETARGETING_PAUSE = 1.5f;

	void Start()
	{
		SetTargets(_targetType);

		_rotateToTarget = GetComponentInChildren<RotateToTarget>();
		_shoot = GetComponentInChildren<ShootTarget>();

		_coroutine = StartCoroutine(SetTarget());
	}

	void OnEnable() => CharacterType.OnCharactersCountChanged += SetTargets;

	void OnDisable() => CharacterType.OnCharactersCountChanged -= SetTargets;

	void SetTargets(CharacterType.Type type)
	{
		_targets = CharacterType.GetAllObjectsOfType(type)
		.Select(x => x.transform).ToHashSet();
	}

	public void Begin()
	{
		Stop();
		_coroutine = StartCoroutine(SetTarget());
	}

	public void Stop()
	{
		if (_coroutine != null)
			StopCoroutine(_coroutine);

		_rotateToTarget.enabled = false;
		_shoot.gameObject.SetActive(false);
		_shoot.StopShooting();
	}

	IEnumerator SetTarget()
	{
		WaitForSeconds wait = new WaitForSeconds(RETARGETING_PAUSE);
		while (true)
		{
			Transform target = null;
			float minAngle = 360;
			_rotateToTarget.enabled = false;
			_shoot.gameObject.SetActive(false);
			_shoot.StopShooting();

			foreach (var c in _targets)
			{
				if (Vector3.Distance(c.position, transform.position) > _viewDistance)
					continue;
				Vector3 a = _rotateToTarget.transform.forward;
				Vector3 b = (_rotateToTarget.transform.position - c.transform.position).normalized;
				float angle = Vector3.Angle(a, b);
				if (angle < minAngle)
				{
					minAngle = angle;
					target = c;
				}
			}
			if (target != null)
			{
				_rotateToTarget.enabled = true;
				_shoot.gameObject.SetActive(true);
				_shoot.StartShooting(target);
			}
			_rotateToTarget.Target = target;
			yield return wait;
		}
	}

#if UNITY_EDITOR

	private void OnDrawGizmosSelected()
	{
		_rotateToTarget ??= GetComponentInChildren<RotateToTarget>();
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(_rotateToTarget.transform.position, _viewDistance);
	}

#endif
}

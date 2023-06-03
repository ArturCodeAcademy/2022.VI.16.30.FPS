using System.Linq;
using UnityEngine;

public class Knife : Item
{
	[SerializeField, Min(0)] private float _damage = 10;
	[SerializeField, Min(0)] private float _attackDistance = 1.8f;

	private Animator _animator;

	private const string ATTACK_TRIGGER = "Attack";

	protected override void Awake()
	{
		_animator = GetComponentInChildren<Animator>();
		base.Awake();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			_animator.SetTrigger(ATTACK_TRIGGER);
			Transform cam = Camera.main.transform;
			RaycastHit[] hits = Physics.RaycastAll(cam.position, cam.forward, _attackDistance);
			IHitable hitable = hits.Where(x => x.transform != Player.Instance.transform)
													.OrderBy(x => Vector3.Distance(x.transform.position, transform.position))
													.Select(x => x.transform.GetComponent<IHitable>())
													.Where(x => x != null)
													.FirstOrDefault();
			hitable?.Hit(_damage);
		}
	}

	public override void Drop()
	{
		base.Drop();
		_animator.enabled = false;
	}

	public override void Interact()
	{
		base.Interact();
		_animator.enabled = true;
	}
}

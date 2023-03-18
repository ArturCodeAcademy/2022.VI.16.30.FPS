using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health), typeof(Animator))]
public class OnZombieEndHealth : MonoBehaviour
{
    [SerializeField] private GameObject _worldSpaceCanvas;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _scanner;
    [SerializeField, Min(0)] private float _bodyDestroyPause;

    private Health _health;
    private Animator _animator;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
        _health = GetComponent<Health>();
	}

	private void OnEnable()
	{
		_health.OnHealthEnd.AddListener(OnEndHealth);
	}

	private void OnEndHealth()
	{
		_animator.SetTrigger(Random.Range(0, 2) == 0? "FallF" : "FallB");
		Destroy(GetComponent<Zombie>());
		Destroy(GetComponent<ZombiePatrol>());
		Destroy(GetComponent<ZombieAttack>());
		Destroy(GetComponent<NavMeshAgent>());
		Destroy(_worldSpaceCanvas);
		Destroy(_canvas);
		Destroy(_scanner);
		Destroy(gameObject, _bodyDestroyPause);
	}

	private void OnDisable()
	{
		_health?.OnHealthEnd.RemoveListener(OnEndHealth);
	}
}

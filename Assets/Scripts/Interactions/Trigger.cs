using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public UnityEvent OnCharacterTriggerEnter;

    [SerializeField] private CharacterType.Type _characterType;
    [SerializeField] private bool _destroyAfterUse;

	private void Awake()
	{
		OnCharacterTriggerEnter ??= new UnityEvent();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.TryGetComponent(out CharacterType ct))
			return;
		if (ct.ObjectType != _characterType)
			return;

		OnCharacterTriggerEnter?.Invoke();
		if (_destroyAfterUse)
			if (GetComponents<Trigger>().Length > 1)
				Destroy(this);
			else
				Destroy(gameObject);
	}
}

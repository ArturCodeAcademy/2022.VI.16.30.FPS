using UnityEngine;

public class MedKitInteractable : MonoBehaviour, IInteractable
{
    public string Info => $"Health: {(int)_healthPoints}";

    [SerializeField, Min(0.1f)] private float _healthPoints;

    private Health _playerHealth;

    public void Interact()
    {
        _playerHealth ??= Player.Instance.GetComponent<Health>();
        _healthPoints -= _playerHealth.AddHealth(_healthPoints);

        if (_healthPoints <= 0 )
            Destroy(gameObject);
    }
}

using UnityEngine;

[RequireComponent(typeof(Health))]
public class OnPlayerEndHealth : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Pause _pause;
    [SerializeField] private MonoBehaviour[] _componentsToDisable;

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _health.OnHealthEnd.AddListener(OnEndHealth);
        _gameOverPanel?.SetActive(false);
    }

    private void OnEndHealth()
    {
        foreach (MonoBehaviour component in _componentsToDisable)
            component.enabled = false;
        _gameOverPanel?.SetActive(true);
        if (_pause != null)
            _pause.enabled = false;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDestroy()
    {
        _health?.OnHealthEnd.RemoveListener(OnEndHealth);
    }
}

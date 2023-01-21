using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] TMP_Text _info;

    private Transform _playerCamera;
    private Transform _hoveredInteractable;
    private IInteractable _interactable;

    private const float RAY_LENGTH = 2f;

    private void Awake()
    {
        _playerCamera = Camera.main.transform;
    }

    private void Update()
    {
        Physics.Raycast(_playerCamera.position, _playerCamera.forward,
            out RaycastHit hit, RAY_LENGTH);
        if (_hoveredInteractable != hit.transform)
        {
            _hoveredInteractable = hit.transform;
            _interactable = _hoveredInteractable?.GetComponent<IInteractable>();   
        }
        if (_info.text != (_interactable?.Info ?? string.Empty))
            _info.text = _interactable?.Info ?? string.Empty;

        if (Input.GetKeyDown(KeyCode.E))
            _interactable?.Interact();
    }
}

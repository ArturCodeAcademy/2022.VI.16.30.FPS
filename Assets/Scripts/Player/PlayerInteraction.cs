using System.Linq;
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
        RaycastHit hit = Physics.RaycastAll(_playerCamera.position, _playerCamera.forward, RAY_LENGTH)
            .OrderBy(x => Vector3.Distance(_playerCamera.position, x.point))
            .Where(x => x.transform != transform).FirstOrDefault();
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

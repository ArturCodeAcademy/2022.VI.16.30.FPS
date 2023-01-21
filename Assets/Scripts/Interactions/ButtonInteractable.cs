using UnityEngine;
using UnityEngine.Events;

public class ButtonInteractable : MonoBehaviour, IInteractable
{
    [field: SerializeField] public bool Interactable { get; set; }

    public UnityEvent OnClick;

    public string Info => _info;

    [SerializeField] private string _info;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        OnClick ??= new UnityEvent();
    }

    public void Interact()
    {
        if (!Interactable)
            return;

        OnClick?.Invoke();
        _animator.SetTrigger("Click");
    }
}

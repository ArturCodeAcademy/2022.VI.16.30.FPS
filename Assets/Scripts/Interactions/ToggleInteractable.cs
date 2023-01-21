using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ToggleInteractable : MonoBehaviour, IInteractable
{
    [field: SerializeField] public bool Interactable { get; set; }
    [field: SerializeField] public bool IsOn { get; private set; }

    public string Info => IsOn? _onInfo : _offInfo;

    public UnityEvent OnToggleOn;
    public UnityEvent OnToggleOff;
    public UnityEvent OnToggleSwitched;

    [SerializeField] private Material _onMaterial;
    [SerializeField] private Material _offMaterial;
    [SerializeField] private Transform _handleTransform;
    [SerializeField] private MeshRenderer _handleRenderer;
    [SerializeField] private string _onInfo;
    [SerializeField] private string _offInfo;

    private Coroutine _changeStateCoroutine;

    private const float SPEED = 80f;
    private const float ON_ANGLE = 20f;
    private const float OFF_ANGLE = -20f;

    private void Awake()
    {
        OnToggleOn ??= new UnityEvent();
        OnToggleOff ??= new UnityEvent();
        OnToggleSwitched ??= new UnityEvent();
        _changeStateCoroutine = StartCoroutine(ChangeState());
    }

    public void Interact()
    {
        if (!Interactable)
            return;

        if (IsOn)
            OnToggleOff?.Invoke();
        else
            OnToggleOn?.Invoke();
        OnToggleSwitched?.Invoke();

        IsOn = !IsOn;
        if (_changeStateCoroutine != null)
            StopCoroutine(_changeStateCoroutine);
        _changeStateCoroutine = StartCoroutine(ChangeState());
    }

    private IEnumerator ChangeState()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        _handleRenderer.material = IsOn ? _onMaterial : _offMaterial;
        Vector3 euler = _handleTransform.localEulerAngles;
        if (euler.x > 180)
            euler.x -= 360;
        while (IsOn
                ? _handleTransform.localRotation.x < ON_ANGLE
                : _handleTransform.localRotation.x > OFF_ANGLE)
        {
            float deltaAngle = Time.deltaTime * SPEED;
            euler.x = Mathf.Clamp(euler.x + (IsOn ? deltaAngle : -deltaAngle),
                OFF_ANGLE, ON_ANGLE);
            _handleTransform.localEulerAngles = euler;
            yield return wait;
        }
    }
}

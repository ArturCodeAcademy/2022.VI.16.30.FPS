using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public CharacterType.Type TargetsType => _targetsType;
    public UnityEventScanner OnScannerViewEnter;
    public UnityEventScanner OnScannerViewExit;

    [SerializeField, Min(0)] private float _viewDistance;
    [SerializeField, Range(0, 360)] private float _viewAngle;
    [SerializeField] private CharacterType.Type _targetsType;

    private HashSet<ScannerTarget> _targets;

    void Awake()
    {
        _targets = new HashSet<ScannerTarget>();
        OnScannerViewEnter ??= new UnityEventScanner();
        OnScannerViewExit ??= new UnityEventScanner();
    }

    void OnEnable()
    {
        CharacterType.OnCharactersCountChanged += OnCharactersCountChanged;
    }

    void Start()
    {
        if (_targets.Count == 0)
            _targets = new HashSet<ScannerTarget>(CharacterType.GetAllObjectsOfType(_targetsType)
                .Select(x => new ScannerTarget()
                { Target = x.transform, IsVisible = false, Type = x}));
    }

    void FixedUpdate()
    {
        foreach (ScannerTarget target in _targets)
            CheckTarget(target);
    }

    void OnDisable()
    {
        CharacterType.OnCharactersCountChanged -= OnCharactersCountChanged;
    }

    private void OnCharactersCountChanged(CharacterType.Type type)
    {
        IEnumerable<ScannerTarget> updatedCollection = CharacterType.GetAllObjectsOfType(_targetsType)
            .Select(x => new ScannerTarget()
            { Target = x.transform, IsVisible = false, Type = x });
        var temp = new HashSet<ScannerTarget>(_targets);
        temp.ExceptWith(updatedCollection);
        _targets.IntersectWith(updatedCollection);
        _targets.AddRange(updatedCollection);
        foreach (ScannerTarget target in temp)
            if (target.IsVisible)
                OnScannerViewExit?.Invoke(new ScannerEventArgs()
                {
                    Target = target.Target, Sender = transform, TargetType = target.Type.ObjectType
                });
    }

    private void CheckTarget(ScannerTarget target)
    {
        if (Vector3.Distance(target.Target.position, transform.position) > _viewDistance)
        {
            if (target.IsVisible)
            { 
                OnScannerViewExit?.Invoke(new ScannerEventArgs()
                {
                    Target = target.Target, Sender = transform, TargetType = target.Type.ObjectType
                });
                target.IsVisible = false;
            }
            return;
        }

        float angle = Vector3.Angle(transform.forward, target.Target.position - transform.position);

        if (angle <= _viewAngle / 2)
        {
            Vector3 direction = target.Target.position - transform.position;
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, _viewDistance))
            {
                if (!target.IsVisible && hit.transform == target.Target.transform)
                {
                    target.IsVisible = true;
                    OnScannerViewEnter?.Invoke(new ScannerEventArgs()
                    {
                        Target = target.Target,
                        Sender = transform,
                        TargetType = target.Type.ObjectType
                    });
                }
                else if (target.IsVisible && hit.transform != target.Target.transform)
                {
                    target.IsVisible = false;
                    OnScannerViewExit?.Invoke(new ScannerEventArgs()
                    {
                        Target = target.Target,
                        Sender = transform,
                        TargetType = target.Type.ObjectType
                    });
                }
            }
        }
        else if (target.IsVisible && angle > _viewAngle / 2)
        {
            target.IsVisible = false;
            OnScannerViewExit?.Invoke(new ScannerEventArgs()
            {
                Target = target.Target,
                Sender = transform,
                TargetType = target.Type.ObjectType
            });
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _viewDistance);

        Gizmos.color = Color.blue;
        float yRotation = Mathf.Deg2Rad * transform.rotation.eulerAngles.y;
        float leftRad = Mathf.Deg2Rad * _viewAngle / 2 + yRotation;
        float rightRad = -Mathf.Deg2Rad * _viewAngle / 2 + yRotation;
        Vector3 leftAngle = new Vector3(Mathf.Sin(leftRad), 0, Mathf.Cos(leftRad)).normalized;
        Vector3 rightAngle = new Vector3(Mathf.Sin(rightRad), 0, Mathf.Cos(rightRad)).normalized;
        leftAngle *= _viewDistance;
        rightAngle *= _viewDistance;
        Gizmos.DrawLine(transform.position, transform.position + leftAngle);
        Gizmos.DrawLine(transform.position, transform.position + rightAngle);
    }

#endif
}

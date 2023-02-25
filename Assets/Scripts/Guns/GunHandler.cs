using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunHandler : MonoBehaviour
{
    public event Action<GunBase> OnGunChanged;
    public GunBase CurrentGun => _currentGun;

    private List<GunBase> _guns;
    private GunBase _currentGun;
    private int _currentGunIndex;

    private float _aimCurveValue;
    
    private void Awake()
    {
        _guns = new List<GunBase>();
		_guns = new List<GunBase>(GetComponentsInChildren<GunBase>());
		foreach (var gun in _guns)
			gun.gameObject.SetActive(false);
		_currentGunIndex = 0;
		_currentGun = _guns[_currentGunIndex];
		_currentGun.gameObject.SetActive(true);
	}

    private void Update()
    {
        SwitchGuns();
        Aim();  
    }

    private void Aim()
    {
        if (Input.GetMouseButton(1))
            _aimCurveValue += Time.deltaTime;
        else
            _aimCurveValue -= Time.deltaTime;
        _aimCurveValue = Mathf.Clamp(_aimCurveValue, 0.0f, _currentGun.AimingTransitionDuration);

        _currentGun.transform.localPosition = Vector3.Lerp(_currentGun.NormalPosition, _currentGun.AimingPosition, _aimCurveValue / _currentGun.AimingTransitionDuration);
        _currentGun.transform.localRotation = Quaternion.Lerp(_currentGun.NormalRotation, _currentGun.AimingRotation, _aimCurveValue / _currentGun.AimingTransitionDuration);
    }

    private void SwitchGuns()
    {
        if (Input.mouseScrollDelta.y == 0)
            return;

		_currentGun.gameObject.SetActive(false);

        if (Input.mouseScrollDelta.y > 0)
            _currentGunIndex++;
        else if (Input.mouseScrollDelta.y < 0)
            _currentGunIndex--;

        if (_currentGunIndex < 0)
            _currentGunIndex = _guns.Count - 1;
        if (_currentGunIndex >= _guns.Count)
            _currentGunIndex = 0;
        
        _currentGun = _guns[_currentGunIndex];
        _currentGun.gameObject.SetActive(true);
		_aimCurveValue = 0;
        OnGunChanged?.Invoke(_currentGun);
	}
}

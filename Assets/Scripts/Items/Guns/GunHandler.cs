using System;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    public event Action<Item> OnGunChanged;
    public Item CurrentGun => _currentGun;

    private List<Item> _guns;
    private Item _currentGun;
    private int _currentGunIndex;

    private float _aimCurveValue;
    
    private void Awake()
    {
        _guns = new List<Item>();
		_guns = new List<Item>(GetComponentsInChildren<Item>());
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
        if (_currentGun is not GunBase gun)
            return;

        if (Input.GetMouseButton(1))
            _aimCurveValue += Time.deltaTime;
        else
            _aimCurveValue -= Time.deltaTime;
        _aimCurveValue = Mathf.Clamp(_aimCurveValue, 0.0f, gun.AimingTransitionDuration);

        gun.transform.localPosition = Vector3.Lerp(gun.NormalPosition, gun.AimingPosition, _aimCurveValue / gun.AimingTransitionDuration);
        gun.transform.localRotation = Quaternion.Lerp(gun.NormalRotation, gun.AimingRotation, _aimCurveValue / gun.AimingTransitionDuration);
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

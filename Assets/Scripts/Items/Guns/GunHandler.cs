using System;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    public event Action<Item> OnGunChanged;
    public Item CurrentGun => _currentGun;

    private List<Item> _guns = new List<Item>();
    private Item _currentGun;
    private int _currentGunIndex;

    private float _aimCurveValue;

    private void Update()
    {
        SwitchGuns();
        Aim();
        DropItem();
    }

    public void AddItem(Item item)
    {
        _currentGun?.gameObject.SetActive(false);
        _guns.Add(item);
        _currentGunIndex = _guns.Count - 1;
        _currentGun = _guns[_currentGunIndex];
        _currentGun.gameObject.SetActive(true);
        _currentGun.transform.SetParent(transform);
        _currentGun.transform.localPosition = _currentGun.NormalPosition;
        _currentGun.transform.localRotation = _currentGun.NormalRotation;
        OnGunChanged?.Invoke(_currentGun);
    }

    private void DropItem()
    {
        if (!Input.GetKeyDown(KeyCode.G)
         || _currentGun == null
         || _currentGun is INotThrowable)
            return;

        _currentGun.Drop();
        _guns.RemoveAt(_currentGunIndex);
        _currentGunIndex--;
        if (0 < _guns.Count)
        {
            if (_currentGunIndex < 0)
                _currentGunIndex = _guns.Count - 1;

            _currentGun = _guns[_currentGunIndex];
            _currentGun.gameObject.SetActive(true);
            OnGunChanged?.Invoke(_currentGun);
        }
        else
        {
            _currentGun = null;
            _currentGunIndex = 0;
        }
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
        if (Input.mouseScrollDelta.y == 0 || _guns.Count == 0)
            return;

        int previousGunIndex = _currentGunIndex;

        _currentGun?.gameObject.SetActive(false);

        do
        {
            if (Input.mouseScrollDelta.y > 0)
                _currentGunIndex++;
            else if (Input.mouseScrollDelta.y < 0)
                _currentGunIndex--;

            if (_currentGunIndex < 0)
                _currentGunIndex = _guns.Count - 1;
            if (_currentGunIndex >= _guns.Count)
                _currentGunIndex = 0;

            _currentGun = _guns[_currentGunIndex];
        }
        while (_currentGun is ISwitchSkipable ss && ss.Skip() && previousGunIndex != _currentGunIndex);

        _currentGun?.gameObject.SetActive(true);
		_aimCurveValue = 0;
        OnGunChanged?.Invoke(_currentGun);
	}
}

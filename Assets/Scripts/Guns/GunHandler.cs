using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text _ammoCountText;
    [SerializeField] private Image _ammoTypeSpriteRenderer;

    private List<GunBase> _guns;
    private GunBase _currentGun;
    private int _currentGunIndex;
    private AmmoBackpack _ammoBackpack;

    private float _aimCurveValue;
    
    private void Awake()
    {
        _guns = new List<GunBase>();
    }

    private void Start()
    {
        _ammoBackpack = Player.Instance.GetComponent<AmmoBackpack>();
        _guns = new List<GunBase>(GetComponentsInChildren<GunBase>());
        foreach (var gun in _guns)
            gun.gameObject.SetActive(false);
        _currentGunIndex = 0;
        _currentGun = _guns[_currentGunIndex];
        _currentGun.gameObject.SetActive(true);
        _currentGun.OnShoot += UpdateGunInfoPanel;
        UpdateGunInfoPanel();

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

		_currentGun.OnShoot -= UpdateGunInfoPanel;
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
		_currentGun.OnShoot += UpdateGunInfoPanel;
		_aimCurveValue = 0;
        UpdateGunInfoPanel();
	}

    private void UpdateGunInfoPanel()
    {
        if (_currentGun == null)
        {
			_ammoCountText.text = string.Empty;
			_ammoTypeSpriteRenderer.sprite = default;
		}
        else
        {
            _ammoCountText.text = $"{_currentGun.AmmoCountInGun} / {_ammoBackpack.GetAmmoCount(_currentGun.AmmoType)}";
            _ammoTypeSpriteRenderer.sprite = AmmoTypes.Instance[_currentGun.AmmoType];
        }
    }
}

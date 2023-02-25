using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
	[SerializeField] private TMP_Text _ammoCountText;
	[SerializeField] private Image _ammoTypeSpriteRenderer;
	[SerializeField] private Slider _reloadingSlider;

	private GunBase _currentGun;
	private GunHandler _gunHandler;
	private AmmoBackpack _ammoBackpack;

	private void Start()
	{
		_ammoBackpack = Player.Instance.GetComponent<AmmoBackpack>();
		_gunHandler = Player.Instance.GetComponentInChildren<GunHandler>();
		_gunHandler.OnGunChanged += OnGunSwiched;
		OnGunSwiched(_gunHandler.CurrentGun);
	}

	private void Update()
	{
		if (!_reloadingSlider.gameObject.activeSelf)
			return;

		_reloadingSlider.value += Time.deltaTime;
	}

	private void OnStartReloading(float duration)
	{
		_reloadingSlider.gameObject.SetActive(true);
		_reloadingSlider.maxValue = duration;
		_reloadingSlider.value = 0;
	}

	private void HideSlider()
	{
		_reloadingSlider.gameObject.SetActive(false);
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

	private void OnGunSwiched(GunBase gun)
	{
		if (_currentGun != null)
		{ 
			_currentGun.OnShoot -= UpdateGunInfoPanel;
			_currentGun.OnStartReloading -= OnStartReloading;
			_currentGun.OnDropReloading -= HideSlider;
			_currentGun.OnReloaded -= HideSlider;
			_currentGun.OnReloaded -= UpdateGunInfoPanel;
		}
		_currentGun = gun;
		if (_currentGun != null)
		{
			_currentGun.OnShoot += UpdateGunInfoPanel;
			_currentGun.OnStartReloading += OnStartReloading;
			_currentGun.OnDropReloading += HideSlider;
			_currentGun.OnReloaded += HideSlider;
			_currentGun.OnReloaded += UpdateGunInfoPanel;
			UpdateGunInfoPanel();
		}
	}
}

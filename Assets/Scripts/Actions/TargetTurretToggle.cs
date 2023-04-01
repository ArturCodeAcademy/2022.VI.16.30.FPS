using UnityEngine;

public class TargetTurretToggle : MonoBehaviour
{
    private TurretForTarget _turretTarget;
    private RotateToTarget _rotateToTarget;
    private Laser _laser;

	private void Start()
	{
		_turretTarget = GetComponent<TurretForTarget>();
		_rotateToTarget = GetComponent<RotateToTarget>();
		_laser = GetComponent<Laser>();
	}

	public void TurnOn()
	{
		_turretTarget.Begin();
	}

	public void TurnOff()
	{
		_turretTarget.Stop();
	}
}

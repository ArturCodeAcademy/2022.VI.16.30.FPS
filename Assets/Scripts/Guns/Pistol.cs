using UnityEngine;

public class Pistol : GunBase
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot(_damage, _normalSpread, _penetratingPower, _impulsePower);
        }
    }
}

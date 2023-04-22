using UnityEngine;

[DisallowMultipleComponent]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public GunHandler GunHandler { get; private set; }

    [field: SerializeField] public Camera Camera;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"Disallow multiple component {gameObject}", gameObject);
            Destroy(this);
            return;
        }

        Instance = this;

        GunHandler = GetComponentInChildren<GunHandler>();
    }
}

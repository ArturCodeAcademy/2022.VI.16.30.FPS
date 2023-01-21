using UnityEngine;

[DisallowMultipleComponent]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"Disallow multiple component {gameObject}", gameObject);
            Destroy(this);
            return;
        }
        Instance = this;
    }
}

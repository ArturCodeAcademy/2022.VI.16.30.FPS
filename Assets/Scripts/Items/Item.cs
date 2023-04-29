using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [field:SerializeField] public string Info { get; private set; }

    [field: SerializeField] public Vector3 NormalPosition { get; protected set; }
    [field: SerializeField] public Quaternion NormalRotation { get; protected set; }

    private Collider _collider;
    private Rigidbody _rb;

    private const float DROP_IMPULSE = 1.5f;

    protected virtual void Awake()
    {
        enabled = false;
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
        GunHandler gh = GetComponentInParent<GunHandler>();
        if (gh != null)
            Interact();
    }

    public virtual void Interact()
    {
        _collider.enabled = false;
        enabled = true;
        _rb.useGravity = false;
        Player.Instance.GunHandler.AddItem(this);
    }

    public virtual void Drop()
    {
        enabled = false;
        transform.SetParent(null);
        _collider.enabled = true;
        _rb.useGravity = true;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.AddForce(Player.Instance.Camera.transform.forward * DROP_IMPULSE,
            ForceMode.Impulse);
    }
}

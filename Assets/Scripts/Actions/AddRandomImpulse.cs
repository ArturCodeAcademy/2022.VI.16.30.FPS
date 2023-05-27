using UnityEngine;

public class AddRandomImpulse : MonoBehaviour
{
    [SerializeField] private Collider[] _objects;
    [SerializeField] private float _impulse;

    public void AddImpulse()
    {
        foreach (Collider c in _objects)
        {
            Rigidbody rb = c.GetComponent<Rigidbody>();
            if (rb == null)
                rb = c.GetComponentInChildren<Rigidbody>();
			if (rb == null)
				rb = c.gameObject.AddComponent<Rigidbody>();

            rb.AddForce(Random.insideUnitSphere * _impulse, ForceMode.Impulse);
        }

        Destroy(this);
    }
}

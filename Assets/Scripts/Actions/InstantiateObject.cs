using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Vector3 _position;
    [SerializeField] private Quaternion _rotation;

    public void CreateObject()
    {
        Instantiate(_prefab, _position, _rotation);
    }
}

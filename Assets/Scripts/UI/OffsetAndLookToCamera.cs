using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetAndLookToCamera : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;

    private void LateUpdate()
    {
        transform.position = _offset + transform.parent.position;
        transform.LookAt(Camera.main.transform);
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        if (transform.parent != null)
            transform.position = _offset + transform.parent.position;
    }

#endif
}

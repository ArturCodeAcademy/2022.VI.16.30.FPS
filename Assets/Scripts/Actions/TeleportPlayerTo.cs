using UnityEngine;

public class TeleportPlayerTo : MonoBehaviour
{
    [SerializeField] private Vector3 _teleportPosition;

    public void Teleport()
    {
        Player.Instance.transform.position = _teleportPosition;
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        const float RADIUS = 0.2f;
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_teleportPosition, RADIUS);
    }

#endif
}

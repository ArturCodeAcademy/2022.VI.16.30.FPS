using UnityEngine;

public class TeleportPlayerTo : MonoBehaviour
{
    [SerializeField] private Vector3 _teleportPosition;

    public void Teleport()
    {
        Transform player = Player.Instance.transform;
        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        player.position = _teleportPosition;
        cc.enabled = true;
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

using UnityEngine;
using UnityEngine.Events;

public class Teleporter : MonoBehaviour
{
    [SerializeField] public UnityEvent<Vector3> OnTeleport = new UnityEvent<Vector3>();
    [SerializeField] private GameObject _teleportPoint;

    public void Teleport()
    {
        OnTeleport.Invoke(_teleportPoint.transform.position);
    }
}

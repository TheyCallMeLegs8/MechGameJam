using Unity.Cinemachine;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private int _newCamPriority = 2;
    private int _baseCamPriority;

    private void Start()
    {
        _baseCamPriority = _camera.Priority;
    }

    public void SwitchCamera()
    {
        _camera.Priority = _newCamPriority;
    }

    public void ReturnToBaseCamera()
    {
        _camera.Priority = _baseCamPriority;
    }
}

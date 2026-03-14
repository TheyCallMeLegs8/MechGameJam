using UnityEngine;

public class MechCam : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private RenderTexture _lowQualityTexture;
    [SerializeField] private RenderTexture _highQualityTexture;

    [SerializeField] private Material _lowQualityMaterial;
    [SerializeField] private Material _highQualityMaterial;
    [SerializeField] private MeshRenderer _screen;

    public void SwitchToLowQuality()
    {
        _cam.targetTexture = _lowQualityTexture;
        _screen.material = _lowQualityMaterial;
    }

    public void SwitchToHighQuality()
    {
        _cam.targetTexture = _highQualityTexture;
        _screen.material = _highQualityMaterial;
    }
}

using TMPro;
using UnityEngine;

public class BrokenMechDetector : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _detectionRange = 100.0f;
    [SerializeField] private LayerMask _brokenMechMask;
    [SerializeField] private TextMeshPro _freqScanText; // THis shouldn't be in this script

    // This should just be for the feedback saying how close you are to it
    private void Update()
    {
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hitInfo, _detectionRange, _brokenMechMask))
        {
            if(hitInfo.transform.TryGetComponent(out BrokenMech brokenMech))
            {
                _freqScanText.text = brokenMech._frequencyName;
            }
        }
    }
}

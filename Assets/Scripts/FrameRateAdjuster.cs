using UnityEngine;

public class FrameRateAdjuster : MonoBehaviour
{
    [SerializeField] private int _targetFPS = -1; // -1 = unlimited frame rate

    void Start()
    {
        Application.targetFrameRate = _targetFPS;
    }
}

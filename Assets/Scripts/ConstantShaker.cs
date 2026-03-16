using PrimeTween;
using UnityEngine;

public class ConstantShaker : MonoBehaviour
{
    [SerializeField] private ShakeSettings _shakeSettings;

    private void Awake()
    {
        Shake();
    }

    private void Shake()
    {
        Tween.ShakeLocalPosition(transform, _shakeSettings);
    }
}

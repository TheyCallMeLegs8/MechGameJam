using UnityEngine;
using UnityEngine.Events;

public class ShieldSystem : MonoBehaviour
{
    private bool _isShieldOn = false;
    [SerializeField] public UnityEvent OnDoDamage = new UnityEvent();

    public void TurnShieldOn()
    {
        _isShieldOn = true;
    }

    public void TurnShieldOff()
    {
        _isShieldOn = false;
    }

    public void DamageMech()
    {
        if (_isShieldOn) return;

        OnDoDamage.Invoke();
    }
}

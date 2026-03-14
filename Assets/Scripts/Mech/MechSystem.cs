using UnityEngine;
using UnityEngine.Events;

public class MechSystem : MonoBehaviour
{
    [SerializeField] private bool _canOverclock = false;
    public int CurrentCharges { get; private set; } = 0;
    
    [field: SerializeField] public int ChargeNeeded { get; private set; } = 1;
    [field: SerializeField] public int OverclockChargeNeeded { get; private set; } = 2;

    public bool IsCharged { get; private set; } = false;
    public bool IsOverclocked { get; private set; } = false;

    [SerializeField] private InteractButton _interactButton;
    [SerializeField] public UnityEvent OnDoSystem = new UnityEvent();
    [SerializeField] public UnityEvent OnStopSystem = new UnityEvent();
    [SerializeField] public UnityEvent OnOverclock = new UnityEvent();
    [SerializeField] public UnityEvent OnStopOverclock = new UnityEvent();

    public void AddCharge()
    {
        CurrentCharges++;
        if (CurrentCharges >= ChargeNeeded)
        {
            IsCharged = true;
        }
        if (CurrentCharges >= OverclockChargeNeeded && _canOverclock)
        {
            IsOverclocked = true;
            OnOverclock.Invoke();
        }
    }

    public void ClearCharges()
    {
        CurrentCharges = 0;
        IsCharged = false;
        IsOverclocked = false;
        StopSystem();
        OnStopOverclock.Invoke();
    }

    public void StopSystem()
    {
        OnStopSystem.Invoke();
    }

    public void TryDoSystem()
    {
        if(IsCharged || IsOverclocked)
        {
            OnDoSystem.Invoke();
        }
    }
}

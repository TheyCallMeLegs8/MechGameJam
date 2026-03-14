using UnityEngine;
using UnityEngine.Events;

public class InteractButton : MonoBehaviour, IInteractable
{
    [SerializeField] public UnityEvent OnInteract = new UnityEvent();
    [SerializeField] public UnityEvent OnStopInteract = new UnityEvent();

    [field: SerializeField] public bool CanInteract { get; private set; } = true;

    public void Interact(GameObject interactor)
    {
        OnInteract.Invoke();
    }

    public void StopInteract(GameObject interactor)
    {
        OnStopInteract.Invoke();
    }

    public void SetCanInteract(bool canInteract)
    {
        CanInteract = canInteract;
    }
}

using UnityEngine;
using UnityEngine.Events;

public class InteractButton : MonoBehaviour, IInteractable
{
    [SerializeField] public UnityEvent OnInteract = new UnityEvent();
    [SerializeField] public UnityEvent OnStopInteract = new UnityEvent();

    public void Interact(GameObject interactor)
    {
        OnInteract.Invoke();
    }

    public void StopInteract(GameObject interactor)
    {
        OnStopInteract.Invoke();
    }
}

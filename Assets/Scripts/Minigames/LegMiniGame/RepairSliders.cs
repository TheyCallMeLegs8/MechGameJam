using UnityEngine;

public class RepairSliders : MonoBehaviour, IInteractable
{
    [field: SerializeField] public int Id { get; private set; } = 1;

    public void Interact(GameObject interactor)
    {

    }

    public void StopInteract(GameObject interactor)
    {
        
    }
}

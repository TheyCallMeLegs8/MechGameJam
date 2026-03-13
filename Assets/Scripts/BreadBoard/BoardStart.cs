using System.Collections.Generic;
using UnityEngine;

public class BoardStart : MonoBehaviour, IInteractable
{
    [SerializeField] private List<BoardConnector> _adjConnectors = new List<BoardConnector>();

    public void Interact(GameObject interactor)
    {
        
    }

    public void StopInteract(GameObject interactor)
    {
        
    }
}

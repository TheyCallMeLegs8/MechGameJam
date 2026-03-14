using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardConnector : MonoBehaviour, IInteractable
{
    [SerializeField] private List<BoardConnector> _adjConnectors = new List<BoardConnector>();
    public List<BoardConnector> _adjascentPoint => _adjConnectors;
    [SerializeField] private GameObject _onLight;
    private GameObject _lightInstance;
    public bool IsActive { get; private set; } = false;

    public UnityEvent<BoardConnector> OnClick = new UnityEvent<BoardConnector>();

    public void Interact(GameObject interactor)
    {
        OnClick.Invoke(this);
    }

    public void StopInteract(GameObject interactor)
    {
        
    }

    public bool TryConnect(BoardConnector connector)
    {
        // SHOULD NOT GO THROUGH IF THE ONE YOU WANT TO CONNECT TO IS A STARTER
        // already automatically does it because when you click on BoardStarter
        // it connects to that interact before this one

        BoardConnector spawnedConnector = null;
        for (int i = 0; i < _adjConnectors.Count; i++)
        {
            if(connector != _adjConnectors[i]) continue;
            spawnedConnector = _adjConnectors[i];
        }

        if(spawnedConnector != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SpawnLight()
    {
        IsActive = true;
        _lightInstance = Instantiate(_onLight, transform);
    }

    public void DestroyLight()
    {
        if(_lightInstance != null)
        {
            IsActive = false;
            Destroy(_lightInstance);
        }
    }
}

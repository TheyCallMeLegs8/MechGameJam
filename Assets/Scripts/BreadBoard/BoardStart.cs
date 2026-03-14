using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
public class BoardStart : MonoBehaviour, IInteractable
{
    [SerializeField] private List<BoardConnector> _adjConnectors = new List<BoardConnector>();
    public List<BoardConnector> _adjascentPoint => _adjConnectors;

    public List<BoardConnector> CurrentConnecters { get; private set; } = new List<BoardConnector>();
    [SerializeField] private GameObject _onObjectPrefab;
    private GameObject _onObjectInstance;

    // variables for drawing line renderer
    [SerializeField] private LayerMask _interactMask;
    [SerializeField] private LineRenderer _lineRenderer;
    private bool _isTracking;
    private Vector3 _mousePosition;

    public UnityEvent<BoardStart> OnClick = new UnityEvent<BoardStart>();

    private void OnValidate()
    {
        if(_lineRenderer == null) _lineRenderer = GetComponent<LineRenderer>();
    }

    public void Interact(GameObject interactor)
    {
        OnClick.Invoke(this);
    }

    public void StopInteract(GameObject interactor)
    {
        
    }

    public bool TryConnect(BoardStart starter)
    {
        return true;
    }

    public void AddConnectorToSequence(BoardConnector connector)
    {
        CurrentConnecters.Add(connector);
    }

    public void RemoveConnectorFromSequence(BoardConnector connector)
    {
        CurrentConnecters.Remove(connector);
    }

    public void ClearConnectors()
    {
        CurrentConnecters.Clear();
    }

    public void SpawnLight()
    {
        _onObjectInstance = Instantiate(_onObjectPrefab, transform);
    }

    public void DestroyLight()
    {
        if (_onObjectInstance != null)
        {
            Destroy(_onObjectInstance);
        }
    }
}

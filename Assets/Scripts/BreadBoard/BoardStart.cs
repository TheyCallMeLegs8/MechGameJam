using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
public class BoardStart : MonoBehaviour, IInteractable
{
    [SerializeField] private int _neededValue;
    [SerializeField] private int _overclockValue;

    [SerializeField] private List<BoardConnector> _adjConnectors = new List<BoardConnector>();
    public List<BoardConnector> _adjascentPoint => _adjConnectors;

    public List<BoardConnector> CurrentConnecters { get; private set; } = new List<BoardConnector>();
    [SerializeField] private GameObject _onObjectPrefab;
    private GameObject _onObjectInstance;

    [SerializeField] private LineRenderer _lineRenderer;

    public UnityEvent<BoardStart> OnClick = new UnityEvent<BoardStart>();

    [SerializeField] public UnityEvent OnAddConnector = new UnityEvent();
    [SerializeField] public UnityEvent OnRemoveAllConnectors = new UnityEvent();

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
        _lineRenderer.positionCount++;
        CurrentConnecters.Add(connector);
        OnAddConnector.Invoke();
        _lineRenderer.SetPosition(CurrentConnecters.Count, connector.transform.position);
    }

    public void RemoveConnectorFromSequence(BoardConnector connector)
    {
        _lineRenderer.positionCount--;
        CurrentConnecters.Remove(connector);
    }

    public void ClearConnectors()
    {
        _lineRenderer.positionCount = 0;
        CurrentConnecters.Clear();
        OnRemoveAllConnectors.Invoke();
    }

    public void SpawnLight()
    {
        _lineRenderer.positionCount = 1;
        _onObjectInstance = Instantiate(_onObjectPrefab, transform);
        _lineRenderer.SetPosition(0, transform.position);
    }

    public void DestroyLight()
    {
        if (_onObjectInstance != null)
        {
            _lineRenderer.positionCount = 0;
            Destroy(_onObjectInstance);
        }
    }
}

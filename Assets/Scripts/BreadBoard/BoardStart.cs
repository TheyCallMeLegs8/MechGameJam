using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BoardStart : MonoBehaviour, IInteractable
{
    [SerializeField] private List<BoardConnector> _adjConnectors = new List<BoardConnector>();
    [SerializeField] private LayerMask _interactMask;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GameObject _onObjectPPrefab;
    private bool _isTracking;
    private Vector3 _mousePosition;

    private void OnValidate()
    {
        if(_lineRenderer == null) _lineRenderer = GetComponent<LineRenderer>();
    }

    public void Interact(GameObject interactor)
    {

        /*
        _lineRenderer.positionCount = 2;
        _isTracking = true;
        _lineRenderer.SetPosition(0, transform.localPosition);*/
    }

    private void Update()
    {
        /*
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (_isTracking)
        {
            _lineRenderer?.SetPosition(1, _mousePosition);
        }*/
    }

    public void StopInteract(GameObject interactor)
    {
        
    }
}

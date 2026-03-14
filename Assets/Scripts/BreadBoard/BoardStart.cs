using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BoardStart : MonoBehaviour, IInteractable
{
    [SerializeField] private List<BoardConnector> _adjConnectors = new List<BoardConnector>();
    [SerializeField] private LayerMask _interactMask;
    [SerializeField] private LineRenderer _lineRenderer;
    private bool _isTracking;

    private void OnValidate()
    {
        if(_lineRenderer == null) _lineRenderer = GetComponent<LineRenderer>();
    }

    public void Interact(GameObject interactor)
    {
        Debug.Log("HELLLO");
        _lineRenderer.positionCount = 2;
        _isTracking = true;
        _lineRenderer.SetPosition(0, transform.position);
    }

    private void Update()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, Mathf.Infinity, _interactMask))
        {
            
        }
        if (_isTracking)
        {
            _lineRenderer.SetPosition(1, hitInfo.point);
        }
    }

    public void StopInteract(GameObject interactor)
    {
        
    }
}

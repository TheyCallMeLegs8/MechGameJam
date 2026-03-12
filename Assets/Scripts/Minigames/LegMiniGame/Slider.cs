using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Clamp> _clamps = new List<Clamp>();
    [SerializeField] private Vector3 _disposePosition;
    [SerializeField] private GameObject _brokenMarker;
    
    private int _totalClamps;
    private bool _isClamped = true;
    private bool _isDisposed = false;
    private bool _isBroken = false;

    private void OnEnable()
    {
        foreach (Clamp clamp in _clamps)
        {
            clamp.OnClamp.AddListener(AddClamp);
            clamp.OnUnClamp.AddListener(SubtractClamp);
            AddClamp();
        }
    }

    private void OnDisable()
    {
        foreach (Clamp clamp in _clamps)
        {
            clamp.OnClamp.RemoveListener(AddClamp);
            clamp.OnUnClamp.RemoveListener(SubtractClamp);
        }
    }

    public void SetBroken()
    {
        _isBroken = true;
        _brokenMarker.SetActive(true);
    }

    private void SubtractClamp()
    {
        _totalClamps--;
        if (_totalClamps <= 0 ) _isClamped = false;
    }

    private void AddClamp()
    {
        _totalClamps++;
        if (_totalClamps > 0) _isClamped = true;
    }

    public void Interact(GameObject interactor)
    {
        if (_isClamped) return;
        if (_isDisposed) return;
        transform.localPosition = _disposePosition;
        _isDisposed = true;
    }

    public void StopInteract(GameObject interactor)
    {

    }
}

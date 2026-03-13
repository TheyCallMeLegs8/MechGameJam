using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Slider : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Clamp> _clamps = new List<Clamp>();
    [field: SerializeField] public GameObject _sliderMesh { get; private set; }
    [SerializeField] private GameObject _brokenMarker;
    [field: SerializeField] public int Id { get; private set; } = 1;
    private bool _isRepairSlider = false;

    private int _totalClamps;
    private bool _isClamped = true;
    public bool IsDisposed { get; private set; } = false;
    public bool IsBroken { get; private set; } = false;

    [SerializeField] public UnityEvent OnDispose = new UnityEvent();

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
        IsBroken = true;
        _brokenMarker.SetActive(true);
    }

    public void SetFixed()
    {
        IsBroken = false;
        _brokenMarker.SetActive(false);
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

    public void SetIsRepairSlider(bool isRepairSlider)
    {
        _isRepairSlider = isRepairSlider;
    }

    public void Interact(GameObject interactor)
    {
        if (_isClamped) return;
        if (IsDisposed) return;

        if (!_isRepairSlider)
        {
            OnDispose.Invoke();
            IsDisposed = true;
        }
    }

    public void StopInteract(GameObject interactor)
    {

    }
}

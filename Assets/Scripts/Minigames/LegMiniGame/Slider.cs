using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class Slider : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Clamp> _clamps = new List<Clamp>();
    private List<Clamp> _savedClamps = new List<Clamp>();
    [field: SerializeField] public GameObject _sliderMesh { get; private set; }
    [SerializeField] private GameObject _brokenMarker;
    [field: SerializeField] public int Id { get; private set; } = 1;
    [field: SerializeField] public bool IsRepairSlider { get; private set; } = false;
    [SerializeField] private bool _startUnClamped = false;

    public List<Clamp> NewClamps = new List<Clamp>();

    private int _totalClamps;
    private bool _isClamped = true;
    public bool IsDisposed { get; private set; } = false;
    public bool IsBroken { get; private set; } = false;

    [SerializeField] public UnityEvent OnDispose = new UnityEvent();
    [SerializeField] public UnityEvent<GameObject> OnClickRepair = new UnityEvent<GameObject>();
    public bool IsRepairing = false;
    public bool NewIsClamped = false;

    public UnityEvent OnRepairNewSlider = new UnityEvent();
    public int NewClampsAmount = 0;

    private void OnEnable()
    {
        foreach (Clamp clamp in _clamps)
        {
            if(clamp != null && !_startUnClamped)
            {
                clamp?.OnClamp.AddListener(AddClamp);
                clamp?.OnUnClamp.AddListener(SubtractClamp);
                AddClamp();
            }
        }

        for(int i = 0;  i < _clamps.Count; i++)
        {
            _savedClamps.Add(_clamps[i]);
        }
    }

    private void OnDisable()
    {
        foreach (Clamp clamp in _clamps)
        {
            if (clamp != null)
            {
                _savedClamps.Remove(clamp);

                clamp?.OnClamp.RemoveListener(AddClamp);
                clamp?.OnUnClamp.RemoveListener(SubtractClamp);
            }
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
        IsRepairSlider = isRepairSlider;
    }

    public void Interact(GameObject interactor)
    {
        if (_isClamped && !_startUnClamped) return;
        if (IsDisposed) return;
        if (NewClampsAmount != 0) return;

        if (!IsRepairSlider)
        {
            OnDispose.Invoke();
            IsDisposed = true;
        }
        else
        {
            OnClickRepair.Invoke(gameObject);
        }
    }

    public void SetIsRepairing(bool isRepairing)
    {
        IsRepairing = isRepairing;
    }

    public void SetNewSliderForClamps(Slider newSlider, bool getNewSliders)
    {
        if (getNewSliders)
        {
            AddNewSliders(newSlider);
        }
        else
        {
            RemoveNewSliders(newSlider);
        }
    }

    private void AddNewSliders(Slider newSlider)
    {        
        for (int i = 0; i < _savedClamps.Count; i++)
        {
            newSlider.NewClamps.Add(_savedClamps[i]);
        }

        foreach(Clamp clamp in newSlider.NewClamps)
        {
            clamp.SetNewRepairSlider(newSlider);
        }
    }

    private void RemoveNewSliders(Slider newSlider)
    {
        newSlider.NewClamps.Clear();
        foreach (Clamp clamp in newSlider.NewClamps)
        {
            clamp.SetNewRepairSlider(null);
        }
    }

    public void NewSliderAddClamp(Slider newSlider)
    {
        // prevent original one from doing this
        if (newSlider.NewClamps.Count == 0) return;
        newSlider.NewClampsAmount++;

        if(newSlider.NewClampsAmount == newSlider.NewClamps.Count)
        {
            OnRepairNewSlider.Invoke();
        }
    }

    public void NewSliderRemoveClamp(Slider newSlider)
    {
        // prevent original one from doing this
        if (newSlider.NewClamps.Count == 0) return;
        newSlider.NewClampsAmount--;
    }


    public void StopInteract(GameObject interactor)
    {

    }
}

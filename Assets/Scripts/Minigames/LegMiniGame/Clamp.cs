using UnityEngine;
using UnityEngine.Events;

public class Clamp : MonoBehaviour, IInteractable
{
    [SerializeField] private float _moveToPoint;
    [SerializeField] private Slider _slider;
    private Slider _newRepairSlider;

    private bool _isClamped = true;
    private bool _canInteract = true;
    [SerializeField] public UnityEvent OnUnClamp = new UnityEvent();
    [SerializeField] public UnityEvent OnClamp = new UnityEvent();

    private void OnDisable()
    {
        _newRepairSlider?.OnRepairNewSlider.RemoveListener(StopInteract);
    }

    public void Interact(GameObject interactor)
    {
        if(!_canInteract) return;
        if (!_slider.IsBroken) return;
        if (_slider.IsDisposed && _newRepairSlider == null) return;
        if (_newRepairSlider != null && _newRepairSlider?.Id != _slider.Id) return;

        if (_isClamped)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + _moveToPoint, transform.localPosition.z);
            _isClamped = false;
            OnUnClamp.Invoke();

            if (_newRepairSlider != null)
            {
                _newRepairSlider.NewSliderRemoveClamp(_newRepairSlider);
            }
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - _moveToPoint, transform.localPosition.z);
            _isClamped = true;
            OnClamp.Invoke();

            if (_newRepairSlider?.Id == _slider.Id)
            {
                _newRepairSlider.NewSliderAddClamp(_newRepairSlider);
            }

        }
    }

    public void SetNewRepairSlider(Slider newRepairSlider)
    {
        if(newRepairSlider.Id != _slider.Id) return;
        _newRepairSlider = newRepairSlider;
        _newRepairSlider.OnRepairNewSlider.AddListener(StopInteract);
    }

    private void StopInteract()
    {
        _canInteract = false;
    }

    public void StopInteract(GameObject interactor)
    {
        
    }
}

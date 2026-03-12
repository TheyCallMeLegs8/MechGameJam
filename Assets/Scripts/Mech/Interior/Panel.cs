using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

public class Panel : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _panelHinge;
    [SerializeField] private Quaternion _panelTargetRot;
    [SerializeField] private float _panelSpeed = 10.0f;
    [SerializeField] private Ease _openEasing;

    private Quaternion _baseRotation;

    private bool _isOpen = false;

    [SerializeField] public UnityEvent OnOpenPanel = new UnityEvent();
    [SerializeField] public UnityEvent OnClosePanel = new UnityEvent();

    private void Start()
    {
        _baseRotation = transform.localRotation;
    }

    public void Interact(GameObject interactor)
    {
        if (!_isOpen)
        {
            OpenPanel();
        }
    }

    public void StopInteract(GameObject interactor)
    {

    }

    public void OpenPanel()
    {
        Tween.LocalRotationAtSpeed(_panelHinge, _panelTargetRot, _panelSpeed, _openEasing);
        _isOpen = true;
        OnOpenPanel.Invoke();
    }

    public void ClosePanel()
    {
        Tween.LocalRotationAtSpeed(_panelHinge, _baseRotation, _panelSpeed, _openEasing);
        _isOpen = false;
        OnClosePanel.Invoke();
    }

    public void EndMinigame()
    {
        if (_isOpen)
        {
            ClosePanel();
        }
    }
}

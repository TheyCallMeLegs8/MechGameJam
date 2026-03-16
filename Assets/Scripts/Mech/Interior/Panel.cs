using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

public class Panel : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _panelHinge;
    [SerializeField] private Quaternion _panelTargetRot;
    [SerializeField] private float _panelSpeed = 10.0f;
    [SerializeField] private Ease _openEasing;
    [SerializeField] private SoundPlayer _soundPlayer;

    private Quaternion _baseRotation;

    private bool _isOpen = false;

    [SerializeField] public UnityEvent OnOpenPanel = new UnityEvent();
    [SerializeField] public UnityEvent OnClosePanel = new UnityEvent();
    [SerializeField] public UnityEvent<Panel> OnFix = new UnityEvent<Panel>();
    [SerializeField] private GameObject _greenLight;
    [SerializeField] private GameObject _redLight;


    [field: SerializeField] public bool IsBroken { get; private set; } = false;

    private void Start()
    {
        _baseRotation = transform.localRotation;
    }

    public void Interact(GameObject interactor)
    {
        if (!_isOpen && IsBroken)
        {
            OpenPanel();
        }
    }

    public void StopInteract(GameObject interactor)
    {

    }

    public void SetBroken(bool isBroken)
    {
        IsBroken = isBroken;
        _redLight.SetActive(isBroken);
        _greenLight.SetActive(!isBroken);
        if(IsBroken == true)
        {
            _soundPlayer.PlayAudio();
        }
        else
        {
            _soundPlayer.StopAudio();
        }
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

    public void FixPanel()
    {
        SetBroken(false);
        OnFix.Invoke(this);
    }
}

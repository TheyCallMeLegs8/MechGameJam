using UnityEngine;
using UnityEngine.Events;

public class RadarButtonGame : MonoBehaviour
{
    [field: SerializeField] public InteractButton _button { get; private set; }
    [SerializeField] private MeshRenderer _lightMesh;
    [SerializeField] private Material _readyMat;
    [SerializeField] private Material _onMat;
    [SerializeField] private Material _offMat;
    private InteractButton _interactButton;

    public bool IsReady { get; private set; } = false;
    public bool IsOn { get; private set; } = false;
    public bool IsOff { get; private set; } = true;

    [SerializeField] public UnityEvent<RadarButtonGame> OnClick = new UnityEvent<RadarButtonGame>();

    private void OnValidate()
    {
        if(_button == null) _button = GetComponent<InteractButton>();
    }

    private void Awake()
    {
        _interactButton = GetComponent<InteractButton>();
    }

    private void OnEnable()
    {
        _interactButton.OnInteract.AddListener(Click);
    }

    private void OnDisable()
    {
        _interactButton.OnInteract.RemoveListener(Click);
    }

    // sends event
    private void Click()
    {
        OnClick.Invoke(this);
    }

    public void SetReady()
    {
        IsOff = false;
        IsOn = false;
        IsReady = true;
        _lightMesh.material = _readyMat;
    }

    public void TurnOn()
    {
        IsOff = false;
        IsOn = true;
        IsReady = false;
        _lightMesh.material = _onMat;
    }

    public void TurnOff()
    {
        IsOff = true;
        IsOn = false;
        IsReady = false;
        _lightMesh.material = _offMat;
    }
}

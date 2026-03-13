using UnityEngine;

public class PuzzleLight : MonoBehaviour
{
    [SerializeField] private Material _onMat;
    [SerializeField] private Material _offMat;
    [SerializeField] private MeshRenderer _meshRenderer;

    //[SerializeField] private bool _startOn = false; // just for Demo

    public bool IsOn { get; private set; }
    public bool IsOff { get; private set; }

    private void OnValidate()
    {
        if(_meshRenderer == null) _meshRenderer = GetComponent<MeshRenderer>();
    }

    // called my mini game Manager
    public void StartOff()
    {
        IsOff = true;
        IsOn = false;
        _meshRenderer.material = _offMat;
    }

    public void Switch()
    {
        if (IsOff)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }

    private void TurnOn()
    {
        IsOff = false;
        IsOn = true;
        _meshRenderer.material = _onMat;
    }

    private void TurnOff()
    {
        IsOff = true;
        IsOn = false;
        _meshRenderer.material = _offMat;
    }
}

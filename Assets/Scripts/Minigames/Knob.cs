using UnityEngine;
using UnityEngine.Events;

public class Knob : MonoBehaviour, IInteractable
{
    [SerializeField] private PuzzleLight[] puzzleLights;
    [SerializeField] private GameObject _knobTop;

    [SerializeField] public UnityEvent OnInteract = new UnityEvent();

    public void Interact(GameObject interactor)
    {
        OnInteract.Invoke();

        foreach (PuzzleLight light in puzzleLights)
        {
            light.Switch();
        }
        _knobTop.transform.localRotation = new Quaternion(_knobTop.transform.localRotation.x, _knobTop.transform.localRotation.y * -1, _knobTop.transform.localRotation.z, _knobTop.transform.localRotation.w);
        _knobTop.transform.localScale = new Vector3(_knobTop.transform.localScale.x, _knobTop.transform.localScale.y * -1, _knobTop.transform.localScale.z);
    }

    public void StopInteract(GameObject interactor)
    {

    }
}

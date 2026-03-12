using UnityEngine;

public class Knob : MonoBehaviour, IInteractable
{
    [SerializeField] private PuzzleLight[] puzzleLights;
    [SerializeField] private GameObject _knobTop;

    public void Interact(GameObject interactor)
    {
        foreach(PuzzleLight light in puzzleLights)
        {
            light.Switch();
        }
        _knobTop.transform.localPosition = new Vector3(_knobTop.transform.localPosition.x, _knobTop.transform.localPosition.y * -1, _knobTop.transform.localPosition.z);
    }

    public void StopInteract(GameObject interactor)
    {

    }
}

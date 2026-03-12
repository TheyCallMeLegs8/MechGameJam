using UnityEngine;
using UnityEngine.Events;

public class Clamp : MonoBehaviour, IInteractable
{
    [SerializeField] private float _moveToPoint;

    private bool _isClamped = true;

    [SerializeField] public UnityEvent OnUnClamp = new UnityEvent();
    [SerializeField] public UnityEvent OnClamp = new UnityEvent();

    public void Interact(GameObject interactor)
    {
        if (_isClamped)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + _moveToPoint, transform.localPosition.z);
            _isClamped = false;
            OnUnClamp.Invoke();
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - _moveToPoint, transform.localPosition.z);
            _isClamped = true;
            OnClamp.Invoke();
        }
    }

    public void StopInteract(GameObject interactor)
    {
        
    }
}

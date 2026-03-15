using UnityEngine;
using UnityEngine.Events;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField] public UnityEvent OnCollide = new UnityEvent();
    [SerializeField] public UnityEvent OnEnterPoisonGas = new UnityEvent();
    [SerializeField] public UnityEvent OnExitPoisonGas = new UnityEvent();

    private void OnCollisionEnter(Collision collision)
    {
        // dont hit self
        if(collision.gameObject == gameObject) return;
        OnCollide.Invoke();
    }

    public void EnterPoisonGas()
    {
        OnEnterPoisonGas.Invoke();
    }

    public void ExitPoisonGas()
    {
        OnExitPoisonGas.Invoke();
    }
}

using UnityEngine;
using UnityEngine.Events;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField] public UnityEvent OnCollide = new UnityEvent();

    private void OnCollisionEnter(Collision collision)
    {
        // dont hit self
        if(collision.gameObject == gameObject) return;
        OnCollide.Invoke();
    }
}

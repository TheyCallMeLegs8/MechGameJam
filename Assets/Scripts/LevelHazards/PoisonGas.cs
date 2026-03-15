using UnityEngine;

public class PoisonGas : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out CollisionDetector collisionDetector))
        {
            collisionDetector.EnterPoisonGas();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CollisionDetector collisionDetector))
        {
            collisionDetector.ExitPoisonGas();
        }
    }
}

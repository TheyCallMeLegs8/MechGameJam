using UnityEngine;
using UnityEngine.Events;

public class DamageSystem : MonoBehaviour
{
    public UnityEvent OnDoDamage = new UnityEvent();

    public void DoDamage()
    {
        // damage a randome active system
        OnDoDamage.Invoke();
    }
}

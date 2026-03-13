using UnityEngine;
using UnityEngine.Events;

public abstract class MiniGameBase : MonoBehaviour
{
    [SerializeField] public UnityEvent OnGameComplete = new UnityEvent();

    public virtual void StartMinigame() { }
    public virtual void EndMinigame() { }
}

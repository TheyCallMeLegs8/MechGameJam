using UnityEngine;
using UnityEngine.Events;

public class Chair : MonoBehaviour
{
    [SerializeField] public UnityEvent OnPlayerLeave = new UnityEvent();
    private PlayerControls _player;

    public void GetPlayer(PlayerControls player)
    {
        _player = player;
        _player.EnterWalkingState.AddListener(PlayerWalkingState);
    }

    private void PlayerWalkingState()
    {
        OnPlayerLeave.Invoke();
        RemovePlayer();
    }

    private void RemovePlayer()
    {
        _player.EnterWalkingState.RemoveListener(PlayerWalkingState);
        _player = null;
    }
}

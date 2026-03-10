using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Movement))]

public class PlayerControls : MonoBehaviour
{
    [field: SerializeField] public PlayerInput PlayerInput { get; private set; }
    [field: SerializeField] public Movement Movement { get; private set; }

    private void OnValidate()
    {
        if(PlayerInput == null) PlayerInput = GetComponent<PlayerInput>();
        if(Movement == null) Movement = GetComponent<Movement>();
    }

    public void OnMove(InputValue inputValue)
    {
        Movement.SetMoveInput2D(inputValue.Get<Vector2>());
    }

    public void OnLook(InputValue inputValue)
    {
        Movement.SetLookInput(inputValue.Get<Vector2>());
    }
}

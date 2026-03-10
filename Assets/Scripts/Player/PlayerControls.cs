using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Movement))]

public class PlayerControls : MonoBehaviour
{
    [field: SerializeField] public PlayerInput PlayerInput { get; private set; }
    [field: SerializeField] public Movement Movement { get; private set; }
    [field: SerializeField] public CinemachineCamera Camera { get; private set; }
    private Vector2 LookInput;
    private void OnValidate()
    {
        if(PlayerInput == null) PlayerInput = GetComponent<PlayerInput>();
        if(Movement == null) Movement = GetComponent<Movement>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMove(InputValue inputValue)
    {
        Movement.SetMoveInput2D(inputValue.Get<Vector2>());
    }

    public void OnLook(InputValue inputValue)
    {
        LookInput = inputValue.Get<Vector2>();
        Movement.SetLookInput(inputValue.Get<Vector2>());
    }

    private void Update()
    {
        // map 2D input to 3D space before moving character
        Vector3 right = UnityEngine.Camera.main.transform.right; // thumb
        Vector3 up = Vector3.up;                     // pointer finger
        Vector3 forward = Vector3.Cross(right, up);  // middle finger
        Vector3 moveInput3D = forward * Movement.MoveInput2D.y + right * Movement.MoveInput2D.x;

        // send move input to movement component
        Movement.SetMoveInput(moveInput3D);
    }

    private void FixedUpdate()
    {
        LookUpdate();
    }

    public void LookUpdate()
    {
        Vector2 input = new Vector2(LookInput.x * Movement.LookSensitivity.x, LookInput.y * Movement.LookSensitivity.y);
        // handles look up and down
        Movement.CurrentPitch -= input.y;
        Camera.transform.localRotation = Quaternion.Euler(Movement.CurrentPitch, 0f, 0f);

        // handles looking side to side
        transform.Rotate(Vector3.up * input.x);
    }
}

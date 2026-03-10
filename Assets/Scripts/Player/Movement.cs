using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CapsuleCollider))]

public class Movement : MonoBehaviour
{
    [Header("Componenets")]
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public CinemachineCamera Camera { get; private set; }

    [Header("Locomotion")]
    [field: SerializeField] public float Speed { get; private set; } = 5.0f;

    [Header("Rotation")]
    [field:SerializeField] public Vector2 LookSensitivity { get; private set; } = new Vector2(0.1f, 0.1f);
    [Tooltip("Only used when want to rotate specific object at speed")]
    [field: SerializeField] public float TurnSpeed { get; private set; } = 30.0f;
    [field: SerializeField] public float MaxPitch { get; private set; } = 85.0f;
    public Vector2 LookInput { get; private set; }
    private float _currentPitch = 0.0f;
    
    public float CurrentPitch
    {
        get => _currentPitch;

        set
        {
            _currentPitch = Mathf.Clamp(value, -MaxPitch, MaxPitch);
        }
    }

    public Vector2 MoveInput2D { get; private set; }
    public bool HasMoveInput { get; private set; } = false;
    public Vector3 MoveInput { get; private set; }
    public Vector3 LocalMoveInput { get; private set; }

    private void OnValidate()
    {
        if (CharacterController == null) CharacterController = GetComponent<CharacterController>();
    }

    public void SetMoveInput2D(Vector2 input)
    {
        MoveInput2D = input;
    }

    public void SetLookInput(Vector2 input)
    {
        LookInput = input;
    }

    private void Update()
    {
        // map 2D input to 3D space before moving character
        Vector3 right = UnityEngine.Camera.main.transform.right; // thumb
        Vector3 up = Vector3.up;                     // pointer finger
        Vector3 forward = Vector3.Cross(right, up);  // middle finger
        Vector3 moveInput3D = forward * MoveInput2D.y + right * MoveInput2D.x;

        // send move input to movement component
        SetMoveInput(moveInput3D);
    }

    private void FixedUpdate()
    {
        CharacterController.Move(MoveInput * Speed * Time.deltaTime);
        LookUpdate();
    }

    public void LookUpdate()
    {
        Vector2 input = new Vector2(LookInput.x * LookSensitivity.x, LookInput.y * LookSensitivity.y);
        // handles look up and down
        CurrentPitch -= input.y;
        Camera.transform.localRotation = Quaternion.Euler(CurrentPitch, 0f, 0f);

        Debug.Log(input);
        transform.Rotate(Vector3.up * input.x);
    }

    public void RotateXAtSpeed(float speed)
    {
        transform.Rotate(Vector3.up * speed * Time.fixedDeltaTime);
    }

    public void SetMoveInput(Vector3 input)
    {
        input = Vector3.ClampMagnitude(input, 1f);
        // set input to 0 if small incoming value
        HasMoveInput = input.magnitude > 0.1f;
        input = HasMoveInput ? input : Vector3.zero;
        // remove y component of movement but retain overall magnitude
        Vector3 flattened = new Vector3(input.x, 0f, input.z);
        flattened = flattened.normalized * input.magnitude;
        MoveInput = flattened;
        // finds movement input as local direction rather than world direction
        LocalMoveInput = transform.InverseTransformDirection(MoveInput);
    }
}

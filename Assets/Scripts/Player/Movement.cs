using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CapsuleCollider))]

public class Movement : MonoBehaviour
{
    [Header("Componenets")]
    [field: SerializeField] public CharacterController CharacterController { get; private set; }

    [Header("Locomotion")]
    [field: SerializeField] public float Speed { get; private set; } = 5.0f;

    [Header("Rotation")]
    [field:SerializeField] public Vector2 LookSensitivity { get; private set; } = new Vector2(0.1f, 0.1f);
    [Tooltip("Only used when want to rotate specific object at speed")]
    [field: SerializeField] public float TurnSpeed { get; private set; } = 30.0f;
    [field: SerializeField] public float EndRotationSpeed { get; private set; } = 60.0f;
    [field: SerializeField] public ShakeSettings EndShakeSettingsLeft { get; private set; }
    [field: SerializeField] public ShakeSettings EndShakeSettingRight { get; private set; }
    [field: SerializeField] public float MaxPitch { get; private set; } = 85.0f;
    private float _currentPitch = 0.0f;
    
    public float CurrentPitch
    {
        get => _currentPitch;

        set
        {
            _currentPitch = Mathf.Clamp(value, -MaxPitch, MaxPitch);
        }
    }

    public Vector2 LookInput { get; private set; }
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

    private void FixedUpdate()
    {
        CharacterController.Move(MoveInput * Speed * Time.deltaTime);
    }

    public void RotateXPosAtSpeed(Transform targetTransform, float speed)
    {
        targetTransform.Rotate(Vector3.up * speed * Time.deltaTime);
    }

    public void MatchTransformRotations(Transform anchorTransform, Transform movingTransform, float speed)
    {
        Tween.LocalRotationAtSpeed(movingTransform, anchorTransform.localRotation, speed);
    }

    public void EndRotation(int punchDir)
    {
        if(punchDir <= 0)
        {
            Tween.PunchLocalRotation(transform, EndShakeSettingsLeft);
        }
        else
        {
            Tween.PunchLocalRotation(transform, EndShakeSettingRight);
        }
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

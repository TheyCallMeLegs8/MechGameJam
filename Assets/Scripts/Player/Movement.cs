using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class Movement : MonoBehaviour
{
    [Header("Componenets")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Rigidbody _rigidBody;

    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _turnSpeed = 30.0f;
    [SerializeField] private float _turnSpeedMultiplier = 1.0f;

    private Vector2 _moveInput2D;
    private bool _hasMoveInput = false;
    private Vector3 _moveInput;
    private Vector3 _localMoveInput;

    private Vector3 _lookDirection;
    private float _lookX;
    private bool _hasTurnInput = false;

    private void OnValidate()
    {
        if (_characterController == null) _characterController = GetComponent<CharacterController>();
        if (_rigidBody == null) _rigidBody = GetComponent<Rigidbody>();
    }

    public void OnMove(InputValue inputValue)
    {
        _moveInput2D = inputValue.Get<Vector2>();
    }

    private void Update()
    {
        // map 2D input to 3D space before moving character
        Vector3 right = Camera.main.transform.right; // thumb
        Vector3 up = Vector3.up;                     // pointer finger
        Vector3 forward = Vector3.Cross(right, up);  // middle finger
        Vector3 moveInput3D = forward * _moveInput2D.y + right * _moveInput2D.x;

        // send move input to movement component
        SetMoveInput(moveInput3D);
        //SetLookDirection(moveInput3D);
        SetLookDirection(Camera.main.transform.forward);
        if (_hasTurnInput)
        {
            Quaternion rotation = _rigidBody.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(_lookDirection);
            rotation = Quaternion.Slerp(transform.rotation, targetRotation, _turnSpeed * _turnSpeedMultiplier * Time.deltaTime);
            _rigidBody.MoveRotation(rotation);
        }
        _characterController.Move(moveInput3D * _speed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
    }

    private void SetMoveInput(Vector3 input)
    {
        input = Vector3.ClampMagnitude(input, 1f);
        // set input to 0 if small incoming value
        _hasMoveInput = input.magnitude > 0.1f;
        input = _hasMoveInput ? input : Vector3.zero;
        // remove y component of movement but retain overall magnitude
        Vector3 flattened = new Vector3(input.x, 0f, input.z);
        flattened = flattened.normalized * input.magnitude;
        _moveInput = flattened;
        // finds movement input as local direction rather than world direction
        _localMoveInput = transform.InverseTransformDirection(_moveInput);
    }

    public void SetLookDirection(Vector3 direction)
    {
        if (direction.magnitude < 0.1f)
        {
            _hasTurnInput = false;
            return;
        }
        _hasTurnInput = true;
        _lookDirection = new Vector3(direction.x, 0f, direction.z).normalized;
    }

    public void SetLookPosition(Vector3 position)
    {
        Vector3 direction = Vector3.ClampMagnitude(position - transform.position, 1f);
        SetLookDirection(direction);
    }
}

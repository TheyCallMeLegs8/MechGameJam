using PrimeTween;
using System.Collections;
using System.Text.RegularExpressions;
using Unity.Cinemachine;
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
    [SerializeField] private CinemachineCamera _camera;

    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _turnSpeed = 30.0f;
    [SerializeField] private float _turnSpeedMultiplier = 1.0f;
    [SerializeField] private int _targetFPS = -1;

    [Space]
    [SerializeField] private Vector2 _lookSensitivity = new Vector2(0.1f, 0.1f);
    [SerializeField] private float _maxPitch = 85.0f;
    [SerializeField] private float _lookX;
    private Vector2 _lookInput;
    private float _currentPitch = 0.0f;
    public float CurrentPitch
    {
        get => _currentPitch;

        set
        {
            _currentPitch = Mathf.Clamp(value, -_maxPitch, _maxPitch);
        }
    }

    private bool _canLookTEST = true;

    private Vector2 _moveInput2D;
    private bool _hasMoveInput = false;
    private Vector3 _moveInput;
    private Vector3 _localMoveInput;

    private Vector3 _lookDirection;
    private bool _hasTurnInput = false;

    private void Start()
    {
        Application.targetFrameRate = _targetFPS;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnValidate()
    {
        if (_characterController == null) _characterController = GetComponent<CharacterController>();
        if (_rigidBody == null) _rigidBody = GetComponent<Rigidbody>();
    }

    public void OnMove(InputValue inputValue)
    {
        _moveInput2D = inputValue.Get<Vector2>();
    }

    public void OnLook(InputValue inputValue)
    {
        _lookInput = inputValue.Get<Vector2>();
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

        //LookUpdate(_turnSpeed);

        if (Input.GetKey(KeyCode.M))
        {
            _canLookTEST = true;
            //StartCoroutine(LookWithSpeed());
        }
        else
        {
            _canLookTEST = false;
        }

        /*
        SetLookDirection(Camera.main.transform.forward);
        if (_hasTurnInput)
        {
            Quaternion rotation = _rigidBody.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(_lookDirection);
            rotation = Quaternion.Slerp(transform.rotation, targetRotation, _turnSpeed * _turnSpeedMultiplier * Time.deltaTime);
            _rigidBody.MoveRotation(rotation);
        }*/
    }

    private void FixedUpdate()
    {
        _characterController.Move(_moveInput * _speed * Time.deltaTime);
        if (_canLookTEST)
        {
            
        }
        LookUpdate(_turnSpeed);
    }

    private void LookUpdate(float speed)
    {
        Vector2 input = new Vector2(_lookInput.x * _lookSensitivity.x, _lookInput.y * _lookSensitivity.y);
        // handles look up and down
        CurrentPitch -= input.y;
        _camera.transform.localRotation = Quaternion.Euler(CurrentPitch, 0f, 0f);

        Debug.Log(input);
        // handles look left and right, SHOULD BE INPUT.X IN HERE
        transform.Rotate(Vector3.up * input.x);
        //transform.Rotate(Vector3.up * speed * Time.fixedDeltaTime);
    }

    private IEnumerator LookWithSpeed()
    {
        while (_canLookTEST)
        {
            LookUpdate(_turnSpeed);
            yield return null;
        }
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

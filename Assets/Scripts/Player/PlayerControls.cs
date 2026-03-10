using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Movement))]

public class PlayerControls : MonoBehaviour
{
    [Header("Components")]
    [field: SerializeField] public PlayerInput PlayerInput { get; private set; }
    [field: SerializeField] public Movement Movement { get; private set; }
    [field: SerializeField] public CinemachineCamera Camera { get; private set; }

    [Header("Interactable")]
    [SerializeField] private float _interactRange = 3.0f;
    [SerializeField] private LayerMask _interactMask;
    [SerializeField] private Vector3 _interactOffset = Vector3.zero;
    private IInteractable _currentInteractObj;

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

    public void OnInteract(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            if (Physics.Raycast(Camera.transform.position + _interactOffset, Camera.transform.forward, out RaycastHit hitInfo, _interactRange, _interactMask))
            {
                if (hitInfo.transform.gameObject.TryGetComponent(out IInteractable interactable))
                {
                    _currentInteractObj = interactable;
                    _currentInteractObj.Interact(gameObject);
                }
            }
        }
        else
        {
            if (_currentInteractObj != null) _currentInteractObj.StopInteract(gameObject);
        }
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

        Debug.DrawLine(Camera.transform.position + _interactOffset, Camera.transform.forward * _interactRange, Color.red);
        if(Physics.Raycast(Camera.transform.position + _interactOffset, Camera.transform.forward, out RaycastHit hitInfo, _interactRange, _interactMask))
        {
            if (hitInfo.transform.gameObject.TryGetComponent(out IInteractable interactable))
            {
                // Do something
            }
        }
    }

    public void LookUpdate()
    {
        Vector2 input = new Vector2(LookInput.x * Movement.LookSensitivity.x, LookInput.y * Movement.LookSensitivity.y);
        // handles look up and down
        Movement.CurrentPitch -= input.y * Time.deltaTime;
        Camera.transform.localRotation = Quaternion.Euler(Movement.CurrentPitch, 0f, 0f);

        // handles looking side to side
        transform.Rotate(Vector3.up * input.x * Time.deltaTime);
    }
}

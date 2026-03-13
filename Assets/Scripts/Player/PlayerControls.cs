using Unity.Cinemachine;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum PlayerStates
{
    Walking,
    Sitting,
    Minigame
}

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Movement))]

public class PlayerControls : MonoBehaviour
{
    [Header("Components")]
    [field: SerializeField] public PlayerInput PlayerInput { get; private set; }
    [field: SerializeField] public Movement Movement { get; private set; }
    [field: SerializeField] public CinemachineCamera PlayerCamera { get; private set; }

    private bool _canMove = true;
    private bool _canLook = true;

    [Header("Interactable")]
    [SerializeField] private float _interactRange = 3.0f;
    [SerializeField] private LayerMask _interactMask;
    [SerializeField] private Vector3 _interactOffset = Vector3.zero;

    private IInteractable _currentInteractable;
    private GameObject _currentInteractObject;
    private IInteractable _currentClickedInteractable;

    private PlayerStates _currentState;
    private PlayerStates _previousState;
    private bool _canExit = false;

    private Panel _currentPanel;

    [SerializeField] public UnityEvent EnterWalkingState = new UnityEvent();
    [SerializeField] public UnityEvent EnterMinigameState = new UnityEvent();
    [SerializeField] public UnityEvent EnterSittingState = new UnityEvent();

    private void OnValidate()
    {
        if(PlayerInput == null) PlayerInput = GetComponent<PlayerInput>();
        if(Movement == null) Movement = GetComponent<Movement>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _currentState = PlayerStates.Walking;
    }

    public void OnMove(InputValue inputValue)
    {
        if(!_canMove) return; // ?
        Movement.SetMoveInput2D(inputValue.Get<Vector2>());
    }

    public void OnLook(InputValue inputValue)
    {
        if(!_canLook) return;
        Movement.SetLookInput(inputValue.Get<Vector2>());
    }

    // gets interacables from update and when you click it interacts with them and stores one for when you un-click
    public void OnInteract(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            _currentInteractable?.Interact(gameObject);
            _currentClickedInteractable = _currentInteractable;

            if (_currentInteractObject != null)
            {
                if (_currentInteractObject.TryGetComponent(out Panel panel))
                {
                    if (_currentPanel != null) return;
                    _currentPanel = panel;
                    MinigameState();
                }
            }
        }
        else
        {
            _currentClickedInteractable?.StopInteract(gameObject);
            _currentClickedInteractable = null;
        }
    }

    public void OnExit()
    {
        if(!_canExit) return;
        Exit();
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

        _currentInteractable = null;
        _currentInteractObject = null;
        if (_currentState == PlayerStates.Walking)
        {
            if (Physics.Raycast(PlayerCamera.transform.position + _interactOffset, PlayerCamera.transform.forward, out RaycastHit hitInfo, _interactRange, _interactMask))
            {
                if (hitInfo.transform.gameObject.TryGetComponent(out IInteractable interactable))
                {
                    _currentInteractable = interactable;
                    _currentInteractObject = hitInfo.transform.gameObject;
                }
            }
        }
        else if(_currentState == PlayerStates.Minigame)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, Mathf.Infinity, _interactMask))
            {
                if (hitInfo.transform.gameObject.TryGetComponent(out IInteractable interactable))
                {
                    _currentInteractable = interactable;
                    _currentInteractObject = hitInfo.transform.gameObject;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        LookUpdate();
    }

    public void LookUpdate()
    {
        if(!_canLook) return;
        Vector2 input = new Vector2(Movement.LookInput.x * Movement.LookSensitivity.x, Movement.LookInput.y * Movement.LookSensitivity.y);
        // handles look up and down
        Movement.CurrentPitch -= input.y * Time.deltaTime;
        PlayerCamera.transform.localRotation = Quaternion.Euler(Movement.CurrentPitch, 0f, 0f);

        // handles looking side to side
        transform.Rotate(Vector3.up * input.x * Time.deltaTime);
    }

    private void Exit()
    {
        WalkingState();
        if (_currentPanel != null)
        {
            _currentPanel.ClosePanel();
        }
        _currentPanel = null;
    }

    private void ChangeState(PlayerStates state)
    {
        _previousState = _currentState;
        _currentState = state;
    }

    private void MinigameState()
    {
        ChangeState(PlayerStates.Minigame);
        EnterMinigameState.Invoke();
        Movement.SetMoveInput2D(Vector3.zero);
        Movement.SetLookInput(Vector3.zero);
        _canMove = false;
        _canLook = false;
        _canExit = true;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void WalkingState()
    {
        ChangeState(PlayerStates.Walking);
        EnterWalkingState.Invoke();
        Movement.SetMoveInput2D(Vector3.zero);
        _canMove = true;
        _canLook = true;
        Movement.SetLookInput(Vector3.zero);
        _canExit = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

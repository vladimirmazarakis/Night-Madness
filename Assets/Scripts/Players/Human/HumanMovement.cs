using EZCameraShake;
using Mirror;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(CharacterController))]
public class HumanMovement : NetworkBehaviour
{
    [Header("Script Settings")]
    [Tooltip("The speed at which the player walks.")][SerializeField] private float _walkingSpeed = 1.0f; 
    [Tooltip("The speed at which the player crouch walks.")][SerializeField] private float _crouchingSpeed = 0.5f; 
    [Tooltip("The speed at which the player runs.")] [SerializeField] private float _runningSpeed = 2.0f;
    [SerializeField] private Transform _movementRayPoint;
    [SerializeField] private Camera _mainCamera;

    private float _actualSpeed = 1.0f;
    private CharacterController _controller;
    private InputMaster _input;
    private float _gravity = -9.81f;
    private bool _isSprinting = false;
    private bool _isCrouching = false;
    private bool _isKnocked = false;

    private void Awake()
    {
        if(_mainCamera is null)
        {
            Debug.LogError("Main camera is null. Please assign a camera to the main camera field.");
        }
        _input = new InputMaster();
        _input.Human.Movement.Enable();
        _input.Human.Sprinting.Enable();
        _input.Human.Sprinting.started += SprintingStarted;
        _input.Human.Sprinting.canceled += SprintingCanceled;
        _input.Human.Crouching.Enable();
        _input.Human.Crouching.started += CrouchingStarted;
        _input.Human.Crouching.canceled += CrouchingCanceled;
    }

    private void Start()
    {
        if(!isLocalPlayer)
        {
            this.enabled = false;
            return;
        }
        _controller = GetComponent<CharacterController>();
        _actualSpeed = _walkingSpeed;
    }

    private void Update()
    {
        Movement();
        ApplyGravity();
    }

    private void Movement()
    {
        var movementVector = _input.Human.Movement.ReadValue<Vector2>();

        if(movementVector.x == 0 && movementVector.y == 0)
        {
            IsMoving = false;
            return;
        }

        // creates the direction for the player to move in according to the camera
        var direction = ((movementVector.x * _mainCamera.transform.right) + (movementVector.y * _mainCamera.transform.forward)) * _actualSpeed;

        // do not move the play on y axis
        direction.y = 0;        

        // convert direction to rotation
        Quaternion rotation = Quaternion.LookRotation(direction);

        // smoothly rotate player
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10);

        _controller.SimpleMove(direction);
        IsMoving = true;
    }


    private void ApplyGravity()
    {
        _controller.Move(new Vector3(0, _gravity, 0) * Time.deltaTime);
    }

    private void SprintingStarted(CallbackContext ctx)
    {
        if(_isCrouching || _isKnocked || _isSprinting)
        {
            return;
        }
        if(IsMoving)
        {
            _actualSpeed = _runningSpeed;
            _isSprinting = true;
        }
    }

    private void SprintingCanceled(CallbackContext ctx)
    {
        if(_isSprinting)
        {
            _actualSpeed = _walkingSpeed;
            _isSprinting = false;
        }
    }

    private void CrouchingStarted(CallbackContext ctx)
    {
        if(_isSprinting || _isKnocked || _isCrouching)
        {
            return;
        }
        _actualSpeed = _crouchingSpeed;
        _isCrouching = true;
    }

    private void CrouchingCanceled(CallbackContext ctx)
    {
        if(_isCrouching)
        {
            _actualSpeed = _walkingSpeed;
            _isCrouching = false;
        }
    }

    public bool IsMoving { get; set; }

    public bool IsCrouching => _isCrouching;

    public bool IsSprinting => _isSprinting;
}

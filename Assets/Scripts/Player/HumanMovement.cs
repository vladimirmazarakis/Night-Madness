using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(HumanController))]
public class HumanMovement : MonoBehaviour, IMoveable
{
    #region Movement Settings
    [Header("Movement Settings")]
    [SerializeField] private Transform _movementRayPoint;
    [SerializeField] private float _movementSpeed = 2f;
    [SerializeField] private float _knockedSpeed = 0.75f;
    [SerializeField] private float _sprintSpeed = 5f;
    [SerializeField] private float _crouchSpeed = 1f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private LayerMask _movementFilterLayerMask;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private Camera _camera;

    private Rigidbody _rb;
    private RaycastHit _slopeHit;
    private bool _canMove = true;
    private bool _canSprint = true;
    private bool _canCrouch = true;
    private bool _isMoving = false;
    private InputMaster _inputMaster;
    private InputAction _movement;
    private HumanController _controller;
    #endregion
    #region Assignings and other unity methods.
    private void Awake()
    {
        _inputMaster = new InputMaster();
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _controller = GetComponent<HumanController>();
        _controller.gotKnocked += OnGotKnocked;
    }
    private void OnEnable()
    {
        _inputMaster.Human.Crouching.performed += Crouching_performed;
        _inputMaster.Human.Crouching.canceled += Crouching_canceled;
        _inputMaster.Human.Sprinting.performed += Sprinting_performed;
        _inputMaster.Human.Sprinting.canceled += Sprinting_canceled;
        _movement = _inputMaster.Shared.Movement;
        _movement.Enable();
        _inputMaster.Human.Sprinting.Enable();
        _inputMaster.Human.Crouching.Enable();
    }
    private void OnDisable()
    {
        _movement.Disable();
        _inputMaster.Human.Sprinting.Disable();
    }
    private void FixedUpdate()
    {
        if (_canMove)
        {
            Move();
        }
    }
    private void Update()
    {
        MovementCheck();
    }
    #endregion
    #region Movement
    private bool OnSlope() 
    {
        if(Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _groundLayerMask))
        {
            if(_slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public void Move()
    {
        if (_controller.isGrounded)
        {
            Vector2 movementInput = _movement.ReadValue<Vector2>();
            if (movementInput.x == 0 && movementInput.y == 0) 
            {
                _isMoving = false;
            }
            float speed = 0;
            if (!_controller.isKnocked)
            {
                switch (_controller.humanState)
                {
                    case HumanState.Default:
                        speed = _movementSpeed;
                        break;
                    case HumanState.isSprinting:
                        speed = _sprintSpeed;
                        break;
                    case HumanState.isCrouching:
                        speed = _crouchSpeed;
                        break;
                }
            }
            else
            {
                speed = _knockedSpeed;
            }
            Vector3 movement = new Vector3();
            var speedX = speed * movementInput.x;
            var speedY = speed * movementInput.y;
            movement = (speedX * _camera.transform.right) + (speedY * _camera.transform.forward);
            Vector3 movementNoSpeed = (movementInput.x * _camera.transform.right) + (movementInput.y * _camera.transform.forward);
            movementNoSpeed.Normalize();
            movementNoSpeed.y = 0;
            movement.y = _rb.velocity.y;
            MovementFilter(movementNoSpeed, movement);
        }
        else
        {
            BlockMovement();
        }
    }
    private void MovementFilter(Vector3 rayDirection, Vector3 movement)
    {
        if(!Physics.Raycast(_movementRayPoint.position,rayDirection, 0.5f, _movementFilterLayerMask))
        {
            if (OnSlope())
            {
                movement = Vector3.ProjectOnPlane(movement, _slopeHit.normal);
            }
            _rb.velocity = movement;
            if (rayDirection != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(rayDirection);
                rotation.x = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
            }
        }
        else if(Physics.Raycast(transform.position, rayDirection, 0.5f, _movementFilterLayerMask) && _isMoving)
        {
            ResetMovement();
        }
    }
    private void MovementCheck()
    {
        Vector2 movement = _movement.ReadValue<Vector2>();
        if (_controller.isGrounded)
        {
            _isMoving = (movement.x > 0 || movement.x < 0 || movement.y > 0 || movement.y < 0) ? true : false;
        }
        else
        {
            _isMoving = false;
        }
    }
    private void ResetMovement()
    {
        _movement.Reset();
        _isMoving = false;
        _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
    }
    private void BlockMovement()
    {
        _movement.Reset();
    }
    #endregion
    #region Events
    private void OnGotKnocked(object sender, System.EventArgs e)
    {
        _canSprint = false;
        _canCrouch = false;
    }
    private void Sprinting_performed(InputAction.CallbackContext obj)
    {
        if (_canSprint)
        {
            _controller.humanState = HumanState.isSprinting;
        }
        else
        {
            _controller.humanState = HumanState.Default;
        }
    }
    private void Sprinting_canceled(InputAction.CallbackContext obj)
    {
        _controller.humanState = HumanState.Default;
    }
    private void Crouching_performed(InputAction.CallbackContext obj)
    {
        if (_canCrouch)
        {
            _controller.humanState = HumanState.isCrouching;
        }
        else
        {
            _controller.humanState = HumanState.Default;
        }
    }
    private void Crouching_canceled(InputAction.CallbackContext obj)
    {
        _controller.humanState = HumanState.Default;
    }
    #endregion
    #region Properties
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
    }
    public bool IsSprinting
    {
        get
        {
            if (!IsMoving && _controller.humanState == HumanState.isSprinting)
            {
                return false;
            }
            else
            {
                return _controller.humanState == HumanState.isSprinting;
            }
        }
    }
    public bool IsCrouching 
    {
        get 
        {
            if(!IsMoving && _controller.humanState == HumanState.isCrouching)
            {
                return false;
            }
            else
            {
                return _controller.humanState == HumanState.isCrouching;
            }
        } 
    }
    #endregion
}

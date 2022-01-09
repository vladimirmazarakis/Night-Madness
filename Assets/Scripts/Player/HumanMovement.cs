using Assets.Scripts.Enums;
using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanMovement : MonoBehaviour
{
    #region Privates.
    [SerializeField] private float _movementSpeed = 2f;
    [SerializeField] private float _knockedSpeed = 0.75f;
    [SerializeField] private float _sprintSpeed = 5f;
    [SerializeField] private float _crouchSpeed = 1f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private LayerMask _movementFilterLayerMask;
    [SerializeField] private Camera _camera;
    private Rigidbody _rb;
    private bool _isGrounded = true;
    private bool _canMove = true;
    private bool _isSprinting = false;
    private bool _canSprint = true;
    private bool _isCrouching = false;
    private bool _canCrouch = true;
    private bool _isMoving = false;
    private InputMaster _inputMaster;
    private InputAction _movement;
    private HumanController _controller;
    #endregion
    #region Publics.
    public HumanState HumanState;
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
    private void Update()
    {
        if (_canMove)
        {
            Movement();
        }
    }
    #endregion
    #region Methods.
    private void Movement()
    {
        if (_isGrounded)
        {
            Vector2 movementInput = _movement.ReadValue<Vector2>();
            float speed = 0;
            if (!_controller.isKnocked)
            {
                switch (HumanState)
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
    }
    private void MovementFilter(Vector3 rayDirection, Vector3 movement)
    {
        RaycastHit hit;
        if(!Physics.Raycast(transform.position,rayDirection, 0.5f, _movementFilterLayerMask))
        {
            _rb.velocity = movement;
            _isMoving = (_rb.velocity.x > 0 || _rb.velocity.x < 0 || _rb.velocity.z > 0 || _rb.velocity.z < 0 && _isGrounded);
            if (rayDirection != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(rayDirection);
                rotation.x = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
            }
        }
        else if(Physics.Raycast(transform.position, rayDirection, 0.5f, _movementFilterLayerMask) && _isMoving)
        {
            _movement.Reset();
            _isMoving = false;
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
        }
    }
    #endregion
    #region Events.
    private void OnGotKnocked(object sender, System.EventArgs e)
    {
        _canSprint = false;
        _canCrouch = false;
    }
    private void Sprinting_performed(InputAction.CallbackContext obj)
    {
        if (_canSprint)
        {
            HumanState = HumanState.isSprinting;
        }
        else
        {
            HumanState = HumanState.Default;
        }
    }
    private void Sprinting_canceled(InputAction.CallbackContext obj)
    {
        HumanState = HumanState.Default;
    }
    private void Crouching_performed(InputAction.CallbackContext obj)
    {
        if (_canCrouch)
        {
            HumanState = HumanState.isCrouching;
        }
        else
        {
            HumanState = HumanState.Default;
        }
    }
    private void Crouching_canceled(InputAction.CallbackContext obj)
    {
        HumanState = HumanState.Default;
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
            if (!IsMoving && HumanState == HumanState.isSprinting)
            {
                return false;
            }
            else
            {
                return HumanState == HumanState.isSprinting;
            }
        }
    }
    public bool IsCrouching 
    {
        get 
        {
            if(!IsMoving && HumanState == HumanState.isCrouching)
            {
                return false;
            }
            else
            {
                return HumanState == HumanState.isCrouching;
            }
        } 
    }
    #endregion
}

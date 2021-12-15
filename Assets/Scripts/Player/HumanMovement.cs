using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 15f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private Camera _camera;
    private Rigidbody _rb;
    private bool _isGrounded = true;
    private bool _isSprinting = false;
    private bool _isMoving = false;
    private InputMaster _inputMaster;
    private InputAction _movement;

    #region Assignings.
    private void Awake()
    {
        _inputMaster = new InputMaster();
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        _inputMaster.Human.Sprinting.started += Sprinting_started;
        _inputMaster.Human.Sprinting.performed += Sprinting_performed;
        _inputMaster.Human.Sprinting.canceled += Sprinting_canceled;
        _movement = _inputMaster.Shared.Movement;
        _movement.Enable();
        _inputMaster.Human.Sprinting.Enable();
    }
    private void OnDisable()
    {
        _movement.Disable();
        _inputMaster.Human.Sprinting.Disable();
    }
    #endregion

    /// <summary>
    /// Calling movement every frame.
    /// </summary>
    private void Update()
    {
        Movement();
    }

    #region Movement.
    /// <summary>
    /// Moves the character relative to camera.
    /// </summary>
    private void Movement()
    {
        if (_isGrounded)
        {
            Vector2 movementInput = _movement.ReadValue<Vector2>();
            var speed = _isSprinting ? _sprintSpeed : _movementSpeed;
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
        if(!Physics.Raycast(transform.position,rayDirection, 0.5f))
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
        else if(Physics.Raycast(transform.position, rayDirection, 0.5f) && _isMoving)
        {
            _movement.Reset();
            _isMoving = false;
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
        }
    }
    private void Sprinting_started(InputAction.CallbackContext obj)
    {
        _isSprinting = true;
    }
    private void Sprinting_performed(InputAction.CallbackContext obj)
    {
        _isSprinting = true;
    }
    private void Sprinting_canceled(InputAction.CallbackContext obj)
    {
        _isSprinting = false;
    }
    #endregion

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
            if(!IsMoving && _isSprinting)
            {
                return false;
            }
            else
            {
                return _isSprinting;
            }
        }
    }
}

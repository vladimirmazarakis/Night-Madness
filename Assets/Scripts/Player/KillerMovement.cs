using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(KillerController))]
public class KillerMovement : MonoBehaviour, IMoveable
{
    [SerializeField] private Transform _movementRayPoint;
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _movementIgnoreLayerMask;

    private Rigidbody _rb;
    private bool _isMoving = false;
    private InputMaster _inputMaster;
    private InputAction _movement;
    private InputAction _look;
    private KillerController _controller;
    private RaycastHit _slopeHit;

    #region Assignings.
    private void Awake()
    {
        _inputMaster = new InputMaster();
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _controller = GetComponent<KillerController>();
    }
    private void OnEnable()
    {
        _movement = _inputMaster.Shared.Movement;
        _movement.Enable();
    }
    private void OnDisable()
    {
        _movement.Disable();
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Update()
    {
        MovementCheck();
    }
    #endregion
    #region Movement
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _groundLayerMask))
        {
            if (_slopeHit.normal != Vector3.up)
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
            movementInput.x = 0;
            if((_rb.velocity.x == 0 && _rb.velocity.z == 0) && (movementInput.x == 0 && movementInput.y == 0))
            {
                return;
            }
            var speed = _movementSpeed;
            Vector3 movement = new Vector3();
            var speedX = speed * movementInput.x;
            var speedY = speed * movementInput.y;
            movement = speedY * _camera.transform.forward;
            Vector3 movementNoSpeed = (movementInput.y * _camera.transform.forward);
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
        var hit = Physics.Raycast(_movementRayPoint.position, rayDirection, 0.5f, _movementIgnoreLayerMask);
        if(!hit)
        {
            if (OnSlope())
            {
                movement = Vector3.ProjectOnPlane(movement, _slopeHit.normal);
            }
            _rb.velocity = movement;
        }
        else if(hit && _isMoving)
        {
            _movement.Reset();
            _isMoving = false;
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
        }
    }
    private void MovementCheck()
    {
        Vector3 movement = _movement.ReadValue<Vector2>();
        _isMoving = (movement.y > 0 || movement.y < 0 && _controller.isGrounded);
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

    public bool IsMoving 
    { 
        get 
        {
            return _isMoving;
        } 
    }

    public float InputY
    {
        get
        {
            return _movement.ReadValue<Vector2>().y;
        }
    }
}

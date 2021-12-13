using UnityEngine;
using UnityEngine.InputSystem;

public class KillerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 5f;

    private Camera _camera;
    private Rigidbody _rb;
    private bool _isGrounded = true;
    private bool _isMoving = false;
    private InputMaster _inputMaster;
    private InputAction _movement;
    private InputAction _look;

    #region Assignings.
    private void Awake()
    {
        _inputMaster = new InputMaster();
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main;
    }
    private void OnEnable()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + 10, transform.localEulerAngles.z);
        _movement = _inputMaster.Shared.Movement;
        _movement.Enable();
    }
    private void OnDisable()
    {
        _movement.Disable();
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
    }
    private void MovementFilter(Vector3 rayDirection, Vector3 movement)
    {
        if(!Physics.Raycast(transform.position,rayDirection, 0.5f))
        {
            _rb.velocity = movement;
            _isMoving = (_rb.velocity.x > 0 || _rb.velocity.x < 0 || _rb.velocity.z > 0 || _rb.velocity.z < 0 && _isGrounded);
        }
        else if(Physics.Raycast(transform.position, rayDirection, 0.5f) && _isMoving)
        {
            _movement.Reset();
            _isMoving = false;
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
        }
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

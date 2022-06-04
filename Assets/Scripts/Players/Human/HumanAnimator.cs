using UnityEngine;
using Mirror;

[RequireComponent(typeof(HumanMovement))]
public class HumanAnimator : NetworkBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _animationTransitionTime = 3f;
    
    private HumanMovement _movement;

    private int _knockedLayerIndex;

    private int _crouchingLayerIndex;

    private void Start()
    {
        if(!isLocalPlayer)
        {
            this.enabled = false;
            return;
        }
        _movement = GetComponent<HumanMovement>();
        _knockedLayerIndex = _animator.GetLayerIndex("Knocked");
        _crouchingLayerIndex = _animator.GetLayerIndex("Crouch");
    }

    private void Update()
    {
        ApplyAnimations();
    }

    private void ApplyAnimations()
    {
        UpdateIsMoving();
        UpdateIsSprinting();
        UpdateIsCrouching();
    }

    private void UpdateIsSprinting()
    {
        if(_movement.IsSprinting)
        {
            _animator.SetBool("IsSprinting", true);
        }
        else
        {
            _animator.SetBool("IsSprinting", false);
        }
    }

    private void UpdateIsCrouching()
    {
        var crouchLayerWeight = _animator.GetLayerWeight(_crouchingLayerIndex);
        if(_movement.IsCrouching)
        {
            if(crouchLayerWeight != 1)
            {
                // lerp crouchLayerWeight to 1 over _animationTransitionTime seconds
                _animator.SetLayerWeight(_crouchingLayerIndex, Mathf.Lerp(crouchLayerWeight, 1f, _animationTransitionTime * Time.deltaTime));
            }
        }
        else
        {
            if(crouchLayerWeight != 0)
            {
                // lerp crouchLayerWeight to 0 over _animationTransitionTime seconds
                _animator.SetLayerWeight(_crouchingLayerIndex, Mathf.Lerp(crouchLayerWeight, 0f, _animationTransitionTime * Time.deltaTime));
            }
        }
    }

    private void UpdateIsMoving()
    {
        if(_movement.IsMoving)
        {
            _animator.SetBool("IsMoving", true);
        }
        else
        {
            _animator.SetBool("IsMoving", false);
        }
    }
}

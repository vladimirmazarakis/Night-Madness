using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HumanMovement))]
public class HumanAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private HumanMovement _humanMovement;
    private void Start()
    {
        _humanMovement = GetComponent<HumanMovement>();
    }
    private void Update()
    {
        UpdateAnimations();
    }
    private void UpdateAnimations()
    {
        if (_humanMovement.IsMoving)
        {
            _animator.SetBool("IsMoving", _humanMovement.IsMoving);
            if (_humanMovement.IsSprinting)
            {
                _animator.SetBool("IsSprinting", _humanMovement.IsSprinting);
            }
            else
            {
                _animator.SetBool("IsSprinting", _humanMovement.IsSprinting);
            }
        }
        else
        {
            _animator.SetBool("IsMoving", _humanMovement.IsMoving);
            _animator.SetBool("IsSprinting", _humanMovement.IsSprinting);
        }
    }
}

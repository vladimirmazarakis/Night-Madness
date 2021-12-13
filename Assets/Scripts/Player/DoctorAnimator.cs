using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KillerMovement), typeof(KillerController))]
public class DoctorAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private KillerMovement _killerMovement;
    private KillerController _controller;
    private void Start()
    {
        _killerMovement = GetComponent<KillerMovement>();
        _controller = GetComponent<KillerController>();
    }
    private void Update()
    {
        UpdateAnimations();
    }
    private void UpdateAnimations()
    {
        _animator.SetBool("IsMoving", _killerMovement.IsMoving);
        _animator.SetFloat("InputY", _killerMovement.InputY);
        _animator.SetBool("IsAttacking", _controller.IsAttacking);
    }
}

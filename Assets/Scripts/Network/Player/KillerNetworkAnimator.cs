using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KillerMovement))]
public class KillerNetworkAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private KillerMovement _killerMovement;
    private KillerNetworkController _controller;
    private void Start()
    {
        _killerMovement = GetComponent<KillerMovement>();
        _controller = GetComponent<KillerNetworkController>();
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

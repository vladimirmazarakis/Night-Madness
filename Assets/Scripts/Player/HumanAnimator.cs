using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HumanMovement), (typeof(HumanController)))]
public class HumanAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _animationLayerSmoothTime = 10f;
    private HumanMovement _humanMovement;
    private HumanController _controller;
    private void Start()
    {
        _humanMovement = GetComponent<HumanMovement>();
        _controller = GetComponent<HumanController>();
        _controller.gotKnocked += OnHumanGotKnocked;
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

    #region EventHandling
    private void OnHumanGotKnocked(object sender, EventArgs e)
    {
        Debug.Log("OnHumanGotKnocked event fired.");
        var knockedLayerIndex = _animator.GetLayerIndex("Knocked");
        var baseLayerIndex = _animator.GetLayerIndex("Base");
        _animator.SetLayerWeight(knockedLayerIndex, 1);
        _animator.SetLayerWeight(baseLayerIndex,0);
    }
    #endregion
}

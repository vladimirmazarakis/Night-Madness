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
    private bool _isKnocked = false;
    private float _knockedWeight = 0;
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
        if(_isKnocked && _knockedWeight != 1) 
        {
            var knockedLayerIndex = _animator.GetLayerIndex("Knocked");
            _knockedWeight = Mathf.Lerp(_knockedWeight, 1.0f, _animationLayerSmoothTime * Time.deltaTime);
            _animator.SetLayerWeight(knockedLayerIndex, _knockedWeight);
        }
    }

    #region EventHandling
    private void OnHumanGotKnocked(object sender, EventArgs e)
    {
        Debug.Log("OnHumanGotKnocked event fired.");
        _isKnocked = true;
    }
    #endregion
}

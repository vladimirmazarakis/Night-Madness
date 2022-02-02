using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using UnityEngine;

[RequireComponent(typeof(HumanMovement), (typeof(HumanController)))]
public class HumanAnimator : MonoBehaviour
{
    #region Privates.
    [SerializeField] private Animator _animator;
    [SerializeField] private float _animationLayerSmoothTime = 10f;
    private HumanMovement _humanMovement;
    private HumanController _controller;
    private float _knockedWeight = 0;
    private float _crouchWeight = 0;
    #endregion
    #region Assignings and other unity methods.
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
    #endregion
    #region Methods.
    private void UpdateAnimations()
    {
        if (_controller.isGrounded)
        {
            _animator.SetBool("IsMoving", _humanMovement.IsMoving);
            var crouchLayerIndex = _animator.GetLayerIndex("Crouch");
            switch (_controller.humanState)
            {
                case HumanState.isSprinting:
                    if (_humanMovement.IsMoving)
                    {
                        _animator.SetBool("IsSprinting", true);
                    }
                    else
                    {
                        _animator.SetBool("IsSprinting", false);
                    }
                    break;
                case HumanState.isCrouching:
                    crouchLayerIndex = _animator.GetLayerIndex("Crouch");
                    _crouchWeight = Mathf.Lerp(_crouchWeight, 1.0f, _animationLayerSmoothTime * Time.deltaTime);
                    _animator.SetLayerWeight(crouchLayerIndex, _crouchWeight);
                    break;
                case HumanState.Default:
                    crouchLayerIndex = _animator.GetLayerIndex("Crouch");
                    _crouchWeight = Mathf.Lerp(_crouchWeight, 0.0f, _animationLayerSmoothTime * Time.deltaTime);
                    _animator.SetLayerWeight(crouchLayerIndex, _crouchWeight);
                    _animator.SetBool("IsSprinting", false);
                    break;
            }
            if (_controller.isKnocked)
            {
                _animator.SetBool("IsSprinting", false);
            }
            if (_controller.isKnocked && _knockedWeight != 1)
            {
                var knockedLayerIndex = _animator.GetLayerIndex("Knocked");
                _knockedWeight = Mathf.Lerp(_knockedWeight, 1.0f, _animationLayerSmoothTime * Time.deltaTime);
                _animator.SetLayerWeight(knockedLayerIndex, _knockedWeight);
            }
        }
        else
        {
            //Falling animation
        }
    }
    #endregion
    #region Events.
    private void OnHumanGotKnocked(object sender, EventArgs e)
    {
        Debug.Log("OnHumanGotKnocked event fired.");
    }
    #endregion
}

using Assets.Scripts.Models;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _cinemachine;
    [SerializeField] private CapsuleColliderSettings _normalCapsuleColliderSettings;
    [SerializeField] private CapsuleColliderSettings _knockedCapsuleColliderSettings;
    [Header("Test Variables")]
    [SerializeField] private bool _isKnocked = false;
    [SerializeField] private int _health = 100;
    private AudioListener _audioListener;
    private HumanMovement _humanMovement;
    private HumanAnimator _humanAnimator;
    private CapsuleCollider _collider;
    private void Start()
    {
        _humanMovement = GetComponent<HumanMovement>();
        _humanAnimator = GetComponent<HumanAnimator>();
        _audioListener = _camera.GetComponent<AudioListener>();
        _collider = GetComponent<CapsuleCollider>();
    }
    private void Update()
    {
        HealthCheck();
    }
    private void HealthCheck()
    {
        if(_health <= 0 && !_isKnocked)
        {
            _isKnocked = true;
            gotKnocked.Invoke(this, EventArgs.Empty);
            _collider.center = _knockedCapsuleColliderSettings.Center;
            _collider.direction = (int)_knockedCapsuleColliderSettings.Direction;
            _collider.radius = _knockedCapsuleColliderSettings.Radius;
            _collider.height = _knockedCapsuleColliderSettings.Height;
        }
    }

    /// <summary>
    /// Subtracts from the health the given amount.
    /// </summary>
    /// <param name="DamageAmount">The amount to subtract from the health.</param>
    public void GiveDamage(int DamageAmount)
    {
        _health -= DamageAmount;
    }

    public void DisableAllSelfComponents() 
    {
        _cinemachine.SetActive(false);
        _humanMovement.enabled = false;
        _humanAnimator.enabled = false;
        _camera.enabled = false;
        _audioListener.enabled = false;
    }

    /// <summary>
    /// User health (Only-get).
    /// </summary>
    public int Health 
    { 
        get 
        {
            return _health; 
        } 
    }

    #region Events
    public event EventHandler gotKnocked;
    #endregion
}

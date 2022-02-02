using Assets.Scripts.Attributes;
using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    #region General
    [Header("General")]
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _cinemachine;
    private AudioListener _audioListener;
    private HumanMovement _humanMovement;
    private HumanAnimator _humanAnimator;
    #endregion
    #region Collider Settings
    [Header("Collider Settings")]
    [SerializeField] private CapsuleColliderSettings _normalCapsuleColliderSettings;
    [SerializeField] private CapsuleColliderSettings _knockedCapsuleColliderSettings;
    private CapsuleCollider _collider;
    #endregion
    #region Player Info
    [Header("Player Info")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField]/*[ReadOnly]*/ private int _health = 100;
    [HideInInspector] public HumanState humanState;
    [HideInInspector] public bool isKnocked = false;
    [HideInInspector] public bool isGrounded = false;
    #endregion
    #region Assignings and other unity methods.
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
        GroundCheck();
    }
    #endregion
    #region Checks
    private void GroundCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(_groundCheck.position, Vector3.down, out hit, 0.5f)) 
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    private void HealthCheck()
    {
        if((_health <= 0 && !isKnocked))
        {
            isKnocked = true;
            gotKnocked.Invoke(this, EventArgs.Empty);
            _collider.center = _knockedCapsuleColliderSettings.Center;
            _collider.direction = (int)_knockedCapsuleColliderSettings.Direction;
            _collider.radius = _knockedCapsuleColliderSettings.Radius;
            _collider.height = _knockedCapsuleColliderSettings.Height;
        }
    }
    #endregion
    #region Controller
    public void GiveDamage(int DamageAmount)
    {
        _health -= DamageAmount;
    }
    #endregion
    #region Properties.
    public int Health 
    { 
        get 
        {
            return _health; 
        } 
    }
    #endregion
    #region Events.
    public event EventHandler gotKnocked;
    #endregion
}

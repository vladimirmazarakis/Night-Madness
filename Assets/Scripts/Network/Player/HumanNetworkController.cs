using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanNetworkController : NetworkBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _cinemachine;
    private AudioListener _audioListener;
    private int _health = 100;
    private HumanMovement _humanMovement;
    private HumanAnimator _humanAnimator;
    private void Start()
    {
        _humanMovement = GetComponent<HumanMovement>();
        _humanAnimator = GetComponent<HumanAnimator>();
        _audioListener = _camera.GetComponent<AudioListener>();
        if (!hasAuthority)
        {
            _cinemachine.SetActive(false);
            _humanMovement.enabled = false;
            _humanAnimator.enabled = false;
            _camera.enabled = false;
            _audioListener.enabled = false;
        }
    }
    private void Update()
    {
        HealthCheck();
    }
    private void HealthCheck()
    {
        if(_health <= 0)
        {
            Destroy(transform.gameObject);
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
}

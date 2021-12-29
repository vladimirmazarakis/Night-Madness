using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMeshWrapper : MonoBehaviour
{ 
    #region Privates.
    private PlayerHumanAudio _playerHumanAudio;
    private Animator _animator;
    #endregion
    #region Assignings and other unity methods.
    private void Start()
    {
        _playerHumanAudio = GetComponentInParent<PlayerHumanAudio>();
        _animator = GetComponentInParent<Animator>();
    }
    #endregion
    #region Methods.
    public void PlayStepSound()
    {
        _playerHumanAudio.PlayStepSound();
    }
    public void StopGettingKnockedAnimation()
    {
        _animator.SetBool("IsGettingKnocked", false);
    }
    #endregion
}

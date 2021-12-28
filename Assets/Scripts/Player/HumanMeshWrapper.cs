using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMeshWrapper : MonoBehaviour
{
    private PlayerHumanAudio _playerHumanAudio;
    private Animator _animator;
    private void Start()
    {
        _playerHumanAudio = GetComponentInParent<PlayerHumanAudio>();
        _animator = GetComponentInParent<Animator>();
    }
    public void PlayStepSound()
    {
        _playerHumanAudio.PlayStepSound();
    }
    public void StopGettingKnockedAnimation()
    {
        _animator.SetBool("IsGettingKnocked", false);
    }
}

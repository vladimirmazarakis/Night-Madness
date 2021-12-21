using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMeshWrapper : MonoBehaviour
{
    private PlayerHumanAudio _playerHumanAudio;
    private void Start()
    {
        _playerHumanAudio = GetComponentInParent<PlayerHumanAudio>();
    }
    public void PlayStepSound()
    {
        _playerHumanAudio.PlayStepSound();
    }
}

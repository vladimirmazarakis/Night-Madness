using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerMeshWrapper : MonoBehaviour
{
    private KillerController _killerController;
    private PlayerKillerAudio _killerAudio;

    private void Start()
    {
        _killerController = GetComponentInParent<KillerController>();
        _killerAudio = GetComponentInParent<PlayerKillerAudio>();
    }

    public void Attack()
    {
        _killerController.Attack();
    }

    public void PlayStepSound() 
    { 
        _killerAudio.PlayStepSound();
    }
}

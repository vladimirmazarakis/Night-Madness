using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerHumanAudio : MonoBehaviour
{
    [Header("Foot steps settings")]
    [SerializeField] private List<AudioClip> _woodSteps;
    [Range(0,100)][SerializeField] private float _stepsDelay = 0.3f;
    private string _gameObjectTag = "Wood";
    private AudioSource _audioSource;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    public void PlayStepSound()
    {
        Debug.Log(_gameObjectTag);
        switch (_gameObjectTag)
        {
            case "Wood":
                System.Random random = new System.Random();
                var ran = random.Next(_woodSteps.Count);
                _audioSource.clip = _woodSteps[ran];
                _audioSource.Play();
                break;
        }
    }
    //private void OnCollisionStay(Collision collision)
    //{
    //    _gameObjectTag = collision.gameObject.tag;
    //}
}

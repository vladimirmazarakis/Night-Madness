using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(AudioSource),typeof(KillerMovement),typeof(KillerController))]
public class PlayerKillerAudio : MonoBehaviour
{
    #region Privates.
    #region StepSounds
    [SerializeField] private List<AudioClip> _woodSteps;
    [SerializeField] private List<AudioClip> _grassSteps;
    [SerializeField] private List<AudioClip> _gravelSteps;
    [SerializeField] private List<AudioClip> _dirtSteps;
    #endregion
    [Range(0,100)][SerializeField] private float _stepsSoundSmooth = 0.3f;
    [SerializeField] private Transform _feetPosition;
    [SerializeField] private float _feetCheckRayDistance = 0.5f;
    private string _gameObjectTag = null;
    private AudioSource _audioSource;
    private bool _playingSound = false;
    private KillerMovement _movement;
    private KillerController _controller;
    #endregion
    #region Assignings and other unity methods.
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _movement = GetComponent<KillerMovement>();
        _controller = GetComponent<KillerController>();
    }
    private void Update()
    {
        GetGameObjectBelowFeet();
        if (!_movement.IsMoving && _audioSource.volume > 0)
        {
            AudioHelper.FadeOut(_audioSource, _stepsSoundSmooth);
        }
        else if(_audioSource.isPlaying && _audioSource.volume < 1)
        {
            AudioHelper.FadeIn(_audioSource, _stepsSoundSmooth);
        }
    }
    #endregion
    #region Methods.
    public void PlayStepSound()
    {
        System.Random random = new System.Random();
        int randomIndex = 0;
        if (_movement.IsMoving)
        {
            _playingSound = true;
            switch (_gameObjectTag)
            {
                case "Wood":
                    randomIndex = random.Next(_woodSteps.Count);
                    _audioSource.clip = _woodSteps[randomIndex];
                    _audioSource.Play();
                    break;
                case "Grass":
                    randomIndex = random.Next(_grassSteps.Count);
                    _audioSource.clip = _grassSteps[randomIndex];
                    _audioSource.Play();
                    break;
                case "Gravel":
                    randomIndex = random.Next(_gravelSteps.Count);
                    _audioSource.clip = _gravelSteps[randomIndex];
                    _audioSource.Play();
                    break;
                case "Dirt":
                    randomIndex = random.Next(_dirtSteps.Count);
                    _audioSource.clip = _dirtSteps[randomIndex];
                    _audioSource.Play();
                    break;
            }
            _playingSound = false;
        }
    }
    private void GetGameObjectBelowFeet()
    {
        RaycastHit hit;
        if(Physics.Raycast(_feetPosition.position, Vector3.down, out hit, _feetCheckRayDistance))
        {
            _gameObjectTag = hit.transform.gameObject.tag;
        }
    }
    #endregion
}

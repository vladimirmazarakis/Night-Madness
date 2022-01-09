using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerHumanAudio : MonoBehaviour
{
    #region Privates.
    #region StepSounds
    [SerializeField] private List<AudioClip> _woodSteps;
    [SerializeField] private List<AudioClip> _woodRunningSteps;
    [SerializeField] private List<AudioClip> _woodKnockedSteps;
    [SerializeField] private List<AudioClip> _grassSteps;
    [SerializeField] private List<AudioClip> _grassRunningSteps;
    [SerializeField] private List<AudioClip> _grassKnockedSteps;
    [SerializeField] private List<AudioClip> _gravelSteps;
    [SerializeField] private List<AudioClip> _gravelRunningSteps;
    [SerializeField] private List<AudioClip> _gravelKnockedSteps;
    [SerializeField] private List<AudioClip> _dirtSteps;
    [SerializeField] private List<AudioClip> _dirtRunningSteps;
    [SerializeField] private List<AudioClip> _dirtKnockedSteps;
    #endregion
    [Range(0,100)][SerializeField] private float _stepsSoundSmooth = 0.3f;
    [SerializeField] private Transform _feetPosition;
    [SerializeField] private float _feetCheckRayDistance = 0.5f;
    private string _gameObjectTag = null;
    private AudioSource _audioSource;
    private bool _playingSound = false;
    private HumanMovement _movement;
    private HumanController _controller;
    private CapsuleCollider _bodyCollider;
    #endregion
    #region Assignings and other unity methods.
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _movement = GetComponent<HumanMovement>();
        _controller = GetComponent<HumanController>();
        _bodyCollider = GetComponent<CapsuleCollider>();
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
        if (_movement.IsMoving && _movement.HumanState != HumanState.isCrouching)
        {
            _playingSound = true;
            switch (_gameObjectTag)
            {
                case "Wood":
                    if (_controller.isKnocked)
                    {
                        randomIndex = random.Next(_woodKnockedSteps.Count);
                        _audioSource.clip = _woodKnockedSteps[randomIndex];
                    }
                    else
                    {
                        randomIndex = _movement.IsSprinting ? random.Next(_woodRunningSteps.Count) : random.Next(_woodSteps.Count);
                        _audioSource.clip = _movement.IsSprinting ? _woodRunningSteps[randomIndex] : _woodSteps[randomIndex];
                    }
                    _audioSource.Play();
                    break;
                case "Grass":
                    if (_controller.isKnocked)
                    {
                        randomIndex = random.Next(_grassKnockedSteps.Count);
                        _audioSource.clip = _grassKnockedSteps[randomIndex];
                    }
                    else
                    {
                        randomIndex = _movement.IsSprinting ? random.Next(_grassRunningSteps.Count) : random.Next(_grassSteps.Count);
                        _audioSource.clip = _movement.IsSprinting ? _grassRunningSteps[randomIndex] : _grassSteps[randomIndex];
                    }
                    _audioSource.Play();
                    break;
                case "Gravel":
                    if (_controller.isKnocked)
                    {
                        randomIndex = random.Next(_gravelKnockedSteps.Count);
                        _audioSource.clip = _gravelKnockedSteps[randomIndex];
                    }
                    else
                    {
                        randomIndex = _movement.IsSprinting ? random.Next(_gravelRunningSteps.Count) : random.Next(_gravelSteps.Count);
                        _audioSource.clip = _movement.IsSprinting ? _gravelRunningSteps[randomIndex] : _gravelSteps[randomIndex];
                    }
                    _audioSource.Play();
                    break;
                case "Dirt":
                    if (_controller.isKnocked)
                    {
                        randomIndex = random.Next(_dirtKnockedSteps.Count);
                        _audioSource.clip = _dirtKnockedSteps[randomIndex];
                    }
                    else
                    {
                        randomIndex = _movement.IsSprinting ? random.Next(_dirtRunningSteps.Count) : random.Next(_dirtSteps.Count);
                        _audioSource.clip = _movement.IsSprinting ? _dirtRunningSteps[randomIndex] : _dirtSteps[randomIndex];
                    }
                    _audioSource.Play();
                    break;
            }
            _playingSound = false;
        }
    }
    private void GetGameObjectBelowFeet()
    {
        Vector3 down = new Vector3(0,-0.5f,0);
        RaycastHit hit;
        if(Physics.Raycast(_feetPosition.position, down, out hit, _feetCheckRayDistance))
        {
            _gameObjectTag = hit.transform.gameObject.tag;
            Debug.Log(hit.transform.gameObject.name);
        }
    }
    #endregion
}

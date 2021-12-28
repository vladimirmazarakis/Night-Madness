using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerHumanAudio : MonoBehaviour
{
    #region StepSounds
    [Header("Wood")]
    [SerializeField] private List<AudioClip> _woodSteps;
    [SerializeField] private List<AudioClip> _woodRunningSteps;
    [SerializeField] private List<AudioClip> _woodKnockedSteps;
    [Header("Grass")]
    [SerializeField] private List<AudioClip> _grassSteps;
    [SerializeField] private List<AudioClip> _grassRunningSteps;
    [SerializeField] private List<AudioClip> _grassKnockedSteps;
    [Header("Gravel")]
    [SerializeField] private List<AudioClip> _gravelSteps;
    [SerializeField] private List<AudioClip> _gravelRunningSteps;
    [SerializeField] private List<AudioClip> _gravelKnockedSteps;
    [Header("Dirt")]
    [SerializeField] private List<AudioClip> _dirtSteps;
    [SerializeField] private List<AudioClip> _dirtRunningSteps;
    [SerializeField] private List<AudioClip> _dirtKnockedSteps;
    #endregion
    [Header("Other settings")]
    [Range(0,100)][SerializeField] private float _stepsSoundSmooth = 0.3f;
    private string _gameObjectTag = null;
    private AudioSource _audioSource;
    private bool _playingSound = false;
    private HumanMovement _movement;
    private HumanController _controller;
    private bool _isKnocked = false;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _movement = GetComponent<HumanMovement>();
        _controller = GetComponent<HumanController>();
        _controller.gotKnocked += OnHumanGotKnocked;
    }

    private void OnHumanGotKnocked(object sender, System.EventArgs e)
    {
        _isKnocked = true;
    }

    private void Update()
    {
        if (!_movement.IsMoving && _audioSource.volume > 0)
        {
            AudioHelper.FadeOut(_audioSource, _stepsSoundSmooth);
        }
        else if(_audioSource.isPlaying && _audioSource.volume < 1)
        {
            AudioHelper.FadeIn(_audioSource, _stepsSoundSmooth);
        }
    }

    public void PlayStepSound()
    {
        Debug.Log(_gameObjectTag);
        System.Random random = new System.Random();
        int randomIndex = 0;
        if (_movement.IsMoving)
        {
            _playingSound = true;
            switch (_gameObjectTag)
            {
                case "Wood":
                    if (_isKnocked)
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
                    if (_isKnocked)
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
                    if (_isKnocked)
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
                    if (_isKnocked)
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
    private void OnCollisionStay(Collision collision)
    {
        _gameObjectTag = collision.gameObject.tag;
        Debug.Log(_gameObjectTag);
    }
}

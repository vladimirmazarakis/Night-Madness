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
    [Header("Grass")]
    [SerializeField] private List<AudioClip> _grassSteps;
    [SerializeField] private List<AudioClip> _grassRunningSteps;
    [Header("Gravel")]
    [SerializeField] private List<AudioClip> _gravelSteps;
    [SerializeField] private List<AudioClip> _gravelRunningSteps;
    [Header("Dirt")]
    [SerializeField] private List<AudioClip> _dirtSteps;
    [SerializeField] private List<AudioClip> _dirtRunningSteps;
    #endregion
    [Header("Other settings")]
    [Range(0,100)][SerializeField] private float _stepsSoundSmooth = 0.3f;
    private string _gameObjectTag = null;
    private AudioSource _audioSource;
    private bool _playingSound = false;
    private HumanMovement _movement;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _movement = GetComponent<HumanMovement>();
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
                    randomIndex = _movement.IsSprinting ? random.Next(_woodRunningSteps.Count) : random.Next(_woodSteps.Count);
                    _audioSource.clip = _movement.IsSprinting ? _woodRunningSteps[randomIndex] : _woodSteps[randomIndex];
                    _audioSource.Play();
                    break;
                case "Grass":
                    randomIndex = _movement.IsSprinting ? random.Next(_grassRunningSteps.Count) : random.Next(_grassSteps.Count);
                    _audioSource.clip = _movement.IsSprinting ? _grassRunningSteps[randomIndex] : _grassSteps[randomIndex];
                    _audioSource.Play();
                    break;
                case "Gravel":
                    randomIndex = _movement.IsSprinting ? random.Next(_gravelRunningSteps.Count) : random.Next(_gravelSteps.Count);
                    _audioSource.clip = _movement.IsSprinting ? _gravelRunningSteps[randomIndex] : _gravelSteps[randomIndex];
                    _audioSource.Play();
                    break;
                case "Dirt":
                    randomIndex = _movement.IsSprinting ? random.Next(_dirtRunningSteps.Count) : random.Next(_dirtSteps.Count);
                    _audioSource.clip = _movement.IsSprinting ? _dirtRunningSteps[randomIndex] : _dirtSteps[randomIndex];
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]

public class Alarm : MonoBehaviour
{
    [SerializeField] private DoorTrigger _door;

    private Animator _animator;
    private AudioSource _audioSource;
    private bool _isPlayerInside = false;
    private float _volumeChangeStep = 0.002f;
    private float _minVolume = 0.3f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_isPlayerInside)
        {
            if (_audioSource.volume == 1 || _audioSource.volume <= _minVolume)
            {
                _volumeChangeStep *= -1;
            }

            _audioSource.volume -= _volumeChangeStep;
        }
    }

    private void OnEnable()
    {
        _door.PlayAlarm += OnPlayerInside;
        _door.ShutdownAlarm += OnPlayerOutside;
    }

    private void OnDisable()
    {
        _door.PlayAlarm -= OnPlayerInside;
        _door.ShutdownAlarm += OnPlayerOutside;
    }

    public void OnPlayerInside()
    {
        _isPlayerInside = true;
        _animator.SetTrigger("Alarm");
        _audioSource.Play();
    }

    public void OnPlayerOutside()
    {
        _isPlayerInside = false;
        _animator.Play("Idle");
        _audioSource.Stop();
    }
}

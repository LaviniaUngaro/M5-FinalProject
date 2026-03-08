using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _backgroundMusic;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _footstepSound;
    [SerializeField] private AudioClip _doorsOpening;
    [SerializeField] private AudioClip _alertSound;
    [SerializeField] private AudioClip _winSound;
    [SerializeField] private AudioClip _gameOverSound;

    protected static SoundManager _instance;

    public static SoundManager Instance => _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
    }

    public void StopBackgroundMusic()
    {
        _backgroundMusic.Stop();
    }

    public void OnWalk()
    {
        _audioSource.PlayOneShot(_footstepSound);
    }

    public void OnDoorsOpening()
    {
        _audioSource.PlayOneShot(_doorsOpening);
    }

    public void OnEnemiesAlert()
    {
        _audioSource.PlayOneShot(_alertSound);
    }

    public void OnWin()
    {
        _audioSource.PlayOneShot(_winSound);
    }

    public void OnGameOver()
    {
        _audioSource.PlayOneShot(_gameOverSound);
    }
}

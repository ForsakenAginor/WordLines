using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomMusicChoser : MonoBehaviour
{
    [SerializeField] private AudioClip[] _clips;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        ChoseRandomClip();
    }

    public void ChoseRandomClip()
    {
        if (_clips.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(_clips));

        int clipNumber = UnityEngine.Random.Range(0, _clips.Length);
        _audioSource.clip = _clips[clipNumber];
        _audioSource.loop = true;
        _audioSource.Play();
    }
}

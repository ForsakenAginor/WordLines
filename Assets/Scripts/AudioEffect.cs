using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioEffect : MonoBehaviour
{
    private AudioSource _audioSorce;

    private void Awake()
    {
        _audioSorce = GetComponent<AudioSource>();
    }

    public void Play()
    {
        _audioSorce.Play();
    }
}

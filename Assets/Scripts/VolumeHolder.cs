using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VolumeHolder : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _maxVolume;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _maxVolume = _audioSource.volume;
    }

    public void ChangeVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        _audioSource.volume = _maxVolume * volume;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class SoundInitializer : MonoBehaviour
{
    [Header("AudioSources")]
    [SerializeField] private AudioSource[] _allSources;
    [SerializeField] private AudioSource[] _effectsSources;
    [SerializeField] private AudioSource[] _musicSources;

    [Header("Sliders")]
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _effectsVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;

    private AudioData _audioData = new();

    private void Start()
    {
        _masterVolumeSlider.value = _audioData.MasterVolumeValue;
        _effectsVolumeSlider.value = _audioData.EffectVolumeValue;
        _musicVolumeSlider.value = _audioData.MusicVolumeValue;

        VolumeChanger masterChanger = new(_allSources, _audioData.MasterVolumeValue);
        VolumeChanger effectsChanger = new(_effectsSources, _audioData.EffectVolumeValue);
        VolumeChanger musicChanger = new(_musicSources, _audioData.MusicVolumeValue);
        VolumeChangeView masterChangerView = new(masterChanger, _masterVolumeSlider);
        VolumeChangeView effectsChangerView = new(effectsChanger, _effectsVolumeSlider);
        VolumeChangeView musicChangerView = new(musicChanger, _musicVolumeSlider);
    }

    public void SaveSettings()
    {
        _audioData.MasterVolumeValue = _masterVolumeSlider.value;
        _audioData.EffectVolumeValue = _effectsVolumeSlider.value;
        _audioData.MusicVolumeValue = _musicVolumeSlider.value;
    }
}

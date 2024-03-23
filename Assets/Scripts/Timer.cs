using TMPro;
using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(AudioSource))]
public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Color _attentionColor;
    [SerializeField] private float _roundTime = 240;
    [SerializeField] private  float _pulseStartTime = 30f;
    [SerializeField] private  float _changeColorTime = 15f;

    private float _remainingSeconds;
    private Tweener _tweener;
    private Color _commonColor;
    private AudioSource _audioSource;

    public event Action TimeEnded;

    private void OnValidate()
    {
        float minimumRoundTime = 1f;
        _roundTime = _roundTime <= 0 ? minimumRoundTime : _roundTime;
        _pulseStartTime = _pulseStartTime < _roundTime ? _pulseStartTime : _roundTime;
        _changeColorTime = _changeColorTime < _roundTime ? _changeColorTime : _roundTime;
    }

    private void Awake()
    {
        _remainingSeconds = _roundTime;
        _commonColor = _text.color;
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _remainingSeconds -= Time.deltaTime;
        _text.text = _remainingSeconds.ToString("0");

        if( _remainingSeconds < _pulseStartTime && _tweener == null)
        {
            float pulseFrequency = 0.5f;
            float pulseAmplitude = 1.3f;
            int infinityLoop = -1;
            _tweener = transform.DOScale(pulseAmplitude, pulseFrequency).SetLoops(infinityLoop, LoopType.Yoyo).SetEase(Ease.Linear);
            _audioSource.Play();
        }

        if(_remainingSeconds < _changeColorTime && _text.color != _attentionColor)        
            _text.color = _attentionColor;

        if (_remainingSeconds <= 0)
        {
            _audioSource.Pause();
            TimeEnded?.Invoke();
        }
    }

    public void Restart()
    {
        _tweener.Kill();
        _tweener = null;
        _text.color = _commonColor;
        _audioSource.Pause();
        _remainingSeconds = _roundTime;
    }
}

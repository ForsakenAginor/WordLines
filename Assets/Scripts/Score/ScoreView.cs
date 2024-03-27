using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _scoreDisplays;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private float _scoreMaxGradient = 1000f;

    private Score _score;

    private void OnValidate()
    {
        _scoreMaxGradient = _scoreMaxGradient < 0 ? 0 : _scoreMaxGradient;
    }

    public void Init(Score score)
    {
        _score = score ?? throw new ArgumentNullException(nameof(score));
        _score.ValueChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        _score.ValueChanged -= OnValueChanged;
    }

    private void OnValueChanged()
    {
        for (int i = 0; i < _scoreDisplays.Length; i++)        
            _scoreDisplays[i].text = _score.Value.ToString();
        
        StartPulseAnimation();
        ChangeColor();
    }

    private void StartPulseAnimation()
    {
        float scale = 1.5f;
        float animationTime = 0.5f;
        int animationLoops = 2;
        transform.DOScale(scale, animationTime).SetLoops(animationLoops, LoopType.Yoyo);
    }

    private void ChangeColor()
    {
        for (int i = 0; i < _scoreDisplays.Length; i++)
            _scoreDisplays[i].color = _gradient.Evaluate((float)_score.Value / _scoreMaxGradient);
    }
}

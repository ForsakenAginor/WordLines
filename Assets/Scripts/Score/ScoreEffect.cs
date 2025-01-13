using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreEffect : MonoBehaviour
{
    private const string RusCombo = "КОМБО";
    private const string EngCombo = "Combo";

    [SerializeField] private TMP_Text _score;

    private readonly Vector3 _standardScale = Vector3.one;
    private readonly Vector3 _targetScale = Vector3.one * 3f;
    private readonly float _duration = 4f;
    private string _combo;
    private Canvas _canvas;
    private Transform _parent;

    private void Awake()
    {
        _parent = transform.parent;
        _canvas = GetComponentInParent<Canvas>();
    }

    public float Duration => _duration;

    private void OnEnable()
    {
        transform.DOScale(_targetScale, _duration).SetEase(Ease.Linear);
        _score.DOFade(0f, _duration).SetEase(Ease.Linear);
    }

    private void OnDisable()
    {
        transform.localScale = _standardScale;
        float notTransparentAlpha = 1f;
        _score.alpha = notTransparentAlpha;
        transform.SetParent(_parent);
    }

    public void Init(int score, Vector3 position, int combo, LocalizationLanguages language)
    {
        _combo = language == LocalizationLanguages.English ? EngCombo : RusCombo;
        CreateEffectMessage(score, combo);
        _score.transform.localPosition = position;
        transform.SetParent(_canvas.transform);
    }

    private void CreateEffectMessage(int score, int combo)
    {
        const int NonComboStatus = 1;
        const int MaxComboStatus = 5;

        Dictionary<int, Color> comboColorPairs = new()
        {
            { 2, Color.yellow },
            { 3, Color.blue },
            { 4, Color.green },
            { 5, Color.red },
        };

        if(combo == NonComboStatus)
        {
            _score.color = Color.white;
            _score.text = $"+{score}";
        }
        else if(comboColorPairs.ContainsKey(combo))
        {
            _score.color = comboColorPairs[combo];
            _score.text = $"+{score}\nx{combo} {_combo}";
        }
        else
        {
            _score.color = comboColorPairs[MaxComboStatus];
            _score.text = $"+{score}\nx{combo} {_combo}";
        }
    }
}

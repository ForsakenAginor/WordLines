using System;
using TMPro;
using UnityEngine;

public class ScoreRecordView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bestScoreField;
    [SerializeField] private TextMeshProUGUI _bestOfDayScoreField;

    private ScoreRecordsManager _scoreRecord;

    public void Init(ScoreRecordsManager scoreRecord)
    {
        _scoreRecord = scoreRecord ?? throw new ArgumentNullException(nameof(scoreRecord));
        _bestScoreField.text = _scoreRecord.BestScore.ToString();
        _bestOfDayScoreField.text = _scoreRecord.TodayBestScore.ToString();
        _scoreRecord.RecordRefreshed += OnRecordRefreshed;
        _scoreRecord.TodayRecordRefreshed += OnTodayRecordRefreshed;
    }

    private void OnDisable()
    {
        _scoreRecord.RecordRefreshed -= OnRecordRefreshed;
    }

    private void OnTodayRecordRefreshed(int newRecord)
    {
        _bestOfDayScoreField.text = newRecord.ToString();
    }

    private void OnRecordRefreshed(int newRecord)
    {
        _bestScoreField.text = newRecord.ToString();
    }
}

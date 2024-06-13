using System;
using UnityEngine;

public class ScoreRecordsManager
{
    private const string BestScoreKey = nameof(BestScoreKey);
    private const string BestTodayScoreKey = nameof(BestTodayScoreKey);
    private const string TodayKey = nameof(TodayKey);

    private RecordsData _record;

    public ScoreRecordsManager()
    {
        _record = CreateRecord();
    }

    public event Action<int> RecordRefreshed;
    public event Action<int> TodayRecordRefreshed;

    public int BestScore => _record.BestScore;
    public int TodayBestScore => _record.TodayBestScore;

    public void SetBestScores(int score)
    {
        _record = CreateRecord(score);
        SaveRecord();
    }

    private RecordsData CreateRecord(int score = 0)
    {
        int bestScore;
        int todayBestScore;
        int savedScore;
        string today = DateTime.Today.ToShortDateString();

        if (PlayerPrefs.HasKey(BestScoreKey))
        {
            savedScore = PlayerPrefs.GetInt(BestScoreKey);

            if (savedScore < score)
            {
                bestScore = score;
                RecordRefreshed?.Invoke(score);
            }
            else
            {
                bestScore = savedScore;
            }
        }
        else
        {
            RecordRefreshed?.Invoke(score);
            bestScore = score;
        }

        if (PlayerPrefs.HasKey(TodayKey) && PlayerPrefs.GetString(TodayKey) == today)
        {
            savedScore = PlayerPrefs.GetInt(BestTodayScoreKey);

            if (savedScore < score)
            {
                todayBestScore = score;
                TodayRecordRefreshed?.Invoke(score);
            }
            else
            {
                todayBestScore = savedScore;
            }
        }
        else
        {
            TodayRecordRefreshed?.Invoke(score);
            todayBestScore = score;
        }

        return new(bestScore, todayBestScore, today);
    }

    private void SaveRecord()
    {
        PlayerPrefs.SetString(TodayKey, _record.Today);
        PlayerPrefs.SetInt(BestScoreKey, _record.BestScore);
        PlayerPrefs.SetInt(BestTodayScoreKey, _record.TodayBestScore);
    }

    private class RecordsData
    {
        private readonly int _bestScore;
        private readonly int _todayBestScore;
        private readonly string _today;

        public RecordsData(int bestScore, int todayBestScore, string today)
        {
            if (_bestScore < 0)
                throw new ArgumentOutOfRangeException(nameof(bestScore));

            if (_todayBestScore < 0)
                throw new ArgumentOutOfRangeException($"{nameof(todayBestScore)}");

            if (today == null)
                throw new ArgumentNullException(nameof(today));

            _bestScore = bestScore;
            _todayBestScore = todayBestScore;
            _today = today;
        }

        public int BestScore => _bestScore;
        public int TodayBestScore => _todayBestScore;
        public string Today => _today;
    }
}

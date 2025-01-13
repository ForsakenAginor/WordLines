using System;
using UnityEngine;

public class ScoreRecordsManager
{
    private const string BestEnglishScoreKey = nameof(BestEnglishScoreKey);
    private const string BestEnglishTodayScoreKey = nameof(BestEnglishTodayScoreKey);
    private const string EnglishTodayKey = nameof(EnglishTodayKey);

    private const string BestRussianScoreKey = nameof(BestRussianScoreKey);
    private const string BestRussianTodayScoreKey = nameof(BestRussianTodayScoreKey);
    private const string RussianTodayKey = nameof(RussianTodayKey);

    private readonly string _bestScoreKey;
    private readonly string _bestTodayScoreKey;
    private readonly string _todayKey;

    private RecordsData _record;

    public ScoreRecordsManager(LocalizationLanguages languge)
    {
        if(languge == LocalizationLanguages.English)
        {
            _bestScoreKey = BestEnglishScoreKey;
            _bestTodayScoreKey = BestEnglishTodayScoreKey;
            _todayKey = EnglishTodayKey;
        }
        else if(languge == LocalizationLanguages.Russian)
        {
            _bestScoreKey = BestRussianScoreKey;
            _bestTodayScoreKey = BestRussianTodayScoreKey;
            _todayKey = RussianTodayKey;
        }

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

    private RecordsData CreateRecord(int score = 0 )
    {
        int bestScore;
        int todayBestScore;
        int savedScore;
        string today = DateTime.Today.ToShortDateString();

        if (PlayerPrefs.HasKey(_bestScoreKey))
        {
            savedScore = PlayerPrefs.GetInt(_bestScoreKey);

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

        if (PlayerPrefs.HasKey(_todayKey) && PlayerPrefs.GetString(_todayKey) == today)
        {
            savedScore = PlayerPrefs.GetInt(_bestTodayScoreKey);

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
        PlayerPrefs.SetString(_todayKey, _record.Today);
        PlayerPrefs.SetInt(_bestScoreKey, _record.BestScore);
        PlayerPrefs.SetInt(_bestTodayScoreKey, _record.TodayBestScore);
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

public enum LocalizationLanguages
{
    Russian,
    English,
}

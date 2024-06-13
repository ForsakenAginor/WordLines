using System;
using System.IO;
using UnityEngine;

public class ScoreRecordsManager
{
    private const string RecordsFileName = "Records.json";

    private readonly RecordsData _record;
    private readonly string _path = Path.Combine(Application.streamingAssetsPath, RecordsFileName);
    private readonly Yandex _yandex;

    public ScoreRecordsManager()
    {
        if (File.Exists(_path))
        {
            _record = JsonUtility.FromJson<RecordsData>(File.ReadAllText(_path));

            if(_record.Today != DateTime.Today.ToBinary())
            {
                _record.Today = DateTime.Today.ToBinary();
                _record.TodayScore = 0;
            }

            SaveRecord();
        }
        else
        {
            _record = new()
            {
                Today = DateTime.Today.ToBinary()
            };
        }
    }

    public ScoreRecordsManager(Yandex yandex)
    {
        _yandex = yandex != null ? yandex : throw new ArgumentNullException(nameof(yandex));/*
        _record = _yandex.Load();

        if(_record == null)
            _record = new RecordsData();

        if (_record.Today != DateTime.Today.ToBinary())
        {
            _record.Today = DateTime.Today.ToBinary();
            _record.TodayScore = 0;
        }

        SaveRecord();*/
        _record = new();
    }

    public event Action<int> RecordRefreshed;
    public event Action<int> TodayRecordRefreshed;

    public int BestScore => _record.Score;
    public int TodayBestScore => _record.TodayScore;

    public void SetBestScores(int score)
    {
        if (score > _record.Score)
        {
            _record.Score = score;
            RecordRefreshed?.Invoke(score);
            SaveRecord();
        }

        if(score > _record.TodayScore)
        {
            _record.TodayScore = score;
            TodayRecordRefreshed?.Invoke(score);
            SaveRecord();
        }
    }

    private void SaveRecord()
    {/*
#if UNITY_WEBGL
        _yandex.Save(_record);
#else
        File.WriteAllText(_path, JsonUtility.ToJson(_record));
#endif*/
    }
}

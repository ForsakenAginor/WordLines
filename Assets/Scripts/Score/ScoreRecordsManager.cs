using System;
using System.IO;
using UnityEngine;

public class ScoreRecordsManager
{
    private const string RecordsFileName = "Records.json";

    private Record _record;
    private string _path = Path.Combine(Application.streamingAssetsPath, RecordsFileName);

    public ScoreRecordsManager()
    {
        if (File.Exists(_path))
        {
            _record = JsonUtility.FromJson<Record>(File.ReadAllText(_path));

            if(_record.Today != DateTime.Today.ToBinary())
            {
                _record.Today = DateTime.Today.ToBinary();
                _record.TodayScore = 0;
            }

            SaveRecord();
        }
        else
        {
            _record = new Record();
            _record.Today = DateTime.Today.ToBinary();
        }
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
    {
        File.WriteAllText(_path, JsonUtility.ToJson(_record));
    }

    [Serializable]
    private class Record
    {
        public int Score;
        public int TodayScore;
        public long Today;
    }
}

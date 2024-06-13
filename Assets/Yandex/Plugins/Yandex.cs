using System.Runtime.InteropServices;
using UnityEngine;

public class Yandex : MonoBehaviour
{/*
    private RecordsData _recordsData;

    [DllImport("__Internal")]
    private static extern void SaveExtern(string jsonData);

    [DllImport("__Internal")]
    private static extern void LoadExtern();

    private void Awake()
    {
        _recordsData = Load();
    }

    public void Save(RecordsData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        SaveExtern(jsonData);
    }

    public RecordsData Load()
    {
        LoadExtern();
        return _recordsData;
    }

    public void SetRecordsData(string jsonData)
    {
        _recordsData = JsonUtility.FromJson<RecordsData>(jsonData);
        Debug.Log($"{_recordsData.Score}");
        Debug.Log($"{_recordsData.TodayScore}");
        Debug.Log($"{_recordsData.Today}");
    }*/
}

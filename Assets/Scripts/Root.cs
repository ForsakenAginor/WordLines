using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Root : MonoBehaviour
{
    [Header("NounsList")]
    [SerializeField] private Transform _nounRecordHolder;
    [SerializeField] private NounRecordView _nounViewPrefab;

    [Header("")]
    [SerializeField] private Timer _timer;

    [Header("Score")]
    [SerializeField] private ScoreEffect _scoreEffectPrefab;
    [SerializeField] private ScoreView _scoreView;
    [SerializeField] private ScoreRecordView _scoreRecordView;

    [Header("Board")]
    [SerializeField] private GameObject _boardHolder;
    [SerializeField] private CellMover _cellPrefab;
    [SerializeField] private GraphicRaycaster _raycaster;

    [Header("")]
    [SerializeField] private Canvas _endGameScreen;

    [Header("")]
    [SerializeField] private AudioSource _audioSource;

    [Header("YandexSDK")]
    [SerializeField] private Yandex _yandex;

    private string _rawNounsInfo;
    private BoardManager _board;
    private NounDictionary _nounDictionary;
    private Letters _letters;
    private Score _score;
    private ScoreEffectPool _effectCreator;
    private ScoreRecordsManager _recordsManager;
    private NounsList _records;

    public void Init(string dictionary)
    {
        Time.timeScale = 0;
        _letters = new Letters();

        _rawNounsInfo = dictionary;
        _nounDictionary = new NounDictionary(_rawNounsInfo);
        _board = _boardHolder.AddComponent(typeof(BoardManager)) as BoardManager;
        _board.Init(_letters, _nounDictionary, _cellPrefab);
        InputBlocker _ = new(_board, _raycaster);
        _score = new();
        _scoreView.Init(_score);

#if UNITY_WEBGL
        _recordsManager = new ScoreRecordsManager(_yandex);
#else
        _recordsManager = new ScoreRecordsManager();        
#endif
        _scoreRecordView.Init(_recordsManager);
        _effectCreator = _boardHolder.AddComponent<ScoreEffectPool>();
        _effectCreator.Init(_scoreEffectPrefab);
        _records = transform.AddComponent<NounsList>();
        _records.Init(_nounRecordHolder, _nounViewPrefab);

        _board.WordFound += OnWordFound;
        _timer.TimeEnded += OnTimeEnded;
    }

    private void OnDisable()
    {
        _board.WordFound -= OnWordFound;
        _timer.TimeEnded -= OnTimeEnded;
    }

    public void Launch()
    {
        float commonTimeScale = 1f;
        Time.timeScale = commonTimeScale;
    }

    public void RestartGame()
    {
        _score.Restart();
        _records.Restart();
        _timer.Restart();
        _nounDictionary = new NounDictionary(_rawNounsInfo);
        _board.ResetBoard(_letters, _nounDictionary);
        _endGameScreen.gameObject.SetActive(false);
        float commonTimeScale = 1f;
        Time.timeScale = commonTimeScale;
    }

    public void Close()
    {
        Application.Quit();
    }

    private void OnTimeEnded()
    {
        Time.timeScale = 0;
        _recordsManager.SetBestScores(_score.Value);
        _endGameScreen.gameObject.SetActive(true);
    }

    private void OnWordFound(string word, int combo, Vector3 position)
    {
        if (_nounDictionary.Nouns.ContainsKey(word) == false)
            throw new ArgumentOutOfRangeException(nameof(word));

        _audioSource.Play();
        _records.AddRecord(word, _nounDictionary.Nouns[word]);
        _nounDictionary.RemoveWord(word);
        int addingScore = Score.CalcScore(word, combo);
        _score.AddScore(addingScore);
        _effectCreator.SpawnEffect(position, addingScore, combo);
    }
}

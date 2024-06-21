using Agava.YandexGames;
using System;
using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Root : MonoBehaviour
{
    private const string FileName = "nouns.txt";

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
    [SerializeField] private GameObject _tutorial;
    [SerializeField] private LeaderboardOpener _leaderboardOpener;

    [Header("")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private RandomMusicChoser _musicChoser;

    [Header("")]
    [SerializeField] private Silencer _silencer;

    private readonly string _path = Path.Combine(Application.streamingAssetsPath, FileName);
    private string _rawNounsInfo;
    private BoardManager _board;
    private NounDictionary _nounDictionary;
    private Letters _letters;
    private Score _score;
    private ScoreEffectPool _effectCreator;
    private ScoreRecordsManager _recordsManager;
    private NounsList _records;
    private AdvertiseShower _advertiseShower;

    private IEnumerator Start()
    {
        string result = string.Empty;
        using UnityWebRequest loadingRequest = UnityWebRequest.Get(_path);
        yield return loadingRequest.SendWebRequest();

        if (loadingRequest.result == UnityWebRequest.Result.ConnectionError || loadingRequest.result == UnityWebRequest.Result.ProtocolError)
            Debug.LogErrorFormat(this, "Unable to load text due to {0} - {1}", loadingRequest.responseCode, loadingRequest.error);
        else
            result = System.Text.Encoding.UTF8.GetString(loadingRequest.downloadHandler.data);

#if UNITY_WEBGL && !UNITY_EDITOR
        StickyAd.Show();
#endif
        Init(result);
        ShowTutorial();
    }

    private void Init(string dictionary)
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

        _recordsManager = new ScoreRecordsManager();
        _leaderboardOpener.Init(_recordsManager);

        _scoreRecordView.Init(_recordsManager);
        _effectCreator = _boardHolder.AddComponent<ScoreEffectPool>();
        _effectCreator.Init(_scoreEffectPrefab);
        _records = transform.AddComponent<NounsList>();
        _records.Init(_nounRecordHolder, _nounViewPrefab);

        _advertiseShower = new(_silencer);

        _board.WordFound += OnWordFound;
        _timer.TimeEnded += OnTimeEnded;
    }

    private void ShowTutorial()
    {
        TutorialData tutorialData = new();

        if (tutorialData.IsTutorialCompleted == false)
        {
            Time.timeScale = 1f;
            _timer.gameObject.SetActive(false);
            _tutorial.SetActive(true);
        }
    }

    private void OnDisable()
    {
        _board.WordFound -= OnWordFound;
        _timer.TimeEnded -= OnTimeEnded;
    }

    public void StartGame()
    {
        _timer.gameObject.SetActive(true);
        float commonTimeScale = 1f;
        Time.timeScale = commonTimeScale;
    }

    public void RestartGame()
    {
        _score.Restart();
        _records.Restart();
        _timer.gameObject.SetActive(true);
        _timer.Restart();
        _nounDictionary = new NounDictionary(_rawNounsInfo);
        _board.ResetBoard(_letters, _nounDictionary);
        _endGameScreen.gameObject.SetActive(false);
        _musicChoser.ChoseRandomClip();
#if UNITY_WEBGL && !UNITY_EDITOR
        _advertiseShower.ShowAdvertise();
#else
        Time.timeScale = 1f;
#endif
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

using System;
using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Lean.Localization;
using System.Linq;

public class Root : MonoBehaviour
{
    private const string FileRusName = "nouns.txt";
    private const string FileEngName = "engNouns.txt";

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

    [Header("UI")]
    [SerializeField] private Canvas _endGameScreen;
    [SerializeField] private GameObject _tutorial;
    [SerializeField] private GameObject _buttonsPanel;
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _loadingPanel;

    [Header("")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private RandomMusicChoser _musicChoser;


    private readonly string _rusFilePath = Path.Combine(Application.streamingAssetsPath, FileRusName);
    private readonly string _engFilePath = Path.Combine(Application.streamingAssetsPath, FileEngName);
    private string _rawNounsInfo;
    private BoardManager _board;
    private NounDictionary _nounDictionary;
    private ILetters _letters;
    private Score _score;
    private ScoreEffectPool _effectCreator;
    private ScoreRecordsManager _recordsManager;
    private NounsList _records;
    private string _rusWords;
    private string _engWords;

    private IEnumerator Start()
    {
        _loadingPanel.SetActive(true);
        LeanLocalization.UpdateTranslations();

        using UnityWebRequest loadingRequest = UnityWebRequest.Get(_rusFilePath);
        yield return loadingRequest.SendWebRequest();

        if (loadingRequest.result == UnityWebRequest.Result.ConnectionError || loadingRequest.result == UnityWebRequest.Result.ProtocolError)
            Debug.LogErrorFormat(this, "Unable to load text due to {0} - {1}", loadingRequest.responseCode, loadingRequest.error);
        else
            _rusWords = System.Text.Encoding.UTF8.GetString(loadingRequest.downloadHandler.data);

        using UnityWebRequest secondLoadingRequest = UnityWebRequest.Get(_engFilePath);
        yield return secondLoadingRequest.SendWebRequest();

        if (secondLoadingRequest.result == UnityWebRequest.Result.ConnectionError || secondLoadingRequest.result == UnityWebRequest.Result.ProtocolError)
            Debug.LogErrorFormat(this, "Unable to load text due to {0} - {1}", secondLoadingRequest.responseCode, secondLoadingRequest.error);
        else
            _engWords = System.Text.Encoding.UTF8.GetString(secondLoadingRequest.downloadHandler.data);
        /*
#if UNITY_WEBGL && !UNITY_EDITOR
        StickyAd.Show();
#endif
        */
        Init();
        ShowTutorial();
    }

    private void Init()
    {
        Time.timeScale = 0;

        string languageString = LeanLocalization.Instances.First().CurrentLanguage;
        LocalizationLanguages language;

        if (languageString == "English")
        {
            _letters = new EngLetters();
            _rawNounsInfo = _engWords;
            language = LocalizationLanguages.English;
        }
        else if (languageString == "Russian")
        {
            _letters = new RusLetters();
            _rawNounsInfo = _rusWords;
            language = LocalizationLanguages.Russian;
        }
        else
        {
            throw new Exception("Can't find current localization language");
        }

        _nounDictionary = new NounDictionary(_rawNounsInfo);

        _board = _boardHolder.AddComponent(typeof(BoardManager)) as BoardManager;
        _board.Init(_letters, _nounDictionary, _cellPrefab);
        InputBlocker _ = new(_board, _raycaster);
        _score = new();
        _scoreView.Init(_score);

        _recordsManager = new ScoreRecordsManager(language);

        _scoreRecordView.Init(_recordsManager);
        _effectCreator = _boardHolder.AddComponent<ScoreEffectPool>();
        _effectCreator.Init(_scoreEffectPrefab, language);
        _records = transform.AddComponent<NounsList>();
        _records.Init(_nounRecordHolder, _nounViewPrefab);

        _loadingPanel.SetActive(false);

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
#if UNITY_WEBGL && !UNITY_EDITOR
        _loadingPanel.SetActive(true);
        ResetProgress();
        Time.timeScale = 1f;
        /*
        _advertiseShower.ShowAdvertise(ResetProgress);
        */
#elif !UNITY_EDITOR && UNITY_STANDALONE
        _loadingPanel.SetActive(true);
        ResetProgress();
        Time.timeScale = 1f;
#endif

#if UNITY_EDITOR
        ResetProgress();
        Time.timeScale = 1f;
#endif
    }

    private void ResetProgress()
    {
        _buttonsPanel.SetActive(false);
        _score.Restart();
        _records.Restart();
        _timer.gameObject.SetActive(true);
        _timer.Restart();
        _timer.gameObject.SetActive(false);

        _nounDictionary = new NounDictionary(_rawNounsInfo);
        _board.ResetBoard(_letters, _nounDictionary);

        _endGameScreen.gameObject.SetActive(false);
        _musicChoser.ChoseRandomClip();
        _startPanel.SetActive(true);
        _loadingPanel.SetActive(false);
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
        int addingScore = Score.CalcScore(word, combo, _letters);
        _score.AddScore(addingScore);
        _effectCreator.SpawnEffect(position, addingScore, combo);
    }
}

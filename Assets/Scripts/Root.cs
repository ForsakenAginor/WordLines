using System;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private NounsList _records;
    [SerializeField] private Score _score;
    [SerializeField] private Timer _timer;
    [SerializeField] private GameObject _boardHolder;
    [SerializeField] private CellMover _cellPrefab;
    [SerializeField] private InputBlocker _inputBlocker;
    [SerializeField] private Canvas _endGameScreen;

    private BoardInitializer _board;
    private NounDictionary _nounDictionary;
    private Letters _letters;

    private void Awake()
    {
        Time.timeScale = 0;
        _letters = new Letters();
        _nounDictionary = new NounDictionary();
        _board = _boardHolder.AddComponent(typeof(BoardInitializer)) as BoardInitializer;
        _board.Init(_letters, _nounDictionary, _cellPrefab);
        _inputBlocker.Init(_board);
    }

    private void OnEnable()
    {
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
        _nounDictionary = new NounDictionary();
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
        _endGameScreen.gameObject.SetActive(true);
    }

    private void OnWordFound(string word, int combo)
    {
        if(_nounDictionary.Nouns.ContainsKey(word) == false)
            throw new ArgumentOutOfRangeException(nameof(word));

        _records.AddRecord(word, _nounDictionary.Nouns[word]);
        _nounDictionary.RemoveWord(word);
        _score.SetScore(word, combo);
    }
}

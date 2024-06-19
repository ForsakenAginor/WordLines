using Agava.YandexGames;
using LeaderboardSystem;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardOpener : MonoBehaviour
{
    [SerializeField] private YandexLeaderboard _leaderboard;
    [SerializeField] private GameObject _leaderboardPanel;
    [SerializeField] private Button _openLeaderboardButton;
    [SerializeField] private GameObject _holderPanel;
    [SerializeField] private GameObject _autorizationPanel;

    private ScoreRecordsManager _playerScore;

    private void Start()
    {
        _playerScore = new ();
        _openLeaderboardButton.onClick.AddListener(TryOpenLeaderboard);
    }

    private void OnDestroy()
    {
        _openLeaderboardButton.onClick.RemoveListener(TryOpenLeaderboard);
    }

    public void TryOpenLeaderboard()
    {
        if (_playerScore == null)
            throw new NullReferenceException("LeaderboardOpener can't initialized");

        bool isAuthorized;
#if UNITY_EDITOR
        isAuthorized = false;
#else   
        isAuthorized = PlayerAccount.IsAuthorized;
#endif
        if (isAuthorized)        
            PlayerAccount.RequestPersonalProfileDataPermission(OnSuccessCallback, OnErrorCallback);        
        else        
            _autorizationPanel.SetActive(true);        

        _holderPanel.SetActive(false);
    }

    private void OnErrorCallback(string _)
    {
        _leaderboard.SetPlayerScore(_playerScore.BestScore, Fill);
    }

    private void OnSuccessCallback()
    {
        _leaderboard.SetPlayerScore(_playerScore.BestScore, Fill);
    }

    private void Fill()
    {
        _leaderboard.Fill();
        _leaderboardPanel.SetActive(true);
    }
}

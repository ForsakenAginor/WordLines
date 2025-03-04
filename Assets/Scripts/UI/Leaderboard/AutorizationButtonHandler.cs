using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AutorizationButtonHandler : MonoBehaviour
{
    [SerializeField] private LeaderboardOpener _leaderboardOpener;
    [SerializeField] private GameObject _holderPanel;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ShowLeaderboard);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ShowLeaderboard);       
    }

    private void ShowLeaderboard()
    {
        PlayerAccount.Authorize(OnSuccessAutorize);
    }

    private void OnSuccessAutorize()
    {
        _leaderboardOpener.TryOpenLeaderboard();
        _holderPanel.SetActive(false);
    }
}

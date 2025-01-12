using UnityEngine;
using UnityEngine.UI;
using Localization;
using UnityEngine.SceneManagement;

public class MainMenuRoot : MonoBehaviour
{
    private const string MainSceneName = "MainScene";

    [SerializeField] private Button _toEnglish;
    [SerializeField] private Button _toRussian;
    [SerializeField] private Button _startGame;

    private LocalizationInitializer _localization;

    private void Awake()
    {
        _localization = new LocalizationInitializer();

        _toEnglish.onClick.AddListener(OnEnglishLanguageButtonClick);
        _toRussian.onClick.AddListener(OnRussianLanguageButtonClick);
        _startGame.onClick.AddListener(OnStartGameButtonClick);
    }

    private void OnDisable()
    {
        _toEnglish.onClick.RemoveListener(OnEnglishLanguageButtonClick);
        _toRussian.onClick.RemoveListener(OnRussianLanguageButtonClick);
        _startGame.onClick.RemoveListener(OnStartGameButtonClick);
    }

    private void OnStartGameButtonClick()
    {
        SceneManager.LoadScene(MainSceneName);
    }

    private void OnRussianLanguageButtonClick()
    {
        _localization.ApplyLocalization("ru");
    }

    private void OnEnglishLanguageButtonClick()
    {
        _localization.ApplyLocalization("en");
    }
}
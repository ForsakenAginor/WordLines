using Localization;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootTrap : MonoBehaviour
{
    private const string MenuSceneName = "MenuScene";

    private void Awake()
    {
        //YandexGamesSdk.CallbackLogging = true;
    }

    private IEnumerator Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        // Always wait for it if invoking something immediately in the first scene.
        /*
        while (YandexGamesSdk.IsInitialized == false)
            yield return YandexGamesSdk.Initialize();
            */
        yield return null;
#elif UNITY_EDITOR
        yield return null;
#endif

        LocalizationInitializer localization = new LocalizationInitializer();
        localization.ApplyLocalization("en");

        SceneManager.LoadScene(MenuSceneName);
    }
}

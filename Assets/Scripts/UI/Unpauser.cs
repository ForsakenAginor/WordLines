using UnityEngine;
using UnityEngine.UI;

public class Unpauser : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void OnEnable()
    {
        _button.onClick.AddListener(UnpauseGame);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(UnpauseGame);
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1f;
    }
}

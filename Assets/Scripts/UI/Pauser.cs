using UnityEngine;
using UnityEngine.UI;

public class Pauser : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void OnEnable()
    {
        _button.onClick.AddListener(PauseGame);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(PauseGame);        
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }
}
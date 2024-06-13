using UnityEngine;
using UnityEngine.UI;

public class MenuSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _targetPanel = null;
    [SerializeField] private GameObject _holderPanel;
    [SerializeField] private Button _toggleButton;

    private void OnEnable()
    {
        _toggleButton.onClick.AddListener(ShowTargetPanel);
    }

    private void OnDisable()
    {
        _toggleButton.onClick.RemoveListener(ShowTargetPanel);
    }

    private void ShowTargetPanel()
    {
        _holderPanel.SetActive(false);

        if (_targetPanel != null)
            _targetPanel.SetActive(true);
    }
}

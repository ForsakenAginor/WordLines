using UnityEngine;
using UnityEngine.UI;

public class GameRestarter : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Root _root;

    private void OnEnable()
    {
        _button.onClick.AddListener(_root.RestartGame);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(_root.RestartGame);
    }
}

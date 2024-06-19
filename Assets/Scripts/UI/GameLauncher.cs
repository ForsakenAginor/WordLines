using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GameLauncher : MonoBehaviour
{
    [SerializeField] private Root _root;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(_root.StartGame);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(_root.StartGame);
    }
}

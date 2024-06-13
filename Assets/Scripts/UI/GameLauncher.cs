using UnityEngine;
using UnityEngine.UI;

public class GameLauncher : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Root _root;

    private void OnEnable()
    {
        _button.onClick.AddListener(_root.Launch);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(_root.Launch);        
    }
}

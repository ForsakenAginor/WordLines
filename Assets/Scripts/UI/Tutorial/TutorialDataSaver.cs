using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TutorialDataSaver : MonoBehaviour
{
    private readonly TutorialData _tutorialData = new();
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(SaveData);
    }

    private void OnDisable()
    {
        _button.onClick.AddListener(SaveData);        
    }

    private void SaveData()
    {
        _tutorialData.IsTutorialCompleted = true;
    }
}

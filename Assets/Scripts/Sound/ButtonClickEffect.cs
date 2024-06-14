using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickEffect : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(PlayClickSound);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(PlayClickSound);        
    }

    private void PlayClickSound()
    {
        _audioSource.Play();
    }
}

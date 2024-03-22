using UnityEngine;
using UnityEngine.UI;

public class MuteButtonHandler : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private VolumeChanger _volumeChanger;
    [SerializeField] private Scrollbar _bar;

    public void Switch()
    {
        if (_image.enabled == false)
        {
            _image.enabled = true;
            float mute = 0f;
            _volumeChanger.ChangeGlobalVolume(mute);
        }
        else
        {
            _image.enabled = false;
            _volumeChanger.ChangeGlobalVolume(_bar.value);
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class NounRecordView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform _background;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _noun;

    public void Init(string noun, string description)
    {
        _noun.text = noun;
        _description.text = $"{description}";
        _description.ForceMeshUpdate();
        float scaleStep = 1.2f;
        float half = 2f;
        float middleHeight = (Screen.height / half) * 0.7f;
        float widthFactor = 1500f;

        while (_description.isTextOverflowing)
        {
            if (_background.rect.height < middleHeight)
                _background.sizeDelta = new Vector2(_background.rect.width, _background.rect.height * scaleStep);
            else
                _background.sizeDelta = new Vector2(_background.rect.width * scaleStep, _background.rect.height);

            _description.ForceMeshUpdate();

            if (_background.rect.width > widthFactor)
            {
                _description.overflowMode = TextOverflowModes.Truncate;
                break;
            }
        }

        SetPosition();
        _background.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetPosition();
        _background.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _background.gameObject.SetActive(false);
    }

    private void SetPosition()
    {
        float half = 2f;
        float middleHeight = Screen.height / half;
        int directionFactor;

        if (_noun.transform.position.y < middleHeight)
            directionFactor = 1;
        else
            directionFactor = -1;

        _background.localPosition = new Vector3(_background.sizeDelta.x / half, directionFactor * _background.sizeDelta.y / half, 0);
    }
}

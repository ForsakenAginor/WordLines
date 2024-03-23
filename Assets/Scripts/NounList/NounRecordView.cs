using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class NounRecordView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform _background;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _noun;

    public  void Init(string noun, string description)
    {
        _noun.text = noun;
        _description.text = $"{description}";
        _description.ForceMeshUpdate();
        float scaleStep = 1.5f;

        while (_description.isTextOverflowing)
        {
            _background.sizeDelta = new Vector2(_background.rect.width, _background.rect.height * scaleStep);
            _description.ForceMeshUpdate();
        }

        float half = 2f;
        _background.localPosition = new Vector3(_background.sizeDelta.x / half, -_background.sizeDelta.y / half, 0);
        _background.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _background.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _background.gameObject.SetActive(false);
    }
}

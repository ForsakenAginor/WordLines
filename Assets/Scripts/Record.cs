using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Record : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _descriptionPanel;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _noun;

    private Transform _parentCanvas;

    public  void Init(string noun, string description)
    {
        _parentCanvas = GetComponentInParent<Canvas>().transform;
        _descriptionPanel.SetActive(false);
        _description.text = description;
        _noun.text = noun;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //_descriptionPanel.transform.SetParent(_parentCanvas);
        _descriptionPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //_descriptionPanel.transform.SetParent(transform);
        _descriptionPanel.SetActive(false);
    }
}

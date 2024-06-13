using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class CellView : MonoBehaviour
{
    [SerializeField] private TMP_Text _letter;
    [SerializeField] private TMP_Text _weight;

    private readonly Dictionary<int, Color> _colors = new()
    {
        { 1, Color.green },
        { 2, Color.cyan },
        { 3, new Color(0.34f, 0.34f, 0.95f) },
        { 5, Color.yellow },
        { 6, new Color(0.8f, 0.5f, 0) },
        { 10, new Color(0.8f, 0.17f, 0.17f) },
        { 15, new Color(0.5f, 0.7f, 0.67f) },
    };
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();           
    }

    public void Init(char letter)
    {
        _letter.text = letter.ToString();
        int weight = Letters.GetLetterValue(letter);
        _weight.text = weight.ToString();
        
        if(_colors.ContainsKey(weight))
            _image.color = _colors[weight];
        else
            _image.color = Color.white;
        /*
        transform.localScale = Vector3.zero;
        float normalScale = 1f;
        float duration = 2f;
        transform.DOScale(normalScale, duration);*/
    }

}

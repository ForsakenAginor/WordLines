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

    private Image _image;
    private Dictionary<int, Color> _colors = new Dictionary<int, Color>()
    {
        { 1, Color.green },
        { 2, Color.cyan },
        { 3, Color.blue },
        { 5, Color.yellow },
        { 6, new Color(0.8f, 0.5f, 0) },
        { 10, Color.red },
        { 15, Color.white },
    };

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

        transform.localScale = Vector3.zero;
        transform.DOScale(1, 2);
    }

}

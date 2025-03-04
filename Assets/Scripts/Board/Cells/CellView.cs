using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


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
    };
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();           
    }

    public void Init(char letter, int weight)
    {
        _letter.text = letter.ToString();
        
        if(_colors.ContainsKey(weight))
            _image.color = _colors[weight];
        else
            _image.color = Color.white;
    }
}

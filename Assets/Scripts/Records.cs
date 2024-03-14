using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Records : MonoBehaviour
{
    [SerializeField] private Transform _contentHolder;
    [SerializeField] private TMP_Text _prefab;

    private List<Transform> _words = new List<Transform>();

    public void AddRecord(string record)
    {
        IncrementIndexes();
        TMP_Text word = Instantiate(_prefab, _contentHolder);
        word.text = record;
        word.transform.SetSiblingIndex(0);
        _words.Add(word.transform);
    }

    private void IncrementIndexes()
    {
        foreach (Transform word in _words)
        {
            int currentIndex = transform.GetSiblingIndex();
            currentIndex++;
            word.SetSiblingIndex(currentIndex);
        }
    }
}

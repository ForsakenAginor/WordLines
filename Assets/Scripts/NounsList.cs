using UnityEngine;
using System.Collections.Generic;

public class NounsList : MonoBehaviour
{
    [SerializeField] private Transform _contentHolder;
    [SerializeField] private Record _prefab;

    private readonly List<Transform> _words = new();

    public void Restart()
    {
        for (int i = 0; i < _words.Count; i++)
            Destroy(_words[i].gameObject);

        _words.Clear();
    }

    public void AddRecord(string noun, string description)
    {
        Record word = Instantiate(_prefab, _contentHolder);
        word.Init(noun, description);
        _words.Add(word.transform);
        SetIndexes();
    }

    private void SetIndexes()
    {
        int index = 0;

        for (int i = _words.Count - 1; i >= 0; i--)
        {
            _words[i].SetSiblingIndex(index);
            index++;
        }
    }
}

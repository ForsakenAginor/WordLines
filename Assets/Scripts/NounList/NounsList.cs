using UnityEngine;
using System.Collections.Generic;
using System;

public class NounsList : MonoBehaviour
{
    private Transform _nounViewHolder;
    private NounRecordView _prefab;
    private readonly List<Transform> _words = new();

    public void Init(Transform nounViewHolder, NounRecordView prefab)
    {
        _nounViewHolder = nounViewHolder != null ? nounViewHolder : throw new ArgumentNullException(nameof(nounViewHolder));
        _prefab = prefab != null ? prefab : throw new ArgumentNullException(nameof(prefab));
    }

    public void Restart()
    {
        for (int i = 0; i < _words.Count; i++)
            Destroy(_words[i].gameObject);

        _words.Clear();
    }

    public void AddRecord(string noun, string description)
    {
        if (_nounViewHolder == null || _prefab == null)
            throw new NullReferenceException();

        NounRecordView word = Instantiate(_prefab, _nounViewHolder);
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

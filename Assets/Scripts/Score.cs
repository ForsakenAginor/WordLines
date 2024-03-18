using System;
using UnityEngine;

public class Score : MonoBehaviour
{
    private int _value;

    public event Action ValueChanged;

    public int Value
    {
        get { return _value; }
        private set
        {
            _value = value;
            ValueChanged?.Invoke();
        }
    }

    public void Restart()
    {
        Value = 0;
    }

    public void SetScore(string word, int combo)
    {
        if (string.IsNullOrEmpty(word))
            throw new ArgumentException(nameof(word));

        if(combo <= 0)
            throw new ArgumentOutOfRangeException(nameof(combo));

        int wordValue = 0;

        for (int i = 0; i < word.Length; i++)
            wordValue += Letters.GetLetterValue(word[i]);

        wordValue *= word.Length * combo;
        Value += wordValue;
    }
}

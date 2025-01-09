using System;

public class Score
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

    public static int CalcScore(string word, int combo, ILetters letters)
    {
        if (string.IsNullOrEmpty(word))
            throw new ArgumentException(nameof(word));

        if (combo <= 0)
            throw new ArgumentOutOfRangeException(nameof(combo));

        if (letters == null)
            throw new ArgumentNullException(nameof(letters));

        int wordValue = 0;

        for (int i = 0; i < word.Length; i++)
            wordValue += letters.GetLetterValue(word[i]);

        wordValue *= word.Length * combo;
        return wordValue;
    }

    public void Restart()
    {
        Value = 0;
    }

    public void AddScore(int value)
    {
        Value += value;
    }
}

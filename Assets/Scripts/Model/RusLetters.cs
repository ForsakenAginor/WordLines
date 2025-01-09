using System.Collections.Generic;

public class RusLetters : ILetters
{
    private static readonly Dictionary<char, int> _letterValuePairs = new()
    {
        {'А', 1 },
        {'Б', 3 },
        {'В', 2 },
        {'Г', 3 },
        {'Д', 3 },
        {'Е', 1 },
        {'Ж', 5 },
        {'З', 5 },
        {'И', 1 },
        {'Й', 5 },
        {'К', 2 },
        {'Л', 2 },
        {'М', 2 },
        {'Н', 2 },
        {'О', 1 },
        {'П', 2 },
        {'Р', 2 },
        {'С', 2 },
        {'Т', 2 },
        {'У', 2 },
        {'Ф', 10 },
        {'Х', 5 },
        {'Ц', 10 },
        {'Ч', 5 },
        {'Ш', 10 },
        {'Щ', 10 },
        {'Ъ', 10 },
        {'Ы', 5 },
        {'Ь', 5 },
        {'Э', 6 },
        {'Ю', 6 },
        {'Я', 2 }
    };

    private readonly List<char> _randomLetters = new();

    public RusLetters()
    {
        SetRandomLetters();
    }

    public int GetLetterValue(char letter)
    {
        return _letterValuePairs[letter];
    }

    public char GetRandomLetter()
    {
        return _randomLetters[UnityEngine.Random.Range(0, _randomLetters.Count)];
    }

    private void SetRandomLetters()
    {
        int weightFactor = 30;

        foreach (char letter in _letterValuePairs.Keys)
        {
            int amount = weightFactor / _letterValuePairs[letter];

            for (int i = 0; i < amount; i++)
                _randomLetters.Add(letter);
        }
    }
}

public class EngLetters : ILetters
{
    private static readonly Dictionary<char, int> _letterValuePairs = new()
    {
        {'A', 1 },
        {'B', 5 },
        {'C', 3 },
        {'D', 3 },
        {'E', 1 },
        {'F', 5 },
        {'G', 5 },
        {'H', 2 },
        {'I', 1 },
        {'J', 10 },
        {'K', 6 },
        {'L', 3 },
        {'M', 3 },
        {'N', 2 },
        {'O', 1 },
        {'P', 5 },
        {'Q', 10 },
        {'R', 2 },
        {'S', 2 },
        {'T', 1 },
        {'U', 3 },
        {'V', 6 },
        {'W', 5 },
        {'X', 10 },
        {'Y', 5 },
        {'Z', 10 },
    };

    private readonly List<char> _randomLetters = new();

    public EngLetters()
    {
        SetRandomLetters();
    }

    public int GetLetterValue(char letter)
    {
        return _letterValuePairs[letter];
    }

    public char GetRandomLetter()
    {
        return _randomLetters[UnityEngine.Random.Range(0, _randomLetters.Count)];
    }

    private void SetRandomLetters()
    {
        int weightFactor = 30;

        foreach (char letter in _letterValuePairs.Keys)
        {
            int amount = weightFactor / _letterValuePairs[letter];

            for (int i = 0; i < amount; i++)
                _randomLetters.Add(letter);
        }
    }
}

public interface ILetters
{
    public int GetLetterValue(char letter);

    public char GetRandomLetter();
}

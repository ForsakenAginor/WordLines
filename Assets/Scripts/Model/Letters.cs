using System.Collections.Generic;

public class Letters 
{
    private static readonly Dictionary<char, int> _letterValuePairs = new Dictionary<char, int>()
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
        {'Й', 3 },
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
        {'Ъ', 15 },
        {'Ы', 3 },
        {'Ь', 5 },
        {'Э', 6 },
        {'Ю', 6 },
        {'Я', 2 }
    };

    private readonly List<char> _randomLetters = new List<char>();

    public  Letters()
    {
        SetRandomLetters();
    }

    public IReadOnlyDictionary<char, int> LetterValuePairs => _letterValuePairs;

    public static int GetLetterValue(char letter)
    {
        return _letterValuePairs[letter];
    }

    public static IEnumerable<char> GetAlfabet()
    {
        return _letterValuePairs.Keys;
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

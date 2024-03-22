using System;
using System.Collections.Generic;
using System.Linq;

public class MatchFinder
{
    private readonly IEnumerable<string> _nouns;
    private readonly Dictionary<int, IEnumerable<string>> _sortedNouns = new();

    public MatchFinder(NounDictionary dictionary)
    {
        if (dictionary == null)
            throw new ArgumentNullException(nameof(dictionary));

        _nouns = dictionary.Nouns.Keys;
        dictionary.WordRemoved += OnWordRemoved;
        SortNounsBySize();
    }

    public bool TryFind(string letters, int index, out string bestResult)
    {
        if (letters.Length <= index)
            throw new ArgumentOutOfRangeException(nameof(letters));

        List<string> wordsToSearch = CreatePseudoWords(letters, index);
        List<string> nouns = new();

        for(int i = 0; i < wordsToSearch.Count; i++)
        {
            if (_sortedNouns[wordsToSearch[i].Length].Contains(wordsToSearch[i]))
                nouns.Add(wordsToSearch[i]);
        }

        bestResult = nouns.OrderByDescending(o => o.Length).FirstOrDefault();

        return bestResult != null;
    }


    public void SortNounsBySize()
    {
        _sortedNouns.Clear();
        int boardSize = BoardConfig.Width > BoardConfig.Height ? BoardConfig.Width : BoardConfig.Height;

        for (int i = 2; i <= boardSize; i++)
        {
            IEnumerable<string> nouns = _nouns.Where(o => o.Length == i).ToList();
            _sortedNouns.Add(i, nouns);
        }
    }

    private void OnWordRemoved()
    {
        SortNounsBySize();
    }


    private List<string> CreatePseudoWords(string letters, int index)
    {
        List<string> results = new();
        string result;
        int lastIndex = letters.Length - 1;

        for (int i = 0; i <= index && i < lastIndex; i++)
        {
            result = new string(letters.Skip(i).ToArray());
            results.Add(result);
        }

        int value = letters.Length - 2;

        for (int i = value; i >= index && i > 0; i--)
        {
            result = new string(letters.SkipLast(lastIndex - i).ToArray());
            results.Add(result);
        }

        if (index > 0 && index < lastIndex)
        {
            letters = new string(letters.Skip(1).SkipLast(1).ToArray());
            index--;
            results = results.Union(CreatePseudoWords(letters, index)).ToList();
        }

        return results;
    }
}

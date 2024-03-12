using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchFinder
{
    private IEnumerable<string> _nouns;

    public MatchFinder(IEnumerable<string> nouns)
    {
        _nouns = nouns ?? throw new ArgumentNullException(nameof(nouns));
    }

    public bool TryFind(string letters, int index, out string bestResult)
    {
        if (letters.Length <= index)
            throw new ArgumentOutOfRangeException(nameof(letters));

        List<string> wordsToSearch = CreatePseudoWords(letters, index);
        bestResult = wordsToSearch.Intersect(_nouns).OrderByDescending(o => o.Length).FirstOrDefault();

        return bestResult != null;
    }

    private List<string> CreatePseudoWords(string letters, int index)
    {
        List<string> results = new List<string>();
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

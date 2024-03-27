using System;
using System.Collections.Generic;
using System.Linq;

public class NounDictionary
{
    private readonly string _fileContent;
    private readonly Dictionary<string, string> _nouns = new();

    public NounDictionary(string rawDictionary)
    {
        _fileContent = rawDictionary ?? throw new ArgumentNullException(nameof(rawDictionary));
        Fill();
        FilterBySize(BoardConfig.Width > BoardConfig.Height ? BoardConfig.Width : BoardConfig.Height);
    }

    public event Action WordRemoved;

    public IReadOnlyDictionary<string, string> Nouns => _nouns;

    public void RemoveWord(string word)
    {
        if (_nouns.ContainsKey(word) == false)
            throw new ArgumentOutOfRangeException(nameof(word));

        _nouns.Remove(word);
        WordRemoved?.Invoke();
    }

    private void Fill()
    {
        const string StringSeparator = "\n";
        const string NounDefinitionSeparator = ":";

        string[] strings = _fileContent.Split(StringSeparator);

        foreach (string item in strings)
        {
            string noun = item.Split(NounDefinitionSeparator).First().ToUpper();
            string definition = new string((item.ToCharArray().Skip(noun.Length + NounDefinitionSeparator.Length).ToArray())).Trim();
            _nouns.Add(noun, definition);
        }
    }

    private void FilterBySize(int maxSize)
    {
        var tempList = _nouns.Keys.ToList();

        foreach (string noun in tempList)
        {
            if (noun.Length > maxSize)
                _nouns.Remove(noun);
        }
    }
}

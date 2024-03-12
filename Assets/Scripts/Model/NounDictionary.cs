using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class NounDictionary
{
    private const string FileName = "nouns.txt";

    private string _fileContent;
    private Dictionary<string, string> _nouns = new Dictionary<string, string>();
    
    public NounDictionary()
    {
        string path = $"{Application.streamingAssetsPath}/{FileName}"; 
        _fileContent = File.ReadAllText(path);
        Fill();
        FilterBySize(BoardConfig.Width > BoardConfig.Height ? BoardConfig.Width : BoardConfig.Height);
    }

    public IReadOnlyDictionary<string, string> Nouns => _nouns;

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
    
    public void FilterBySize(int maxSize)
    {
        var tempList = _nouns.Keys.ToList();

        foreach(string noun in tempList)
        {
            if(noun.Length > maxSize)
                _nouns.Remove(noun);
        }
    }
}

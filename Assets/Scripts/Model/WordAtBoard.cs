using System.Collections.Generic;

public class WordAtBoard
{
    private readonly string _word;
    private readonly List<Cell> _wordPosition;

    public WordAtBoard(string bestWord, List<Cell> wordPosition)
    {
        _word = bestWord;
        _wordPosition = wordPosition;
    }

    public string Word  => _word;
    public IEnumerable<Cell> WordPosition => _wordPosition;
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board
{
    private List<Cell> _cellsList = new List<Cell>();
    private MatchFinder _matchFinder;
    private Letters _letters;

    public Board(Letters letters, MatchFinder matchfinder)
    {
        _matchFinder = matchfinder ?? throw new ArgumentNullException(nameof(matchfinder));
        _letters = letters ?? throw new ArgumentNullException(nameof(letters));

        for (int y = 0; y < BoardConfig.Height; y++)
        {
            for (int x = 0; x < BoardConfig.Width; x++)
            {
                char letter = letters.GetRandomLetter();
                _cellsList.Add(new Cell(x, y, letter));
            }
        }
        FixInitiallyMatches();
    }

    public IEnumerable<Cell> GetCells() => _cellsList;
    
    public void RemoveCells(IEnumerable<Cell> cells)
    {

    }

    public void SwapCells(Vector2 first, Vector2 second)
    {
        Cell firstCell = _cellsList.Where(o => o.XPosition == first.x && o.YPosition == first.y).FirstOrDefault();
        Cell secondCell = _cellsList.Where(o => o.XPosition == second.x && o.YPosition == second.y).FirstOrDefault();
        firstCell.Move(second - first);
        secondCell.Move(first - second);
    }

    public void GetMatchedWord(Vector2 cell, out string bestWord, out IEnumerable<Cell> wordPosition )
    {
        string row = new string(GetCells().Where(o => o.YPosition == (int)cell.y).OrderBy(o => o.XPosition).Select(o => o.Content).ToArray());
        string column = new string(GetCells().Where(o => o.XPosition == (int)cell.x).OrderBy(o => o.YPosition).Select(o => o.Content).ToArray());

        bestWord = string.Empty;
        string rowNoun = string.Empty;
        string columnNoun = string.Empty;

        if (_matchFinder.TryFind(row, (int)cell.x, out rowNoun) | _matchFinder.TryFind(column, (int)cell.y, out columnNoun))
        {
            if(rowNoun == null)
                rowNoun = string.Empty;

            if(columnNoun == null)
                columnNoun = string.Empty;

            if( rowNoun.Length > columnNoun.Length)
            {
                bestWord = rowNoun;
                int wordLenght = bestWord.Length;
                int startIndex = Mathf.Clamp((int)cell.x - wordLenght + 1, 0, row.Length);
                int wordStartIndex = row.IndexOf(bestWord, startIndex);
                wordPosition = GetCells().Where(o => o.YPosition == cell.y && o.XPosition >= wordStartIndex && o.XPosition < (wordStartIndex + wordLenght));
            }
            else
            {
                bestWord = columnNoun;
                int wordLenght = bestWord.Length;
                int startIndex = Mathf.Clamp((int)cell.y - wordLenght + 1, 0, column.Length);
                int wordStartIndex = column.IndexOf(bestWord, startIndex);
                wordPosition = GetCells().Where(o => o.XPosition == cell.x && o.YPosition >= wordStartIndex && o.YPosition < (wordStartIndex + wordLenght));
            }
        }
        else
        {
            wordPosition = new List<Cell>();
        }
    }

    private void FixInitiallyMatches()
    {
        foreach (Cell cell in _cellsList)
        {
            bool hasMatch;

            do
            {
                string row = new string(GetCells().Where(o => o.YPosition == cell.YPosition).OrderBy(o => o.XPosition).Select(o => o.Content).ToArray());
                string column = new string(GetCells().Where(o => o.XPosition == cell.XPosition).OrderBy(o => o.YPosition).Select(o => o.Content).ToArray());
                hasMatch = _matchFinder.TryFind(row, cell.XPosition, out _) || _matchFinder.TryFind(column, cell.YPosition, out _);

                if (hasMatch)                
                    cell.ChangeContent(_letters.GetRandomLetter());
                
            } while (hasMatch);
        }
    }
}



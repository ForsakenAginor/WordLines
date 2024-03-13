using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board
{
    private List<Cell> _cells = new List<Cell>();
    private MatchFinder _matchFinder;
    private Letters _letters;

    public Board(Letters letters, MatchFinder matchfinder)
    {
        _matchFinder = matchfinder ?? throw new ArgumentNullException(nameof(matchfinder));
        _letters = letters ?? throw new ArgumentNullException(nameof(letters));

        for (int y = 0; y < BoardConfig.Height; y++)
        {
            for (int x = 0; x < BoardConfig.Width; x++)
                _cells.Add(new Cell(x, y, letters.GetRandomLetter()));
        }

        FixInitiallyMatches();
    }

    public IEnumerable<Cell> GetCells() => _cells;

    public void RemoveCells(IEnumerable<Cell> cellsForDeleting)
    {
        foreach(Cell cell in cellsForDeleting)
        {
            var dropdownCells = _cells.Where(o => o.YPosition < cell.YPosition);

            foreach (Cell dropdownCell in dropdownCells)
                dropdownCell.MoveDown();

            _cells.Add(new Cell(cell.XPosition, 0, _letters.GetRandomLetter()));
            _cells.Remove(cell);
        }
    }

    public void SwapCells(Vector2 first, Vector2 second)
    {
        Cell firstCell = _cells.Where(o => o.XPosition == first.x && o.YPosition == first.y).FirstOrDefault();
        Cell secondCell = _cells.Where(o => o.XPosition == second.x && o.YPosition == second.y).FirstOrDefault();
        firstCell.Move(second - first);
        secondCell.Move(first - second);
    }

    public void GetMatchedWord(Vector2 cell, out string bestWord, out IEnumerable<Cell> wordCells)
    {
        string row = new string(GetCells().Where(o => o.YPosition == (int)cell.y).OrderBy(o => o.XPosition).Select(o => o.Content).ToArray());
        string column = new string(GetCells().Where(o => o.XPosition == (int)cell.x).OrderBy(o => o.YPosition).Select(o => o.Content).ToArray());

        bestWord = string.Empty;
        string rowNoun = string.Empty;
        string columnNoun = string.Empty;

        if (_matchFinder.TryFind(row, (int)cell.x, out rowNoun) | _matchFinder.TryFind(column, (int)cell.y, out columnNoun))
        {
            rowNoun = rowNoun == null ? string.Empty : rowNoun;
            columnNoun = columnNoun == null ? string.Empty : columnNoun;

            if (rowNoun.Length > columnNoun.Length)
            {
                bestWord = rowNoun;
                int wordStartIndex = GetWordStartIndex((int)cell.x, row, rowNoun);
                wordCells = GetCells().Where(o => o.YPosition == cell.y && o.XPosition >= wordStartIndex && o.XPosition < (wordStartIndex + rowNoun.Length)).ToList();
            }
            else
            {
                bestWord = columnNoun;
                int wordStartIndex = GetWordStartIndex((int)cell.y, column, columnNoun);
                wordCells = GetCells().Where(o => o.XPosition == cell.x && o.YPosition >= wordStartIndex && o.YPosition < (wordStartIndex + columnNoun.Length)).ToList();
            }
        }
        else
        {
            wordCells = new List<Cell>();
        }
    }

    private static int GetWordStartIndex(int position, string rawString, string subString)
    {
        int minStartIndex = position - subString.Length + 1; 
        int startIndex = Mathf.Clamp(minStartIndex, 0, rawString.Length);

        return rawString.IndexOf(subString, startIndex);
    }

    private void FixInitiallyMatches()
    {
        foreach (Cell cell in _cells)
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



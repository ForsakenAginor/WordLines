using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Board
{
    private readonly List<Cell> _cells = new();
    private readonly MatchFinder _matchFinder;
    private readonly ILetters _letters;

    public Board(ILetters letters, MatchFinder matchfinder)
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

    public IEnumerable<Cell> Cells => _cells;

    public IEnumerable<Cell> ReplaceCells(IEnumerable<Cell> cellsForDeleting)
    {
        cellsForDeleting = cellsForDeleting.OrderBy(o => o.YPosition);
        List<Cell> newCells = new();

        foreach (Cell cell in cellsForDeleting)
        {
            var dropdownCells = _cells.Where(o => o.YPosition < cell.YPosition && o.XPosition == cell.XPosition).ToList();

            foreach (Cell dropdownCell in dropdownCells)
                dropdownCell.MoveDown();

            Cell newCell = new(cell.XPosition, 0, _letters.GetRandomLetter());
            _cells.Add(newCell);
            newCells.Add(newCell);
            _cells.Remove(cell);
        }

        return newCells;
    }

    public void SwapCells(Vector2 first, Vector2 second)
    {
        Cell firstCell = _cells.Where(o => o.XPosition == first.x && o.YPosition == first.y).FirstOrDefault();
        Cell secondCell = _cells.Where(o => o.XPosition == second.x && o.YPosition == second.y).FirstOrDefault();
        firstCell.Move(second - first);
        secondCell.Move(first - second);
    }
    
    public WordAtBoard FindWordMultiThreading(Vector2[] cells)
    {
        int maxTaskCount = 6;

        if (cells.Length < maxTaskCount)
            return FindWord(cells);

        List<WordAtBoard> results = new();
        List<Task<WordAtBoard>> tasks = new();
        int skippedCells;

        for (int i = 1; i < maxTaskCount; i++)
        {
            skippedCells = cells.Length / maxTaskCount;
            Vector2[] cellsPart = cells.ToList().Take(skippedCells).ToArray();
            cells = cells.ToList().Skip(skippedCells).ToArray();
            Task<WordAtBoard> scanTask = new(() => FindWord(cellsPart));
            tasks.Add(scanTask);
        }

        Task<WordAtBoard> lastTask = new(() => FindWord(cells));
        tasks.Add(lastTask);

        foreach(Task<WordAtBoard> task in tasks)
            task.Start();

        foreach (Task<WordAtBoard> task in tasks)
            task.Wait();

        foreach (Task<WordAtBoard> task in tasks)
            results.Add(task.Result);

        return results.Where(o => o != null).OrderByDescending(o => o.Word.Length).FirstOrDefault();
    }

    public WordAtBoard FindWord(Vector2[] cells)
    {
        List<WordAtBoard> words = new();

        foreach (var cell in cells)
        {
            GetMatchedWord(cell, out WordAtBoard word);
            words.Add(word);
        }

        return words.Where(o => o != null).OrderByDescending(o => o.Word.Length).FirstOrDefault();
    }

    private void GetMatchedWord(Vector2 cell, out WordAtBoard word)
    {
        string row = new(Cells.Where(o => o.YPosition == (int)cell.y).OrderBy(o => o.XPosition).Select(o => o.Content).ToArray());
        string column = new(Cells.Where(o => o.XPosition == (int)cell.x).OrderBy(o => o.YPosition).Select(o => o.Content).ToArray());
        string bestWord = string.Empty;
        List<Cell> wordPosition = new();
        word = null;

        if (_matchFinder.TryFind(row, (int)cell.x, out string rowNoun) | _matchFinder.TryFind(column, (int)cell.y, out string columnNoun))
        {
            rowNoun ??= string.Empty;
            columnNoun ??= string.Empty;

            if (rowNoun.Length > columnNoun.Length)
            {
                bestWord = rowNoun;
                int wordStartIndex = GetWordStartIndex((int)cell.x, row, rowNoun);
                wordPosition = Cells.Where(o => o.YPosition == cell.y && o.XPosition >= wordStartIndex && o.XPosition < (wordStartIndex + rowNoun.Length)).ToList();
            }
            else
            {
                bestWord = columnNoun;
                int wordStartIndex = GetWordStartIndex((int)cell.y, column, columnNoun);
                wordPosition = Cells.Where(o => o.XPosition == cell.x && o.YPosition >= wordStartIndex && o.YPosition < (wordStartIndex + columnNoun.Length)).ToList();
            }

            word = new WordAtBoard(bestWord, wordPosition);
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
                string row = new(Cells.Where(o => o.YPosition == cell.YPosition).OrderBy(o => o.XPosition).Select(o => o.Content).ToArray());
                string column = new(Cells.Where(o => o.XPosition == cell.XPosition).OrderBy(o => o.YPosition).Select(o => o.Content).ToArray());
                hasMatch = _matchFinder.TryFind(row, cell.XPosition, out _) || _matchFinder.TryFind(column, cell.YPosition, out _);

                if (hasMatch)
                    cell.ChangeContent(_letters.GetRandomLetter());

            } while (hasMatch);
        }
    }
}



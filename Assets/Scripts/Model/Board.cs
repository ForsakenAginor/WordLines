using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public IEnumerable<Cell> Cells => _cells;

    public IEnumerable<Cell> ReplaceCells(IEnumerable<Cell> cellsForDeleting)
    {
        cellsForDeleting = cellsForDeleting.OrderBy(o => o.YPosition);
        List<Cell> newCells = new List<Cell>();

        foreach (Cell cell in cellsForDeleting)
        {
            var dropdownCells = _cells.Where(o => o.YPosition < cell.YPosition && o.XPosition == cell.XPosition).ToList();

            foreach (Cell dropdownCell in dropdownCells)
                dropdownCell.MoveDown();

            Cell newCell = new Cell(cell.XPosition, 0, _letters.GetRandomLetter());
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

    public WordAtBoard HZ(Vector2[] cells)
    {
        List<WordAtBoard> results = new List<WordAtBoard>();

        if (cells.Length == 2)
        {
            int size = cells.Length / 2;
            Vector2[] firstHalf = cells.ToList().Take(size).ToArray();
            Vector2[] secondHalf = cells.ToList().Skip(size).ToArray();

            Task<WordAtBoard> scanTask1 = new Task<WordAtBoard>(() => FindWord(firstHalf));
            Task<WordAtBoard> scanTask2 = new Task<WordAtBoard>(() => FindWord(secondHalf));
            scanTask1.Start();
            scanTask2.Start();
            scanTask1.Wait();
            scanTask2.Wait();
            results.Add(scanTask1.Result);
            results.Add(scanTask2.Result);
        }
        else
        {
            int size = cells.Length / 4;
            Vector2[] first = cells.ToList().Take(size).ToArray();
            Vector2[] second = cells.ToList().Skip(size).Take(size).ToArray();
            Vector2[] third = cells.ToList().Skip(size).Skip(size).Take(size).ToArray();
            Vector2[] fourth = cells.ToList().Skip(size).Skip(size).Skip(size).ToArray();

            Task<WordAtBoard> scanTask1 = new Task<WordAtBoard>(() => FindWord(first));
            Task<WordAtBoard> scanTask2 = new Task<WordAtBoard>(() => FindWord(second));
            Task<WordAtBoard> scanTask3 = new Task<WordAtBoard>(() => FindWord(third));
            Task<WordAtBoard> scanTask4 = new Task<WordAtBoard>(() => FindWord(fourth));
            scanTask1.Start();
            scanTask2.Start();
            scanTask3.Start();
            scanTask4.Start();
            scanTask1.Wait();
            scanTask2.Wait();
            scanTask3.Wait();
            scanTask4.Wait();
            results.Add(scanTask1.Result);
            results.Add(scanTask2.Result);
            results.Add(scanTask3.Result);
            results.Add(scanTask4.Result);
        }

        return results.Where(o => o != null).OrderByDescending(o => o.Word.Length).FirstOrDefault();
    }

    public WordAtBoard FindWord(Vector2[] cells)
    {
        List<WordAtBoard> words = new List<WordAtBoard>();

        foreach (var cell in cells)
        {
            GetMatchedWord(cell, out WordAtBoard word);
            words.Add(word);
        }

        return words.Where(o => o != null).OrderByDescending(o => o.Word.Length).FirstOrDefault();
    }

    private void GetMatchedWord(Vector2 cell, out WordAtBoard word)
    {
        string row = new string(Cells.Where(o => o.YPosition == (int)cell.y).OrderBy(o => o.XPosition).Select(o => o.Content).ToArray());
        string column = new string(Cells.Where(o => o.XPosition == (int)cell.x).OrderBy(o => o.YPosition).Select(o => o.Content).ToArray());
        string bestWord = string.Empty;
        List<Cell> wordPosition = new List<Cell>();
        word = null;

        if (_matchFinder.TryFind(row, (int)cell.x, out string rowNoun) | _matchFinder.TryFind(column, (int)cell.y, out string columnNoun))
        {
            rowNoun = rowNoun == null ? string.Empty : rowNoun;
            columnNoun = columnNoun == null ? string.Empty : columnNoun;

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
                string row = new string(Cells.Where(o => o.YPosition == cell.YPosition).OrderBy(o => o.XPosition).Select(o => o.Content).ToArray());
                string column = new string(Cells.Where(o => o.XPosition == cell.XPosition).OrderBy(o => o.YPosition).Select(o => o.Content).ToArray());
                hasMatch = _matchFinder.TryFind(row, cell.XPosition, out _) || _matchFinder.TryFind(column, cell.YPosition, out _);

                if (hasMatch)
                    cell.ChangeContent(_letters.GetRandomLetter());

            } while (hasMatch);
        }
    }
}



using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private CellMover _cellPrefab;
    private List<CellMover> _cells;
    private Board _board;
    private int _comboMultiplier = 1;

    public event Action<string, int, Vector3> WordFound;
    public event Action ChainScanStarted;
    public event Action ChainScanFinished;

    public void Init(Letters letters, NounDictionary nouns, CellMover cellPrefab)
    {
        if (letters == null)
            throw new ArgumentNullException(nameof(letters));

        if (nouns == null)
            throw new ArgumentNullException(nameof(nouns));

        if (cellPrefab == null)
            throw new ArgumentNullException(nameof(cellPrefab));

        _cells = new List<CellMover>();
        _cellPrefab = cellPrefab;
        MatchFinder finder = new(nouns);
        _board = new Board(letters, finder);

        foreach (var cell in _board.Cells)
        {
            CellMover cellMover = Instantiate(_cellPrefab, transform);
            cellMover.Init(cell, transform);
            _cells.Add(cellMover);
        }

        OnEnable();
    }

    private void OnEnable()
    {
        for (int i = 0; i < _cells.Count; i++)
            _cells[i].CellsSwapped += OnCellsSwapped;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _cells.Count; i++)
            _cells[i].CellsSwapped -= OnCellsSwapped;
    }

    public void ResetBoard(Letters letters, NounDictionary nouns)
    {
        if (letters == null)
            throw new ArgumentNullException(nameof(letters));

        if (nouns == null)
            throw new ArgumentNullException(nameof(nouns));

        if (_cells == null)
            throw new Exception();

        MatchFinder finder = new(nouns);
        _board = new Board(letters, finder);

        for (int i = 0; i < _cells.Count; i++)
            _cells[i].Init(_board.Cells.ToList()[i], transform);
    }

    private void OnCellsSwapped(Vector2 first, Vector2 second)
    {
        _board.SwapCells(first, second);
        StartScanCycle(new Vector2[] { first, second });
    }

    private void StartScanCycle(Vector2[] cells)
    {
        WordAtBoard bestResult = _board.FindWord(cells);

        if (bestResult == null)
            return;

        ChainScanStarted?.Invoke();
        DestroyCells(bestResult);
        _comboMultiplier++;
        StartCoroutine(ScanBoard());
    }

    private IEnumerator ScanBoard()
    {
        float delaySeconds = 1;
        int standardComboMultiplier = 1;
        WaitForSeconds delay = new(delaySeconds);
        Vector2[] cells = _cells.Select(o => o.CellPosition).ToArray();

        while (true)
        {
            WordAtBoard bestResult = _board.FindWord(cells);

            if (bestResult == null)
            {
                ChainScanFinished?.Invoke();
                _comboMultiplier = standardComboMultiplier;
                yield break;
            }

            DestroyCells(bestResult);
            _comboMultiplier++;
            yield return delay;
        }
    }

    private void UpdateAffectedCells(IEnumerable<Cell> cells)
    {
        List<Cell> sortedCells = new(cells);
        sortedCells = sortedCells.OrderBy(o => o.YPosition).ToList();

        foreach (Cell cell in sortedCells)
        {
            var dropdownCells = _cells.Where(o => o.CellPosition.y < cell.YPosition && o.CellPosition.x == cell.XPosition).OrderBy(o => o.CellPosition.y).ToList();
            float moveDuration = 1f;

            foreach (CellMover cellMover in dropdownCells)
                cellMover.SetCellPosition(cellMover.CellPosition - Vector2.down, moveDuration);
        }
    }

    private void DestroyCells(WordAtBoard word)
    {
        IEnumerable<Cell> cells = word.WordPosition;
        List<Cell> newCells = _board.ReplaceCells(cells).ToList();
        IEnumerable<Vector2> positionsCellsForDeleting = cells.Select(o => new Vector2(o.XPosition, o.YPosition)).ToList();
        List<CellMover> cellsForDeleting = new();

        foreach (var cell in _cells)
        {
            if (positionsCellsForDeleting.Any(o => o == cell.CellPosition))
                cellsForDeleting.Add(cell);
        }

        Vector3 position = cellsForDeleting.First().transform.localPosition;
        WordFound?.Invoke(word.Word, _comboMultiplier, position);
        UpdateAffectedCells(cells);

        for (int i = 0; i < cellsForDeleting.Count(); i++)
            cellsForDeleting[i].Init(newCells[i], transform);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardInitializer : MonoBehaviour
{
    [SerializeField] private CellMover _cellPrefab;

    private List<CellMover> _cells = new List<CellMover>();
    private Board _board;

    public event Action<string> WordFound;

    private void Awake()
    {
        Letters letters = new Letters();
        NounDictionary nouns = new NounDictionary();
        MatchFinder finder = new MatchFinder(nouns.Nouns.Keys);
        _board = new Board(letters, finder);
        Init();
    }

    private void Init()
    {
        foreach (var cell in _board.Cells())
        {
            CellMover cellMover = Instantiate(_cellPrefab, transform);
            cellMover.Init(cell, transform);
            _cells.Add(cellMover);
        }
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

        DestroyCells(bestResult.WordPosition);
        WordFound?.Invoke(bestResult.Word);
        StartCoroutine(ScanBoard());
    }

    private IEnumerator ScanBoard()
    {
        float delaySeconds = 1;
        WaitForSeconds delay = new WaitForSeconds(delaySeconds);
        Vector2[] cells = _cells.Select(o => o.CellPosition).ToArray();

        while (true)
        {
            WordAtBoard bestResult = _board.FindWord(cells);

            if (bestResult == null)
                yield break;

            DestroyCells(bestResult.WordPosition);
            WordFound?.Invoke(bestResult.Word);
            yield return delay;
        }
    }

    private void UpdateAffectedCells(IEnumerable<Cell> cells)
    {
        List<Cell> hz = new List<Cell>(cells);
        hz = hz.OrderBy(o => o.YPosition).ToList();

        foreach (Cell cell in hz)
        {
            var dropdownCells = _cells.Where(o => o.CellPosition.y < cell.YPosition && o.CellPosition.x == cell.XPosition).OrderBy(o => o.CellPosition.y).ToList();

            foreach (CellMover cellMover in dropdownCells)
                cellMover.SetCellPosition(cellMover.CellPosition - Vector2.down, 1);
        }
    }

    private void DestroyCells(IEnumerable<Cell> cells)
    {
        List<Cell> newCells = _board.ReplaceCells(cells).ToList();
        IEnumerable<Vector2> positionsCellsForDeleting = cells.Select(o => new Vector2(o.XPosition, o.YPosition)).ToList();
        List<CellMover> cellsForDeleting = new List<CellMover>();

        foreach (var cell in _cells)
        {
            if (positionsCellsForDeleting.Any(o => o == cell.CellPosition))
                cellsForDeleting.Add(cell);
        }

        UpdateAffectedCells(cells);

        for (int i = 0; i < cellsForDeleting.Count(); i++)
            cellsForDeleting[i].Init(newCells[i], transform);
    }
}

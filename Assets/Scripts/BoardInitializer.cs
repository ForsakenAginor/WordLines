using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoardInitializer : MonoBehaviour
{
    [SerializeField] private CellMover _cellPrefab;

    private List<CellMover> _cells = new List<CellMover>();
    private Board _board;

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
        foreach (var cell in _board.GetCells())
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
        IEnumerable<Cell> firstCells;
        string firstWord;
        _board.GetMatchedWord(first, out firstWord, out firstCells);
        IEnumerable<Cell> secondCells;
        string secondWord;
        _board.GetMatchedWord(second, out secondWord, out secondCells);

        if (string.IsNullOrEmpty(firstWord) && string.IsNullOrEmpty(secondWord))
            return;

        if (firstWord.Length > secondWord.Length)
            DestroyCells(firstCells);
        else
            DestroyCells(secondCells);
    }

    private void DestroyCells(IEnumerable<Cell> cells)
    {
        IEnumerable<Vector2> positionsCellsForDeleting = cells.Select(o => new Vector2(o.XPosition, o.YPosition));
        List<CellMover> cellsForDeleting = new List<CellMover>();

        foreach (var cell in _cells)
        {
            if (positionsCellsForDeleting.Any(o => o == cell.CellPosition))
                cellsForDeleting.Add(cell);
        }

        for (int i = 0; i < cellsForDeleting.Count(); i++)
        {
            _cells.Remove(cellsForDeleting[i]);
            Destroy(cellsForDeleting[i].gameObject);
        }
    }
}

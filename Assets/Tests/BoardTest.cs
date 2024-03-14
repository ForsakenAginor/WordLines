using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.TestTools;

public class BoardTest
{
    [Test]
    public void ConstructorTest()
    {
        BoardFake board = new BoardFake();

        var firstRow = board.Cells.Where(o => o.YPosition == 0).ToList();
        board.ReplaceCells(firstRow);
        string result = "И Й К \n" +
                        "Г Д Е \n" +
                        "Ё Ж З \n";

        Assert.AreEqual(result, board.ShowBoard());
    }

    [Test]
    public void ConstructorTest1()
    {
        BoardFake board = new BoardFake();

        var secondRow = board.Cells.Where(o => o.YPosition == 1).ToList();
        board.ReplaceCells(secondRow);
        string result = "И Й К \n" +
                        "А Б В \n" +
                        "Ё Ж З \n";

        Assert.AreEqual(result, board.ShowBoard());
    }

    [Test]
    public void ConstructorTest2()
    {
        BoardFake board = new BoardFake();

        var list0 = board.Cells.Where(o => o.XPosition == 0 && o.YPosition > 0).ToList();
        board.ReplaceCells(list0);
        string result = "Й Б В \n" +
                        "И Д Е \n" +
                        "А Ж З \n";

        Assert.AreEqual(result, board.ShowBoard());
    }

    private class BoardFake
    {
        private List<Cell> _cells = new List<Cell>();
        private int _iterator = 0;
        private List<char> _chars = new List<char>();

        public BoardFake()
        {
            Letters letters = new Letters();
            _chars = letters.LetterValuePairs.Keys.ToList();

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    _cells.Add(new Cell(x, y, _chars[_iterator]));
                    _iterator++;
                }
            }
        }

        public List<Cell> Cells => _cells;

        public string ShowBoard()
        {
            string result = string.Empty;

            for (int i = 0; i < 3; i++)
            {
                var row = _cells.Where(o => o.YPosition == i).OrderBy(o => o.XPosition);

                foreach (var cell in row)
                    result += $"{cell.Content} ";

                result += "\n";
            }

            return result;
        }

        public IEnumerable<Cell> ReplaceCells(IEnumerable<Cell> cellsForDeleting)
        {
            cellsForDeleting = cellsForDeleting.OrderBy(o => o.YPosition);
            List<Cell> newCells = new List<Cell>();

            foreach (Cell cell in cellsForDeleting)
            {
                var dropdownCells = _cells.Where(o => o.YPosition < cell.YPosition && o.XPosition == cell.XPosition).ToList();

                foreach (Cell dropdownCell in dropdownCells)
                    dropdownCell.MoveDown();

                Cell newCell = new Cell(cell.XPosition, 0, _chars[_iterator]);
                _iterator++;
                _cells.Add(newCell);
                newCells.Add(newCell);
                _cells.Remove(cell);
            }

            return newCells;
        }
    }
}


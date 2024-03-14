using UnityEngine;

public class Cell 
{
    private int _xPosition;
    private int _yPosition;
    private char _content;

    public Cell(int xPosition, int yPosition, char content)
    {
        _xPosition = xPosition;
        _yPosition = yPosition;
        _content = content;
    }

    public int XPosition => _xPosition;
    public int YPosition => _yPosition;
    public char Content => _content;

    public void ChangeContent(char content)
    {
        _content = content;
    }

    public void MoveDown()
    {
        _yPosition++;
    }

    public void MoveToTop()
    {
        _yPosition = 0;
    }

    public void Move(Vector2 direction)
    {
        _xPosition += (int)direction.x;
        _yPosition += (int)direction.y;
    }
}

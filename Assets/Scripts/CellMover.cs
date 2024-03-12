using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CellMover : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Vector2 _cellPosition;
    private Transform _draggingCanvas;
    private Transform _parent;

    public event UnityAction<Vector2, Vector2> CellsSwapped;

    public Vector2 CellPosition => _cellPosition;

    public void Init(Cell cell, Transform parent)
    {
        _draggingCanvas = GetComponentInParent<Canvas>().transform;

        CellView view = GetComponent<CellView>();
        view.Init(cell.Content);
        SetParent(parent);
        SetCellPosition(new Vector2(cell.XPosition, cell.YPosition));
    }
    public void Init()
    {
        _draggingCanvas = GetComponentInParent<Canvas>().transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(_draggingCanvas);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);
        var cellViewes = result.Where(o => o.gameObject != gameObject && o.gameObject.TryGetComponent<CellView>(out _));

        if (cellViewes.Count() > 0)
        {
            cellViewes.First().gameObject.TryGetComponent<CellMover>(out CellMover target);

            if (target.CanAcceptDrop(this))
            {
                transform.SetParent(_parent);
                Vector2 tempVector = _cellPosition;
                SetCellPosition(target.CellPosition);
                target.SetCellPosition(tempVector);
                CellsSwapped?.Invoke(target.CellPosition, CellPosition);
                return;
            }
        }

        transform.SetParent(_parent);
        SetCellPosition(_cellPosition);
    }

    public void SetParent(Transform parent)
    {
        _parent = parent;
    }

    public void SetCellPosition(Vector2 position)
    {
        int leftBorder = (BoardConfig.CellSize - BoardConfig.Width * BoardConfig.CellSize) / 2;
        int topBorder = (BoardConfig.Height * BoardConfig.CellSize - BoardConfig.CellSize) / 2;
        _cellPosition = position;
        gameObject.transform.localPosition = new Vector3(leftBorder + position.x * BoardConfig.CellSize, topBorder - position.y * BoardConfig.CellSize);
        gameObject.name = $"Cell ({position.x}, {position.y})";
    }

    public bool CanAcceptDrop(CellMover drop)
    {
        float maxDragDistance = 1;
        return Vector2.SqrMagnitude(drop.CellPosition - _cellPosition) == maxDragDistance;
    }
}

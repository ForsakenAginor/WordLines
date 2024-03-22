using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CellMover : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private const int LeftBorder = (CellConfig.CellSize - BoardConfig.Width * CellConfig.CellSize) / 2;
    private const int TopBorder = (BoardConfig.Height * CellConfig.CellSize - CellConfig.CellSize) / 2;

    private Vector2 _cellPosition;
    private Transform _draggingCanvas;
    private Transform _parent;
    private Tweener _tweener;

    public event UnityAction<Vector2, Vector2> CellsSwapped;

    public Vector2 CellPosition => _cellPosition;

    public void Init(Cell cell, Transform parent)
    {
        _draggingCanvas = GetComponentInParent<Canvas>().transform;
        CellView view = GetComponent<CellView>();
        view.Init(cell.Content);
        SetParent(parent);
        transform.localPosition = new Vector3(LeftBorder + cell.XPosition * CellConfig.CellSize, TopBorder );
        _tweener = transform.DOLocalMove(transform.position, 0).SetEase(Ease.Linear).SetAutoKill(false);
        float moveDuration = 1f;
        SetCellPosition(new Vector2(cell.XPosition, cell.YPosition), moveDuration);
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
        float swapTime = 0.5f;
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
                SetCellPosition(target.CellPosition, swapTime);
                target.SetCellPosition(tempVector, swapTime);
                CellsSwapped?.Invoke(target.CellPosition, CellPosition);
                return;
            }
        }

        transform.SetParent(_parent);
        SetCellPosition(_cellPosition, swapTime);
    }

    public void SetParent(Transform parent)
    {
        _parent = parent;
    }

    public void SetCellPosition(Vector2 position, float duration)
    {
        _cellPosition = position;
        _tweener.ChangeValues(transform.localPosition, new Vector3(LeftBorder + position.x * CellConfig.CellSize, TopBorder - position.y * CellConfig.CellSize), duration).Restart();
        gameObject.name = $"Cell ({position.x}, {position.y})";
    }

    public bool CanAcceptDrop(CellMover drop)
    {
        float maxDragDistance = 1;
        return Vector2.SqrMagnitude(drop.CellPosition - _cellPosition) == maxDragDistance;
    }
}

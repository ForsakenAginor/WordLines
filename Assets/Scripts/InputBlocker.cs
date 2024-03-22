using System;
using UnityEngine;
using UnityEngine.UI;

public class InputBlocker : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster _graphicRaycaster;

    private BoardManager _board;

    public void Init(BoardManager board)
    {
        _board = board != null ? board : throw new ArgumentNullException(nameof(board));
        OnEnable();
    }

    private void OnEnable()
    {
        _board.ChainScanStarted += OnChainScanStarted;
        _board.ChainScanFinished += OnChainScanFinished;
    }

    private void OnDisable()
    {
        _board.ChainScanStarted -= OnChainScanStarted;
        _board.ChainScanFinished -= OnChainScanFinished;
    }

    private void OnChainScanFinished()
    {
        _graphicRaycaster.enabled = true;
    }

    private void OnChainScanStarted()
    {
        _graphicRaycaster.enabled = false;
    }
}


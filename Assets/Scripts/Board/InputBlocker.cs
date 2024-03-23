using System;
using UnityEngine.UI;

public class InputBlocker
{
    private readonly GraphicRaycaster _graphicRaycaster;
    private readonly BoardManager _board;

    public InputBlocker(BoardManager board, GraphicRaycaster graphicRaycaster)
    {
        _board = board != null ? board : throw new ArgumentNullException(nameof(board));
        _graphicRaycaster = graphicRaycaster != null ? graphicRaycaster : throw new ArgumentNullException(nameof(graphicRaycaster));

        _board.ChainScanStarted += OnChainScanStarted;
        _board.ChainScanFinished += OnChainScanFinished;
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


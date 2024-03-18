using System;
using UnityEngine;
using UnityEngine.UI;

public class InputBlocker : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster _graphicRaycaster;

    private BoardInitializer _board;

    public void Init(BoardInitializer board)
    {
        _board = board ?? throw new ArgumentNullException(nameof(board));
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


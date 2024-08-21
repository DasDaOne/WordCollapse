using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectionVisuals : MonoBehaviour
{
    [SerializeField] private GameFieldGrid gameFieldGrid;
    [SerializeField] private SelectionVisualPiece selectionVisualPiecePrefab;

    private SelectionVisualPiece[] visualPieces;

    private RectTransform rectTransform;
    private RectTransform canvasRectTransform;

    private float CanvasScaleFactor => canvasRectTransform.localScale.x;
    private Vector2 Offset => rectTransform.GetScreenRect().min / CanvasScaleFactor;
    
    private void Awake()
    {
        rectTransform = transform as RectTransform;
        canvasRectTransform = GetComponentInParent<Canvas>().transform as RectTransform;
    }

    public void Initialize()
    {
        visualPieces = new SelectionVisualPiece[GameConstants.MaxWordLength];
        
        for (int i = 0; i < GameConstants.MaxWordLength; i++)
        {
            visualPieces[i] = Instantiate(selectionVisualPiecePrefab, transform);
            visualPieces[i].Initialize(gameFieldGrid.CellSize);
            visualPieces[i].SetState(false);
        }
    }

    public void UpdateVisuals(List<LetterCell> letterCells)
    {
        foreach (var visualPiece in visualPieces)
            visualPiece.SetState(false);

        if (letterCells == null)
            return;

        if (letterCells.Count == 1)
        {
            visualPieces[0].SetState(true);
            visualPieces[0].SetPoints(
                gameFieldGrid.GridPointToScreen(letterCells[0].index) / CanvasScaleFactor - Offset,
                gameFieldGrid.GridPointToScreen(letterCells[0].index) / CanvasScaleFactor- Offset);
            
            return;
        }

        for (int i = 0; i < letterCells.Count - 1; i++)
        {
            visualPieces[i].SetState(true);
            visualPieces[i].SetPoints(
                gameFieldGrid.GridPointToScreen(letterCells[i].index) / CanvasScaleFactor- Offset,
                gameFieldGrid.GridPointToScreen(letterCells[i + 1].index) / CanvasScaleFactor- Offset);
        }
    }
}

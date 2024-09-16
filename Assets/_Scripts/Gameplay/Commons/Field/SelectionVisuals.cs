using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectionVisuals : MonoBehaviour
{
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

        visualPieces = new SelectionVisualPiece[GameConstants.MaxWordLength];

        for (int i = 0; i < GameConstants.MaxWordLength; i++)
        {
            visualPieces[i] = Instantiate(selectionVisualPiecePrefab, transform);
        }
    }

    public void Initialize(float gridCellSize)
    {
        foreach (var selectionVisualPiece in visualPieces)
        {
            selectionVisualPiece.Initialize(gridCellSize);
            selectionVisualPiece.SetState(false);
        }
    }

    public void UpdateVisuals(List<Vector2> positions)
    {
        foreach (var visualPiece in visualPieces)
            visualPiece.SetState(false);

        if (positions == null)
            return;

        if (positions.Count == 1)
        {
            visualPieces[0].SetState(true);
            visualPieces[0].SetPoints(positions[0] - Offset, positions[0] - Offset);
            return;
        }

        for (int i = 0; i < positions.Count - 1; i++)
        {
            visualPieces[i].SetState(true);
            visualPieces[i].SetPoints(positions[i] - Offset, positions[i + 1] - Offset);
        }
    }
}

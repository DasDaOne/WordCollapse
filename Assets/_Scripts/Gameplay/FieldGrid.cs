using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class FieldGrid : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
	[SerializeField] private GameFieldGridController gameFieldGridController;
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private LetterCell letterCellPrefab;
    [SerializeField] private WordBuilder wordBuilder;
    
    private LetterCell[,] letterGrid;

    private bool isSelecting;
    
    private void Start()
    {
	    gameFieldGridController.RecalculateGridSize(gridSize);
	    
	    SpawnGrid();
    }

    private void SpawnGrid()
    {
	    letterGrid = new LetterCell[gridSize.x, gridSize.y];

	    for (int x = 0; x < gridSize.x; x++)
	    {
		    for (int y = 0; y < gridSize.y; y++)
		    {
			    letterGrid[x, y] = Instantiate(letterCellPrefab, transform);
			    letterGrid[x, y].index = new Vector2Int(y, x);
		    }
	    }
    }

    private LetterCell GetFieldLetterCell(Vector2Int pos)
    {
	    if (pos.x < 0 || pos.x >= gridSize.y ||
	        pos.y < 0 || pos.y >= gridSize.x)
		    return null;

	    return letterGrid[pos.y, pos.x];
    }

    public void OnPointerDown(PointerEventData eventData)
    {
	    isSelecting = true;
	    
	    wordBuilder.StartSelection(GetFieldLetterCell(gameFieldGridController.ScreenPointToGrid(eventData.position)));
    }

    public void OnPointerMove(PointerEventData eventData)
    {
	    if(!isSelecting)
		    return;

	    if(wordBuilder.LastLetterCell == GetFieldLetterCell(gameFieldGridController.ScreenPointToGrid(eventData.position)))
		    return;

	    Vector2Int lastCellIndex = wordBuilder.LastLetterCell.index;
	    Vector2 delta = eventData.position - gameFieldGridController.GridPointToScreen(lastCellIndex);
	    
	    float angle = Mathf.RoundToInt(Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg / 45f) * 45f;

	    Vector2Int offset = Vector2Int.RoundToInt(Quaternion.Euler(0, 0, angle) * Vector3.right);;
	    offset.y *= -1;
	    
	    wordBuilder.UpdateSelection(GetFieldLetterCell(lastCellIndex + offset));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
	    isSelecting = false;
	    
	    wordBuilder.EndSelection();
    }
}
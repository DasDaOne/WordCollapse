using System.Collections.Generic;
using UnityEngine;

public class GameFieldGrid : MonoBehaviour
{
	[SerializeField] private LetterCell letterCellPrefab;

	private LetterCell[,] Grid { get; set; }

    private RectTransform canvasRectTransform;
    private RectTransform thisRectTransform;

    private Rect RectTransformRect => thisRectTransform.GetScreenRect();
    private Vector2 GridStartPos => RectTransformRect.min / CanvasScaleFactor;
    public float CellSize { get; private set; }
    public float ScreenCellSize => CellSize * CanvasScaleFactor;

    private float CanvasScaleFactor => canvasRectTransform.localScale.x;

    private void Awake()
    {
	    canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
	    thisRectTransform = transform as RectTransform;
    }

    public void CreateGrid(Vector2Int gridSize)
    {
	    Grid = new LetterCell[gridSize.x, gridSize.y];
	    
	    CellSize = Mathf.Min(thisRectTransform!.rect.width, thisRectTransform.rect.height) /
	                     Mathf.Min(gridSize.x, gridSize.y);
	    
	    for (int x = 0; x < gridSize.x; x++)
	    {
		    for (int y = 0; y < gridSize.y; y++)
		    {
			    Grid[x, y] = Instantiate(letterCellPrefab, transform);
			    Grid[x, y].Init(new Vector2Int(x, y));

			    RectTransform rt = Grid[x, y].transform as RectTransform;
			    
			    rt!.anchoredPosition = GetCellAnchorPos(new Vector2(x, y));
			    rt.sizeDelta = Vector2.one * CellSize;
		    }
	    }
    }

    private Vector2 GetCellAnchorPos(Vector2 pos) => pos * CellSize;

    public void DeleteCells(List<LetterCell> cellsToDelete)
    {
	    foreach (var cell in cellsToDelete)
	    {
		    cell.DestroyCell();
		    Grid[cell.Index.x, cell.Index.y] = null;
	    }
	    
	    RegenerateField();
    }

    private void RegenerateField()
    {
	    for (int x = 0; x < Grid.GetLength(0); x++)
	    {
		    for (int y = 0; y < Grid.GetLength(1); y++)
		    {
			    if(Grid[x, y] == null)
				    continue;
			    
			    CheckCellFalling(new Vector2Int(x, y));
		    }
	    }
    }

    private void CheckCellFalling(Vector2Int pos)
    {
	    if(pos.y <= 0)
		    return;
	    
	    if(Grid[pos.x, pos.y - 1] != null)
		    return;

	    int firstNeighbourIndex = 0;
	    
	    for (int i = pos.y; i > 0; i--)
	    {
		    if(Grid[pos.x, i - 1] == null)
			    continue;

		    firstNeighbourIndex = i;
		    break;
	    }

	    Vector2Int newPos = new (pos.x, firstNeighbourIndex);

	    Grid[pos.x, pos.y].Fall(GetCellAnchorPos(newPos), newPos);
	    Grid[newPos.x, newPos.y] = Grid[pos.x, pos.y];
	    Grid[pos.x, pos.y] = null;
    }

    public LetterCell GetLetterCell(Vector2Int gridPos)
    {
	    if (gridPos.x < 0 || gridPos.x >= Grid.GetLength(0) ||
	        gridPos.y < 0 || gridPos.y >= Grid.GetLength(1))
		    return null;

	    return Grid[gridPos.x, gridPos.y];
    }

    public LetterCell GetLetterCellFromScreen(Vector2 screenPos)
    {
	    screenPos /= CanvasScaleFactor;
	    
	    Vector2 relativePos = new (
		    screenPos.x - GridStartPos.x,
		    screenPos.y - GridStartPos.y);

	    return GetLetterCell(Vector2Int.FloorToInt(new Vector2(
		    relativePos.x / CellSize,
		    relativePos.y / CellSize)));
    }
    
    public Vector2 GridPointToScreen(Vector2Int gridPoint)
    {
	    return (GridStartPos + (Vector2)gridPoint * CellSize + Vector2.one * CellSize / 2) * CanvasScaleFactor;
    }
}

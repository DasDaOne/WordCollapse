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
			    Grid[x, y].index = new Vector2Int(x, y);

			    RectTransform rt = Grid[x, y].transform as RectTransform;
			    
			    rt!.anchoredPosition = new Vector2(x, y) * CellSize;
			    rt.sizeDelta = Vector2.one * CellSize;
		    }
	    }
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

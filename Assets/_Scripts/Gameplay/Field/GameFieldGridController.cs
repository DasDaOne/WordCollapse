using UnityEngine;
using UnityEngine.UI;

public class GameFieldGridController : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayoutGroup;

    private RectTransform canvasRectTransform;
    private RectTransform thisRectTransform;

    private Rect RectTransformRect => thisRectTransform.GetScreenRect();
    private Vector2 GridStartPos => new Vector2(RectTransformRect.xMin, RectTransformRect.yMax) / CanvasScaleFactor;
    public Vector2 CellSize => gridLayoutGroup.cellSize;
    
    private float CanvasScaleFactor => canvasRectTransform.localScale.x;

    private void Awake()
    {
	    canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
	    thisRectTransform = transform as RectTransform;
    }

    public void RecalculateGridSize(Vector2Int gridSize)
    {
	    RectTransform rectTransform = transform as RectTransform;

	    float cellSize = Mathf.Min(rectTransform!.rect.width, rectTransform.rect.height) /
	                     Mathf.Min(gridSize.x, gridSize.y);

	    gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
    }
    
    public Vector2Int ScreenPointToGrid(Vector2 screenPoint)
    {
	    screenPoint /= CanvasScaleFactor;
	    
	    Vector2 relativePos = new (
		    screenPoint.x - GridStartPos.x,
		    GridStartPos.y - screenPoint.y);

	    return Vector2Int.FloorToInt(new Vector2(
		    relativePos.x / CellSize.x,
		    relativePos.y / CellSize.y));
    }
    
    public Vector2 GridPointToScreen(Vector2Int gridPoint)
    {
	    return (GridStartPos + 
	            new Vector2(gridPoint.x * CellSize.x + CellSize.x / 2, -gridPoint.y * CellSize.y - CellSize.y / 2)) * CanvasScaleFactor;
    }
}

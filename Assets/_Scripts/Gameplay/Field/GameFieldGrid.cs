using System.Collections.Generic;
using UnityEngine;

public class GameFieldGrid : MonoBehaviour
{
	[SerializeField] private LetterCell letterCellPrefab;

	private LetterCell[,] Grid { get; set; }
	private Level.CellType[,] levelArrangement;

    private RectTransform canvasRectTransform;
    private RectTransform thisRectTransform;

    private Vector2 startPosOffset;
    
    private Rect RectTransformRect => thisRectTransform.GetScreenRect();
    private Vector2 GridStartPos => RectTransformRect.min / CanvasScaleFactor + startPosOffset;
    public float CellSize { get; private set; }
    public float ScreenCellSize => CellSize * CanvasScaleFactor;
    
    private float CanvasScaleFactor => canvasRectTransform.localScale.x;

    private void Awake()
    {
	    canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
	    thisRectTransform = transform as RectTransform;
    }

    public void CreateGrid(Level level)
    {
	    levelArrangement = level.LevelArrangement;
	    
	    int sizeX = levelArrangement.GetLength(0);
	    int sizeY = levelArrangement.GetLength(1);
	    
	    Grid = new LetterCell[sizeX, sizeY];
	    
	    CellSize = Mathf.Min(thisRectTransform!.rect.width, thisRectTransform.rect.height) /
	                     Mathf.Max(sizeX, sizeY);

	    if (sizeY > sizeX)
		    startPosOffset = new Vector2((sizeY - sizeX) * CellSize / 2, 0);
	    else if (sizeX > sizeY)
		    startPosOffset = new Vector2(0, (sizeX - sizeY) * CellSize / 2);
	    
	    for (int x = 0; x < sizeX; x++)
	    {
		    for (int y = 0; y < sizeY; y++)
		    {
			    Grid[x, y] = InstantiateNewCell(levelArrangement[x, y], new Vector2Int(x, y));
		    }
	    }
    }

    private LetterCell InstantiateNewCell(Level.CellType cellType, Vector2Int pos)
    {
	    if (cellType == Level.CellType.Empty)
		    return null;
	    
	    LetterCell cell = Instantiate(letterCellPrefab, transform);
	    
	    cell.Init(pos);
	    
	    RectTransform rt = cell.transform as RectTransform;

	    rt!.anchoredPosition = GetCellAnchorPos(pos);
	    rt.sizeDelta = Vector2.one * CellSize;

	    return cell;
    }

    private Vector2 GetCellAnchorPos(Vector2 pos) => pos * CellSize + startPosOffset;

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

	    for (int x = 0; x < Grid.GetLength(0); x++)
	    {
		    for (int y = 0; y < Grid.GetLength(1); y++)
		    {
			    if(Grid[x, y] != null || levelArrangement[x, y] == Level.CellType.Empty)
				    continue;

			    LetterCell cell = InstantiateNewCell(Level.CellType.Default,new Vector2Int(x, y + Grid.GetLength(1)));

			    cell.Fall(GetCellAnchorPos(new Vector2(x, y)), new Vector2Int(x, y));
			    
			    Grid[x, y] = cell;
		    }   
	    }
    }

    private void CheckCellFalling(Vector2Int pos)
    {
	    if(pos.y <= 0)
		    return;
	    
	    if(Grid[pos.x, pos.y - 1] != null)
		    return;

	    int floor = -1;

	    for (int y = 0; y < pos.y; y++)
	    {
		    LetterCell cell = Grid[pos.x, y];
		    
		    if(cell != null || levelArrangement[pos.x, y] == Level.CellType.Empty)
			    continue;

		    floor = y;
		    break;
	    }
	    
	    if(floor == -1)
		    return;

	    Vector2Int newPos = new (pos.x, floor);

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

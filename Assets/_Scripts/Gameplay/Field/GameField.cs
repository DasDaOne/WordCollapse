using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class GameField : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
	[SerializeField] private GameFieldGrid gameFieldGrid;
	[SerializeField] private TextAsset levelAsset;
    [SerializeField] private WordBuilder wordBuilder;
    [SerializeField] private SelectionVisuals selectionVisuals;

    private bool isSelecting;
    
    private void Start()
    {
	    gameFieldGrid.CreateGrid(LevelParser.ParseLevel(levelAsset.text));
	    
	    selectionVisuals.Initialize();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
	    isSelecting = true;
	    
	    wordBuilder.StartSelection(gameFieldGrid.GetLetterCellFromScreen(eventData.position));
    }

    public void OnPointerMove(PointerEventData eventData)
    {
	    if(!isSelecting)
		    return;

	    if(wordBuilder.LastLetterCell == gameFieldGrid.GetLetterCellFromScreen(eventData.position))
		    return;

	    Vector2Int lastCellIndex = wordBuilder.LastLetterCell.Index;
	    Vector2 delta = eventData.position - gameFieldGrid.GridPointToScreen(lastCellIndex);
	    
	    if(delta.magnitude < gameFieldGrid.ScreenCellSize * 0.75f)
		    return;

	    float angle = Mathf.RoundToInt(Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg / 45f) * 45f;

	    Vector2Int offset = Vector2Int.RoundToInt(Quaternion.Euler(0, 0, angle) * Vector3.right);;
	    
	    wordBuilder.UpdateSelection(gameFieldGrid.GetLetterCell(lastCellIndex + offset));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
	    isSelecting = false;
	    
	    List<LetterCell> selectedWord = wordBuilder.EndSelection();

	    if(selectedWord == null)
		    OnInvalidWordSelected();
	    else
		    OnValidWordSelected(selectedWord);
    }

    private void OnValidWordSelected(List<LetterCell> word)
    {
	    gameFieldGrid.DeleteCells(word);
	    Debug.Log("Valid Word!!!!");
    }

    private void OnInvalidWordSelected()
    {
	    Debug.Log("Invalid Word :((((");
    }
}
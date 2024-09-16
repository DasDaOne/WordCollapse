using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class GameplaySelection : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
	[SerializeField] private TMP_Text wordVisualsText;

    private bool isSelecting;

    private List<LetterCell> currentSelection;
    private LetterCell LastSelectedLetter =>
	    currentSelection == null || currentSelection.Count == 0 ? null : currentSelection[^1];
    private string SelectionToString(List<LetterCell> word) => word != null ? string.Join("", word.Select(x => x.StoredLetter)) : "";

    
    private WordDatabase wordDatabase;
    private SelectionVisuals selectionVisuals;
    private GameplayGrid gameplayGrid;

    public void Initialize(GameplayGrid grid, WordDatabase database, SelectionVisuals visuals)
    {
	    gameplayGrid = grid;
	    wordDatabase = database;
	    selectionVisuals = visuals;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
	    LetterCell selectedLetterCell = gameplayGrid.GetLetterCellFromScreen(eventData.position);

	    if (!selectedLetterCell)
		    return;
	    
	    isSelecting = true;

	    currentSelection = new List<LetterCell> {selectedLetterCell};
        
	    selectedLetterCell.OnSelectionTrigger();

	    UpdateWordVisuals();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
	    if(!isSelecting)
		    return;

	    if(LastSelectedLetter == gameplayGrid.GetLetterCellFromScreen(eventData.position))
		    return;

	    Vector2Int lastCellIndex = LastSelectedLetter.Index;
	    Vector2 delta = eventData.position - gameplayGrid.GridPointToScreen(lastCellIndex);
	    
	    if(delta.magnitude < gameplayGrid.ScreenCellSize * 0.75f)
		    return;

	    float angle = Mathf.RoundToInt(Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg / 45f) * 45f;

	    Vector2Int offset = Vector2Int.RoundToInt(Quaternion.Euler(0, 0, angle) * Vector3.right);;

	    LetterCell selectedLetterCell = gameplayGrid.GetLetterCell(lastCellIndex + offset);
	    
	    if (selectedLetterCell == null)
		    return;
        
	    if (currentSelection.Contains(selectedLetterCell))
	    {
		    if(currentSelection.Count <= 1 || currentSelection[^2] != selectedLetterCell)
			    return;
            
		    currentSelection.RemoveAt(currentSelection.Count - 1);
		    selectedLetterCell.OnSelectionTrigger();
		    UpdateWordVisuals();
		    return;
	    }

	    if (currentSelection.Count >= GameConstants.MaxWordLength)
		    return;

	    currentSelection.Add(selectedLetterCell);
	    selectedLetterCell.OnSelectionTrigger();
	    UpdateWordVisuals();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
	    if(currentSelection == null || currentSelection.Count == 0)
		    return;
	    
	    isSelecting = false;
	    
	    List<LetterCell> selectedWord = currentSelection;

	    currentSelection = null;
	    UpdateWordVisuals();

	    if (selectedWord.Count < GameConstants.MinWordLength)
	    {
		    OnInvalidWordSelected();
		    return;
	    }
	    
	    if(wordDatabase.IsWordValid(SelectionToString(selectedWord)))
		    OnValidWordSelected(selectedWord);
    }

    private void OnValidWordSelected(List<LetterCell> word)
    {
	    gameplayGrid.DeleteCells(word);
    }

    private void OnInvalidWordSelected()
    {
    }
    
    private void UpdateWordVisuals()
    {
	    wordVisualsText.text = SelectionToString(currentSelection);

	    selectionVisuals.UpdateVisuals(currentSelection?.Select(x => gameplayGrid.GridPointToCanvas(x.Index)).ToList());
    }
}
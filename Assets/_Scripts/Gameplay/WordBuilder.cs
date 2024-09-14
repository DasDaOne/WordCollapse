using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WordBuilder : MonoBehaviour
{
    [SerializeField] private TMP_Text wordText;
    [SerializeField] private SelectionVisuals selectionVisuals;
    [SerializeField] private WordDatabase wordDatabase;
    [Space]
    [SerializeField] private bool debugAllWordsValid;

    private Vector2Int currentDragPos;

    private List<LetterCell> currentSelection;

    public LetterCell LastLetterCell
    {
        get
        {
            if (currentSelection == null || currentSelection.Count == 0)
                return null;

            return currentSelection[^1];
        }
    }

    private void Awake()
    {
        wordDatabase.Initialize();
    }

    public void StartSelection(LetterCell letterCell)
    {
        currentSelection = new List<LetterCell> {letterCell};
        
        letterCell.OnSelectionTrigger();

        UpdateWordVisuals();
    }
    
    public void UpdateSelection(LetterCell letterCell)
    {
        if (letterCell == null)
            return;
        
        if (currentSelection.Contains(letterCell))
        {
            if(currentSelection.Count <= 1 || currentSelection[^2] != letterCell)
                return;
            
            currentSelection.RemoveAt(currentSelection.Count - 1);
           
            letterCell.OnSelectionTrigger();
            
            UpdateWordVisuals();
            return;
        }

        if (currentSelection.Count >= GameConstants.MaxWordLength)
            return;

        currentSelection.Add(letterCell);
        
        letterCell.OnSelectionTrigger();

        UpdateWordVisuals();
    }
    
    public List<LetterCell> EndSelection()
    {
        List<LetterCell> selectedWord = currentSelection;

        ResetSelection();
        
        if(selectedWord.Count < GameConstants.MinWordLength)
            return null;

        if (debugAllWordsValid)
            return selectedWord;
        
        return wordDatabase.IsWordValid(SelectionToString(selectedWord)) ? selectedWord : null;
    }

    private void ResetSelection()
    {
        currentSelection = null;
        UpdateWordVisuals();
    }

    private string SelectionToString(List<LetterCell> word) => word != null ? string.Join("", word.Select(x => x.StoredLetter)) : "";
    

    private void UpdateWordVisuals()
    {
        wordText.text = SelectionToString(currentSelection);
        selectionVisuals.UpdateVisuals(currentSelection);
    }
}

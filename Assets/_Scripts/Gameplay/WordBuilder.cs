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

    private Vector2Int currentDragPos;

    private List<LetterCell> builtWord;
    private string BuiltWordString => builtWord != null ? string.Join("", builtWord.Select(x => x.StoredLetter)) : "";

    public LetterCell LastLetterCell
    {
        get
        {
            if (builtWord == null || builtWord.Count == 0)
                return null;

            return builtWord[^1];
        }
    }

    private void Awake()
    {
        wordDatabase.Initialize();
    }

    public void StartSelection(LetterCell letterCell)
    {
        builtWord = new List<LetterCell> {letterCell};
        
        letterCell.OnSelectionTrigger();

        UpdateWordVisuals();
    }
    
    public void UpdateSelection(LetterCell letterCell)
    {
        if (letterCell == null)
            return;
        
        if (builtWord.Contains(letterCell))
        {
            if(builtWord.Count <= 1 || builtWord[^2] != letterCell)
                return;
            
            builtWord.RemoveAt(builtWord.Count - 1);
           
            letterCell.OnSelectionTrigger();
            
            UpdateWordVisuals();
            return;
        }

        if (builtWord.Count >= GameConstants.MaxWordLength)
            return;

        builtWord.Add(letterCell);
        
        letterCell.OnSelectionTrigger();

        UpdateWordVisuals();
    }
    
    public void EndSelection()
    {
        string word = BuiltWordString;
        
        Debug.Log($"word {word} is {(wordDatabase.IsWordValid(word) ? "valid" : "invalid")}");

        builtWord = null;
        
        UpdateWordVisuals();
    }

    private void UpdateWordVisuals()
    {
        wordText.text = BuiltWordString;
        selectionVisuals.UpdateVisuals(builtWord);
    }
}

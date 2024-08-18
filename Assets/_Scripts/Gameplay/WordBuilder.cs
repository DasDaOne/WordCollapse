using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WordBuilder : MonoBehaviour
{
    [SerializeField] private TMP_Text wordText;
    
    private Vector2Int currentDragPos;

    private List<LetterCell> builtWord;

    public LetterCell LastLetterCell
    {
        get
        {
            if (builtWord == null || builtWord.Count == 0)
                return null;

            return builtWord[^1];
        }
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

        builtWord.Add(letterCell);
        
        letterCell.OnSelectionTrigger();

        UpdateWordVisuals();
    }
    
    public void EndSelection()
    {
        builtWord = null;
        
        UpdateWordVisuals();
    }

    private void UpdateWordVisuals()
    {
        if (builtWord == null)
        {
            wordText.text = "";
            return;
        }
        
        wordText.text = string.Join("", builtWord.Select(x => x.StoredLetter));
    }
}

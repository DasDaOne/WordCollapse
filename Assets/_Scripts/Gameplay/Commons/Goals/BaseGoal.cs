using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class BaseGoal : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text text;
    [SerializeField] private string goalText;
    
    public event Action onGoalCompleted;
    public bool IsCompleted { get; private set; }
    
    private int goalAmount;

    public void SetupGoal(int amount)
    {
        goalAmount = amount;
        text.text = string.Format(goalText, 0, goalAmount);

        canvasGroup.alpha = 1;
    }

    protected void UpdateGoal(int newAmount)
    {
        if(IsCompleted)
            return;

        newAmount = Mathf.Min(newAmount, goalAmount);
        
        text.text = string.Format(goalText, newAmount, goalAmount);

        if (newAmount < goalAmount) return;
        
        canvasGroup.alpha = 0.5f;
        IsCompleted = true;
        onGoalCompleted?.Invoke();
    }
}

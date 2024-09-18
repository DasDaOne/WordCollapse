using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class BaseGoal : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text text;
    [SerializeField] private string goalText = "{0}/{1}";
    
    public event Action onGoalCompleted;
    public bool IsCompleted { get; private set; }

    protected int Goal;
    protected int GoalCompletion;

    public void SetupGoal(GameplayGrid grid, int goal)
    {
        Goal = goal;
        text.text = string.Format(goalText, 0, Goal);

        canvasGroup.alpha = 1;
        
        SubscribeToEvents(grid);
    }

    protected abstract void SubscribeToEvents(GameplayGrid grid);

    protected void UpdateGoalVisuals()
    {
        if(IsCompleted)
            return;
        
        text.text = string.Format(goalText, GoalCompletion, Goal);

        if (GoalCompletion < Goal) return;
        
        canvasGroup.alpha = 0.5f;
        IsCompleted = true;
        onGoalCompleted?.Invoke();
    }
}

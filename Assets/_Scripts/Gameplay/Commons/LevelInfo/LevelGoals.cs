using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGoals : MonoBehaviour
{
    [SerializeField] private GoalPrefab[] goalPrefabs;

    [Serializable]
    private struct GoalPrefab
    {
        public BaseGoal prefab;
        public Level.GoalType goalType;
    }

    private List<BaseGoal> levelGoals;

    public void Initialize(GameplayGrid gameplayGrid, Level.Goal[] goals)
    {
        levelGoals = new List<BaseGoal>();

        foreach (var goal in goals)
        {
            levelGoals.Add(Instantiate(goalPrefabs.First(x => x.goalType == goal.GoalType).prefab, transform));

            levelGoals[^1].SetupGoal(gameplayGrid, goal.GoalAmount);
        }
    }
}

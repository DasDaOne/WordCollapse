using UnityEngine;

public class GoalLetters : BaseGoal
{
	protected override void SubscribeToEvents(GameplayGrid grid)
	{
		grid.OnDestroyLetters += OnDestroyLetters;
	}

	private void OnDestroyLetters(int lettersAmount)
	{
		GoalCompletion = Mathf.Clamp(GoalCompletion + lettersAmount, 0, Goal);
		
		UpdateGoalVisuals();
	}
}

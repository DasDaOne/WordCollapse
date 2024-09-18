using UnityEngine;

public class GoalWords : BaseGoal
{
	protected override void SubscribeToEvents(GameplayGrid grid)
	{
		grid.OnDestroyWord += OnWordDestroyed;
	}

	private void OnWordDestroyed()
	{
		GoalCompletion = Mathf.Clamp(GoalCompletion + 1, 0, Goal);
		
		UpdateGoalVisuals();
	}
}

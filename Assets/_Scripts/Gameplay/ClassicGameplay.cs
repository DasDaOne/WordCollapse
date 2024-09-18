using UnityEngine;

public class ClassicGameplay : MonoBehaviour
{
	[Header("LevelInfo")]
	[SerializeField] private LevelTimer levelTimer;
	[SerializeField] private LevelGoals levelGoals;
	[Header("Gameplay")]
	[SerializeField] private GameplayGrid gameplayGrid;
	[SerializeField] private SelectionVisuals selectionVisuals;
	[SerializeField] private GameplaySelection gameplaySelection;
	[Space]
	[SerializeField] private WordDatabase wordDatabase;

	public void Initialize(Level level)
	{
		levelTimer.Initialize();
		levelGoals.Initialize(gameplayGrid, level.LevelGoals);

		gameplayGrid.CreateGrid(level);
		selectionVisuals.Initialize(gameplayGrid.CellSize);
		gameplaySelection.Initialize(gameplayGrid, wordDatabase, selectionVisuals);
	}
}

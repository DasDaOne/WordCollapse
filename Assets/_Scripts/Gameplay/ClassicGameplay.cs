using UnityEngine;

public class ClassicGameplay : MonoBehaviour
{
	[SerializeField] private LevelInfoPanel levelInfoPanel;
	[SerializeField] private GameplayGrid gameplayGrid;
	[SerializeField] private SelectionVisuals selectionVisuals;
	[SerializeField] private GameplaySelection gameplaySelection;
	[Space]
	[SerializeField] private WordDatabase wordDatabase;

	public void Initialize(Level level)
	{
		gameplayGrid.CreateGrid(level);
		selectionVisuals.Initialize(gameplayGrid.CellSize);
		gameplaySelection.Initialize(gameplayGrid, wordDatabase, selectionVisuals);

		levelInfoPanel.Initialize();
	}
}

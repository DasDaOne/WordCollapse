using UnityEngine;
using UnityEngine.UI;

public class GameplayPanel : MovePanel
{
	[Header("Gameplay")]
	[SerializeField] private Transform gameplayParent;
	[SerializeField] private ClassicGameplay gameplayPrefab;
	[Header("Restart")]
	[SerializeField] private Button restartButton;
	[SerializeField] private GameplayRestartPanel gameplayRestartPanel;
	[Space]
	[SerializeField] private TextAsset levelTextAsset;

	private ClassicGameplay currentGameplay;
	
	protected override Vector2 EndPos => StartPos;

	protected override void OnShow()
	{
		CreateGameplay();
	}

	private void CreateGameplay()
	{
		if(currentGameplay != null)
			Destroy(currentGameplay.gameObject);
		
		currentGameplay = Instantiate(gameplayPrefab, gameplayParent);

		currentGameplay.Initialize(LevelParser.ParseLevel(levelTextAsset.text));
	}

	protected override void OnHide()
	{
		if(currentGameplay == null)
			return;
		
		Destroy(currentGameplay.gameObject);
	}
	
	private void OnEnable()
	{
		restartButton.onClick.AddListener(OnRestartButtonClick);
		gameplayRestartPanel.OnRestartConfirmed += OnRestartConfirmed;
	}

	private void OnDisable()
	{
		restartButton.onClick.RemoveListener(OnRestartButtonClick);
		gameplayRestartPanel.OnRestartConfirmed -= OnRestartConfirmed;
	}

	private void OnRestartButtonClick()
	{
		gameplayRestartPanel.Show();
	}

	private void OnRestartConfirmed()
	{
		CreateGameplay();
	}
}

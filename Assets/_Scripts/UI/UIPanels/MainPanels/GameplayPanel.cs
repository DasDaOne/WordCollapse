using UnityEngine;

public class GameplayPanel : UIPanel
{
	[SerializeField] private Transform gameplayParent;
	[SerializeField] private ClassicGameplay gameplayPrefab;
	[Space]
	[SerializeField] private TextAsset levelTextAsset;

	private ClassicGameplay currentGameplay;
	
	protected override void OnShow()
	{
		base.OnShow();
		currentGameplay = Instantiate(gameplayPrefab, gameplayParent);

		currentGameplay.Initialize(LevelParser.ParseLevel(levelTextAsset.text));
	}

	protected override void OnHide()
	{
		if(currentGameplay == null)
			return;
		
		Destroy(currentGameplay);
	}
}

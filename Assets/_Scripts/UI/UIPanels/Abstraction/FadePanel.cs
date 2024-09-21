using DG.Tweening;

public class FadePanel : UIPanel
{
	protected override void ShowPanelAnimated(TweenCallback onComplete)
	{
		AttachedCanvasGroup.DOFade(1, AnimationTime).OnComplete(onComplete);
	}

	protected override void ShowPanelInstant()
	{
		AttachedCanvasGroup.alpha = 1;
	}

	protected override void HidePanelAnimated(TweenCallback onComplete)
	{
		AttachedCanvasGroup.DOFade(0, AnimationTime).OnComplete(onComplete);
	}

	protected override void HidePanelInstant()
	{
		AttachedCanvasGroup.alpha = 0;
	}
}

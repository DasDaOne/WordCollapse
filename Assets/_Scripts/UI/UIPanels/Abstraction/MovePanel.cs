using System;
using DG.Tweening;
using UnityEngine;

public class MovePanel : UIPanel
{
	protected virtual Vector2 EndPos => new (-AttachedCanvasRT.sizeDelta.x, 0);
	protected virtual Vector2 StartPos => new (AttachedCanvasRT.sizeDelta.x, 0);
	protected virtual Vector2 PosInside => new (0, 0);

	protected override Tween ShowPanelAnimated(TweenCallback onComplete)
	{
		AttachedCanvasGroup.alpha = 1;
		AttachedRectTransform.anchoredPosition = StartPos;

		return AttachedRectTransform.DOAnchorPos(PosInside, AnimationTime).OnComplete(onComplete);
	}

	protected override void ShowPanelInstant()
	{
		AttachedCanvasGroup.alpha = 1;
		
		AttachedRectTransform.anchoredPosition = PosInside;
	}

	protected override Tween HidePanelAnimated(TweenCallback onComplete)
	{
		return AttachedRectTransform.DOAnchorPos(EndPos, AnimationTime).OnComplete(() => 
		{
			AttachedCanvasGroup.alpha = 0;
			onComplete?.Invoke();
		});
	}

	protected override void HidePanelInstant()
	{
		AttachedCanvasGroup.alpha = 0;

		AttachedRectTransform.anchoredPosition = EndPos;
	}
}

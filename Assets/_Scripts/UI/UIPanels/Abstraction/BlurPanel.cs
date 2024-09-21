using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BlurPanel : UIPanel
{
	private Image cachedImage;

	private Image AttachedImage
	{
		get
		{
			if (cachedImage == null)
			{
				cachedImage = GetComponent<Image>();
				cachedImage.material = new Material(cachedImage.material);
			}

			return cachedImage;
		}
	}
	
	protected override Tween ShowPanelAnimated(TweenCallback onComplete)
	{
		return DoBlurAnimation(new Color(.75f, .75f, .75f, 1), 3.5f, 1, AnimationTime).OnComplete(onComplete);
	}

	protected override void ShowPanelInstant()
	{
		AttachedCanvasGroup.alpha = 1;
	}

	protected override Tween HidePanelAnimated(TweenCallback onComplete)
	{
		return DoBlurAnimation(new Color(1, 1, 1, 1), 0, 0, AnimationTime).OnComplete(onComplete);
	}

	protected override void HidePanelInstant()
	{
		AttachedCanvasGroup.alpha = 0;
	}

	private Tween DoBlurAnimation(Color blurColor, float blurSize, float cgAlpha, float time)
	{
		Sequence anim = DOTween.Sequence();

		anim.Join(AttachedImage.material.DOColor(blurColor, "_MultiplyColor", time));
		anim.Join(AttachedImage.material.DOFloat(blurSize, "_Size", time));
		anim.Join(AttachedCanvasGroup.DOFade(cgAlpha, time));

		return anim;
	}
}

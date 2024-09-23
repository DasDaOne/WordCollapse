using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class LetterCell : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public Vector2Int Index { get; private set; }

    private RectTransform rectTransform;
    
    public string StoredLetter { get; private set; }

    private Tween currentAnimation;
    
    public void Init(Vector2Int index)
    {
        rectTransform = transform as RectTransform;

        Index = index;

        StoredLetter = WeightedRandomLetterProvider.GetLetter();
        text.text = StoredLetter.ToUpper();
    }

    public void DestroyCell()
    {
        Destroy(gameObject);
    }

    public void Fall(Vector2 endAnchorPos, Vector2Int newIndex, float delay = 0)
    {
        Index = newIndex;

        rectTransform.DOAnchorPos(endAnchorPos, 0.5f).SetDelay(delay);
    }

    public void PlaySelectionTriggerAnimation()
    {
        currentAnimation?.Kill();
        text.rectTransform.anchoredPosition = Vector2.zero;
        
        Sequence anim = DOTween.Sequence();

        float height = rectTransform.sizeDelta.y;
        
        anim.Append(text.rectTransform.DOAnchorPosY(height * .125f, 0.125f));
        anim.Append(text.rectTransform.DOAnchorPosY(0, 0.125f));

        currentAnimation = anim;
    }

    public void PlaySelectionFailAnimation()
    {
        currentAnimation?.Kill();
        text.rectTransform.anchoredPosition = Vector2.zero;

        Sequence anim = DOTween.Sequence();

        float width = rectTransform.sizeDelta.x;

        anim.Append(text.rectTransform.DOAnchorPosX(width * .0625f, 0.0625f));
        anim.Append(text.rectTransform.DOAnchorPosX(width * -.0625f, 0.0625f));
        anim.Append(text.rectTransform.DOAnchorPosX(0, 0.0625f));

        currentAnimation = anim;
    }

    private void OnDestroy()
    {
        currentAnimation?.Kill();

        if(rectTransform != null)
            rectTransform.DOKill();
    }
}

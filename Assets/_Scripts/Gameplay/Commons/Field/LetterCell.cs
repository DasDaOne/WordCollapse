using DG.Tweening;
using TMPro;
using UnityEngine;

public class LetterCell : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public Vector2Int Index { get; private set; }

    private RectTransform rectTransform;
    
    public string StoredLetter { get; private set; }

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

    public void Fall(Vector2 endAnchorPos, Vector2Int newIndex)
    {
        Index = newIndex;

        rectTransform.DOAnchorPos(endAnchorPos, 0.5f);
    }

    public void OnSelectionTrigger()
    {
    }
}

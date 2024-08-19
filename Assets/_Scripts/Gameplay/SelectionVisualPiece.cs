using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionVisualPiece : MonoBehaviour
{
    [SerializeField] private Image visualImage;

    private float selectionThickness;
    
    public void Initialize(float thickness)
    {
        selectionThickness = thickness;
        
        RectTransform rectTransform = transform as RectTransform;

        rectTransform!.sizeDelta = new Vector2(0, thickness);
        
        visualImage.rectTransform.sizeDelta = new Vector2(thickness, thickness);
        visualImage.rectTransform.anchoredPosition = new Vector2(-thickness / 2f, 0);

        visualImage.pixelsPerUnitMultiplier = visualImage.sprite.texture.height / thickness;
    }

    public void SetState(bool state)
    {
        visualImage.enabled = state;
    }

    public void SetPoints(Vector2 start, Vector2 end)
    {
        RectTransform rectTransform = transform as RectTransform;

        Vector2 delta = end - start;
        
        rectTransform!.anchoredPosition = start;
        rectTransform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg);
;
        visualImage.rectTransform.sizeDelta = new Vector2(delta.magnitude + selectionThickness, selectionThickness);
    }
}

using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private Transform clockTransform;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private string timerFormatString;

    private float timeOnLevel;

    private Sequence clockAnimation;
    
    public void Initialize()
    {
        clockAnimation = DOTween.Sequence();

        clockAnimation.Append(ClockTickTween(-90));
        clockAnimation.Append(ClockTickTween(-180));
        clockAnimation.Append(ClockTickTween(-270));
        clockAnimation.Append(ClockTickTween(-360));
        
        clockAnimation.SetLoops(-1).OnUpdate(OnClockUpdate);
    }

    private Tween ClockTickTween(float rotation) =>
        clockTransform.DORotate(new Vector3(0, 0, rotation), 1).SetEase(Ease.InOutBack);
    
    private void OnClockUpdate()
    {
        timeOnLevel += Time.deltaTime;
        timerText.text = string.Format(timerFormatString, TimeSpan.FromSeconds(timeOnLevel));
    }
}

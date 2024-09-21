using System;
using UnityEngine;
using UnityEngine.UI;

public class GameplayRestartPanel : BlurPanel
{
    [SerializeField] private Button confirmRestartButton;
    [SerializeField] private Button[] closeButtons;

    public event Action OnRestartConfirmed;
    
    private void OnEnable()
    {
        confirmRestartButton.onClick.AddListener(OnConfirmRestartButtonClick);

        foreach (var closeButton in closeButtons)
            closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    private void OnDisable()
    {
        confirmRestartButton.onClick.RemoveListener(OnConfirmRestartButtonClick);

        foreach (var closeButton in closeButtons)
            closeButton.onClick.RemoveListener(OnCloseButtonClick);
    }

    private void OnConfirmRestartButtonClick()
    {
        Hide();
        OnRestartConfirmed?.Invoke();
    }

    private void OnCloseButtonClick()
    {
        Hide();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MovePanel
{
    [SerializeField] private Button playButton;

    protected override Vector2 StartPos => EndPos;

    private void OnEnable()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(OnPlayButtonClick);
    }

    private void OnPlayButtonClick()
    {
        UIPanelsController.Instance.ShowMainPanel(MainPanelType.Gameplay);
    }
}

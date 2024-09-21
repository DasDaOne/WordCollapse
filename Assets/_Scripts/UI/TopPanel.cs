using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopPanel : MonoBehaviour
{
    [SerializeField] private Button homeButton;
    [SerializeField] private Button settingsButton;

    private void OnEnable()
    {
        homeButton.onClick.AddListener(OnHomeButtonClick);
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
    }

    private void OnDisable()
    {
        homeButton.onClick.RemoveListener(OnHomeButtonClick);
        settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
    }

    private void OnHomeButtonClick()
    {
        UIPanelsController.Instance.ShowMainPanel(MainPanelType.MainMenu);
    }

    private void OnSettingsButtonClick()
    {
        
    }
}

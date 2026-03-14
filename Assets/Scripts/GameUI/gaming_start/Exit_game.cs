using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit_game : MonoBehaviour
{
    public GameObject exitConfirmationPanel;

    void Start()
    {
        // Hide the panel at the start
        exitConfirmationPanel.SetActive(false);
    }

    public void OnExitButtonClicked()
    {
        // Show confirmation popup
        exitConfirmationPanel.SetActive(true);
    }

    public void OnYesButtonClicked()
    {
        // Quit the game or stop play mode in Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnNoButtonClicked()
    {
        // Hide the popup
        exitConfirmationPanel.SetActive(false);
    }
}


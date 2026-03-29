using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro is recommended

public class MouseClick : MonoBehaviour
{
    public TMP_Text infoText;       // Assign your TextMeshPro UI element in the Inspector
    public string[] messages;       // Add your words/sentences in the Inspector
    private int currentIndex = 0;

    void Update()
    {
        // Detect left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            ShowNextMessage();
        }
    }

    void ShowNextMessage()
    {
        if (messages.Length == 0) return;

        // Display current message
        infoText.text = messages[currentIndex];

        // Move to next message
        currentIndex++;

        // If all messages have been shown, go back to menu
        if (currentIndex >= messages.Length)
        {
             SceneManager.LoadSceneAsync(0); // Replace with your menu scene name
        }
    }
}
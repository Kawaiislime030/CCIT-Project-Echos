using TMPro;
using UnityEngine;

public class ChatUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textDisplay;
    private string[] lines;
    private int index;
    private System.Action onFinished;
    private bool isActive = false;

    public void StartDialogue(string[] _lines, System.Action _onFinished)
    {
        lines = _lines;
        index = 0;
        onFinished = _onFinished;
        isActive = true;
        ShowNextLine();
    }

    private void Update()
    {
        if (isActive && Input.GetKeyDown(KeyCode.Mouse1))
        {
            ShowNextLine();
        }
    }

    public void ShowNextLine()
    {
        if (index < lines.Length)
        {
            textDisplay.text = lines[index];
            index++;
        }
        else
        {
            isActive = false;
            onFinished?.Invoke();
        }
    }
}

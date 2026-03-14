using UnityEngine;
using TMPro;
using System.Collections;

public class SignTrigger : MonoBehaviour
{
    [TextArea]
    public string message;          
    public TMP_Text signTextUI;     
    public float typingSpeed = 0.05f;

    private bool hasTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player") && !hasTriggered)
    {
        hasTriggered = true; // only once
        
        DeadZone deadZone = FindObjectOfType<DeadZone>();
        if (deadZone != null)
        {
            deadZone.SetCheckpoint(transform);
            Debug.Log("Checkpoint set at sign: {gameObject.name}");
        }
        else
        {
            Debug.LogError(" DeadZone not found in scene!");
        }
        
        signTextUI.text = "";
        signTextUI.gameObject.SetActive(true);
        StartCoroutine(TypeMessageAndClose());
    }
}

    IEnumerator TypeMessageAndClose()
    {
        // Typewriter effect
        foreach (char c in message.ToCharArray())
        {
            signTextUI.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Wait a short moment after finishing
        yield return new WaitForSeconds(1f);

        // Hide the text automatically
        signTextUI.gameObject.SetActive(false);
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    private DialogueController dialogueUI;
    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    
    private bool hasTriggered = false;
    public bool HasBeenTriggered { get { return hasTriggered; } }
    
    public int levelNumber = 1;
    
    // Add player reference
    private Player player;
    // Store previous state
    private PlayerState previousState;

    private void Start(){
        dialogueUI = DialogueController.Instance;
        dialogueUI.ShowDialogueUI(false);
        
        // Get player reference
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.GetComponent<Player>();
    }
    
    public bool CanInteract()
    {
        return !isDialogueActive;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            
            DeadZone deadZone = FindObjectOfType<DeadZone>();
            if (deadZone != null)
            {
                deadZone.SetCheckpoint(transform);
            }
            StartDialogue();
        }
    }
    

    public void Interact()
    {
        if (dialogueData == null)
            return;
        
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    void StartDialogue()
{
    Debug.Log($"Starting dialogue with {gameObject.name}");
    
    // Pause player movement and input
    if (PauseController.Instance != null)
    {
        PauseController.Instance.PauseGame();
    }
    
    isDialogueActive = true;
    dialogueIndex = 0;
    dialogueUI.ShowDialogueUI(true);
    DisplayCurrentLine();
}



    void NextLine()
    {
        Debug.Log("Next Line shown");
        if (isTyping)
        {
            StopAllCoroutines();
            if (dialogueData.dialogueLines != null && dialogueIndex < dialogueData.dialogueLines.Length)
            {
                dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            }
            isTyping = false;
        }
        
        dialogueUI.ClearChoices();
        
        if (dialogueData.endDialogueLines != null && 
            dialogueIndex < dialogueData.endDialogueLines.Length && 
            dialogueData.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }
        
        if (dialogueData.choices != null)
        {
            foreach (DialogueChoice dialogueChoice in dialogueData.choices)
            {
                if (dialogueChoice.dialogueIndex == dialogueIndex)
                {
                    DisplayChoices(dialogueChoice);
                    return;
                }
            }
        }
        
        if (dialogueData.dialogueLines != null && dialogueIndex + 1 < dialogueData.dialogueLines.Length)
        {
            dialogueIndex++;
            DisplayCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueUI.SetDialogueText("");
        
        if (dialogueData.dialogueLines == null || dialogueIndex >= dialogueData.dialogueLines.Length)
        {
            Debug.LogError($"Dialogue line missing at index {dialogueIndex}");
            isTyping = false;
            NextLine();
            yield break;
        }
        
        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueUI.SetDialogueText(dialogueUI.dialogueText.text += letter);
            //SoundEffectManager.PlayVoice(dialogueData.voiceSound, dialogueData.voicePitch);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;
        yield return new WaitForSeconds(dialogueData.autoProgressDelay);
        NextLine();
    }
    
    void DisplayChoices(DialogueChoice choice)
    {
        for(int i=0; i < choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];
            int points = 0;
            
            if (choice.choicePoints != null && i < choice.choicePoints.Length)
            {
                points = choice.choicePoints[i];
            }
            
            int choiceIndex = i;
            
            dialogueUI.CreateChoiceButton(choice.choices[i], () => 
                ChooseOption(nextIndex, choice.choices[choiceIndex], points));
        }   
    }
    
    void ChooseOption(int nextIndex, string choiceText, int points)
    {
        Debug.Log($" Player chose: '{choiceText}' (+{points} points)");
        
        if (PointManager.Instance != null)
        {
            PointManager.Instance.AddLevelPoints(levelNumber, points);
            Debug.Log($"Added {points} points to Level {levelNumber}");
        }
        else
        {
            Debug.LogError("PointManager.Instance is NULL!");
        }
        
        dialogueIndex = nextIndex;
        dialogueUI.ClearChoices();
        DisplayCurrentLine();
    }
    
    void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }
    
    public void EndDialogue()
{
    StopAllCoroutines();
    isDialogueActive = false;
    dialogueUI.SetDialogueText("");
    dialogueUI.ShowDialogueUI(false);
    
    // Resume player input
    if (PauseController.Instance != null)
    {
        PauseController.Instance.ResumeGame();
    }
    
    if (PointManager.Instance != null)
    {
        int levelPoints = PointManager.Instance.GetLevelPoints(levelNumber);
        Debug.Log($"Level {levelNumber} total: {levelPoints}");
    }
}
}
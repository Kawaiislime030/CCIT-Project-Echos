/* READ ME 
This script is to show the total points of all levels with one suggestion*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class EndingSceneTotalRecap : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text centerMessageText;          // Shows both opening and thank you messages
    public Slider totalPointsBar;                // Progress bar for total score
    public Image barFill;                        // Fill image of the bar
    public TMP_Text percentageText;              // Shows percentage (e.g., "70%")
    public TMP_Text rankText;                    // Shows overall rank
    public TMP_Text suggestionText;              // Shows suggestion
    
    [Header("Points Settings")]
    public float maxTotalPoints = 60f;           // Maximum possible points
    public float animationDuration = 2f;
    
    [Header("Typewriter Settings")]
    public float typewriterSpeed = 0.05f;
    
    [Header("Scene Settings")]
    public int mainMenuBuildIndex = 0;
    
    [Header("Overall Rank Settings")]
    public string[] overallRanks = new string[5] {
        "Novice",      // 0-26 points (0-19%)
        "Apprentice",  // 27-53 points (20-39%)
        "Warrior",     // 54-80 points (40-59%)
        "Master",      // 81-107 points (60-79%)
        "Legend"       // 108-135 points (80-100%)
    };
    
    [Header("Overall Suggestions - Based on Rank")]
    [TextArea(3, 5)]
    public string suggestionRank0;  // Novice
    [TextArea(3, 5)]
    public string suggestionRank1;  // Apprentice
    [TextArea(3, 5)]
    public string suggestionRank2;  // Warrior
    [TextArea(3, 5)]
    public string suggestionRank3;  // Master
    [TextArea(3, 5)]
    public string suggestionRank4;  // Legend
    
    [Header("Messages")]
    [TextArea(3, 5)]
    public string openingMessage = "Journey complete, Alice. \nYour total score: {0}/60. \n\nThis is your report. Let's look at your mental state and give some gentle opinion.";
    [TextArea(3, 5)]
    public string thankYouMessage = "Your kingdom will grow and heal as you do.\n No matter your score, you have bravely completed this journey.\n\n\nGoodbye, Alice.";
    
    // Private variables
    private float totalPoints;
    private float totalPercentage;
    private int rankIndex;
    private string overallRank;
    private string overallSuggestion;
    
    private bool isTyping = false;
    private Coroutine currentTypewriter;
    private int currentStep = 0; // 0 = opening message, 1 = rank/results, 2 = thank you, 3 = done
    private bool canProgress = false;
    private bool isTransitioning = false;

    void Start()
    {
        // Calculate total points from all levels
        CalculateTotalPoints();
        
        // Calculate percentage
        totalPercentage = totalPoints / maxTotalPoints;
        
        // Determine rank based on total points
        rankIndex = GetRankIndex();
        overallRank = overallRanks[rankIndex];
        overallSuggestion = GetOverallSuggestion(rankIndex);
        
        Debug.Log($"Total Points: {totalPoints}/{maxTotalPoints} = {totalPercentage*100:F0}%");
        Debug.Log($"Overall Rank: {overallRank}");
        
        // Hide all UI initially
        HideAllUI();
        
        // Start the first step (opening message)
        StartCoroutine(ShowOpeningMessage());
    }

    void CalculateTotalPoints()
    {
        totalPoints = 0;
        
        if (PointManager.Instance != null)
        {
            totalPoints += PointManager.Instance.GetLevelPoints(1);
            totalPoints += PointManager.Instance.GetLevelPoints(2);
            totalPoints += PointManager.Instance.GetLevelPoints(3);
        }
        else
        {
            Debug.LogWarning("PointManager not found! Using test values.");
            totalPoints = 95f;
        }
    }

    void Update()
    {
        // Left click to progress to next step (only if not transitioning and can progress)
        if (Input.GetMouseButtonDown(0) && canProgress && !isTransitioning)
        {
            StartCoroutine(TransitionToNextStep());
        }
    }

    IEnumerator TransitionToNextStep()
    {
        isTransitioning = true;
        canProgress = false;
        
        // Fade out current elements
        HideCurrentStepElements();
        
        yield return new WaitForSeconds(0.2f);
        
        // Move to next step
        currentStep++;
        
        // Show next step based on currentStep
        switch (currentStep)
        {
            case 1:
                StartCoroutine(ShowRankResults());
                break;
            case 2:
                StartCoroutine(ShowThankYouMessage());
                break;
            case 3:
                // All done, enable click to return to menu
                ReturnToMainMenu();
                break;
        }
    }

    IEnumerator ShowOpeningMessage()
    {
        // Show opening message in center
        if (centerMessageText != null)
        {
            centerMessageText.gameObject.SetActive(true);
            centerMessageText.text = "";
            
            string message = string.Format(openingMessage, Mathf.RoundToInt(totalPoints));
            
            // Typewriter effect
            isTyping = true;
            currentTypewriter = StartCoroutine(TypewriterEffect(centerMessageText, message));
            yield return currentTypewriter;
            
            isTyping = false;
            canProgress = true;
        }
    }

    IEnumerator ShowRankResults()
    {
        // Show progress bar
        if (totalPointsBar != null)
        {
            totalPointsBar.gameObject.SetActive(true);
            totalPointsBar.value = 0f;
            
            float targetPercentage = totalPoints / maxTotalPoints;
            float elapsed = 0f;
            
            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / animationDuration;
                float value = Mathf.SmoothStep(0f, targetPercentage, t);
                
                totalPointsBar.value = value;
                
                // Update percentage text during animation
                if (percentageText != null)
                {
                    percentageText.gameObject.SetActive(true);
                    percentageText.text = $"{Mathf.RoundToInt(value * 100)}%";
                }
                
                yield return null;
            }
            
            totalPointsBar.value = targetPercentage;
            
            // Set final percentage
            if (percentageText != null)
            {
                percentageText.text = $"{Mathf.RoundToInt(targetPercentage * 100)}%";
            }
        }
        
        yield return new WaitForSeconds(0.3f);
        
        // Show rank
        if (rankText != null)
        {
            rankText.gameObject.SetActive(true);
            rankText.text = "";
            
            string rankString = $"Rank: {overallRank}";
            
            isTyping = true;
            currentTypewriter = StartCoroutine(TypewriterEffect(rankText, rankString));
            yield return currentTypewriter;
        }
        
        yield return new WaitForSeconds(0.3f);
        
        // Show suggestion
        if (suggestionText != null && !string.IsNullOrEmpty(overallSuggestion))
        {
            suggestionText.gameObject.SetActive(true);
            suggestionText.text = "";
            
            isTyping = true;
            currentTypewriter = StartCoroutine(TypewriterEffect(suggestionText, overallSuggestion));
            yield return currentTypewriter;
        }
        
        isTyping = false;
        canProgress = true;
        isTransitioning = false;
    }

    IEnumerator ShowThankYouMessage()
    {
        // Show thank you message in center
        if (centerMessageText != null)
        {
            centerMessageText.gameObject.SetActive(true);
            centerMessageText.text = "";
            
            // Typewriter effect for thank you
            isTyping = true;
            currentTypewriter = StartCoroutine(TypewriterEffect(centerMessageText, thankYouMessage));
            yield return currentTypewriter;
            
            isTyping = false;
            canProgress = true;
            isTransitioning = false;
        }
    }

    void HideCurrentStepElements()
    {
        switch (currentStep)
        {
            case 0: // Hiding opening message
                if (centerMessageText != null) centerMessageText.gameObject.SetActive(false);
                break;
            case 1: // Hiding rank results
                if (totalPointsBar != null) totalPointsBar.gameObject.SetActive(false);
                if (percentageText != null) percentageText.gameObject.SetActive(false);
                if (rankText != null) rankText.gameObject.SetActive(false);
                if (suggestionText != null) suggestionText.gameObject.SetActive(false);
                break;
            case 2: // Hiding thank you message
                if (centerMessageText != null) centerMessageText.gameObject.SetActive(false);
                break;
        }
    }

    IEnumerator TypewriterEffect(TMP_Text textComponent, string fullText)
{
    isTyping = true;
    textComponent.text = "";

    foreach (char c in fullText)
    {
        textComponent.text += c;
        yield return new WaitForSeconds(typewriterSpeed);
    }

    isTyping = false;
    currentTypewriter = null;
	}

    void ReturnToMainMenu()
    {
        Debug.Log("Returning to main menu...");
        
        if (PointManager.Instance != null)
        {
            PointManager.Instance.ResetAllPoints();
        }
        
        SceneManager.LoadScene(mainMenuBuildIndex);
    }

    int GetRankIndex()
    {
        if (totalPercentage >= 0.8) return 4;  // Legend (80-100%)
        if (totalPercentage >= 0.6) return 3;   // Master (60-79%)
        if (totalPercentage >= 0.4) return 2;   // Warrior (40-59%)
        if (totalPercentage >= 0.2) return 1;   // Apprentice (20-39%)
        return 0;                           // Novice (0-19%)
    }

    string GetOverallSuggestion(int rankIndex)
    {
        switch (rankIndex)
        {
            case 0: return suggestionRank0;
            case 1: return suggestionRank1;
            case 2: return suggestionRank2;
            case 3: return suggestionRank3;
            case 4: return suggestionRank4;
            default: return "Great job completing the game!";
        }
    }

    void HideAllUI()
    {
        if (centerMessageText != null) centerMessageText.gameObject.SetActive(false);
        if (totalPointsBar != null) totalPointsBar.gameObject.SetActive(false);
        if (percentageText != null) percentageText.gameObject.SetActive(false);
        if (rankText != null) rankText.gameObject.SetActive(false);
        if (suggestionText != null) suggestionText.gameObject.SetActive(false);
    }
}
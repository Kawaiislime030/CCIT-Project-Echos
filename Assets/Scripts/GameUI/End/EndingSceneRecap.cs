using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class EndingSceneRecap : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text worldHeaderText;         // Shows "World X Result"
    public Slider pointsBar;                  // Progress bar (visual only)
    public Image barFill;                     // Fill image of the bar
    public TMP_Text percentageText;            // Shows percentage (e.g., "85%")
    public TMP_Text finalSuggestionText;       // Suggestion based on rank 
    public TMP_Text gradeText;                 // Shows "Rank: ???"
    
    [Header("Message Text")]
    public TMP_Text centerMessageText;         // Initial and thank you messages (type effect cannot be skipped)
    
    [Header("Points Settings")]
    public float maxPointsPerLevel = 100f;      // Maximum possible points per level
    public float animationDuration = 2f;        // How long bar fills
    
    [Header("Typewriter Settings")]
    public float typewriterSpeed = 0.05f;       // Speed of typewriter effect
    
    [Header("Scene Settings")]
    public int mainMenuBuildIndex = 0;           // Main menu scene build index
    
    [Header("World Colors")]
    public Color world1Color = new Color(0.2f, 0.6f, 1f);    // Blue
    public Color world2Color = new Color(1f, 0.8f, 0.2f);    // Yellow
    public Color world3Color = new Color(1f, 0.3f, 0.3f);    // Red
    
    [Header("RANK NAMES - 5 Ranks per World")]
    public string[] world1Ranks = new string[5] { 
        "Novice",      // 0-29%
        "Apprentice",  // 30-49%
        "Warrior",     // 50-69%
        "Master",      // 70-89%
        "Legend"       // 90-100%
    };
    
    public string[] world2Ranks = new string[5] { 
        "Peasant", 
        "Squire", 
        "Knight", 
        "Lord", 
        "King" 
    };
    
    public string[] world3Ranks = new string[5] { 
        "Traveler", 
        "Adventurer", 
        "Hero", 
        "Champion", 
        "Mythic" 
    };
    
    [Header("World 1 (Low Self Esteem) SUGGESTIONS")]
    [TextArea(2, 4)]
    public string world1SuggestionRank0;  // Novice
    [TextArea(2, 4)]
    public string world1SuggestionRank1;  // Apprentice
    [TextArea(2, 4)]
    public string world1SuggestionRank2;  // Warrior
    [TextArea(2, 4)]
    public string world1SuggestionRank3;  // Master
    [TextArea(2, 4)]
    public string world1SuggestionRank4;  // Legend
    
    [Header("World 2 (Procastination) SUGGESTIONS")]
    [TextArea(2, 4)]
    public string world2SuggestionRank0;  // Peasant
    [TextArea(2, 4)]
    public string world2SuggestionRank1;  // Squire
    [TextArea(2, 4)]
    public string world2SuggestionRank2;  // Knight
    [TextArea(2, 4)]
    public string world2SuggestionRank3;  // Lord
    [TextArea(2, 4)]
    public string world2SuggestionRank4;  // King
    
    [Header("World 3 (Perfectionism) Suggestions")]
    [TextArea(2, 4)]
    public string world3SuggestionRank0;  // Traveler
    [TextArea(2, 4)]
    public string world3SuggestionRank1;  // Adventurer
    [TextArea(2, 4)]
    public string world3SuggestionRank2;  // Hero
    [TextArea(2, 4)]
    public string world3SuggestionRank3;  // Champion
    [TextArea(2, 4)]
    public string world3SuggestionRank4;  // Mythic
    
    
    // Private variables
    private float world1Points;
    private float world2Points;
    private float world3Points;
    private int currentWorld = 1;
    private int sequenceStep = 0;
    
    // Stored ranks
    private string world1Rank = "";
    private string world2Rank = "";
    private string world3Rank = "";
    private int world1RankIndex = 0;
    private int world2RankIndex = 0;
    private int world3RankIndex = 0;
    
    private bool isTypingSuggestion = false;    // Type writer effect on Suggestion
    private bool isTypingCenter = false;        // Type writer effect on CenterMessage
    private Coroutine suggestionTypewriterCoroutine;
    private Coroutine centerTypewriterCoroutine;
    private bool isAnimating = false;

    void Start()
    {
        // Get points from PointManager or use test mode
        // Get points from PointManager
	if (PointManager.Instance != null)
	{
		world1Points = PointManager.Instance.GetLevelPoints(1);
		world2Points = PointManager.Instance.GetLevelPoints(2);
		world3Points = PointManager.Instance.GetLevelPoints(3);
    
		Debug.Log($" Loaded points - W1:{world1Points}, W2:{world2Points}, W3:{world3Points}");
	}
	else{
			Debug.LogError("PointManager not found! Make sure it exists in the scene.");
		world1Points = 0f;
		world2Points = 0f;
		world3Points = 0f;
		}
        
        // Calculate ranks
        world1RankIndex = GetRankIndex(world1Points / maxPointsPerLevel);
        world2RankIndex = GetRankIndex(world2Points / maxPointsPerLevel);
        world3RankIndex = GetRankIndex(world3Points / maxPointsPerLevel);
        
        world1Rank = world1Ranks[world1RankIndex];
        world2Rank = world2Ranks[world2RankIndex];
        world3Rank = world3Ranks[world3RankIndex];
        
        Debug.Log($" Ranks - W1:{world1Rank} ({world1RankIndex}), W2:{world2Rank}, W3:{world3Rank}");       
        HideAllUI();
        
        StartCoroutine(ShowInitialMessage());
    }

    IEnumerator ShowInitialMessage()
    {
        if (centerMessageText != null)
        {
            centerMessageText.gameObject.SetActive(true);
            centerMessageText.text = "";
            centerMessageText.fontSize = 60;
            centerMessageText.color = Color.yellow;
            centerMessageText.fontStyle = FontStyles.Bold;
            
            string message = "All levels completed!\nLet's see your results...";
            
            // CenterMessage Cannot be Skipped
            isTypingCenter = true;
            foreach (char c in message)
            {
                centerMessageText.text += c;
                yield return new WaitForSeconds(typewriterSpeed);
            }
            isTypingCenter = false;
        }
        
        sequenceStep = 0;
        Debug.Log(" Click to start viewing results");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            if (isAnimating)
            {
                Debug.Log("Animation in progress, wait...");
                return;
            }

            
            if (isTypingCenter)
            {
                Debug.Log("Center message typing - cannot skip");
                return; 
            }

            
            if (isTypingSuggestion && suggestionTypewriterCoroutine != null)
            {
                StopCoroutine(suggestionTypewriterCoroutine);
                isTypingSuggestion = false;
                
               
                if (finalSuggestionText != null)
                {
                    string fullText = finalSuggestionText.text;
                }
                Debug.Log(" Suggestion typing skipped");
                return; 
            }

            switch (sequenceStep)
            {
                case 0: // Initial message
                    Debug.Log("Step 0: Initial message -> World 1");
                    HideCenterMessage();
                    sequenceStep = 1;
                    currentWorld = 1;
                    StartCoroutine(ShowWorldResult(1, world1Points, world1Rank, world1RankIndex));
                    break;

                case 1: // World 1 done, ready for World 2
                    Debug.Log("Step 1: World 1 -> World 2");
                    sequenceStep = 2;
                    currentWorld = 2;
                    HideAllUI();
                    StartCoroutine(ShowWorldResult(2, world2Points, world2Rank, world2RankIndex));
                    break;

                case 2: // World 2 done, ready for World 3
                    Debug.Log(" Step 2: World 2 -> World 3");
                    sequenceStep = 3;
                    currentWorld = 3;
                    HideAllUI();
                    StartCoroutine(ShowWorldResult(3, world3Points, world3Rank, world3RankIndex));
                    break;

                case 3: // World 3 done, ready for Final Summary
                    Debug.Log("️ Step 3: World 3 -> Final Summary");
                    sequenceStep = 4;
                    HideAllUI();
                    StartCoroutine(ShowFinalSummary());
                    break;
                    
                case 4: // Final summary done, return to menu
                    Debug.Log(" Step 4: Final Summary -> Main Menu");
                    ReturnToMainMenu();
                    break;
            }
        }
    }

    IEnumerator ShowWorldResult(int worldNumber, float points, string rank, int rankIndex)
    {
        isAnimating = true;
        currentWorld = worldNumber;
        float targetValue = points / maxPointsPerLevel;
        
        // Show header
        if (worldHeaderText != null)
        {
            worldHeaderText.text = $"World {worldNumber} Result";
            worldHeaderText.gameObject.SetActive(true);
            worldHeaderText.color = GetWorldColor(worldNumber);
        }
        
        // Show bar
        if (pointsBar != null)
        {
            pointsBar.gameObject.SetActive(true);
            pointsBar.value = 0f;
        }
        
        // Show percentage
        if (percentageText != null)
        {
            percentageText.gameObject.SetActive(true);
            percentageText.text = "0%";
        }
        
        // Show rank
        if (gradeText != null)
        {
            gradeText.gameObject.SetActive(true);
            gradeText.text = $"Rank: {rank}";
            gradeText.color = GetWorldColor(worldNumber);
        }
        
        // Show suggestion text box (empty for now)
        if (finalSuggestionText != null)
        {
            finalSuggestionText.gameObject.SetActive(true);
            finalSuggestionText.text = "";
        }
        
        // Animate bar and percentage
        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;
            float value = Mathf.SmoothStep(0f, targetValue, t);
            
            if (pointsBar != null)
                pointsBar.value = value;
            
            if (percentageText != null)
                percentageText.text = $"{Mathf.RoundToInt(value * 100)}%";
            
            if (barFill != null)
                barFill.color = GetWorldColor(worldNumber);
            
            yield return null;
        }
        
        // Set final values
        if (pointsBar != null)
            pointsBar.value = targetValue;
        
        if (percentageText != null)
            percentageText.text = $"{Mathf.RoundToInt(targetValue * 100)}%";
        
        // Show suggestion with typewriter (skip the typewriter effect)
        string suggestion = GetSuggestionForWorld(worldNumber, rankIndex);
        yield return StartCoroutine(TypewriterSuggestion(suggestion));
        
        isAnimating = false;
        Debug.Log($"World {worldNumber} complete - Click to continue");
    }

    IEnumerator ShowFinalSummary()
    {
        isAnimating = true;
        if (centerMessageText != null)
        {
            centerMessageText.gameObject.SetActive(true);
            centerMessageText.text = "";
            centerMessageText.fontSize = 48;
            centerMessageText.color = Color.yellow;
            centerMessageText.fontStyle = FontStyles.Bold;
            
            string thankYou = "Thank you for playing!\n\n Left Click to return to main menu";
            
            isTypingCenter = true;
            foreach (char c in thankYou)
            {
                centerMessageText.text += c;
                yield return new WaitForSeconds(typewriterSpeed);
            }
            isTypingCenter = false;
        }
        
        isAnimating = false;
        Debug.Log("Final summary displayed - Click to return to menu");
    }

    void ReturnToMainMenu()
    {
        Debug.Log("Returning to main menu...");
        
        // Reset points for next playthrough (optional)
        if (PointManager.Instance != null)
        {
            PointManager.Instance.ResetAllPoints();
            Debug.Log("Points reset for next game");
        }
        
        SceneManager.LoadScene(mainMenuBuildIndex);
    }

    int GetRankIndex(float progress)
    {
        float percentage = progress * 100f;
        
        if (percentage >= 90f) return 4;  // Legend/King/Mythic
        if (percentage >= 70f) return 3;  // Master/Lord/Champion
        if (percentage >= 50f) return 2;  // Warrior/Knight/Hero
        if (percentage >= 30f) return 1;  // Apprentice/Squire/Adventurer
        return 0;                          // Novice/Peasant/Traveler
    }

    string GetSuggestionForWorld(int world, int rankIndex)
    {
        switch (world)
        {
            case 1:
                if (rankIndex == 0) return world1SuggestionRank0;
                if (rankIndex == 1) return world1SuggestionRank1;
                if (rankIndex == 2) return world1SuggestionRank2;
                if (rankIndex == 3) return world1SuggestionRank3;
                if (rankIndex == 4) return world1SuggestionRank4;
                break;
                
            case 2:
                if (rankIndex == 0) return world2SuggestionRank0;
                if (rankIndex == 1) return world2SuggestionRank1;
                if (rankIndex == 2) return world2SuggestionRank2;
                if (rankIndex == 3) return world2SuggestionRank3;
                if (rankIndex == 4) return world2SuggestionRank4;
                break;
                
            case 3:
                if (rankIndex == 0) return world3SuggestionRank0;
                if (rankIndex == 1) return world3SuggestionRank1;
                if (rankIndex == 2) return world3SuggestionRank2;
                if (rankIndex == 3) return world3SuggestionRank3;
                if (rankIndex == 4) return world3SuggestionRank4;
                break;
        }
        
        return "Great job!";
    }

    Color GetWorldColor(int world)
    {
        switch (world)
        {
            case 1: return world1Color;
            case 2: return world2Color;
            case 3: return world3Color;
            default: return Color.white;
        }
    }

    
    IEnumerator TypewriterSuggestion(string message)
    {
        if (finalSuggestionText == null || string.IsNullOrEmpty(message)) yield break;
        
        if (suggestionTypewriterCoroutine != null)
            StopCoroutine(suggestionTypewriterCoroutine);
        
        isTypingSuggestion = true;
        finalSuggestionText.text = "";
        
        foreach (char c in message.ToCharArray())
        {
            finalSuggestionText.text += c;
            yield return new WaitForSeconds(typewriterSpeed);
        }
        
        isTypingSuggestion = false;
    }

    void HideCenterMessage()
    {
        if (centerMessageText != null)
            centerMessageText.gameObject.SetActive(false);
    }

    void HideAllUI()
    {
        if (worldHeaderText != null) 
        {
            worldHeaderText.gameObject.SetActive(false);
            worldHeaderText.text = "";
        }
        
        if (pointsBar != null) 
        {
            pointsBar.gameObject.SetActive(false);
        }
        
        if (percentageText != null) 
        {
            percentageText.gameObject.SetActive(false);
            percentageText.text = "";
        }
        
        if (finalSuggestionText != null) 
        {
            finalSuggestionText.gameObject.SetActive(false);
            finalSuggestionText.text = "";
        }
        
        if (gradeText != null) 
        {
            gradeText.gameObject.SetActive(false);
            gradeText.text = "";
        }
        
        if (centerMessageText != null) 
        {
            centerMessageText.gameObject.SetActive(false);
            centerMessageText.text = "";
        }
        
        Debug.Log("All UI elements hidden manually");
    }
}
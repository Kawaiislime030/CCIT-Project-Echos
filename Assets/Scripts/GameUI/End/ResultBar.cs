using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultBar : MonoBehaviour
{
    [Header("UI References")]
    public Slider resultSlider;
    public Image fillBar;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI resultTitleText;
    
    [Header("Settings")]
    public float fillDuration = 2f;
    public AnimationCurve fillCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    private float targetValue;
    
    void Start()
    {
        // Set initial values
        targetValue = PlayerPrefs.GetFloat("FinalScore", 0.75f); // Example score
        scoreText.text = $"Score: {targetValue * 100:F0}%";
        
        // Start filling animation
        StartCoroutine(FillBarAnimation());
    }
    
    System.Collections.IEnumerator FillBarAnimation()
    {
        float elapsedTime = 0f;
        float startValue = 0f;
        
        while (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fillDuration;
            
            // Apply animation curve
            float curveValue = fillCurve.Evaluate(t);
            float currentValue = Mathf.Lerp(startValue, targetValue, curveValue);
            
            resultSlider.value = currentValue;
            
            // Change color based on score
            if (currentValue < 0.33f)
                fillBar.color = Color.red;
            else if (currentValue < 0.66f)
                fillBar.color = Color.yellow;
            else
                fillBar.color = Color.green;
            
            yield return null;
        }
        
        resultSlider.value = targetValue;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;
    public int level1Points = 0;
    public int level2Points = 0;
    public int level3Points = 0;
    
    void Awake()
    {
        Debug.Log("PointManager Awake in scene: " + gameObject.scene.name);
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("PointManager set to persist");
        }
        else
        {
            Debug.Log("Duplicate PointManager destroyed in: " + gameObject.scene.name);
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        Debug.Log("PointManager Start in scene: " + gameObject.scene.name);
        Debug.Log("Points - Level1: " + level1Points + ", Level2: " + level2Points + ", Level3: " + level3Points);
    }
    
    void OnDestroy()
    {
        Debug.Log("PointManager destroyed from scene: " + gameObject.scene.name);
    }
    
    public void AddLevelPoints(int level, int points)
    {
        switch (level)
        {
            case 1: 
                level1Points += points; 
                Debug.Log("Level 1: +" + points + " = " + level1Points);
                break;
            case 2: 
                level2Points += points; 
                Debug.Log("Level 2: +" + points + " = " + level2Points);
                break;
            case 3: 
                level3Points += points; 
                Debug.Log("Level 3: +" + points + " = " + level3Points);
                break;
        }
    }
    
    public int GetLevelPoints(int level)
    {
        switch (level)
        {
            case 1: return level1Points;
            case 2: return level2Points;
            case 3: return level3Points;
            default: return 0;
        }
    }
    
    public void ResetAllPoints()
    {
        level1Points = 0;
        level2Points = 0;
        level3Points = 0;
        Debug.Log("All points reset");
    }
}
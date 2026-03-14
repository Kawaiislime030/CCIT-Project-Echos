using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class start_game : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadSceneAsync(1);
    }
    void Start()
    {
    Camera.main.orthographicSize = (Screen.height / 100f) / 2f;
    
    float targetWidth = 10f; // desired width in world units
    float aspectRatio = (float)Screen.width / Screen.height;
    Camera.main.orthographicSize = targetWidth / (2f * aspectRatio);
    }


    
}

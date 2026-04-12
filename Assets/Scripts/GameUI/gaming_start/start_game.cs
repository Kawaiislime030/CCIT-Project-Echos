using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class start_game : MonoBehaviour
{
    [SerializeField] UI_FadeScreen fadeScreen;
    public void StartButton()
    {
        AudioManager.instance.PlaySFX(22, null);
        SaveManager.instance.DeleteSaveData();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }
    void Start()
    {
    Camera.main.orthographicSize = (Screen.height / 100f) / 2f;
    
    float targetWidth = 10f; // desired width in world units
    float aspectRatio = (float)Screen.width / Screen.height;
    Camera.main.orthographicSize = targetWidth / (2f * aspectRatio);
    }

    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay); // wait for fade out to complete
        
        SceneManager.LoadSceneAsync(1);
    }
    
}

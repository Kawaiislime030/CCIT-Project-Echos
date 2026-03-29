using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour, ISaveManager
{
    [Header("End screen")]
    [SerializeField] public UI_FadeScreen fadeScreen;
    [SerializeField] GameObject endText;
    [SerializeField] private GameObject restartButton;
    [Space]


    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;


    [SerializeField] private UI_VolumeSlider[] volumeSettings;

    private void Awake()
    {
        fadeScreen.gameObject.SetActive(true);
    }
    void Start()
    {
        SwitchTo(null);
        SwitchTo(inGameUI);
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchWithKeyTo(characterUI);
        }
        
    }

    public void SwitchTo(GameObject _menu)
    {

        for ( int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;
           
            if (fadeScreen == false)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            AudioManager.instance.PlaySFX(22, null);
            _menu.SetActive(true);
        }

        if (GameManagers.instance != null)
        {
            if (_menu == inGameUI)
                GameManagers.instance.PauseGame(false);
            else
                GameManagers.instance.PauseGame(true);
        }

    }

    public void SwitchWithKeyTo (GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
    
        SwitchTo(_menu);
    }
        
    private void CheckForInGameUI()
    {
        for ( int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
            {
                return;
            }
        }

        SwitchTo(inGameUI);
    }

    public void SwitchOnEndScreen()
    {
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCorutione());

    }

    IEnumerator EndScreenCorutione()
    {
        yield return new WaitForSeconds(1f); // wait for fade out to complete
        endText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
    }

    public void RestartGameButton() => GameManagers.instance.RestartScene();

    public void LoadData(GameData _data)
    {
        foreach(KeyValuePair<string, float> pair in _data.volumeSettings)
        {
            foreach (UI_VolumeSlider item in volumeSettings)
            {
                if (item.parameter == pair.Key)
                {
                    item.slider.value = pair.Value;
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach (UI_VolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parameter, item.slider.value);
        }
    }
}

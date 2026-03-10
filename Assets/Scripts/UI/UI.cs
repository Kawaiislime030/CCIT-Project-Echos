using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;




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
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            _menu.SetActive(true);
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
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                return;
            }
        }

        SwitchTo(inGameUI);
    }
}

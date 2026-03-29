using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    
    void Start()
    {
        if (playerStats != null)
            playerStats.onHealthChanged += UpdateHealthUI;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }
}

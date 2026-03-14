using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatSlot : MonoBehaviour
{
	
    [SerializeField] private string statName;

    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statPointsText;



    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;

        if(statNameText != null)
        {
            statNameText.text = statName;
        }
    }
    void Start()
    {
        UpdateStatValueUI();
        UpdateStatPointsUI();
    }

    void OnEnable()
    {
        UpdateStatValueUI();
        UpdateStatPointsUI();
    }

    public void UpdateStatValueUI()
    {
        if (statValueText == null) return;

        CharacterStats playerStats = PlayerManager.instance?.player?.GetComponent<CharacterStats>();
        
        if (playerStats != null)
        {
            Stat stat = playerStats.GetStat(statType);
            if (stat != null)
            {
                statValueText.text = stat.GetValue().ToString();
            
                if (statType == StatType.maxHealth)
                    statValueText.text = playerStats.GetMaxHealthValue().ToString();

                if (statType == StatType.damage)
                    statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();

                if (statType == StatType.critPower)
                    statValueText.text = (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString();

                if (statType == StatType.critChance)
                    statValueText.text = (playerStats.critChance.GetValue() + playerStats.agillity.GetValue()).ToString();

                if (statType == StatType.evasion)
                    statValueText.text = (playerStats.evasion.GetValue() + playerStats.agillity.GetValue()).ToString();

            }
        }
    }
    public void OnAddStatPoint()
    {
        CharacterStats playerStats = PlayerManager.instance.player.GetComponent<CharacterStats>();

        if (playerStats != null)
        {
            playerStats.AddStatPoint(statType);   // spend point on this stat
            UpdateStatValueUI();   // refresh text
            UpdateStatPointsUI(); // refresh remaining points text

        }
    }

    public void UpdateStatPointsUI()
    {
        // If the text field isn¡¦t assigned in the Inspector, just exit
        if (statPointsText == null) return;

        // Safely get the player stats
        CharacterStats playerStats = PlayerManager.instance?.player?.GetComponent<CharacterStats>();

        // If playerStats is null, just exit
        if (playerStats == null) return;

        // Update the text
        statPointsText.text = "" + playerStats.statPoints;
    }



}

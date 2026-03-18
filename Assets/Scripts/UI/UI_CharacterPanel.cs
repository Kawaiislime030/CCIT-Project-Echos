using UnityEngine;
using TMPro;

public class UI_CharacterPanel : MonoBehaviour
{
    [SerializeField] private CharacterStats playerStats;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI xpText;

    private void Start()
    {
        if (playerStats == null)
            playerStats = PlayerManager.instance.player.GetComponent<CharacterStats>();

        RefreshUI();

        // Optional: subscribe to events so UI updates automatically
        playerStats.onStatsChanged += RefreshUI;
        playerStats.onXPChanged += RefreshUI;
    }

    private void OnDestroy()
    {
        if (playerStats != null)
        {
            playerStats.onStatsChanged -= RefreshUI;
            playerStats.onXPChanged -= RefreshUI;
        }
    }

    private void RefreshUI()
    {
        levelText.text = "Level: " + playerStats.level;
        xpText.text = "XP: " + playerStats.currentXP + "/" + playerStats.xpToNextLevel;
    }

}

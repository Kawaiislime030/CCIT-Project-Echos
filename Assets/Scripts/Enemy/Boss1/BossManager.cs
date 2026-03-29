using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;

    [SerializeField] private GameObject chatBoxUI;
    [SerializeField] private Boss1 boss;
    [SerializeField] private HintUI hintUI;

    private void Awake() => instance = this;

    public void StartIntroDialogue()
    {
        chatBoxUI.SetActive(true);
        chatBoxUI.GetComponent<ChatUI>().StartDialogue(new string[] {
            "Test Message 1",
            "Test Message 2"
        }, OnIntroFinished);
    }

    private void OnIntroFinished()
    {
        chatBoxUI.SetActive(false);
        boss.StartBattle();
    }

    

    public void ShowHint(string msg, float time)
    {
        if (hintUI != null)
            hintUI.ShowHint(msg, time);
    }


}


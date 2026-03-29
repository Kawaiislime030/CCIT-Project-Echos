using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && collision.CompareTag("Player"))
        {
            triggered = true;
            BossManager.instance.StartIntroDialogue();
        }
    }
}

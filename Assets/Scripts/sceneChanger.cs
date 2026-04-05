using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneChanger : MonoBehaviour
{
    [SerializeField] private int sceneToLoad;
    
    private bool isActive = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D portalCollider;
    
    // Boss reference
    private GameObject boss;

    [SerializeField] UI_FadeScreen fadeScreen;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;
        
        if (other.CompareTag("Player"))
        {
            fadeScreen.FadeOut();
            Debug.Log("Player entered portal!");
            StartCoroutine(LoadSceneAfterEndScreen());
        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        portalCollider = GetComponent<Collider2D>();
        
        // Hide portal at start
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
        
        if (portalCollider != null)
            portalCollider.enabled = false;
        
        // Find boss
        boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss == null)
            boss = GameObject.Find("FinalBoss2");
        
        Debug.Log("Portal waiting for boss death...");
    }
    
    void Update()
    {
        // Check if boss is dead
        if (!isActive && boss != null)
        {
            EnemyStats stats = boss.GetComponent<EnemyStats>();
            if (stats != null && stats.currentHealth <= 0)
            {
                // Start the 3-second delay coroutine
                StartCoroutine(ShowPortalAfterDelay());
            }
        }
    }
    
    IEnumerator ShowPortalAfterDelay()
    {
        // Timer for Wait 3 seconds
        yield return new WaitForSeconds(5);
        
        // Activate portal
        isActive = true;
        
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
        
        if (portalCollider != null)
            portalCollider.enabled = true;
        
        Debug.Log("Portal appeared after 3 seconds!");
    }
    private IEnumerator LoadSceneAfterEndScreen()
    {
        // 等待 EndScreen 動畫時間，例如 3 秒
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(sceneToLoad);
    }

    // Optional: Manual activation method
    public void ActivatePortal()
    {
        if (isActive) return;
        StartCoroutine(ShowPortalAfterDelay());
    }
}
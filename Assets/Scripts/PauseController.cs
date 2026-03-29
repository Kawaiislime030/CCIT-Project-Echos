using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance;

    [Header("Settings")]
    public bool isPaused = false;
    
    // Player components
    private GameObject playerObject;
    private Player playerScript;
    private Animator playerAnimator;
    
    // Stored values
    private PlayerState previousState;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        FindPlayer();
    }

    void FindPlayer()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerScript = playerObject.GetComponent<Player>();
            playerAnimator = playerObject.GetComponentInChildren<Animator>();
            
            Debug.Log("Player found in PauseController");
        }
    }

    public void PauseGame()
    {
        if (isPaused) return;
        
        isPaused = true;
        Debug.Log("PAUSING GAME - Player input disabled");

        // Find player if not found
        if (playerObject == null) FindPlayer();

        if (playerObject == null)
        {
            Debug.LogError("Cannot pause - Player not found!");
            return;
        }

        // Store current state
        if (playerScript != null && playerScript.stateMachine != null)
        {
            previousState = playerScript.stateMachine.currentState;
            
            // Force idle state
            playerScript.stateMachine.ChangeState(playerScript.idleState);
        }

        // Disable player script (this stops all input and Update())
        if (playerScript != null)
        {
            playerScript.enabled = false;
        }

        // Ensure idle animation plays
		
        if (playerAnimator != null)
        {
            playerAnimator.Play("Idle", 0, 0f);
        }
		

        Debug.Log("Player input disabled - Idle animation forced");
    }

    public void ResumeGame()
    {
        if (!isPaused) return;
        
        isPaused = false;
        Debug.Log("RESUMING GAME - Player input enabled");

        if (playerObject == null) FindPlayer();

        // Re-enable player script
        if (playerScript != null)
        {
            playerScript.enabled = true;
        }

        // Restore previous state
        if (playerScript != null && playerScript.stateMachine != null && previousState != null)
        {
            playerScript.stateMachine.ChangeState(previousState);
        }

        Debug.Log("Player input enabled - Previous state restored");
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}
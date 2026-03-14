using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeadZone: MonoBehaviour
{
    public float respawnDelay = 1f;
    public float distanceFromNPC = 2f;
    public float heightOffset = 1f;
    
    [Header("Spawnpoint Settings")]
    public Transform defaultSpawnpoint; // Drag your level's default spawnpoint here
    
    // Track the last triggered NPC
    private Transform lastCheckpointSpawnpoint;
    
    void Start()
    {
        // Initialize with default spawnpoint
        if (defaultSpawnpoint != null)
        {
            lastCheckpointSpawnpoint = defaultSpawnpoint;
            Debug.Log($"Default spawnpoint set at: {defaultSpawnpoint.position}");
        }
        else
        {
            Debug.LogError("Default spawnpoint not assigned! Please assign in Inspector.");
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has CharacterStats
        CharacterStats stats = collision.GetComponent<CharacterStats>();
        
        if (stats != null)
        {
            // Check if it's the player
            Player player = collision.GetComponent<Player>();
            
            if (player != null)
            {
                // Kill the player
                stats.KillEntity();
                StartCoroutine(RespawnAfterDeath(player.gameObject));
            }
            else
            {
                // Destroy other entities (enemies, etc.)
                Destroy(collision.gameObject);
            }
        }
    }
    
    // Public method for health-based death
    public void StartRespawn(GameObject player)
    {
        Debug.Log("Starting respawn from death");
        StartCoroutine(RespawnAfterDeath(player));
    }
    
    public void SetCheckpoint(Transform npcTransform){
    // Only set checkpoint if this is actually an NPC (you can check by tag)
		if (npcTransform.CompareTag("NPC")){
        // Create a checkpoint position near the NPC
        GameObject checkpoint = new GameObject($"Checkpoint_{npcTransform.name}");
        checkpoint.transform.position = npcTransform.position + new Vector3(distanceFromNPC, heightOffset, 0);
        
        // Update last checkpoint
        lastCheckpointSpawnpoint = checkpoint.transform;
        
        Debug.Log($"Checkpoint set at NPC: {npcTransform.name} at {checkpoint.transform.position}");
    }
    else
    {
        Debug.Log($"⏭️ Ignoring non-NPC object: {npcTransform.name}");
    }
} 
    IEnumerator RespawnAfterDeath(GameObject player)
    {
        // Wait for death animation
        yield return new WaitForSeconds(respawnDelay);
        
        // Make sure we have a spawnpoint to use
        if (lastCheckpointSpawnpoint == null)
        {
            if (defaultSpawnpoint != null)
            {
                lastCheckpointSpawnpoint = defaultSpawnpoint;
                Debug.Log("Using default spawnpoint as fallback");
            }
            else
            {
                Debug.LogError("No spawnpoint available! Cannot respawn.");
                yield break;
            }
        }
        
        // Reset player health to full
        CharacterStats stats = player.GetComponent<CharacterStats>();
        if (stats != null)
        {
            int maxHealth = stats.GetMaxHealthValue();
            stats.currentHealth = maxHealth;
            
            if (stats.onHealthChanged != null)
                stats.onHealthChanged();
        }
        
        // Return player to idle state
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null && playerScript.stateMachine != null)
        {
            playerScript.stateMachine.ChangeState(playerScript.idleState);
        }
        
        // Set player position to checkpoint
        player.transform.position = lastCheckpointSpawnpoint.position;
        Debug.Log($"lastCheckpointSpawnpoint set to: {lastCheckpointSpawnpoint.position}");
        
        // Reset velocity
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = Vector2.zero;
        
        // Make sure player is active
        player.SetActive(true);
        
        Debug.Log($"Player respawned at checkpoint: {lastCheckpointSpawnpoint.position}");
    }
    
    void OnDrawGizmosSelected()
    {
        if (defaultSpawnpoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(defaultSpawnpoint.position, 0.5f);
            Gizmos.DrawIcon(defaultSpawnpoint.position, "spawnpoint.png", true);
        }
        
        if (lastCheckpointSpawnpoint != null && lastCheckpointSpawnpoint != defaultSpawnpoint)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(lastCheckpointSpawnpoint.position, 0.5f);
            Gizmos.DrawIcon(lastCheckpointSpawnpoint.position, "checkpoint.png", true);
        }
    }
}
using UnityEngine;


/* This is the bullet with normal attack method*/
 
public class Bullet : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 3f;
    public string targetTag = "Player";
    
    public GameObject hitEffect;
    
    // Reflection properties
    public bool isReflected = false;
    public float reflectionSpeedMultiplier = 1.5f;
    public int reflectedDamage = 20;
    
    // Attack check reference - will find automatically
    private GameObject attackCheck;
    private Player player;
    private bool playerIsAttacking = false;
    
    private Rigidbody2D rb;
    private Vector2 currentDirection;
    private SpriteRenderer spriteRenderer;
    private Collider2D bulletCollider;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bulletCollider = GetComponent<Collider2D>();

        // Find player and attack check
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<Player>();
            
            // Find attack check by common names
            Transform found = playerObj.transform.Find("AttackCheck");
            if (found == null)
                found = playerObj.transform.Find("AttackPoint");
            if (found == null)
                found = playerObj.transform.Find("AttackRange");
                
            if (found != null)
            {
                attackCheck = found.gameObject;
                Debug.Log("Attack check found: " + attackCheck.name);
            }
            else
            {
                Debug.LogWarning("Attack check not found! Please name it 'AttackCheck'");
            }
        }

        // Disable gravity
        if (rb != null)
        {
            rb.gravityScale = 0;
        }
        
        // Store initial direction
        if (rb != null && rb.velocity != Vector2.zero)
        {
            currentDirection = rb.velocity.normalized;
        }
        
        // Make sure bullet collider is trigger
        if (bulletCollider != null)
            bulletCollider.isTrigger = true;
        
        Destroy(gameObject, lifetime);
    }
    
    void Update()
    {
        // Check if player is in attack state
        if (player != null && player.stateMachine != null)
        {
            string currentStateName = player.stateMachine.currentState.ToString();
            playerIsAttacking = currentStateName.Contains("Attack");
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Bullet hit: {other.gameObject.name} | Tag: {other.tag}");
        
        // Check if bullet hit the attack check AND player is attacking
        if (!isReflected && attackCheck != null && other.gameObject == attackCheck && playerIsAttacking)
        {
            Debug.Log(" Bullet hit attack check during attack! Reflecting...");
            ReflectBullet();
            return;
        }
        
        // Reflected bullet hitting boss
        if (isReflected && other.CompareTag("Boss"))
        {
            Debug.Log(" Reflected bullet hit boss!");
            
            CharacterStats stats = other.GetComponent<CharacterStats>();
            if (stats == null)
                stats = other.GetComponentInParent<CharacterStats>();
            
            if (stats != null)
            {
                stats.TakeDamage(reflectedDamage);
            }
            
            if (hitEffect != null)
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            
            Destroy(gameObject);
        }
        // Normal bullet hitting player
        else if (!isReflected && other.CompareTag("Player"))
        {
            Debug.Log("✅ Bullet hit player!");
            
            CharacterStats stats = other.GetComponent<CharacterStats>();
            if (stats == null)
                stats = other.GetComponentInParent<CharacterStats>();
            
            if (stats != null)
            {
                stats.TakeDamage(damage);
            }
            
            if (hitEffect != null)
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            
            Destroy(gameObject);
        }
        // Hit platforms only (no wall tag)
        else if (other.CompareTag("Platform"))
        {
            Debug.Log("Bullet hit platform");
            
            if (hitEffect != null)
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            
            Destroy(gameObject);
        }
    }
    
    void ReflectBullet()
    {
        if (isReflected) return;
        
        isReflected = true;
        
        // Reverse direction
        currentDirection = -currentDirection;
        
        // Apply new velocity
        if (rb != null)
        {
            float currentSpeed = rb.velocity.magnitude;
            rb.velocity = currentDirection * (currentSpeed * reflectionSpeedMultiplier);
        }
        
        // Change target to boss
        targetTag = "Boss";
        
        // Visual feedback
        if (spriteRenderer != null)
            spriteRenderer.color = Color.blue;
        
        transform.localScale *= 1.2f;
        
        Debug.Log("Bullet reflected during player attack!");
    }
}
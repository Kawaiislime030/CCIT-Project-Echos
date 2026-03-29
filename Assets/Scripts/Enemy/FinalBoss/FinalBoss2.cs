using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss2 : Enemy
{
    #region States
    public FinalBossIdleState idleState { get; private set; }
    public FinalBossAttackState attackState { get; private set; }
    public FinalBossDeadState deadState { get; private set; }
    #endregion
    
    [Header("Target")]
    public Transform player;
	public float bossAttackRange = 15f;
	public float bossAttackCooldown = 3f;
    
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    

    protected override void Awake()
    {
        base.Awake();
        idleState = new FinalBossIdleState(this, stateMachine, "Idle");
        attackState = new FinalBossAttackState(this, stateMachine, "Attack");
        deadState = new FinalBossDeadState(this, stateMachine, "Die");
    }

    protected override void Start()
    {
        base.Start();
        
        // Set idle time
        idleTime = 2f;
        
        // Find player if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
        
        // Create fire point if not assigned
        if (firePoint == null)
        {
            firePoint = new GameObject("FirePoint").transform;
            firePoint.SetParent(transform);
            firePoint.localPosition = new Vector3(1f, 0f, 0f);
        }

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        if (stats.currentHealth <= 0)
        {
            stateMachine.ChangeState(deadState);
        }
    }

    public void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // Create bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
    }

    public bool PlayerInAttackRange()
    {
        if (player == null) return false;
        float distance = Vector2.Distance(transform.position, player.position);
        return distance <= bossAttackRange;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bossAttackRange);
    }
}
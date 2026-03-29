using UnityEngine;

public class FinalBossDeadState : EnemyState
{
    private FinalBoss2 boss;
    private bool portalSpawned = false;

    public FinalBossDeadState(FinalBoss2 _boss, EnemyStateMachine _stateMachine, string _animBoolName) 
        : base(_boss, _stateMachine, _animBoolName)
    {
        boss = _boss;
    }

    public override void Enter()
    {
        base.Enter();
        
        // Disable boss collider (use 'cd' from Enemy)
        if (boss.cd != null)
            boss.cd.enabled = false;
        
        // Stop movement and disable physics
        if (boss.rb != null)
        {
            boss.rb.velocity = Vector2.zero;
            boss.rb.simulated = false;
        }
        
        // Play death animation
        if (boss.anim != null)
            boss.anim.SetTrigger("Die");
    }

    public override void Update()
    {
        base.Update();

        if (!portalSpawned)
        {
            portalSpawned = true;
            
            // Find and activate portal
            sceneChanger portal = Object.FindObjectOfType<sceneChanger>();
            if (portal != null)
            {
                portal.ActivatePortal();
                Debug.Log("Portal activated!");
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
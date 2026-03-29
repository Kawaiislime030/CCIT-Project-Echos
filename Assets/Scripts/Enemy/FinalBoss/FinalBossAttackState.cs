using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossAttackState : EnemyState
{
    private FinalBoss2 boss;
    private float attackTimer;

    public FinalBossAttackState(FinalBoss2 _boss, EnemyStateMachine _stateMachine, string _animBoolName) 
        : base(_boss, _stateMachine, _animBoolName)
    {
        boss = _boss;
    }

    public override void Enter()
    {
        base.Enter();
        attackTimer = 0f; // Shoot immediately on enter
        Debug.Log("Boss entered Attack State");
    }

    public override void Update()
    {
        base.Update();

        attackTimer -= Time.deltaTime;

        // Shoot when timer reaches 0
        if (attackTimer <= 0)
        {
            boss.Shoot(); // Call the shoot method
            attackTimer = boss.bossAttackCooldown; // Reset timer
            Debug.Log("Boss shot a bullet!");
        }

        // Go back to idle if player leaves range
        if (!boss.PlayerInAttackRange())
        {
            stateMachine.ChangeState(boss.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
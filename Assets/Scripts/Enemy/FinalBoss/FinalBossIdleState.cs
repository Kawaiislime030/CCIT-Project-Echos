using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossIdleState : EnemyState
{
    private FinalBoss2 boss;
    private float idleTimer;

    public FinalBossIdleState(FinalBoss2 _boss, EnemyStateMachine _stateMachine, string _animBoolName) 
        : base(_boss, _stateMachine, _animBoolName)
    {
        boss = _boss;
    }

    public override void Enter()
    {
        base.Enter();
        idleTimer = boss.idleTime; // Using parent's idleTime
        Debug.Log("Boss entered Idle State");
    }

    public override void Update()
    {
        base.Update();

        idleTimer -= Time.deltaTime;

        // Transition to attack when player in range and idle time done
        if (boss.PlayerInAttackRange() && idleTimer <= 0)
        {
            stateMachine.ChangeState(boss.attackState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
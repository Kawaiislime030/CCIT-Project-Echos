using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgehogAttackState : EnemyState
{
    private Enemy_Hedgehog enemy;

    public HedgehogAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Hedgehog _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.lastTimeAttacked = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
}

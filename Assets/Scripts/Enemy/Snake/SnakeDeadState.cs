using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeDeadState : EnemyState
{
    private Enemy_Snake enemy;
    public SnakeDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Snake _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();
    }
}

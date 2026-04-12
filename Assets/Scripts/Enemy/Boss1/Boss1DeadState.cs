using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1DeadState : EnemyState
{
    private Boss1 enemy;
    public Boss1DeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Boss1 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
        enemy.animator.SetTrigger("Dead");
    }

    

    public override void Update()
    {
        base.Update();
        enemy.SetZeroVelocity();
    }
}


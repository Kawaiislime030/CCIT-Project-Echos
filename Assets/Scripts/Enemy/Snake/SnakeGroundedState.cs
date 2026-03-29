using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGroundedState : EnemyState
{
    protected Enemy_Snake enemy;
    protected Transform player; 

    public SnakeGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Snake enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Player").transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDectected() || Vector2.Distance(enemy.transform.position , player.position) < 2)
            stateMachine.ChangeState(enemy.battleState);
    }
}

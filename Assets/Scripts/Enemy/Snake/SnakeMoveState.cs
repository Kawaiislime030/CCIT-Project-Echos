using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMoveState : SnakeGroundedState
{
    public SnakeMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Snake enemy) : base(_enemyBase, _stateMachine, _animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
	Debug.Log("Enter Move State");
    }

    public override void Exit()
    {
        base.Exit();
	Debug.Log("Exit Move State");

    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir , rb.velocity.y);

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
        }

        
    }
}

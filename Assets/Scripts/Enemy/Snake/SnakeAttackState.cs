using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeAttackState : EnemyState
{
    private Enemy_Snake enemy;

    public SnakeAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Snake enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
	Debug.Log("Enter Attack State");
    }

    public override void Exit()
    {
        Debug.Log("Exit Attack State");
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
        
    }


    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if(triggerCalled){
	    Debug.Log("Trigger Called, go back to battleState");
            stateMachine.ChangeState(enemy.battleState);
	}

    }

	 
	

    
}

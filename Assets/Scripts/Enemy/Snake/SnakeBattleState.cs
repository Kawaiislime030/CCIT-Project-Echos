using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnakeBattleState : EnemyState
{

    private Transform player;
    private Enemy_Snake enemy;
    private int moveDir;

    public SnakeBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Snake enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.Find("Player").transform;
        stateTimer = enemy.battleTime;
	Debug.Log("ENTER Battle State");

    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDectected())
	{

            stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDectected().distance < enemy.attackDistance)
            {

                if (CanAttack()){
                    stateMachine.ChangeState(enemy.attackState);
		    }
            }
        }
        else
        {
	    //Attack time ends or far away from enemy
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10) {
		stateMachine.ChangeState(enemy.moveState);
		}
        }

        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
	else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    public override void Exit()
    {
        
        base.Exit();
    }

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
	    Debug.Log("Can Attack");
            return true;
        }
        Debug.Log("Attack on Cooldown");
        return false;
        


    }
}

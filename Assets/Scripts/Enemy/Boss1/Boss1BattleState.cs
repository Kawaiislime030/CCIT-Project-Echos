using System.Collections;
using UnityEngine;

public class Boss1BattleState : EnemyState
{
    private Transform player;
    private Boss1 enemy;

    public Boss1BattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Boss1 _enemy)
        : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        Debug.Log("Boss entered BattleState");
    }

    public override void Update()
    {
        base.Update();

        // 原地偵測玩家是否在攻擊範圍
        Collider2D hit = Physics2D.OverlapCircle(enemy.attackCheck.position, enemy.attackCheckRadius, enemy.whatIsPlayer);
        if (hit != null)
        {
            Debug.Log("Player detected in attack range");

            if (CanAttack())
            {
                int choice = Random.Range(0, 2); // 0 或 1
                if (choice == 0)
                {
                    Debug.Log("Boss chose Roar");
                    stateMachine.ChangeState(enemy.roarState);
                }
                else
                {
                    Debug.Log("Boss chose Bullet");
                    stateMachine.ChangeState(enemy.bulletState);
                }
            }


        }
    }

    public override void Exit()
    {
        base.Exit();
        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.battleState); 
        }

    }

    private bool CanAttack()
    {
        bool ready = Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown;
        Debug.Log("CanAttack = " + ready);
        if (ready)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}

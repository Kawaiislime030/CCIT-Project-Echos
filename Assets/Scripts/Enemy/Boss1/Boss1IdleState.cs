using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1IdleState : EnemyState
{
    private Boss1 enemy;
    public Boss1IdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Boss1 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
        enemyBase.anim.SetBool("Idle", true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.V))
            stateMachine.ChangeState(enemy.roarState);
        
        
    }
}

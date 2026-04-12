using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);  
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<BulletController>() != null)
            {
                hit.GetComponent<BulletController>().FlipArrow();
                SuccessfulCounterAttack();
            }
            if (hit.GetComponent<BossBullet>() != null)
            {
                hit.GetComponent<BossBullet>().FlipArrow(); // 你需要在 BossBullet.cs 裡寫 FlipArrow()
                SuccessfulCounterAttack();
            }

            if (hit.GetComponent<Enemy>() != null)
            {
          
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    SuccessfulCounterAttack();
                    EnemyStats _target = hit.GetComponent<EnemyStats>();

                    player.stats.DoDamage(_target);

                }

               
            }


        }

        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
        private void SuccessfulCounterAttack()
    

        {
         stateTimer = 10; //any value bigger than 1
         player.anim.SetBool("SuccessfulCounterAttack", true);
         }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1RoarState : EnemyState
{
    private Boss1 boss;

    private float roarTimer;

    public Boss1RoarState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Boss1 _boss)
        : base(_enemyBase, _stateMachine, _animBoolName)
    {
        boss = _boss;
    }

    public override void Enter()
    {
        base.Enter();
        boss.roarEffect.SetActive(true); // Roar 特效
        BossManager.instance.ShowHint("Hide under mushroom!", 2f);
        roarTimer = 2f;
    }

    public override void Update()
    {
        base.Update();
        roarTimer -= Time.deltaTime;

        if (roarTimer <= 0f)
        {
            if (SafeZone.playerIsSafe)
            {
                BossManager.instance.ShowHint("You Dodged Roar", 2f);
                boss.OpenAttackWindow();
            }
            else
            {
                BossManager.instance.ShowHint("You Got Roared", 2f);
                boss.KnockbackPlayer();
            }

            stateMachine.ChangeState(boss.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        boss.roarEffect.SetActive(false); // 離開時關閉 Roar 特效
    }
}

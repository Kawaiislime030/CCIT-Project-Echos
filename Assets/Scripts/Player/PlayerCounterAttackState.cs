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

		//canCreateClone = true;
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
			
			if(hit.GetComponent<BulletController>() != null){
				Debug.Log("Counter attack hit bullet!");
				hit.GetComponent<BulletController>().FlipArrow();
				SuccessfulCounterAttack();
			}
				
				
				
            if (hit.GetComponent<Enemy>() != null)
            {
                
                stateTimer = 10;
                player.anim.SetBool("SuccessfulCounterAttack", true);      

                
            }
                

        }

        if(stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);

    }
	
	private void SuccessfulCounterAttack()
	{
		stateTimer = 10; //any value bigger than 1
		player.anim.SetBool("SuccessfulCounterAttack",true);
	}
}

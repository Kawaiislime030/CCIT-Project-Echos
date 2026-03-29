using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Snake : Enemy
{

    #region States 
    public SnakeMoveState moveState { get; private set; }
    public SnakeBattleState battleState { get; private set; }
    public SnakeAttackState attackState { get; private set; }
	public SnakeStunnedState stunnedState { get; private set; }
    public SnakeDeadState deadState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        moveState = new SnakeMoveState(this, stateMachine, "Move", this);
        battleState = new SnakeBattleState(this, stateMachine, "Move", this);
        attackState = new SnakeAttackState(this, stateMachine, "Attack", this);
		stunnedState = new SnakeStunnedState(this, stateMachine, "Stunned", this);
		deadState = new SnakeDeadState(this, stateMachine, "Die", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(moveState);
    }

    protected override void Update()
    {
        base.Update();
    }
	public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
		
	}
}

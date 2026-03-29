using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Hedgehog : Enemy
{
    #region States
    public HedgehogIdleState idleState { get; private set; }
    public HedgehogMoveState moveState { get; private set; }
    public HedgehogBattleState battleState { get; private set; }
    public HedgehogAttackState attackState { get; private set; }
    public HedgehogStunnedState stunnedState { get; private set; }
    public HedgehogDeadState deadState { get; private set; }


    #endregion
    protected override void Awake()
    {
        base.Awake();

        idleState = new HedgehogIdleState(this, stateMachine, "Idle", this);
        moveState = new HedgehogMoveState(this, stateMachine, "Move", this);
        battleState = new HedgehogBattleState(this, stateMachine, "Move", this);
        attackState = new HedgehogAttackState(this, stateMachine, "Attack", this);
        stunnedState = new HedgehogStunnedState(this, stateMachine, "Stunned", this);
        deadState = new HedgehogDeadState(this, stateMachine, "Die", this);

    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
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

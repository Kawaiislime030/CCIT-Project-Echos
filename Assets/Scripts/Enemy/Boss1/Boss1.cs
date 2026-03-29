using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Enemy
{
    #region States

    public Animator animator { get; private set; }
    public Boss1IdleState idleState { get; private set; }
    public Boss1RoarState roarState { get; private set; }
    public Boss1DeadState deadState { get; private set; }
    public Boss1BattleState battleState { get; private set; }
    public Boss1Bullet bulletState { get; private set; }
    #endregion


    public float roarDuration = 3f;
    public GameObject roarEffect;



    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();

        idleState = new Boss1IdleState(this, stateMachine, "Idle", this);
        roarState = new Boss1RoarState(this, stateMachine, "Roar", this);
        deadState = new Boss1DeadState(this, stateMachine, "Idle", this);
        battleState = new Boss1BattleState(this, stateMachine, "Attack", this);
        bulletState = new Boss1Bullet(this, stateMachine, "Bullet", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public void Shooting()
    {
        //float y = Random.
    }
    public void StartBattle()
    {
        stateMachine.Initialize(battleState);
    }

    public void OpenAttackWindow()
    {
        // 玩家可以攻擊 Boss，例如打開弱點
        // 你可以加一個 bool 讓玩家攻擊有效
    }
    public void KnockbackPlayer()
    {
        if (SafeZone.playerIsSafe) return;
        // 找到 Player → 加一個 Force 或位移
        PlayerManager.instance.player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-500, 300));


    }

}

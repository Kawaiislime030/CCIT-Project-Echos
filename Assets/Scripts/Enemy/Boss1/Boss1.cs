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
    public Boss1BulletState bulletState { get; private set; }
    #endregion


    public float roarDuration = 3f;
    public GameObject roarEffect;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;


    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();

        idleState = new Boss1IdleState(this, stateMachine, "Idle", this);
        roarState = new Boss1RoarState(this, stateMachine, "Roar", this);
        deadState = new Boss1DeadState(this, stateMachine, "Die", this);
        battleState = new Boss1BattleState(this, stateMachine, "Attack", this);
        bulletState = new Boss1BulletState(this, stateMachine, "Bullet", this);
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

    public void ShootBullet()
    {
        Vector2 dirToPlayer = (PlayerManager.instance.player.transform.position - shootPoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.GetComponent<BossBullet>().Setup(dirToPlayer.x > 0 ? 1 : -1); 
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

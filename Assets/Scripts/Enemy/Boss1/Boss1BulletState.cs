using UnityEngine;

public class Boss1BulletState : EnemyState
{
    private Boss1 enemy;
    private int bulletCount = 0;
    private int maxBullets = 5;
    private float shootInterval = 1;
    private float shootTimer;
    private float waitAfterShoot = 5;
    private bool finishedShooting = false;



    public Boss1BulletState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Boss1 _enemy)
        : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        bulletCount = 0;
        shootTimer = shootInterval;
        waitAfterShoot = 5f;
        finishedShooting = false;
        BossManager.instance.ShowHint("Press Q to Counter Attack!!", 2f);
    }

    public override void Update()
    {
        base.Update();
        enemy.SetZeroVelocity();

        if (!finishedShooting)
        {
            shootTimer -= Time.deltaTime;

            if (shootTimer <= 0f && bulletCount < maxBullets)
            {
                enemy.ShootBullet();
                bulletCount++;
                shootTimer = shootInterval;
            }

            if (bulletCount >= maxBullets)
            {
                finishedShooting = true;
            }
        }
        else
        {
            waitAfterShoot -= Time.deltaTime;

            if (waitAfterShoot <= 0f)
            {
                stateMachine.ChangeState(enemy.battleState); 
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

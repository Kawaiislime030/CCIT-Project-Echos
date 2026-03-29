using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Bullet : EnemyState
{
    private Boss1 enemy;
    public Boss1Bullet(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Boss1 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    
}

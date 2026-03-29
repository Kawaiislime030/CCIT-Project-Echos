using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    public int xpReward = 25;

    protected override void Start()
    {
        base.Start();

        enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int _damage)
    {
            base.TakeDamage(_damage);
    
            enemy.DamageImpact();
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
        CharacterStats playerStats = PlayerManager.instance.player.GetComponent<CharacterStats>();
        playerStats.GainXP(xpReward);
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(1.5f); // adjust to match animation length
        Destroy(gameObject);
    }

}

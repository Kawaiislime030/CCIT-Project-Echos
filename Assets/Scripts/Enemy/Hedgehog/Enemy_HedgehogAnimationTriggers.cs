using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HedgehogAnimationTriggers : MonoBehaviour
{
    private Enemy_Hedgehog enemy => GetComponentInParent<Enemy_Hedgehog>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            PlayerStats _target = hit.GetComponent<PlayerStats>();
            if (_target != null)
            {
                Debug.Log("Enemy hit player, applying damage");
                enemy.stats.DoDamage(_target);
            }
        }



    }

    private void OpenCounterWindowTrigger() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindowTrigger() => enemy.CloseCounterAttackWindow();
}

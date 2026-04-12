using UnityEngine;

public class RoarEffect : MonoBehaviour
{
    private Boss1 enemy => GetComponentInParent<Boss1>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
           
            if (hit.GetComponent<Player>() != null && !SafeZone.playerIsSafe)
            {
                PlayerStats _target = hit.GetComponent<PlayerStats>();

                enemy.stats.DoDamage(_target);

            }
        }

    }

}

using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 5f;
    private int direction;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Setup(int facingDir)
    {
        direction = facingDir;
        rb.velocity = Vector2.right * direction * speed;
    }

    private void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 打到玩家
        PlayerStats _target = collision.GetComponent<PlayerStats>();
        if (_target != null)
        {
            // 對玩家造成傷害
            _target.TakeDamage(20);
            Debug.Log("Bullet hit player");
            Destroy(gameObject);
        }

        // 打到 Boss
        Boss1 boss = collision.GetComponent<Boss1>();
        if (boss != null && boss.stats != null)
        {
            boss.stats.TakeDamage(50); // 傳入傷害數值
            boss.stateMachine.ChangeState(boss.battleState);
            Debug.Log("Bullet reflected back to Boss");
            Destroy(gameObject);
        }
        

    }
    public void FlipArrow()
    {
        direction *= -1;
        rb.velocity = Vector2.right * direction * speed;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* REMARK:  This is the bullet with Counter Attack (Press Q) */


public class BulletController : MonoBehaviour{
	
    [SerializeField] private int damage;
	[SerializeField] private string targetLayerName = "Player";
	
	[SerializeField] private float speed;
	[SerializeField] private Rigidbody2D rb;
	
	[SerializeField] private bool canMove;
	[SerializeField] private bool flipped;
	private int direction = -1;
	
	private void Update(){
		if (canMove){
			rb.velocity = new Vector2(speed * direction, rb.velocity.y);
		}
	}
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
		{
			collision.GetComponent<CharacterStats>()?.TakeDamage(damage);
			Destroy(gameObject);
			//StuckInto(collision);
		}
		else if (collision.gameObject.layer == LayerMask.NameToLayer("W2_Platform"))
			//StuckInto(collision);
			Destroy(gameObject);
	}
	
	
	private void StuckInto(Collider2D collision)
	{
		
		canMove = false;
		rb.isKinematic = true;
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
		transform.parent = collision.transform;
		
		Destroy(gameObject,Random.Range(5,7));
	}
	
	
	public void FlipArrow(){
		if (flipped){
			Debug.Log("Bullet already flipped, ignoring");
			return;
		}
		
		damage *= 2; //higher damage when reflect (optional)
		speed *= 6; //higher speed when reflect (optional)
		direction *= -1;
		Debug.Log($"Current velocity: {rb.velocity}, speed variable: {speed}");
		flipped = true;
		transform.Rotate(0,180,0);
		targetLayerName = "Enemy";
	}
}
	
	
	
	
	
	
	
	
	
	
	
	
	


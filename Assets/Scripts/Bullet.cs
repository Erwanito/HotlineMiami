using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public float			speed;
	public Rigidbody2D		rb;
	public Vector2			direction;
	public Vector2			origin;
	public float			range;

	protected void Awake ()
	{
		float				angle;
		Vector2 			newVector;
		rb = GetComponent<Rigidbody2D> ();
		newVector = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
		angle = Vector2.Angle (Vector2.up, newVector);
		if (newVector.x < 0)
			transform.localRotation = Quaternion.Euler(0, 0, angle + 90.0f);
		else
			transform.localRotation = Quaternion.Euler(0, 0, -angle + 90.0f);
	}

	virtual protected void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Enemy" || collision.tag == "Player") {
			collision.gameObject.GetComponent<Character>().Death();
			Destroy (this.gameObject);
		} else if (collision.tag == "Wall") {
			Destroy (this.gameObject);
		}
	}

	protected virtual void Update () 
	{
		if (range < 0 || Vector2.Distance(origin, transform.localPosition) < range)
			rb.AddForce (direction * speed, ForceMode2D.Impulse);
		if (GetComponent<SpriteRenderer>().sprite.name == "12")
			transform.localScale = new Vector2 (transform.localScale.x, transform.localScale.y + 0.1f);
		if (Vector2.Distance (origin, transform.localPosition) >= range && range > 0)
			Destroy (this.gameObject);
	}
}

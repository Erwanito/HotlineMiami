using UnityEngine;
using System.Collections;

public class Toss : Bullet {
	public Weapon weapon;

	private void OnDestroy () {
		weapon.transform.localRotation = Quaternion.identity;
		weapon.transform.localPosition = this.transform.localPosition;
		weapon.gameObject.layer = LayerMask.NameToLayer("Default");
		weapon.spriteRenderer.enabled = true;
	}

	override protected void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log(collision.tag);
		if (collision.tag == "Enemy") {
			if (this.weapon.type == Weapon.Type.closeCombat)
				collision.gameObject.GetComponent<Character>().Death();
			else
				collision.gameObject.GetComponent<Enemy>().Stun();
		}
		if (collision.tag == "Enemy" || collision.tag == "Wall")
			Destroy (this.gameObject);
	}

	override protected void Update () {
		if (range < 0 || Vector2.Distance (origin, transform.localPosition) < range)
		{
			rb.AddForce (direction * speed, ForceMode2D.Impulse);
		}
		if (Vector2.Distance (origin, transform.localPosition) >= range && range > 0)
		{
			Destroy (this.gameObject);
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {
	public Sprite		equipped;
	public Sprite		onGround;
	public int			ammos;
	public float		rateOfFire;
	public Bullet		bullet;
	public Toss			tossed;
	public Weapon.Type	type;
	public string		name;
	public AudioClip[]	sounds;

	private float			_cooldown;
	private int				_default_layer;
	private static Dictionary<int, int>	_layers = new Dictionary<int, int>() {
		{ LayerMask.NameToLayer("Weapon"), LayerMask.NameToLayer("Default") },
		{ LayerMask.NameToLayer("PlayerLayer"), LayerMask.NameToLayer("PlayerProjectile") },
		{ LayerMask.NameToLayer("EnemyLayer"), LayerMask.NameToLayer("EnemyProjectile") }
	};

	// Component
	[HideInInspector]public SpriteRenderer	spriteRenderer;
	protected AudioSource					_source;
	protected BoxCollider2D					_collider;

	public enum Type { closeCombat, range };

	void Awake ()
	{
		this.spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		this._source = gameObject.GetComponent<AudioSource>();
		this._collider = gameObject.GetComponent<BoxCollider2D>();
		this._cooldown = 0;
		this._default_layer = this.gameObject.layer;
	}

	public void fire ()
	{
		if (_cooldown <= 0)
		{
			if (ammos > 0 || ammos == -1) {
				bullet.gameObject.layer = Weapon._layers[this.gameObject.layer];
				bullet.direction = transform.up * -1.0f;
				bullet.origin = transform.position;
				Instantiate (bullet, new Vector2(transform.position.x, transform.position.y), transform.rotation);
				_source.clip = this.sounds[0];
				_source.Play();
				if (this.ammos > 0)
					ammos--;
			} else {
				_source.clip = this.sounds[1];
				_source.Play();
			}
			_cooldown = this.rateOfFire;
		}
	}

	public void toss()
	{
		this._collider.enabled = true;
		this.spriteRenderer.sprite = this.onGround;
		this.spriteRenderer.sortingLayerName = "Default";
		this.spriteRenderer.enabled = false;
		this.transform.SetParent (null);
		this.gameObject.layer = this._default_layer;
		this.tossed.gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
		this.tossed.direction = transform.up * -1.0f;
		this.tossed.origin = transform.position;
		this.tossed.weapon = this;
		Instantiate (tossed, new Vector2(transform.position.x, transform.position.y), transform.rotation);
	}

	public void Equip(Transform owner) {
		this.spriteRenderer.sprite = this.equipped;
		this.spriteRenderer.sortingLayerName = "Player";
		this.spriteRenderer.sortingOrder = 2;
		this.transform.SetParent(owner, false);
		this.transform.localPosition = new Vector2 (-0.1f, -0.3f);
		this.gameObject.layer = owner.gameObject.layer;
		this._collider.enabled = false;
	}

	void Update ()
	{
		if (_cooldown > 0)
			_cooldown -= Time.deltaTime;
	}
}

using UnityEngine;
using System.Collections;

public class Enemy : Character {

	public float		stunDuration;
	public bool			patrol;

	private float		_stun;
	private bool		_stunned;
	private bool		_spotted;
	private float		_dist;
	private Player		_target;
	private Transform	_rayFront;
	private Transform	_rayBack;
	public Vector3[]	check;
	private int _i = 0;
	

	override protected void Awake () {
		base.Awake ();
		this.weapon = GameObject.Instantiate (this.weapon, Vector3.zero, Quaternion.identity) as Weapon;
		this.weapon.Equip (this.transform);
		this.weapon.ammos = -1;
	}

	override protected void Start () {
		base.Start();
		this._spotted = false;
		this._target = Player.player;
		this._target.WeaponNoise += this.OnNoiseListener;
		this._rayFront = this.transform.FindChild("RayFront");
		this._rayBack = this.transform.FindChild("RayBack");
		this._gameController.AddEnemy(this);
		this._stun = 0f;
		this._stunned = false;
	}
	

	void OnDestroy () {
		this._target.WeaponNoise -= this.OnNoiseListener;
		GameObject.Destroy(this.weapon);
	}

	void ft_patrol (){
		if (_spotted == false && !this._needToMove) {
			this.TurnTowards (check [_i]);
			this._targetPosition = check [_i];
			this._needToMove = true;
			_i++;
			if (_i == this.check.Length)
				_i = 0;
		} else if (Vector3.Distance (this.transform.localPosition, this._targetPosition) < 0.1) {
			this._needToMove = false;
		}
	}

	void Spot ()
	{
		Vector2 posdevant = new Vector2 (this._rayFront.position.x, this._rayFront.position.y);
		Vector2 posderriere = new Vector2 (this._rayBack.position.x, this._rayBack.position.y);
		RaycastHit2D hit = Physics2D.Raycast(posdevant, _target.transform.localPosition - transform.localPosition,  Mathf.Infinity);
		RaycastHit2D hit2 = Physics2D.Raycast(posderriere, _target.transform.localPosition - transform.localPosition,  Mathf.Infinity);
		if ((hit && hit.collider.tag == "Player" && _dist < 5f) ||
		    (hit2 && hit2.collider.tag == "Player" && _dist < 1.5f))
			_spotted = true;
	}

	public void OnNoiseListener () {
		if (this._dist < 7f)
			this._spotted = true;
	}

	public void Stun () {
		this._stun = this.stunDuration;
	}

	void Move ()
	{
		if (_dist > 10f) {
			_spotted = false;
			this._needToMove = false;
		} else {
			this.TurnTowards(this._target.transform.localPosition);
			this._targetPosition = this._target.transform.localPosition;
			this._needToMove = true;
			this.weapon.fire();
		}
	}

	override protected IEnumerator Dying()
	{
		this._gameController.RemoveEnemy(this);
		StartCoroutine (this.BlinkSprite ());
		yield return new WaitForSeconds (3.0f);
		Destroy (this.gameObject);
	}

	// Update is called once per frame
	override protected void Update () {
		if (!dead)
		{
			if (this._stun > 0) {
				this.doStun();
			} else {
				if (patrol == true)
					ft_patrol();
				_dist = Mathf.Sqrt (((transform.localPosition.x - _target.transform.localPosition.x) * (transform.localPosition.x - _target.transform.localPosition.x)) + ((transform.localPosition.y - _target.transform.localPosition.y) * (transform.localPosition.y - _target.transform.localPosition.y)));
				if (_spotted == false && _target.dead == false)
					Spot();
				else if (_spotted == true && _target.dead == false) {
					Move();
				}
			}

		}
		base.Update();
	}

	private void doStun() {
		this._stun -= Time.deltaTime;
		if (this._stun > 0 && !this._stunned) {
			for (int i = 0; i < this._spriteRenderer.Length; i++)
				this._spriteRenderer[i].color = Color.red;
			this._stunned = true;
		} else if (this._stun < 0 && this._stunned) {
			for (int i = 0; i < this._spriteRenderer.Length; i++)
				this._spriteRenderer[i].color = Color.white;
			this._stunned = false;
		}
	}
}

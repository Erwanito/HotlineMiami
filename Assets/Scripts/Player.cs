using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Character {
	// Component
	private Camera				_playerCamera;

	protected UIController		_uiController;
	
	private Dictionary<string, Vector3>	_move_direction = new Dictionary<string, Vector3>() {
		{"w", new Vector3(0, 0.5f, 0)},
		{"s", new Vector3(0, -0.5f, 0)},
		{"a", new Vector3(-0.5f, 0, 0)},
		{"d", new Vector3(0.5f, 0, 0)}
	};

	public delegate void			WeaponNoiseEvent();
	public event WeaponNoiseEvent	WeaponNoise = delegate {};

	public static Player	player;

	// Use this for initialization
	override protected void Awake () {
		base.Awake();
		Transform tmp = this.transform.FindChild("PlayerCamera");
		if (tmp)
			this._playerCamera = tmp.GetComponent<Camera>();
		this.weapon = null;
		Player.player = this;
	}

	override protected void Start () {
		base.Start();
		this._uiController = UIController.controller;
		this._uiController.SetWeaponName(this.weapon);
	}

	private void FaceMouse ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		this.TurnTowards(ray.origin);
		if (this._playerCamera)
			this._playerCamera.transform.localRotation = Quaternion.Euler (0, 0, -(_angle + 90));
	}

	private void Move () {
		this._targetPosition = this.transform.localPosition;

		foreach(KeyValuePair<string, Vector3> entry in this._move_direction) {
			if (Input.GetKey(entry.Key)) {
				this._targetPosition += entry.Value;
			}
		}
		this._needToMove = (this._targetPosition != this.transform.localPosition);
	}

	void ft_drop_weapon()
	{
		if (Input.GetMouseButtonUp (1) && this.weapon != null) {
			this.PlaySound(action[1]);
			this.weapon.toss();
			this.weapon = null;
			this._uiController.SetWeaponName(this.weapon);
		}
	}

	void OnTriggerStay2D(Collider2D collision)
	{
		if (Input.GetKeyDown ("e") && collision.tag == "Weapon" && this.weapon == null) {
			this.PlaySound(action[1]);
			this.weapon = collision.gameObject.GetComponent<Weapon>();
			this.weapon.Equip(this.transform);
			this._uiController.SetWeaponName(this.weapon);
		}
	}

	private void ft_fire_weapon()
	{
		if (Input.GetMouseButton (0) && this.weapon != null) {
			this.weapon.GetComponent<Weapon> ().fire();
			this._uiController.SetAmmos(this.weapon);
			if (this.weapon.type != Weapon.Type.closeCombat && this.weapon.ammos > 0)
				this.WeaponNoise();
		}
	}

	override protected IEnumerator Dying()
	{
		StartCoroutine (this.BlinkSprite ());
		yield return new WaitForSeconds (1.0f);
		this._gameController.Dead();
	}

	// Update is called once per frame
	override protected void Update () {
		if (!dead)
		{
			FaceMouse ();
			Move ();
			ft_drop_weapon ();
			ft_fire_weapon ();
		}
		base.Update();
	}
}

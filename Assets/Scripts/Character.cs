using UnityEngine;
using System.Collections;

abstract public class Character : MonoBehaviour {

	// public	
	public int					speed = 10;
	public Weapon				weapon;
	public AudioClip[]			action;

	// public but not in ispector
	[HideInInspector]public bool	dead;

	protected float				_angle = 0f;
	protected bool				_needToMove;
	protected Vector3			_targetPosition;
	protected GameController	_gameController;

	// Component
	protected Animator			_animator;
	protected AudioSource		_source;
	protected SpriteRenderer[]	_spriteRenderer;	

	virtual protected void Awake () {
		this._animator = this.gameObject.GetComponentInChildren<Animator>();
		this._source = this.gameObject.GetComponent<AudioSource>();
		this._spriteRenderer = this.gameObject.GetComponentsInChildren<SpriteRenderer>();
		this.dead = false;
	}

	virtual protected void Start () {
		this._gameController = GameController.controller;
	}

	virtual protected void Update () {
		this._animator.SetBool("move", this._needToMove);
	}

	virtual protected void FixedUpdate () {
		if (this._needToMove && !this.dead) 
			transform.position = Vector3.MoveTowards(this.transform.position, this._targetPosition , this.speed * Time.deltaTime);
	}

	public void TurnTowards (Vector3 target) {
		this._angle = Mathf.Atan ((target.y - this.transform.localPosition.y) / (target.x - this.transform.localPosition.x));
		if (target.x - this.transform.localPosition.x < 0 && target.y - this.transform.localPosition.y > 0)
			this._angle = 3.14f + _angle;
		if (target.x - this.transform.localPosition.x < 0 && target.y - this.transform.localPosition.y < 0)
			this._angle = - 3.14f + _angle;
		this._angle = this._angle * 180.0f / 3.14f;
		this.transform.localRotation = Quaternion.Euler (0, 0, _angle + 90);
	}

	public void MoveTowards (Vector3 target) {
		this._targetPosition = target;
	}

	public void Death ()
	{
		if (!this.dead)
		{
			this.dead = true;
			this.PlaySound(action[0]);
			StartCoroutine (this.Dying ());
		}
	}

	virtual protected IEnumerator Dying()
	{
		StartCoroutine (this.BlinkSprite ());
		yield return new WaitForSeconds (3.0f);
	}

	protected void PlaySound (AudioClip clip) {
		this._source.clip = clip;
		this._source.Play();
	}

	protected IEnumerator BlinkSprite()
	{
		while (true)
		{
			for (int i = 0; i < this._spriteRenderer.Length; i++)
				this._spriteRenderer[i].color = Color.clear;
			if (this.weapon)
				this.weapon.spriteRenderer.color = Color.clear;
			yield return new WaitForSeconds(0.1f);
			for (int j = 0; j < this._spriteRenderer.Length; j++)
				this._spriteRenderer[j].color = Color.white;
			if (this.weapon)
				this.weapon.spriteRenderer.color = Color.white;
			yield return new WaitForSeconds(0.1f);
		}
	}

}

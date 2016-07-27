using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

	public int			currentLevel;
	public int			nextLevel;

	private Transform	_GUI;
	private Transform	_looser;
	private Transform	_winner;

	// global
	private float		_rotation;
	private float		_position;
	private int			_selected;
	private Status		_status;
	private enum Status { GUI, Looser, Winner };
	private UIAnimatedText[]	_toAnimate;

	// GUI
	private Text	_weaponName;
	private Text	_ammos;

	public static UIController	controller;
	protected AudioSource					_source;
	public AudioClip[]						winlose;



	void Awake () {
		UIController.controller = this;
		this._selected = 0;
		this._status = UIController.Status.GUI;
		this._source = this.gameObject.GetComponent<AudioSource>();

		this._GUI = this.transform.GetChild(0).transform;
		this._weaponName = this._GUI.GetChild(0).GetComponent<Text>();
		this._ammos = this._GUI.GetChild(1).GetComponent<Text>();

		this._looser = this.transform.GetChild(1).transform;
		this._looser.gameObject.SetActive(false);

		this._winner = this.transform.GetChild(2).transform;
		this._winner.gameObject.SetActive(false);

		this._toAnimate = new UIAnimatedText[] {
			this._looser.GetChild(1).GetComponent<UIAnimatedText>(),
			this._looser.GetChild(2).GetComponent<UIAnimatedText>(),
			this._winner.GetChild(1).GetComponent<UIAnimatedText>(),
			this._winner.GetChild(2).GetComponent<UIAnimatedText>()
		};
	}

	void Update () {
		if (this._status == UIController.Status.Looser) {
			if (Input.GetKeyDown("w") || Input.GetKeyDown("up"))
				this.SelectNewOption(0);
			else if (Input.GetKeyDown("s") || Input.GetKeyDown("down"))
				this.SelectNewOption(1);
			else if (Input.GetKeyDown("return") || Input.GetKeyDown("e") || Input.GetKeyDown("space")) {
				this._toAnimate[this._selected].StopAnimation();
				if (this._selected == 0)
					Application.LoadLevel(this.currentLevel);
				else if (this._selected == 1)
					Application.LoadLevel(0);
			}
		} else if (this._status == UIController.Status.Winner) {
			if (Input.GetKeyDown("w") || Input.GetKeyDown("down"))
				this.SelectNewOption(2);
			else if (Input.GetKeyDown("s") || Input.GetKeyDown("down"))
				this.SelectNewOption(3);
			else if (Input.GetKeyDown("return") || Input.GetKeyDown("e") || Input.GetKeyDown("space")) {
				this._toAnimate[this._selected].StopAnimation();
				if (this._selected == 2)
					Application.LoadLevel(this.nextLevel);
				else if (this._selected == 3)
					Application.LoadLevel(0);
			}
		}
	}

	void SelectNewOption (int i) {
		this._toAnimate[this._selected].StopAnimation();
		this._selected = i;
		this._toAnimate[this._selected].StartAnimation();
	}
	
	public void SetLooser () {
		this._looser.gameObject.SetActive(true);
		this._winner.gameObject.SetActive(false);
		this._status = UIController.Status.Looser;
		this._looser.GetChild(0).transform.GetComponentInChildren<UIAnimatedText>().StartAnimation();
		this.SelectNewOption(0);
		_source.clip = winlose[1];
		_source.Play();
	}
	
	public void SetWinner () {
		this._winner.gameObject.SetActive(true);
		this._looser.gameObject.SetActive(false);
		this._status = UIController.Status.Winner;
		this._winner.GetChild(0).transform.GetComponentInChildren<UIAnimatedText>().StartAnimation();
		this.SelectNewOption(2);
		_source.clip = winlose[0];
		_source.Play();
	}

	public void SetWeaponName (Weapon weapon) {
		if (weapon == null)
			this._weaponName.text = "No Weapon";
		else
			this._weaponName.text = weapon.name;
		this.SetAmmos(weapon);
	}

	public void SetAmmos (Weapon weapon) {
		if (weapon == null)
			this._ammos.text = "-";
		else if (weapon.ammos == -1)
			this._ammos.text = "INF";
		else
			this._ammos.text = weapon.ammos.ToString();
	}
}

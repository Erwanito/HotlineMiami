using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FirstMenu : MonoBehaviour {

	public UIAnimatedText[]		toAnimate;
	public float				colorSpeed;
	public Color[]				colors;
	public Image				panel;

	private int					_selected;

	// Use this for initialization
	void Start () {
		this._selected = 1;
		this.toAnimate[0].StartAnimation();
		this.toAnimate[1].StartAnimation();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("w") || Input.GetKeyDown("up"))
			this.SelectNewOption(1);
		else if (Input.GetKeyDown("s") || Input.GetKeyDown("down"))
			this.SelectNewOption(2);
		else if (Input.GetKeyDown("return") || Input.GetKeyDown("e") || Input.GetKeyDown("space")) {
			this.toAnimate[this._selected].StopAnimation();
			if (this._selected == 1)
				Application.LoadLevel(1);
			else if (this._selected == 2)
				Application.Quit();
		}
		float t = Mathf.PingPong(Time.time, this.colorSpeed) / this.colorSpeed;
		this.panel.color = Color.Lerp(this.colors[0], this.colors[1], t);
	}

	
	void SelectNewOption (int i) {
		this.toAnimate[this._selected].StopAnimation();
		this._selected = i;
		this.toAnimate[this._selected].StartAnimation();
	}

	public void button_start(){
		Application.LoadLevel(1);
	}

	public void quit_button(){
		Application.Quit();
	}

}

using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public Color[]					colors;
	public float 					duration = 3.0F;
	public int						kill;
	public bool						end;
	public Weapon[]					weaponOnStart;
	public Vector3[]				weaponOnStartPosition;	

	private List<Enemy>				enemyList;
	private Camera					playerCamera;

	public static GameController	controller;

	private void Awake () {
		this.enemyList = new List<Enemy>();
		GameController.controller = this;
		this.playerCamera = GameObject.FindObjectOfType<Camera>();
		this.playerCamera.clearFlags = CameraClearFlags.SolidColor;
	}

	private void Start () {
		this.end = false;
		this.kill = 0;
		InitWeapons();
	}
	
	void Update() {
		float t = Mathf.PingPong(Time.time, this.duration) / this.duration;
		this.playerCamera.backgroundColor = Color.Lerp(this.colors[0], this.colors[1], t);
	}

	private void OnDestroy () {
		this.enemyList.Clear();
	}

	public void AddEnemy (Enemy toAdd) {
		this.enemyList.Add(toAdd);
	}

	public void RemoveEnemy (Enemy toAdd) {
		this.enemyList.Remove(toAdd);
		if (this.enemyList.Count == 0) {
			if (! this.end)
				UIController.controller.SetWinner();
			this.end = true;
		}
	}

	public void Dead () {
		if (! this.end)
			UIController.controller.SetLooser();
		this.end = true;
	}

	private void InitWeapons () {
		int		i;

		foreach (Vector3 pos in this.weaponOnStartPosition) {
			i = Random.Range(0, this.weaponOnStart.Length);
			GameObject.Instantiate(this.weaponOnStart[i], pos, Quaternion.identity);
		}
	}
}

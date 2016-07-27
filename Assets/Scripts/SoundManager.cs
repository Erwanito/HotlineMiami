using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	public static SoundManager		instance;
	public AudioSource				source;
	public AudioClip[]				levelMusics;
	public AudioClip[]				deaths;
	// Use this for initialization
	void Awake () {
		source.clip = levelMusics[Random.Range(0, levelMusics.Length)];
		source.Play ();
		source.loop = true;
	}

	public AudioClip Dead () {
		return deaths[Random.Range(0, deaths.Length)];
	}

	// Update is called once per frame
	void Update () {
	
	}
}

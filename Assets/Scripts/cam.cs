using UnityEngine;
using System.Collections;

public class cam : MonoBehaviour {


	public Player	current;

	// Update is called once per frame
	void Update () {

		if (current)
			transform.localPosition = new Vector3(current.transform.localPosition.x, current.transform.localPosition.y, -10);
	}

	public Texture2D cursorTexture;
	//public CursorMode cursorMode = CursorMode.Auto;
	//public Vector2 hotSpot = Vector2.zero;
	void OnMouseEnter() {
		Cursor.SetCursor(cursorTexture, new Vector2(5,0), CursorMode.Auto);
	}
	void OnMouseExit() {
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}

}
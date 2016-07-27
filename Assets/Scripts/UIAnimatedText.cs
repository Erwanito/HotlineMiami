using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIAnimatedText : MonoBehaviour {

	public Color[]			colors;
	public float			rotationAngle;
	public float			rotationStep;
	public float			translationMax;
	public float			translationStep;
	public float			colorSpeed;
	public float			animationSpeed;
	public int				levelId;
	private	Text			_sub;	
	private IEnumerator[]	_coroutine = new IEnumerator[] { null, null, null };

	public void StartAnimation () {
		this._sub = GameObject.Instantiate(this.transform.GetChild(0).GetComponent<Text>(), Vector3.zero, Quaternion.identity) as Text;
		this._sub.transform.SetParent(this.transform);
		this._sub.transform.localPosition = Vector3.zero;
		this._sub.transform.localScale = Vector3.one;
		this._sub.transform.SetAsFirstSibling();
		this._sub.color = this.colors[0];
		this._sub.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		this._coroutine[0] = Rotate();
		this._coroutine[1] = Translate();
		this._coroutine[2] = ChangeColor();
		foreach (IEnumerator anim in this._coroutine)
			StartCoroutine(anim);
	}

	public void StopAnimation () {
		foreach (IEnumerator anim in this._coroutine) {
			if (anim != null)
				StopCoroutine(anim);
		}
		this.transform.localRotation = Quaternion.identity;
		this._coroutine[0] = null;
		this._coroutine[1] = null;
		if (this._sub != null)
			GameObject.Destroy(this._sub.gameObject);
	}

	private IEnumerator Rotate () {
		float i = 0;
		Quaternion rot = Quaternion.identity;
		while (true) {
			while (i < this.rotationAngle) {
				rot.z = i;
				this.transform.localRotation = rot;
				yield return new WaitForSeconds(this.animationSpeed);
				i += this.rotationStep;
			}
			while (i > -this.rotationAngle) {
				rot.z = i;
				this.transform.localRotation = rot;
				yield return new WaitForSeconds(this.animationSpeed);
				i -= this.rotationStep;
			}
		}
	}

	private IEnumerator Translate () {
		float i = 0;
		Vector3 pos = Vector3.zero;
		while (true) {
			while (i < this.translationMax) {
				pos.x = i;
				this._sub.transform.localPosition = pos;
				yield return new WaitForSeconds(this.animationSpeed);
				i += this.translationStep;
			}
			while (i > -this.translationMax) {
				pos.x = i;
				this._sub.transform.localPosition = pos;
				yield return new WaitForSeconds(this.animationSpeed);
				i -= this.translationStep;
			}
		}
	}

	private IEnumerator ChangeColor () {
		while (true) {
			float t = Mathf.PingPong(Time.time, this.colorSpeed) / this.colorSpeed;
			this._sub.color = Color.Lerp(this.colors[0], this.colors[1], t);
			yield return new WaitForSeconds(this.animationSpeed);
		}
	}

	public void LoadLevel () {
		Application.LoadLevel(this.levelId);
	}
}

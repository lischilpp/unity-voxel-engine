using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreenClass {
	public GameObject gameObject;
	public Slider progres;
	public LoadingScreenClass(GameObject gameObject) {
		this.gameObject = gameObject;
		this.progres = gameObject.transform.Find("Progres").gameObject.GetComponent<Slider>();
	}

	public void Show() {
		this.gameObject.SetActive(true);
	}

	public void Hide() {
		this.gameObject.SetActive(false);
	}

	public void Toggle() {
		this.gameObject.SetActive(!this.gameObject.activeSelf);
	}

	public void SetProgres(float value) {
		this.progres.normalizedValue = value;
	}
}

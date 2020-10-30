using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {

	public Material defaultMaterial;
	public Material transparentMaterial;
	public Texture2D defaultTexture;

	public static LoadingScreenClass LoadingScreen;

	// Use this for initialization
	void Awake () {
		LoadingScreen = new LoadingScreenClass(GameObject.Find ("LoadingScreen"));
	}
}

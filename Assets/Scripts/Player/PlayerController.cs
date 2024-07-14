using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public bool freeFlyMode;
	public float moveSpeed = 0.1f;

	public static GameObject playerGO;
	
	void Start () {
		playerGO = GameObject.Find("Player");
		Debug.Log(playerGO);
		playerGO.SetActive(false);
	}

	
	// Update is called once per frame
	void Update () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public static void Spawn() {
		playerGO.SetActive(true);
		PlayerOptions.playerSpawned = true;
	}
}

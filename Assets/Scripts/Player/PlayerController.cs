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
		if (freeFlyMode) {
			if (Input.GetKey(KeyCode.W)) {
				playerGO.transform.position = new Vector3(
					playerGO.transform.position.x + playerGO.transform.forward.x * moveSpeed,
					playerGO.transform.position.y,
					playerGO.transform.position.z + playerGO.transform.forward.z * moveSpeed);
			}
			if (Input.GetKey(KeyCode.A)) {
				playerGO.transform.position = new Vector3(
					playerGO.transform.position.x - playerGO.transform.right.x * moveSpeed,
					playerGO.transform.position.y,
					playerGO.transform.position.z - playerGO.transform.right.z * moveSpeed);
			}
			if (Input.GetKey(KeyCode.S)) {
				playerGO.transform.position = new Vector3(
					playerGO.transform.position.x - playerGO.transform.forward.x * moveSpeed,
					playerGO.transform.position.y,
					playerGO.transform.position.z - playerGO.transform.forward.z * moveSpeed);
			}
			if (Input.GetKey(KeyCode.D)) {
				playerGO.transform.position = new Vector3(
					playerGO.transform.position.x + playerGO.transform.right.x * moveSpeed,
					playerGO.transform.position.y,
					playerGO.transform.position.z + playerGO.transform.right.z * moveSpeed);
			}
			if (Input.GetKey(KeyCode.Space)) {
				playerGO.transform.position = new Vector3(
					playerGO.transform.position.x,
					playerGO.transform.position.y + playerGO.transform.up.y * moveSpeed,
					playerGO.transform.position.z);
			}else if (Input.GetKey(KeyCode.LeftShift)) {
				playerGO.transform.position = new Vector3(
					playerGO.transform.position.x,
					playerGO.transform.position.y - playerGO.transform.up.y * moveSpeed,
					playerGO.transform.position.z);
			}
		}
	}

	public static void Spawn() {
		playerGO.SetActive(true);
		PlayerOptions.playerSpawned = true;
	}
}

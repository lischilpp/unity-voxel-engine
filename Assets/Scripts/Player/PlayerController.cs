using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	public static FPController fpController;

	void Start () {
		GameObject playerGO = GameObject.Find("Player");
		fpController = playerGO.GetComponent<FPController>();
	}
	
	public static void Spawn() {
		fpController.canMove = true;
		PlayerOptions.playerSpawned = true;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	public static GameObject playerGO;
	
	void Start () {
		playerGO = GameObject.Find("Player");
		playerGO.SetActive(false);
	}
	
	public static void Spawn() {
		playerGO.SetActive(true);
		PlayerOptions.playerSpawned = true;
	}
}

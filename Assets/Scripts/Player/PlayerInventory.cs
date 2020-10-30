using UnityEngine;
using System.Collections;

public class PlayerInventory: MonoBehaviour {

	public GameObject hotbarGO;

	public int size = 32;
	public static PlayerHotbar Hotbar;

	void Start() {
		Hotbar = new PlayerHotbar(ref hotbarGO, 8);
	}


	void Update() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			Hotbar.setActiveSlot(0);
		}else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			Hotbar.setActiveSlot(1);
		}else if (Input.GetKeyDown(KeyCode.Alpha3)) {
			Hotbar.setActiveSlot(2);
		}else if (Input.GetKeyDown(KeyCode.Alpha4)) {
			Hotbar.setActiveSlot(3);
		}else if (Input.GetKeyDown(KeyCode.Alpha5)) {
			Hotbar.setActiveSlot(4);
		}else if (Input.GetKeyDown(KeyCode.Alpha6)) {
			Hotbar.setActiveSlot(5);
		}else if (Input.GetKeyDown(KeyCode.Alpha7)) {
			Hotbar.setActiveSlot(6);
		}else if (Input.GetKeyDown(KeyCode.Alpha8)) {
			Hotbar.setActiveSlot(7);
		}

		if (Input.mouseScrollDelta.y != 0)
			Hotbar.switchSlot((Input.mouseScrollDelta.y != 1));
	}
}

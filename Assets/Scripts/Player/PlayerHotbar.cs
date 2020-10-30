using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHotbar {

	public Vector2 textureIndexToPosition(int i) {
		int y = Mathf.FloorToInt(i/Blocks.textureSize);
		int x = i - y * Blocks.textureSize;
		return new Vector2(x, y);
	}

	public int size;
	GameObject gameObject;
	//int[] slots;
	public int selectedSlot = 0;

	public PlayerHotbar(ref GameObject target, int size) {
		this.size = size;
		this.gameObject = target;
		//slots = new int[size];
	}

	public void setActiveSlot(int i) {
		gameObject.transform.GetChild(selectedSlot).GetComponent<RawImage>().color = new Color(255, 255, 255);
		selectedSlot = i;
		gameObject.transform.GetChild(i).GetComponent<RawImage>().color = new Color(255, 0, 0);
	}

	public void switchSlot(bool right) {
		int newSlot = selectedSlot;
		if (right) {
			if (newSlot < size -1 )
				newSlot++;
			else
				newSlot = 0;
		}else {
			if (newSlot > 0 )
				newSlot--;
			else
				newSlot = size - 1;
		}
		setActiveSlot(newSlot);
	}

	public void Update() {
		
	}
}

using UnityEngine;
using System.Collections;

public class ModifyTerrain : MonoBehaviour {
	
	public static GameObject cameraGO;

	void Start() {
		cameraGO = GameObject.FindGameObjectWithTag("MainCamera");
	}

	public void setBlockAt(int x, int y, int z, byte id) {
		if (VoxelTerrain.isInMapBoundaries(x,y,z)) {
			VoxelTerrain.data[x, y, z] = id;
			VoxelTerrain.chunkManager.updateChunkOfBlockPos(x, y, z);
			VoxelTerrain.chunkManager.updateChunkOfBlockPos(x, y + 1, z);
			VoxelTerrain.chunkManager.updateChunkOfBlockPos(x, y, z + 1);
			VoxelTerrain.chunkManager.updateChunkOfBlockPos(x + 1, y, z);
			VoxelTerrain.chunkManager.updateChunkOfBlockPos(x, y, z - 1);
			VoxelTerrain.chunkManager.updateChunkOfBlockPos(x - 1, y, z);
			VoxelTerrain.chunkManager.updateChunkOfBlockPos(x, y - 1, z);
		}
	}

	public void replaceBlockCursor(byte id) {
		Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			if (hit.distance < PlayerOptions.interactionRange) {
				Vector3 hitPos = hit.point + (hit.normal * (-0.5F));
				setBlockAt((int)hitPos.x, (int)hitPos.y, (int)hitPos.z, id);
			}
		}
	}

	public void addBlockCursor(byte id) {
		Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			if (hit.distance < PlayerOptions.interactionRange) {
				Vector3 hitPos = hit.point + (hit.normal * 0.5F);
				setBlockAt((int)hitPos.x, (int)hitPos.y, (int)hitPos.z, id);
			}
		}
	}

	public void sphericalDestroyBlocksAt(int x, int y, int z, int range) {
		int lx, ly, lz;
		for (lx = -range; lx <= range; lx++) {
			for (ly = -range; ly <= range; ly++) {
				for (lz = -range; lz <= range; lz++) {
					if (new Vector3(lx, ly, lz).sqrMagnitude < range) {
						setBlockAt(x + lx, y + ly, z + lz, 0);
					}
				}
			}
		}
	}

	public void shootProjectileCursor() {
		Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			if (hit.distance < 100) {
				Vector3 hitPos = hit.point + (hit.normal * (-0.5F));
				sphericalDestroyBlocksAt((int)hitPos.x, (int)hitPos.y, (int)hitPos.z, 16);
			}
		}
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			replaceBlockCursor(0);
		}
		if (Input.GetMouseButtonDown(1)) {
			addBlockCursor((byte)(PlayerInventory.Hotbar.selectedSlot + 1));
		}
		if (Input.GetMouseButtonDown(2)) {
			replaceBlockCursor(2);
		}
		if (Input.GetKey(KeyCode.F)) {
			shootProjectileCursor();
		}
	}
}

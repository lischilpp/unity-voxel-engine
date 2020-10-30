using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelTerrain : MonoBehaviour {

	public static Vector3i size = new Vector3i (128, 128, 128);
	public Material defaultMaterial;
	public Material transparentMaterial;
	public static GameObject player;
	public int chunkSize = 16;
	public static int chunkSizeShift;
	public GameObject playerGO;

	public static int[,,] data;
	public static ChunkManager chunkManager;
	public static TerrainGenerator terrainGenerator;
	public static Vector3i activeChunkPosition;
	
	void Start() {
		chunkSizeShift = (int)Mathf.Log (chunkSize, 2);
		data = new int[size.x, size.y, size.z];
		terrainGenerator   = new TerrainGenerator(chunkSize);
		chunkManager = new ChunkManager(this, playerGO);
	}

	void Update() {
		chunkManager.Update();
	}

	public static bool isInMapBoundaries(int x, int y, int z) {
		if (
			x >= 0 && x < size.x &&
			y >= 0 && y < size.y &&
			z >= 0 && z < size.z
		)  return true;
		return false;
	}

	public static int getBlockAt(int x, int y, int z) {
		if (isInMapBoundaries(x,y,z)) {
			return data[x, y, z];
		}
		return 0;
	}

	public void setBlockAt(int x, int y, int z, int id) {
		if (isInMapBoundaries(x,y,z))
			data[x, y, z] = id;
	}
	
}
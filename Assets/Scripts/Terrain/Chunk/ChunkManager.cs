using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ChunkState { None, Generated, Loaded };

public class ChunkManager {

	public VoxelTerrain voxelTerrain;

	public int chunkSize = 16;
	public Chunk[,,] chunkData;
	public int[,,] chunkStates;
	public bool[,,] inQueue;
	public ChunkLoadingQueue loadingQueue;

	public bool allChunksLoaded = false;
	public Vector3i chunkCount;

	public ChunkManager(VoxelTerrain voxelTerrain, GameObject playerGO) {
		this.voxelTerrain = voxelTerrain;
		this.chunkSize = voxelTerrain.chunkSize;
		chunkCount = new Vector3i(
			VoxelTerrain.size.x >> VoxelTerrain.chunkSizeShift,
			VoxelTerrain.size.y >> VoxelTerrain.chunkSizeShift,
			VoxelTerrain.size.z >> VoxelTerrain.chunkSizeShift
		);
		chunkData   = new Chunk[chunkCount.x, chunkCount.y, chunkCount.z];
		chunkStates = new int[chunkCount.x, chunkCount.y, chunkCount.z];
		inQueue     = new bool[chunkCount.x, chunkCount.y, chunkCount.z];


		loadingQueue = new ChunkLoadingQueue(this, playerGO);
	}

	public void generateChunk(int chunkX, int chunkY, int chunkZ) {
		VoxelTerrain.terrainGenerator.generateMapDataFor(
			chunkX << VoxelTerrain.chunkSizeShift,
			chunkY << VoxelTerrain.chunkSizeShift,
			chunkZ << VoxelTerrain.chunkSizeShift
		);
		chunkStates[chunkX, chunkY, chunkZ] = (int)ChunkState.Generated;
	}

	public void loadChunk(int x, int y, int z) {

		Vector3 pos = new Vector3(
			x << VoxelTerrain.chunkSizeShift,
			y << VoxelTerrain.chunkSizeShift,
			z << VoxelTerrain.chunkSizeShift
		);

		GameObject meshGO = new GameObject ("Chunk");
		meshGO.AddComponent<MeshFilter> ();
		meshGO.AddComponent<MeshRenderer>();
		meshGO.AddComponent<MeshCollider>();
		meshGO.AddComponent<Chunk>();

		Chunk chunk = meshGO.GetComponent<Chunk>();
		chunk.chunkX = (int)pos.x;
		chunk.chunkY = (int)pos.y;
		chunk.chunkZ = (int)pos.z;
		chunk.isBorderChunk = isBorderChunk(x, y, z);

		//chunk.VoxelTerrain = VoxelTerrain;
		chunk.transform.position = pos;
		meshGO.GetComponent<Renderer>().materials = new Material[] {
			voxelTerrain.defaultMaterial,
			voxelTerrain.transparentMaterial
		};
		chunkData [x, y, z] = chunk;
		chunkStates[x, y, z] = (int)ChunkState.Loaded;
	}

	public void unloadChunk(int x, int y, int z) {
		VoxelTerrain.Destroy(chunkData [x,y,z].transform.gameObject);
		chunkData [x,y,z] = null;
		chunkStates[x,y,z] = (int)ChunkState.Generated;
		inQueue[x,y,z] = false;
	}

	public void updateChunk(int x, int y, int z) {
		if (isChunkPosInBoundaries(x, y, z)) {
			if (chunkData[x, y, z] != null)
				chunkData[x, y, z].update = true;
		}
	}

	public void updateChunkOfBlockPos(int x, int y, int z) {
		Vector3i chunkPos = getChunkPosOfBlockPos(x, y, z);
		updateChunk(chunkPos.x, chunkPos.y, chunkPos.z);
	}

	public bool isChunkPosInBoundaries(int x, int y, int z) {
		if (x >= 0 && y >= 0 && z >= 0 && x < chunkCount.x && y < chunkCount.y && z < chunkCount.z)
			return true;
		return false;
	}

	public bool isBorderChunk(int x, int y, int z) {
		if (x == 0 || y == 0 || z == 0 || x == chunkCount.x - 1 || y == chunkCount.y - 1 || z == chunkCount.z - 1)
			return true;
		return false;
	}

	public Vector3i getChunkPosOfBlockPos(int x, int y, int z) {
		return new Vector3i(
			Mathf.FloorToInt(x >> VoxelTerrain.chunkSizeShift),
			Mathf.FloorToInt(y >> VoxelTerrain.chunkSizeShift),
			Mathf.FloorToInt(z >> VoxelTerrain.chunkSizeShift)
		);
	}

	public Chunk getChunkForBlockPos(int x, int y, int z) {
		return chunkData[
			Mathf.FloorToInt(x >> VoxelTerrain.chunkSizeShift),
			Mathf.FloorToInt(y >> VoxelTerrain.chunkSizeShift),
			Mathf.FloorToInt(z >> VoxelTerrain.chunkSizeShift)
		];
	}

	public void Update() {
		loadingQueue.Update();
	}
}

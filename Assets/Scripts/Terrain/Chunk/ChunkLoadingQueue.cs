using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChunkLoadingQueue {

	public ChunkManager chunkManager;

	List<Vector3i> queue = new List<Vector3i> ();
	public GameObject playerGO;

	public float loadingProgres = 0;
	public int chunksCount;
	float startTime;

	bool chunkRefresh = true;
	int chunkLoadsPerFrame = 12;

	public ChunkLoadingQueue(ChunkManager chunkManager, GameObject playerGO) {
		this.playerGO = playerGO;
		this.chunkManager = chunkManager;
		VoxelTerrain.activeChunkPosition = chunkManager.getChunkPosOfBlockPos((int)playerGO.transform.position.x, (int)playerGO.transform.position.y, (int)playerGO.transform.position.z);
		chunksCount = chunkManager.chunkCount.x * chunkManager.chunkCount.y * chunkManager.chunkCount.z;
		startTime = Time.time;
		chunkManager.voxelTerrain.StartCoroutine(loadChunksForPlayer());
	}

	public void addChunk(int x, int y, int z) {
		queue.Add(new Vector3i(x, y, z));
		chunkManager.inQueue[x, y, z] = true;
	}

	public void loadChunk() {
		if (queue.Count > 0) {
			Vector3i pos = queue[0];
			int x = pos.x;
			int y = pos.y;
			int z = pos.z;

			if (x + 1 < chunkManager.chunkCount.x) { // chunk above
				if (chunkManager.chunkStates[x + 1, y , z] == 0) {
					chunkManager.generateChunk(x + 1, y , z);
					return;
				}
			}
			if (x - 1 >= 0) {  // chunk left
				if (chunkManager.chunkStates[x - 1, y , z] == 0) {
					chunkManager.generateChunk(x - 1, y , z);
					return;
				}
			}
			if (y + 1 < chunkManager.chunkCount.y) { // chunk above
				if (chunkManager.chunkStates[x, y + 1, z] == 0) {
					chunkManager.generateChunk(x, y + 1, z);
					return;
				}
			}
			if (y - 1 >= 0) {  // chunk beneath
				if (chunkManager.chunkStates[x, y - 1 , z] == 0) {
					chunkManager.generateChunk(x, y - 1 , z);
					return;
				}
			}
			if (z + 1 < chunkManager.chunkCount.z) { // chunk behind
				if (chunkManager.chunkStates[x, y, z + 1] == 0) {
					chunkManager.generateChunk(x, y, z + 1);
					return;
				}
			}
			if (z - 1 >= 0) {  // chunk front
				if (chunkManager.chunkStates[x, y , z - 1] == 0) {
					chunkManager.generateChunk(x, y , z - 1);
					return;
				}
			}
			if (chunkManager.chunkStates[x, y, z] == 0) {
				chunkManager.generateChunk(x, y , z);
				return;
			}


			chunkManager.loadChunk(x, y, z);
			chunkManager.chunkStates[x,y,z] = (int)ChunkState.Loaded;
			chunkManager.inQueue[x, y, z] = false;
			queue.Remove (pos);

			if (!PlayerOptions.playerSpawned) {
				loadingProgres = (chunksCount - queue.Count)/ (float)chunksCount;
				PlayerGUI.LoadingScreen.SetProgres(loadingProgres);
			}

		}else {
			if (!chunkManager.allChunksLoaded) {
				chunkManager.allChunksLoaded = true;
				if (!PlayerOptions.playerSpawned) {
					PlayerGUI.LoadingScreen.Hide();
					PlayerController.Spawn();
					chunkLoadsPerFrame = 2;
					Debug.Log ("It took "+(Time.time - startTime)+"s to generate the terrain");

				}
			}
			chunkRefresh = false;
		}
	}

	public void Update() {
		Vector3i newPos = chunkManager.getChunkPosOfBlockPos((int)playerGO.transform.position.x, (int)playerGO.transform.position.y, (int)playerGO.transform.position.z);;
		if (newPos.x != VoxelTerrain.activeChunkPosition.x || newPos.y != VoxelTerrain.activeChunkPosition.y || newPos.z != VoxelTerrain.activeChunkPosition.z) {
			chunkRefresh = true;
			VoxelTerrain.activeChunkPosition = newPos;
		}
	}

	public IEnumerator loadChunksForPlayer() {
		int i, x, y, z,
			cCountX = chunkManager.chunkData.GetLength(0),
			cCountY = chunkManager.chunkData.GetLength(1),
			cCountZ = chunkManager.chunkData.GetLength(2);
		Vector3 activeChunkPosition;
		while(true) {
			if (chunkRefresh) {
				activeChunkPosition = new Vector3((float)VoxelTerrain.activeChunkPosition.x, (float)VoxelTerrain.activeChunkPosition.y, (float)VoxelTerrain.activeChunkPosition.z);
				//Debug.Log (activeChunkPosition);
				for (x = 0; x < cCountX; x++) {
					for (y = 0; y < cCountY; y++) {
						for (z = 0; z < cCountZ; z++) {
							float distanceToPos = Vector3.Distance(new Vector3(x, y, z), activeChunkPosition);
							if (chunkManager.chunkData[x, y, z] != null) {
								float distanceToChunk = Vector3.Distance(new Vector3(chunkManager.chunkData[x, y, z].chunkX / chunkManager.chunkSize, chunkManager.chunkData[x, y, z].chunkY / chunkManager.chunkSize, chunkManager.chunkData[x, y, z].chunkZ / chunkManager.chunkSize), activeChunkPosition);
								if (distanceToChunk > PlayerOptions.renderDistance) {
									if(chunkManager.chunkData[x, y, z] != null){
										chunkManager.unloadChunk(x, y, z);
									}else if (chunkManager.inQueue[x,y,z]) {
										chunkManager.inQueue[x,y,z] = false;
										queue.Remove(new Vector3i(x, y, z));
									}
								}
							}else {
								if (distanceToPos < PlayerOptions.renderDistance) {
									if (!chunkManager.inQueue[x, y, z]) {
										if (chunkManager.chunkStates[x, y, z] != (int)ChunkState.Loaded) {
											addChunk(x, y, z);
											if (chunkManager.allChunksLoaded)
												chunkManager.allChunksLoaded = false;
										}
									}
								}
							}
						}
					}
				}
				for (i=0; i<chunkLoadsPerFrame; i++) {
					loadChunk();
				}
			}
			yield return null;
		}
	}

}

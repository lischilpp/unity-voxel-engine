using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk : MonoBehaviour {

	// ### variables
	public MeshCreator meshCreator;

	public Mesh mesh;
	public MeshCollider collider;
	public Mesh colMesh;

	// position of the chunk
	public int chunkX;
	public int chunkY;
	public int chunkZ;

	// update chunk if necessary
	public bool update = false;
	public bool isBorderChunk; // faster way to get voxel data if false

	// ### functions

	public delegate int BlockDel(int x, int y, int z);
	public BlockDel Block;

	public bool isInBoundariesAndAir(int x, int y, int z) {
		int currentBlock = Block (x, y, z);
		BlockEntry blockData = Blocks.getBlockById(currentBlock);
		return (blockData.physics.isTranslucent);
	}

	public bool needToDrawAdjacendBlock(int x1, int y1, int z1, int x2, int y2, int z2) {
		int currentBlock = Block (x1, y1, z1);
		int adjacendBlock = Block (x2, y2, z2);
		BlockEntry adjacendBlockData = Blocks.getBlockById(adjacendBlock);
		bool isAir = (adjacendBlock == 0);
		if (isAir || adjacendBlockData.physics.isTranslucent ) {
			if (!adjacendBlockData.physics.willConnect || !isAir && currentBlock != adjacendBlock)
				return true;
		}
			
		return false;
	}
	
	public void GenerateMesh() {
		int x,y,z, currentBlock;
		for (x = 0; x < VoxelTerrain.chunkManager.chunkSize; x++) {
			for (y = 0; y < VoxelTerrain.chunkManager.chunkSize; y++) {
				for (z = 0; z < VoxelTerrain.chunkManager.chunkSize; z++) {
					currentBlock = Block(x,y,z);
					if (currentBlock != 0) {
						BlockEntry blockData = Blocks.getBlockById(currentBlock);
						if (isInBoundariesAndAir(x, y + 1, z)) {
							bool isVisible = needToDrawAdjacendBlock(x,y,z, x, y + 1, z);
							meshCreator.CubeTop(x, y, z, blockData.texture.top, isVisible, blockData.physics.isSolid, blockData.physics.isTransparent);
						}
						if (isInBoundariesAndAir(x, y, z - 1)) {
							bool isVisible = needToDrawAdjacendBlock(x,y,z, x, y, z - 1);
							meshCreator.CubeFront(x, y, z, blockData.texture.front, isVisible, blockData.physics.isSolid, blockData.physics.isTransparent);
						}
						if (isInBoundariesAndAir(x + 1, y, z)) {
							bool isVisible = needToDrawAdjacendBlock(x,y,z, x + 1, y, z);
							meshCreator.CubeRight(x, y, z, blockData.texture.right, isVisible, blockData.physics.isSolid, blockData.physics.isTransparent);
						}
						if (isInBoundariesAndAir(x, y, z + 1)) {
							bool isVisible = needToDrawAdjacendBlock(x,y,z, x, y, z + 1);
							meshCreator.CubeBack(x, y, z, blockData.texture.back, isVisible, blockData.physics.isSolid, blockData.physics.isTransparent);
						}
						if (isInBoundariesAndAir(x - 1, y, z)) {
							bool isVisible = needToDrawAdjacendBlock(x,y,z, x - 1, y, z);
							meshCreator.CubeLeft(x, y, z, blockData.texture.left, isVisible, blockData.physics.isSolid, blockData.physics.isTransparent);
						}
						if (isInBoundariesAndAir(x, y - 1, z)) {
							bool isVisible = needToDrawAdjacendBlock(x,y,z, x, y - 1, z);
							meshCreator.CubeBottom(x, y, z, blockData.texture.bottom, isVisible, blockData.physics.isSolid, blockData.physics.isTransparent);
						}
					}
				}
			}
		}

		meshCreator.updateMesh();
		meshCreator.updateCollider();
		meshCreator.clearMeshData();
	}

	void LateUpdate() {
		if (update) {
			//float startTime = Time.realtimeSinceStartup;
			GenerateMesh ();
			update = false;
			//Debug.Log ("Chunk update took " + (Time.realtimeSinceStartup - startTime) + "s");
		}
	}
	
	void Start () {
		mesh = GetComponent<MeshFilter> ().mesh;
		collider = GetComponent<MeshCollider> ();
		colMesh = new Mesh();
		if (isBorderChunk) {
			Block = delegate(int x, int y, int z) {;
				return VoxelTerrain.getBlockAt (
					x + chunkX,
					y + chunkY,
					z + chunkZ
				);
			};
		}else {
			Block = delegate(int x, int y, int z) {
				return VoxelTerrain.data[x + chunkX,
				                         y + chunkY,
				                         z + chunkZ];
			};
		}
		meshCreator = new MeshCreator(ref mesh, ref colMesh, ref collider);
		GenerateMesh ();
	}
}

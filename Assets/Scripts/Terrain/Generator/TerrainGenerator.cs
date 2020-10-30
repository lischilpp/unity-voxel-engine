using UnityEngine;
using System.Collections;

public class TerrainGenerator
{

	public int chunkSize;

	public TerrainGenerator(int chunkSize) { this.chunkSize = chunkSize; }

	public int getNoise2d(int x, int z, float scale, float mag, float exp)
	{
		return (int)Mathf.Pow((SimplexNoise.Noise.Generate((x / scale), (z / scale)) * mag), exp);
	}

	public int getOctaveNoise2d(int x, int z, float scale, float mag, float exp, int octaves)
	{
		float noiseValue = 0;
		for (int i = 0; i < octaves; i++)
		{
			noiseValue += Mathf.Pow((SimplexNoise.Noise.Generate((x / scale), (z / scale)) * mag), exp);
			mag *= 0.9f;
			scale *= 1.2f;
		}
		return (int)noiseValue;
	}

	public int getNoise3d(int x, int y, int z, float scale, float mag, float exp)
	{
		return (int)Mathf.Pow((SimplexNoise.Noise.Generate((x / scale), (y / scale), (z / scale)) * mag), exp);
	}

	public void generateMapDataFor(int px, int py, int pz)
	{
		int x, y, z;
		for (x = px; x < px + chunkSize; x++)
		{
			for (z = pz; z < pz + chunkSize; z++)
			{
				// layer noises
				int bedrock = 2;
				int seaLevel = 50;
				int stoneNoise = getNoise2d(x, z, 32, 4, 1);


				for (y = py; y < py + chunkSize; y++)
				{
					int stone = 50 + stoneNoise + getOctaveNoise2d(x, z, 32, 4, 1, 4) + getNoise3d(x, y, z, 48, 32, 1);

					int dirt = stone + 5;
					int grass = dirt + 3;
					if (y < bedrock)
						VoxelTerrain.data[x, y, z] = (int)BlockType.Bedrock;
					else if (y < stone)
						VoxelTerrain.data[x, y, z] = (int)BlockType.Stone;
					else if (y < dirt)
						VoxelTerrain.data[x, y, z] = (int)BlockType.Dirt;
					else if (y < grass)
						VoxelTerrain.data[x, y, z] = (int)BlockType.GrassBlock;
					else if (y < seaLevel)
						VoxelTerrain.data[x, y, z] = (int)BlockType.Water;
				}
			}
		}
	}

}

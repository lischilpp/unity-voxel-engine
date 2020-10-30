using UnityEngine;
using System.Collections;

public class BlockTexture {
	public Vector2 textureIndexToPosition(int i) {
		int y = Mathf.FloorToInt(i/Blocks.textureSize);
		int x = i - y * Blocks.textureSize;
		return new Vector2(x, y);
	}

	public Vector2 top, front, right, back, left, bottom;

	public BlockTexture(int _top, int _front, int _right, int _back, int _left, int _bottom) {
		top = textureIndexToPosition(_top);
		front = textureIndexToPosition(_front);
		right = textureIndexToPosition(_right);
		back = textureIndexToPosition(_back);
		left = textureIndexToPosition(_left);
		bottom = textureIndexToPosition(_bottom);
	}
}

public class BlockPhysics {
	public bool isSolid;
	public bool isTranslucent;
	public bool isTransparent = false;
	public bool willConnect = false;
	public float light = 0;
	public BlockPhysics(bool _isSolid, bool _isTranslucent) {
		isSolid = _isSolid;
		isTranslucent = _isTranslucent;
	}
	public BlockPhysics(bool _isSolid, bool _isTranslucent, bool _isTransparent) {
		isSolid = _isSolid;
		isTranslucent = _isTranslucent;
		isTransparent = _isTransparent;
	}
	public BlockPhysics(bool _isSolid, bool _isTranslucent, bool _isTransparent, bool _willConnect) {
		isSolid = _isSolid;
		isTranslucent = _isTranslucent;
		isTransparent = _isTransparent;
		willConnect = _willConnect;
	}
}

public class BlockEntry {
	public int id;
	public string name;
	public BlockTexture texture;
	public BlockPhysics physics;

	public BlockEntry(int _id, string _name, BlockTexture _texture, BlockPhysics _physics) {
		id = _id;
		name = _name;
		texture = _texture;
		physics = _physics;
	}
}

public enum BlockType: byte {
	Air,
	Bedrock,
	Stone,
	Dirt,
	GrassBlock,
	Wood,
	Leaves,
	Glass,
	Water,
	Sand,
	IronOre,
	SilverOre,
	GoldOre,
	Grass
}

public static class Blocks {
	
	public static int textureSize = 10;
	
	public static BlockEntry[] blockList = {
		new BlockEntry (0,  "Air",     new BlockTexture (0, 0, 0, 0, 0, 0), new BlockPhysics (false, true)),
		new BlockEntry (1,  "Bedrock", new BlockTexture (0, 0, 0, 0, 0, 0), new BlockPhysics (true, false)),
		new BlockEntry (2,  "Stone",   new BlockTexture (1, 1, 1, 1, 1, 1), new BlockPhysics (true, false)),
		new BlockEntry (3,  "Dirt",    new BlockTexture (2, 2, 2, 2, 2, 2), new BlockPhysics (true, false)),
		new BlockEntry (4,  "Grass Block",   new BlockTexture (4, 3, 3, 3, 3, 2), new BlockPhysics (true, false)),
		new BlockEntry (5,  "Wood",    new BlockTexture (6, 5, 5, 5, 5, 6), new BlockPhysics (true, false)),
		new BlockEntry (6,  "Leaves",  new BlockTexture (7, 7, 7, 7, 7, 7), new BlockPhysics (true, true)),
		new BlockEntry (7,  "Glass",   new BlockTexture (8, 8, 8, 8, 8, 8), new BlockPhysics (true, true, true, false)),
		new BlockEntry (8,  "Water",   new BlockTexture (9, 9, 9, 9, 9, 9), new BlockPhysics (false, true, true, true)),
		new BlockEntry (9,  "Sand",    new BlockTexture (10, 10, 10, 10, 10, 10), new BlockPhysics (true, false)),
		new BlockEntry (10, "Iron Ore",new BlockTexture (11, 11, 11, 11, 11, 11), new BlockPhysics (true, false)),
		new BlockEntry (11, "Silver Ore",new BlockTexture (12, 12, 12, 12, 12, 12), new BlockPhysics (true, false)),
		new BlockEntry (12, "Gold Ore",new BlockTexture (13, 13, 13, 13, 13, 13), new BlockPhysics (true, false))
	};
	
	public static BlockEntry getBlockById(int id) {
		return blockList[id];
	}
}
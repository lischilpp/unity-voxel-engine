using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshCreator {
	float tUnit = 0.1f;
	
	int verticesCount         = 0,
		colliderVerticesCount = 0;
	
	List<int> newTriangles            = new List<int>(),
			  newTransparentTriangles = new List<int>(),
			  newColliderTriangles    = new List<int>();

	List<Vector3> newVertices         = new List<Vector3>(),
				  newColliderVertices = new List<Vector3>();

	List<Vector2> newUV               = new List<Vector2>();

	Mesh mesh, colliderMesh;
	MeshCollider collider;


	public MeshCreator(ref Mesh mesh, ref Mesh colliderMesh, ref MeshCollider collider) {
		this.mesh = mesh;
		this.colliderMesh = colliderMesh;
		this.collider = collider;
	}

	public Mesh updateMesh() {
		mesh.Clear ();
		mesh.vertices = newVertices.ToArray ();
		mesh.subMeshCount = 2;
		mesh.SetTriangles(newTriangles.ToArray(), 0);
		mesh.SetTriangles(newTransparentTriangles.ToArray(), 1);
		mesh.uv = newUV.ToArray();
		mesh.RecalculateNormals ();
		return mesh;
	}

	public void updateCollider() {
		colliderMesh.Clear ();
		colliderMesh.vertices  = newColliderVertices.ToArray ();
		colliderMesh.triangles = newColliderTriangles.ToArray ();
		collider.sharedMesh = null;
		collider.sharedMesh = colliderMesh;
	}

	public void clearMeshData() {
		newVertices.Clear ();
		newTriangles.Clear ();
		newTransparentTriangles.Clear ();
		newUV.Clear ();
		newColliderVertices.Clear ();
		newColliderTriangles.Clear ();
		
		verticesCount = 0;
		colliderVerticesCount = 0;
	}

	public void addTriangles(ref List<int> target, int[] tris) {
		for (int i=0; i<tris.Length; i++) {
			target.Add(tris[i] + verticesCount);
		}
	}
	
	public void Square(Vector3[] vertices, Vector2 texture, bool invertUV, bool isTransparent) {
		
		for (int i = 0; i<vertices.Length; i++) {
			newVertices.Add( vertices[i]);
		}
		
		int[] triangles = new int[] {
			0, 1, 3,
			1, 2, 3
		};
		
		if (isTransparent)
			addTriangles(ref newTransparentTriangles , triangles);
		else
			addTriangles(ref newTriangles , triangles);
		
		
		Vector3 tPos = new Vector3 (tUnit * texture.x, tUnit * texture.y);
		
		if (invertUV) {
			newUV.Add(new Vector2 (tPos.x, tPos.y));
			newUV.Add(new Vector2 (tPos.x + tUnit, tPos.y));
			newUV.Add(new Vector2 (tPos.x + tUnit, tPos.y + tUnit));
			newUV.Add(new Vector2 (tPos.x, tPos.y + tUnit));
		}else {
			newUV.Add(new Vector2 (tPos.x, tPos.y));
			newUV.Add(new Vector2 (tPos.x, tPos.y + tUnit));
			newUV.Add(new Vector2 (tPos.x + tUnit, tPos.y + tUnit));
			newUV.Add(new Vector2 (tPos.x + tUnit, tPos.y));
		}
		
		verticesCount += 4;
	}
	
	public void addColliderTriangles(int[] tris) {
		for (int i=0; i<tris.Length; i++) {
			newColliderTriangles.Add(tris[i] + colliderVerticesCount);
		}
	}
	
	public void SquareCollider(Vector3[] vertices) {
		for (int i = 0; i<vertices.Length; i++) {
			newColliderVertices.Add( vertices[i]);
		}
		
		addColliderTriangles(new int[] {
			0, 1, 3,
			1, 2, 3
		});
		
		colliderVerticesCount += 4;
	}
	
	public void CubeTop(int x, int y, int z, Vector2 texture, bool isVisible, bool isSolid, bool isTransparent) {
		Vector3[] vertices = new Vector3[] {
			new Vector3 (x, y + 1, z),
			new Vector3 (x, y + 1, z + 1),
			new Vector3 (x + 1, y + 1, z + 1),
			new Vector3 (x + 1, y + 1, z)
		};
		if (isVisible)
			Square (vertices, texture, false, isTransparent);
		if (isSolid)
			SquareCollider(vertices);
	}
	
	public void CubeFront(int x, int y, int z, Vector2 texture, bool isVisible, bool isSolid, bool isTransparent) {
		Vector3[] vertices = new Vector3[] {
			new Vector3 (x, y, z),
			new Vector3 (x, y + 1, z),
			new Vector3 (x + 1, y + 1, z),
			new Vector3 (x + 1, y, z)
		};
		if (isVisible)
			Square (vertices, texture, false, isTransparent);
		if (isSolid)
			SquareCollider(vertices);
	}
	
	public void CubeRight(int x, int y, int z, Vector2 texture, bool isVisible, bool isSolid, bool isTransparent) {
		Vector3[] vertices = new Vector3[] {
			new Vector3 (x + 1, y, z),
			new Vector3 (x + 1, y + 1, z),
			new Vector3 (x + 1, y + 1, z + 1),
			new Vector3 (x + 1, y, z + 1)
		};
		if (isVisible)
			Square (vertices, texture, false, isTransparent);
		if (isSolid)
			SquareCollider(vertices);
	}
	
	public void CubeBack(int x, int y, int z, Vector2 texture, bool isVisible, bool isSolid, bool isTransparent) {
		Vector3[] vertices = new Vector3[] {
			new Vector3 (x, y, z + 1),
			new Vector3 (x + 1, y, z + 1),
			new Vector3 (x + 1, y + 1, z + 1),
			new Vector3 (x, y + 1, z + 1)
		};
		if (isVisible)
			Square (vertices, texture, true, isTransparent);
		if (isSolid)
			SquareCollider(vertices);
	}
	
	public void CubeLeft(int x, int y, int z, Vector2 texture, bool isVisible, bool isSolid, bool isTransparent) {
		Vector3[] vertices = new Vector3[] {
			new Vector3 (x, y, z),
			new Vector3 (x, y, z + 1),
			new Vector3 (x, y + 1, z + 1),
			new Vector3 (x, y + 1, z)
		};
		if (isVisible)
			Square (vertices, texture, true, isTransparent);
		if (isSolid)
			SquareCollider(vertices);
	}
	
	public void CubeBottom(int x, int y, int z, Vector2 texture, bool isVisible, bool isSolid, bool isTransparent) {
		Vector3[] vertices = new Vector3[] {
			new Vector3 (x, y, z),
			new Vector3 (x + 1, y, z),
			new Vector3 (x + 1, y, z + 1),
			new Vector3 (x, y, z + 1)
		};
		if (isVisible)
			Square (vertices, texture, true, isTransparent);
		if (isSolid)
			SquareCollider(vertices);
	}
	
}

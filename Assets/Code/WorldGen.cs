using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldGenLayer
{
	[SerializeField] public float perlinScale = 1.0f;
	[SerializeField] public float heightScale = 1.0f;
}

public class WorldGen
	:
	MonoBehaviour
{
	void Start()
	{
		perlinSeed = Random.Range( 0,99999 );

		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		meshColl = GetComponent<MeshCollider>();

		CreateShape();
		UpdateMesh();
	}

	void Update()
	{
		if( Input.GetKeyDown( KeyCode.R ) )
		{
			perlinSeed = Random.Range( 0,99999 );
			CreateShape();
			UpdateMesh();
		}
	}

	void CreateShape()
	{
		verts = new Vector3[( size + 1 ) * ( size + 1 )];
		cols = new Color[verts.Length];
	
		var hSize = ( size % 2 == 0 ? size : size + 1 ) / 2;
		for( int i = 0, z = -hSize; z <= hSize; ++z )
		{
			for( int x = -hSize; x <= hSize; ++x )
			{
				// var y = Random.Range( 0.0f,1.0f );
				var y = GetPerlinHeightVal( x,z );
	
				verts[i] = new Vector3( x,y,z );
				// cols[i] = GenerateRandCol();
				++i;
			}
		}
	
		tris = new int[size * size * 6];
	
		int v = 0;
		int t = 0;
		for( int z = 0; z < size; ++z )
		{
			for( int x = 0; x < size; ++x )
			{
				tris[t + 0] = v + 0;
				tris[t + 1] = v + size + 1;
				tris[t + 2] = v + 1;
				tris[t + 3] = v + 1;
				tris[t + 4] = v + size + 1;
				tris[t + 5] = v + size + 2;
	
				++v;
				t += 6;
			}
			++v;
		}
	}

	void UpdateMesh()
	{
		mesh.Clear();

		mesh.vertices = verts;
		mesh.triangles = tris;
		// mesh.colors = cols;

		mesh.RecalculateNormals();

		meshColl.sharedMesh = mesh;
	}

	Color GenerateRandCol()
	{
		var yellow = new Color(
			255.0f / 255.0f,
			219.0f / 255.0f,
			132.0f / 255.0f
			);
		var brown = new Color(
			161.0f / 255.0f,
			118.0f / 255.0f,
			68.0f / 255.0f
			);
		return( Color.Lerp( yellow,brown,Random.Range( 0.0f,1.0f ) ) );
	}

	float GetPerlinHeightVal( int x,int y )
	{
		x += perlinSeed;
		y += perlinSeed;

		var height = 0.0f;

		foreach( var layer in genLayers )
		{
			height += Mathf.PerlinNoise( x * layer.perlinScale,y * layer.perlinScale ) * layer.heightScale;
		}

		return( height );
	}

	Mesh mesh;
	MeshCollider meshColl;

	Vector3[] verts;
	int[] tris;
	Color[] cols;

	[SerializeField] int size = 20;

	int perlinSeed;
	[SerializeField] List<WorldGenLayer> genLayers = new List<WorldGenLayer>();
}
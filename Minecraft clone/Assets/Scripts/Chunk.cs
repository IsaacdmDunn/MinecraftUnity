using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{

	public ChunkCoord coord;

	MeshRenderer meshRenderer;
	MeshFilter meshFilter;
	GameObject chunkObject;

	MeshCollider meshCollider;

	int vertexIndex = 0;
	List<Vector3> vertices = new List<Vector3>();
	List<int> triangles = new List<int>();
	List<Vector2> uvs = new List<Vector2>();

	byte[,,] voxelMap = new byte[VoxelData.ChunkWidth, VoxelData.ChunkHeight, VoxelData.ChunkWidth];

	World world;

	public Chunk(ChunkCoord _coord, World _world)
	{

		coord = _coord;
		chunkObject = new GameObject();
		chunkObject.transform.position = new Vector3(coord.x * VoxelData.ChunkWidth, 0f, coord.z * VoxelData.ChunkWidth);

		meshRenderer = chunkObject.AddComponent<MeshRenderer>();
		meshFilter = chunkObject.AddComponent<MeshFilter>();
		meshCollider = chunkObject.AddComponent<MeshCollider>();
		world = _world;

		chunkObject.transform.SetParent(world.transform);
		meshRenderer.material = world.material;

		chunkObject.name = coord.x + ", " + coord.z;

		PopulateVoxelMap();
		CreateMeshData();
		CreateMesh();

		meshCollider.sharedMesh = meshFilter.mesh;
	}

	//gets/ set whether a chunk is active or not 
	public bool isActive
	{

		get { return chunkObject.activeSelf; }
		set { chunkObject.SetActive(value); }

	}

	//gets position for chunk
	Vector3 position
	{
		get { return chunkObject.transform.position; }

	}

	//checks if voxel is in chunk
	bool IsVoxelInChunk(int x, int y, int z)
	{

		if (x < 0 || x > VoxelData.ChunkWidth - 1 || y < 0 || y > VoxelData.ChunkHeight - 1 || z < 0 || z > VoxelData.ChunkWidth - 1)
			return false;
		else return true;

	}

	//sets blocks in chunk to a block type
	public void PopulateVoxelMap()
	{

		for (int y = 0; y < VoxelData.ChunkHeight; y++)
		{
			for (int x = 0; x < VoxelData.ChunkWidth; x++)
			{
				for (int z = 0; z < VoxelData.ChunkWidth; z++)
				{

					voxelMap[x, y, z] = world.GetVoxel(new Vector3(x, y, z) + position);

				}
			}
		}

	}

	//adds block mesh to chunk
	public void CreateMeshData()
	{

		for (int y = 0; y < VoxelData.ChunkHeight; y++)
		{
			for (int x = 0; x < VoxelData.ChunkWidth; x++)
			{
				for (int z = 0; z < VoxelData.ChunkWidth; z++)
				{
                    if (world.blocktypes[voxelMap[x,y,z]].isSolid)
                    {
						AddVoxelDataToChunk(new Vector3(x, y, z));
					}
				}
			}
		}

	}

	//gets voxel from map using position
	public byte GetVoxelFromMap(Vector3 pos)
	{

		pos -= position;

		return voxelMap[(int)pos.x, (int)pos.y, (int)pos.z];

	}

	//checks if voxel is in chunk
	bool CheckVoxel(Vector3 pos)
	{

		int x = Mathf.FloorToInt(pos.x);
		int y = Mathf.FloorToInt(pos.y);
		int z = Mathf.FloorToInt(pos.z);

		//if voxel is outside of chunk
		if (!IsVoxelInChunk(x, y, z))
			return world.blocktypes[world.GetVoxel(pos + position)].isSolid;

		return world.blocktypes[voxelMap[x, y, z]].isSolid;

	}

	//adds mesh data for voxel
	void AddVoxelDataToChunk(Vector3 pos)
	{

		for (int p = 0; p < 6; p++)
		{

			if (!CheckVoxel(pos + VoxelData.faceChecks[p]))
			{

				byte blockID = voxelMap[(int)pos.x, (int)pos.y, (int)pos.z];

				vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 0]]);
				vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 1]]);
				vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 2]]);
				vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 3]]);

				AddTexture(world.blocktypes[blockID].GetTextureID(p));

				triangles.Add(vertexIndex);
				triangles.Add(vertexIndex + 1);
				triangles.Add(vertexIndex + 2);
				triangles.Add(vertexIndex + 2);
				triangles.Add(vertexIndex + 1);
				triangles.Add(vertexIndex + 3);
				vertexIndex += 4;

			}
		}

	}

	//create and textures chunk mesh
	public void CreateMesh()
	{

		Mesh mesh = new Mesh();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.uv = uvs.ToArray();

		mesh.RecalculateNormals();

		meshFilter.mesh = mesh;

	}

	//adds texture to voxel
	void AddTexture(int textureID)
	{

		float y = textureID / VoxelData.TextureAtlasSizeInBlocks;
		float x = textureID - (y * VoxelData.TextureAtlasSizeInBlocks);

		x *= VoxelData.NormalizedBlockTextureSize;
		y *= VoxelData.NormalizedBlockTextureSize;

		y = 1f - y - VoxelData.NormalizedBlockTextureSize;

		uvs.Add(new Vector2(x, y));
		uvs.Add(new Vector2(x, y + VoxelData.NormalizedBlockTextureSize));
		uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y));
		uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y + VoxelData.NormalizedBlockTextureSize));


	}

}
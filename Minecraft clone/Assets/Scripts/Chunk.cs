using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    int vertexIndex = 0;
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> UVs = new List<Vector2>();

    public void Start()
    {
        AddVoxelDataToChunk(transform.position);
        GenerateMesh();
    }

    void AddVoxelDataToChunk(Vector3 pos)
    {
        for (int p = 0; p < 6; p++)
        {
            for (int i = 0; i < 6; i++)
            {
                int triangleIndex = VoxelData.voxelTris[p, i];
                vertices.Add(VoxelData.voxelVerts[triangleIndex] + pos);
                triangles.Add(vertexIndex);
                UVs.Add(VoxelData.voxelUVs[i]);

                vertexIndex++;
            }
        }
    }

    void GenerateMesh()
    {
        //creates mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = UVs.ToArray();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
}

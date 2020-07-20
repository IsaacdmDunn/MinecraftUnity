using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelData
{
    public static readonly int ChunkWidth = 5;
    public static readonly int ChunkHeight = 15;

    //creates an array of vertices for a cube
    public static readonly Vector3[] voxelVerts = new Vector3[8]
    {
        new Vector3(0f, 0f, 0f),
        new Vector3(1f, 0f, 0f),
        new Vector3(1f, 1f, 0f),
        new Vector3(0f, 1f, 0f),
        new Vector3(0f, 0f, 1f),
        new Vector3(1f, 0f, 1f),
        new Vector3(1f, 1f, 1f),
        new Vector3(0f, 01, 1f)
    };

    public static readonly Vector3[] faceChecks = new Vector3[6]
    {
        new Vector3(0f, 0f, -1f),
        new Vector3(0f, 0f, 1f),
        new Vector3(0f, 1f, 0f),
        new Vector3(0f, -1f, 0f),
        new Vector3(-1f, 0f, 0f),
        new Vector3(1f, 0f, 0f)
    };

    //creates an array of tris for a cube
    public static readonly int[,] voxelTris = new int[6,4]{
        { 0,3,1,2 }, //back
        { 5,6,4,7 }, //front face
        { 3,7,2,6 }, //top face
        { 1,5,0,4 }, //bottom face
        { 4,7,0,3 }, //left face
        { 1,2,5,6 } //right face
    };

    //creates an array of uv co-ords
    public static readonly Vector2[] voxelUVs = new Vector2[4]
    {
        new Vector2(0f, 0f),
        new Vector2(0f, 1f),
        new Vector2(1f, 0f),
        new Vector2(1f, 1f)
    };
}

using System.Collections.Generic;
using UnityEngine;

public class Minecraft_CubeMeshManager
{
    private const int AtlasSize = 16; // 16x16 atlas grid
    private const float uvUnit = 1f / AtlasSize;

    private Dictionary<CubeType, Mesh> meshCache;
    private static Dictionary<CubeType, CubeUV> cubeUVs;

    public Minecraft_CubeMeshManager()
    {
        meshCache = new Dictionary<CubeType, Mesh>();

        if (cubeUVs == null)
        {
            cubeUVs = new Dictionary<CubeType, CubeUV>()
            {
                { CubeType.Dirt, new CubeUV(new Vector2Int(2, 0), new Vector2Int(2, 0), new Vector2Int(2, 0))},
                { CubeType.Grass, new CubeUV(new Vector2Int(0, 0), new Vector2Int(3, 0), new Vector2Int(2, 0) )},
                { CubeType.Stone, new CubeUV(new Vector2Int(1, 1), new Vector2Int(1, 1), new Vector2Int(1, 1))},
                { CubeType.Sand, new CubeUV(new Vector2Int(2, 1), new Vector2Int(2, 1), new Vector2Int(2, 1))},
            };
        }
    }

    public Mesh GetMesh(CubeType type)
    {
        if (!meshCache.ContainsKey(type))
        {
            meshCache[type] = CreateCubeMesh(type);
        }

        return meshCache[type];
    }

    private Mesh CreateCubeMesh(CubeType type)
    {
        Mesh mesh = new Mesh();
        mesh.name = type.ToString() + "_CubeMesh";

        Vector3[] vertices = BuildCubeVertices();
        int[] triangles = BuildCubeTriangles();
        Vector2[] uvs = BuildCubeUVs(type);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    private Vector3[] BuildCubeVertices()
    {
        return new Vector3[]
        {
            //front
            new Vector3(0, 0, 1), //0
            new Vector3(1, 0, 1), //1
            new Vector3(1, 1, 1), //2
            new Vector3(0, 1, 1), //3

            //back
            new Vector3(1, 0, 0), //4
            new Vector3(0, 0, 0), //5
            new Vector3(0, 1, 0), //6
            new Vector3(1, 1, 0), //7

            //left
            new Vector3(0, 0, 0), //8
            new Vector3(0, 0, 1), //9
            new Vector3(0, 1, 1), //10
            new Vector3(0, 1, 0), //11

            //right
            new Vector3(1, 0, 1), //12
            new Vector3(1, 0, 0), //13
            new Vector3(1, 1, 0), //14
            new Vector3(1, 1, 1), //15

            //top
            new Vector3(0, 1, 1), //16
            new Vector3(1, 1, 1), //17
            new Vector3(1, 1, 0), //18
            new Vector3(0, 1, 0), //19

            //bottom
            new Vector3(0, 0, 0), //20
            new Vector3(1, 0, 0), //21
            new Vector3(1, 0, 1), //22
            new Vector3(0, 0, 1)  //23
        };
    }

    private int[] BuildCubeTriangles()
    {
        return new int[]
        {
            //front triangles
            0, 1, 2,
            0, 2, 3,

            //back triangles
            4, 5, 6,
            4, 6, 7,

            //left triangles
            8, 9, 10,
            8, 10, 11,

            //right triangles
            12, 13, 14,
            12, 14, 15,

            //top triangles
            16, 17, 18,
            16, 18, 19,

            //bottom triangles
            20, 21, 22,
            20, 22, 23
        };
    }

    private Vector2[] BuildCubeUVs(CubeType type)
    {
        CubeUV faceTiles = cubeUVs[type];
        Vector2[] uvs = new Vector2[24];

        //front
        ApplyFaceUVs(uvs, 0, faceTiles.side);

        //back
        ApplyFaceUVs(uvs, 4, faceTiles.side);

        //left
        ApplyFaceUVs(uvs, 8, faceTiles.side);

        //right
        ApplyFaceUVs(uvs, 12, faceTiles.side);

        //top
        ApplyFaceUVs(uvs, 16, faceTiles.top);

        //bottom
        ApplyFaceUVs(uvs, 20, faceTiles.bottom);

        return uvs;
    }

    private void ApplyFaceUVs(Vector2[] uvs, int startIndex, Vector2Int tile)
    {
        float xMin = tile.x * uvUnit;
        float yMin = (AtlasSize - 1 - tile.y) * uvUnit;
        float xMax = xMin + uvUnit;
        float yMax = yMin + uvUnit;

        uvs[startIndex + 0] = new Vector2(xMin, yMin); //bottom left
        uvs[startIndex + 1] = new Vector2(xMax, yMin); //bottom right
        uvs[startIndex + 2] = new Vector2(xMax, yMax); //top right
        uvs[startIndex + 3] = new Vector2(xMin, yMax); //top left
    }
}

using UnityEngine;

public class Minecraft_WorldGenerator : MonoBehaviour
{
    public int width = 25;
    public int length = 25;
    public int height = 3;

    public Minecraft_CubeSpawner cubeSpawner;

    void Awake()
    {
        if (cubeSpawner == null)
        {
            cubeSpawner = FindFirstObjectByType<Minecraft_CubeSpawner>();
        }
    }

    void Start()
    {
        GenerateWorld();
    }

    void GenerateWorld()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < length; z++)
                {
                    CubeType type = ChooseBlockType(x, y, z);
                    cubeSpawner.SpawnCube(new Vector3(x, y, z), type);
                }
            }
        }
    }

    CubeType ChooseBlockType(int x, int y, int z)
    {
        if (y == height - 1)
        {
            return CubeType.Grass;
        }

        if (y == 0)
        {
            return CubeType.Stone;
        }

        return CubeType.Dirt;
    }
}
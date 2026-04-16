using UnityEngine;

public class Minecraft_WorldGenerator : MonoBehaviour
{
    public int width = 20;
    public int length = 20;

    public int worldMinY = 0;
    public int baseHeight = 4;
    public int noiseHeight = 6;
    public int seed = 12345;
    public float noiseScale = 0.1f;

    public Minecraft_CubeSpawner cubeSpawner;

    private float seedOffsetX;
    private float seedOffsetZ;

    void Awake()
    {
        if (cubeSpawner == null)
        {
            cubeSpawner = FindFirstObjectByType<Minecraft_CubeSpawner>();
        }

        Random.InitState(seed);
        seedOffsetX = Random.Range(0f, 9999f);
        seedOffsetZ = Random.Range(0f, 9999f);
    }

    void Start()
    {
        GenerateWorld();
    }

    void GenerateWorld()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                int terrainHeight = GetTerrainHeight(x, z);

                for (int y = worldMinY; y <= terrainHeight; y++)
                {
                    CubeType type = ChooseBlockType(y, terrainHeight);
                    cubeSpawner.SpawnCube(new Vector3(x, y, z), type);
                }
            }
        }
    }

    int GetTerrainHeight(int x, int z)
    {
        float sampleX = seedOffsetX + x * noiseScale;
        float sampleZ = seedOffsetZ + z * noiseScale;

        float noiseValue = Mathf.PerlinNoise(sampleX, sampleZ);

        int heightOffset = Mathf.RoundToInt(noiseValue * noiseHeight);
        return baseHeight + heightOffset;
    }

    CubeType ChooseBlockType(int y, int terrainHeight)
    {
        if (y == terrainHeight)
            return CubeType.Grass;

        if (y >= terrainHeight - 2)
            return CubeType.Dirt;

        return CubeType.Stone;
    }
}
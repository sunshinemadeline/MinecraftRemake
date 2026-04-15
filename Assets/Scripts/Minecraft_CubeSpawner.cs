using UnityEngine;

public class Minecraft_CubeSpawner : MonoBehaviour
{
    public Material atlasMaterial;

    private Minecraft_CubeMeshManager meshManager;
    private int blockLayer;

    void Awake()
    {
        meshManager = new Minecraft_CubeMeshManager();

        blockLayer = LayerMask.NameToLayer("Block");
        if (blockLayer == -1)
        {
            blockLayer = 0;
            Debug.LogWarning("Layer 'Block' not found. Using Default layer.");
        }
    }

    public void SpawnCube(Vector3 position, CubeType type)
    {
        GameObject cube = new GameObject(type.ToString());
        cube.transform.position = position;
        cube.layer = blockLayer;
        cube.isStatic = true;

        MeshFilter mf = cube.AddComponent<MeshFilter>();
        mf.sharedMesh = meshManager.GetMesh(type);

        MeshRenderer mr = cube.AddComponent<MeshRenderer>();
        mr.sharedMaterial = atlasMaterial;

        BoxCollider bc = cube.AddComponent<BoxCollider>();
        bc.center = new Vector3(0.5f, 0.5f, 0.5f);
        bc.size = Vector3.one;
        bc.isTrigger = false;
    }
}
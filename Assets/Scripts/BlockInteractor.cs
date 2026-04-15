using UnityEngine;

public class BlockInteractor : MonoBehaviour
{
    public Camera playerCamera;
    public Minecraft_CubeSpawner cubeSpawner;
    public float interactDistance = 6f;
    public CubeType placementType = CubeType.Dirt;

    private LayerMask blockMask;

    void Start()
    {
        int blockLayer = LayerMask.NameToLayer("Block");
        if (blockLayer == -1)
        {
            blockLayer = 0;
            Debug.LogWarning("Layer 'Block' not found. Using Default layer.");
        }

        blockMask = 1 << blockLayer;

        if (cubeSpawner == null)
        {
            cubeSpawner = FindFirstObjectByType<Minecraft_CubeSpawner>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DestroyBlock();
        }

        if (Input.GetMouseButtonDown(1))
        {
            PlaceBlock();
        }
    }

    void DestroyBlock()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, blockMask))
        {
            Destroy(hit.collider.gameObject);
        }
    }

    void PlaceBlock()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, blockMask))
        {
            Vector3 placePosition = hit.collider.transform.position + hit.normal;

            placePosition = new Vector3(
                Mathf.Round(placePosition.x),
                Mathf.Round(placePosition.y),
                Mathf.Round(placePosition.z)
            );

            Vector3 checkCenter = placePosition + new Vector3(0.5f, 0.5f, 0.5f);
            Collider[] overlaps = Physics.OverlapBox(checkCenter, Vector3.one * 0.45f, Quaternion.identity, blockMask);

            if (overlaps.Length == 0)
            {
                cubeSpawner.SpawnCube(placePosition, placementType);
            }
        }
    }
}
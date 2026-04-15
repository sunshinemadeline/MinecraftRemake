using UnityEngine;

public class Minecraft_WorldQuery : MonoBehaviour
{
    public LayerMask blockMask;

    public bool HasBlock(Vector3Int gridPos)
    {
        Vector3 checkCenter = gridPos + new Vector3(0.5f, 0.5f, 0.5f);
        return Physics.CheckBox(checkCenter, Vector3.one * 0.45f, Quaternion.identity, blockMask);
    }

    public bool IsWalkable(Vector3Int gridPos)
    {
        bool hasGroundBelow = HasBlock(gridPos + Vector3Int.down);
        bool feetClear = !HasBlock(gridPos);
        bool headClear = !HasBlock(gridPos + Vector3Int.up);

        Debug.Log("Checking " + gridPos +
                  " | groundBelow: " + hasGroundBelow +
                  " | feetClear: " + feetClear +
                  " | headClear: " + headClear);

        return hasGroundBelow && feetClear && headClear;
    }
}
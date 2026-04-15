using UnityEngine;

public class Node : System.IComparable<Node>
{
    public Vector3Int position;
    public float gCost;
    public float hCost;
    public Node parent;

    public float fCost => gCost + hCost;

    public Node(Vector3Int pos, float g, float h, Node parent)
    {
        this.position = pos;
        this.gCost = g;
        this.hCost = h;
        this.parent = parent;
    }

    public int CompareTo(Node other)
    {
        int compare = fCost.CompareTo(other.fCost);

        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }

        return compare;
    }
}

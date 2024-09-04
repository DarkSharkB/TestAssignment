using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x, y;    // Property that stores the x and y axis indices of the tile [0 ... 10) when Grid Size is 10

    public Vector2Int Position { get {return new(x, y); } } // Getter to return the position of the tile as a Vector2Int
}

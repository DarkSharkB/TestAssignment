using Unity.VisualScripting;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject baseTile; // The GameObject used as a tile here
    public int gridSize = 10; // The size of the grid; Better don't change

    // A function that returns a Tile component of the GameObject that is hovered by the mouse
    public static Tile GetHoveredTileInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    // Creates a ray from mouse position on screen through the camera
        if(Physics.Raycast(ray, out var hitInfo))                       // Checks if the ray hits something
        {
            return hitInfo.collider.gameObject.GetComponent<Tile>();    // Returns the Tile Component from the hit GameObject which could return a Tile or null
        }
        return null;
    }

    // A function that returns a position of the GameObject that is hovered by the mouse
    public static Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    // Creates a ray from mouse position on screen through the camera
        if(Physics.Raycast(ray, out var hitInfo))                       // Checks if the ray hits something
        {
            return hitInfo.point;                                       // Returns the point where a GameObject is hit.
        }
        return Vector3.zero;                                            // Returns zero vector if nothing hits
    }

    // A function that generates the base grid
    private void GenerateBase()
    {
        int halfGrid = gridSize / 2;
        for(int i = -halfGrid; i < halfGrid; i++)
        {
            for(int j = -halfGrid; j < halfGrid; j++)
            {
                Debug.Log($"Instantiating tile at position: {i}, {j}");
                GameObject tileObject = Instantiate(
                    baseTile,
                    transform.position + new Vector3(i + 0.5f, 0.0f, j + 0.5f),
                    Quaternion.identity
                );                                              // Instantiate a baseTile GameObject at the position plus an offset of this GameObject and zero rotation
                tileObject.transform.parent = transform;        // Parents the instantiated GameObject to this GameObject
                Tile tile = tileObject.GetComponent<Tile>();    // Gets the tile component from the instantiated GameObject
                tile.x = i + halfGrid;   // Sets the x axis index of the tile
                tile.y = j + halfGrid;   // Sets the y axis index of the tile
            }
        }
    }

    private void Start()
    {
        GenerateBase(); // Generates the grid when the game starts
    }
}
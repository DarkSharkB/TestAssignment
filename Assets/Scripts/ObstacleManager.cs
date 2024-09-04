using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField]
    private ObstacleMapData mapData;
    [SerializeField]
    GameObject obstacleTile;

    public bool[] ObstacleMap => mapData.ObstacleMap; // Fancy getter for ObstacleMap

    private void GenerateObstacles()
    {
        int halfGrid = mapData.GridSize / 2;            // Half of Grid Size
        for(int i = -halfGrid; i < halfGrid; i++)       // Goes from -5 to 4 here
        {
            for(int j = -halfGrid; j < halfGrid; j++)   // Same as above loop
            {
                int x = i + halfGrid;                   // Calculates the x axis index of the tile
                int y = j + halfGrid;                   // Calculates the y axis index of the tile
                if(mapData.GetObstacle(x, y))
                {
                    GameObject tileObject = Instantiate(
                        obstacleTile,
                        transform.position + new Vector3(i + 0.5f, 0.0f, j + 0.5f), // The offset size 0.5 is for the 3D displacement for proper alignment
                        Quaternion.identity
                    ); // Instantiate a baseTile GameObject at the position plus an offset of this GameObject and zero rotation
                    tileObject.transform.parent = transform;       // Parents the instantiated GameObject to this GameObject
                    Tile tile = tileObject.GetComponent<Tile>();   // Gets the tile component from the instantiated GameObject
                    tile.x = x;   // Sets the x axis index of the tile
                    tile.y = y;   // Sets the y axis index of the tile
                }
            }
        }
    }

    void Start()
    {
        GenerateObstacles(); // Generates the obstacles when the game starts
    }
}

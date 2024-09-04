using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileInfoDisplayer : MonoBehaviour
{
    TMP_Text displayer;

    // Function which updates the text data on the displayer based on mouse hovering
    void DisplayPosition()
    {
        Tile tile = GridGenerator.GetHoveredTileInfo(); // Get hovered tile data; null if not tile
        displayer.text = "";                            // Resets the text to nothing
        if(tile != null)
        {
            displayer.text = "Tile Position: " + tile.gameObject.transform.position + "\nX: " + tile.x + " Y: " + tile.y; // Sets the text to tile position
        }
    }

    void Start()
    {
        displayer = GetComponent<TMP_Text>(); // Initialises the displayer with TMP_Text GameObject
    }

    void Update()
    {
        DisplayPosition(); // Function call to update display text
    }
}

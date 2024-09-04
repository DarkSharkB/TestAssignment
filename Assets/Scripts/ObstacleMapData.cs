using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

// To create a new ObstacleMapData, go to Create->Assets->Map->Obstacles and click on the new file to edit and store it
[CreateAssetMenu(fileName = "Data", menuName = "Map/Obstacles", order = 1)]
public class ObstacleMapData : ScriptableObject
{
    public int GridSize { get { return 10; } } // Getter for Grid Size
    [SerializeField]
    private bool[] isObstacle = new bool[100]; // The GridMap or ObstacleMap

    // Gets the GridMap data on position whether it has obstacle or not
    public bool GetObstacle(int x, int y)
    {
        return isObstacle[x + GridSize * y];
    }

    // Sets the GridMap on position with obstacle or not
    public void SetObstacle(int x, int y, bool value)
    {
        isObstacle[x + GridSize * y] = value;
    }

    public bool[] ObstacleMap => isObstacle; // Getter for The GridMap or ObstacleMap
}


[CustomEditor(typeof(ObstacleMapData))]
class ObstacleTool : Editor
{
    private SerializedObject serializedMapData; // Will hold the selected object
    private SerializedProperty isObstacleProperty; // Will hold the selected object's isObstacle property

    
    // Helper function which can be used to toggle all the tiles in grid with or without obstacles
    private void ToggleAll(bool value)
    {
        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                isObstacleProperty.GetArrayElementAtIndex(j * 10 + i).boolValue = value;
            }
        }
        serializedMapData.ApplyModifiedProperties();
        Repaint();
    }

    // Intialises the data before displaying on GUI
    private void OnEnable()
    {
        serializedMapData = new SerializedObject(target);                   // Initialises the map data with the selected ScriptableObject in the Editor
        isObstacleProperty = serializedMapData.FindProperty("isObstacle");  // Initialises with the isObstacle property of the selected ScriptableObject in the Editor
    }

    // Allow the drawing of the elements on the Editor GUI
    public override void OnInspectorGUI()
    {
        if(EditorApplication.isPlaying) // To avoid editing of the script at runtime
        {
            EditorGUILayout.LabelField("Unable to edit obstacles during runtime!");
            return;
        }

        serializedMapData.Update(); // Updates the data representation of the serialized map data to user changes in Editor

        EditorGUILayout.LabelField("Obstacles"); // Shows label "Obstacles"

        EditorGUILayout.BeginVertical();        // Begins the Verticle Section

        for(int i = 0; i < 10; i++)
        {
            EditorGUILayout.BeginHorizontal();  // Begins a Horizontal Section that shows a row of the GridMap set data

            for(int j = 0; j < 10; j++)
            {
                bool currentState = isObstacleProperty.GetArrayElementAtIndex(i + 10 * j).boolValue;    // Gets the data from the obstacle property at current i and j  location
                bool obstacleState = EditorGUILayout.Toggle(currentState);                              // Gets the data from the GUI
                if(obstacleState != currentState)
                {
                    isObstacleProperty.GetArrayElementAtIndex(i + 10 * j).boolValue = obstacleState;    // Sets the new data to the obstacle property at current i and j location
                }
            }

            EditorGUILayout.EndHorizontal();            // Ends a Horizontal Section of a row of GridMap data
        }

        EditorGUILayout.EndVertical();                  // Ends the Verticle Section
        
        EditorGUILayout.BeginHorizontal();              // Begins the Horizontal Section that will contain the buttons to toggle between all or none
        
        if(GUILayout.Button("Toggle On All Obstacle"))  // Toggle All
        {
            ToggleAll(true);
        }
        if(GUILayout.Button("Toggle Off All Obstacle")) // Toggle None
        {
            ToggleAll(false);
        }

        EditorGUILayout.EndHorizontal();                // Ends the Horizontal Section that will contain the buttons to toggle between all or none

        serializedMapData.ApplyModifiedProperties();    // Apply all the changes done to the ScriptableObject file
        if(GUI.changed)
        {
            EditorUtility.SetDirty(target);             // Allows the changes to be updated
        }
    }
}
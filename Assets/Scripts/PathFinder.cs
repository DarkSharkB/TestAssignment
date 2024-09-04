using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField]
    private ObstacleManager obstacleManager;

    private class Node // A class that will represent a node
    {
        public Vector2Int Position { get; }     // The position of the node
        public float CostSoFar { get; }         // The cost calculated so far

        public Node(Vector2Int position, float costSoFar)   // Initialiser for the Node
        {
            Position = position;
            CostSoFar = costSoFar;
        }
    }

    private class NodeComparer : IComparer<Node> // A implementation of Comparer interface for comparing Nodes 
    {
        public int Compare(Node a, Node b)
        {
            return a.CostSoFar.CompareTo(b.CostSoFar);
        }
    }

    // A function used to convert world position to grid position
    public static Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int((int)(worldPosition.x + 4.5f), (int)(worldPosition.z + 4.5f));
    }

    // A function used to convert grid position to world position
    public static Vector3 GridToWorldPosition(Vector2Int gridPosition, float yCoordinate = 1.5f)
    {
        return new Vector3(gridPosition.x - 4.5f, yCoordinate, gridPosition.y - 4.5f);
    }
    
    // A function that finds the closest path from and to the points given
    public List<Vector2Int> FindPath(Vector2Int from, Vector2Int to)
    {
        var frontier = new SortedSet<Node>(new NodeComparer());     // A sorted set to store nodes
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();    // A dictionary to store pairs of a point and it's neighbour
        var costSoFar = new Dictionary<Vector2Int, float>();        // A dictionary that stoes the calculated cost of the given point
        var totalCost = new Dictionary<Vector2Int, float>();        // A dictionary that stoes the total cost of the given point

        foreach(var pos in GetAllPositions())   // Initialises the cost and total cost dictionary with max value possible
        {
            costSoFar[pos] = float.MaxValue;
            totalCost[pos] = float.MaxValue;
        }

        frontier.Add(new Node(from, 0));                // Adds a start node
        costSoFar[from] = 0;                            // Sets the cost of start node
        totalCost[from] = ManhattanDistance(from, to);  // Sets the total cost of start node using Manhattan Distance as heuristic

        while(frontier.Count > 0)
        {
            var current = frontier.Min; // Get the lowest node
            frontier.Remove(current);   // Remove the lowest node

            if(current.Position == to)
            {
                return ReconstructPath(cameFrom, current.Position); // Returns recontructed path from the initial node to current position; Occurs only when current position(minimum node) and to position is equivalent. That is when all is done and path is found.
            }

            foreach(var neighbor in GetNeighbors(current.Position))
            {
                if(IsObstacle(neighbor)) continue;  // When there's an obstacle, move on to next iteration

                float tentativeGScore = costSoFar[current.Position] + 1;    // Get the score at current position and add one more to it to use as goal score

                if(tentativeGScore < costSoFar[neighbor])       // If cost calculated is lower than neighbours
                {
                    cameFrom[neighbor] = current.Position;      // Store the current position in neighbours path
                    costSoFar[neighbor] = tentativeGScore;      // And also the cost
                    totalCost[neighbor] = costSoFar[neighbor] + ManhattanDistance(neighbor, to); // Calculated Cost and an Heuristic

                    if(!frontier.Contains(new Node(neighbor, totalCost[neighbor]))) // Check whether the neighbour and cost pair is present
                    {
                        frontier.Add(new Node(neighbor, totalCost[neighbor]));      // Add if not
                    }
                }
            }
        }
        return new(); // No path found
    }

    // A function that returns the four neighbours of a given tile
    private List<Vector2Int> GetNeighbors(Vector2Int position)
    {
        var neighbors = new List<Vector2Int>(); // List to store neighbours
        var directions = new Vector2Int[] { new(0, 1), new(1, 0), new(0, -1), new(-1, 0) }; // Directions to get neighbours from

        foreach(var direction in directions)
        {
            var neighbor = new Vector2Int(position.x + direction.x, position.y + direction.y); // Assume all possible neighbours

            if(IsWithinBounds(neighbor))
            {
                neighbors.Add(neighbor); // Add if in bounds
            }
        }
        return neighbors;
    }

    // A function that checks whether the position is within bounds
    private bool IsWithinBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < 10 && position.y >= 0 && position.y < 10;
    }

    // A function that checks whether the position has obstacle or not
    private bool IsObstacle(Vector2Int position)
    {
        return obstacleManager.ObstacleMap[position.x + position.y * 10];
    }

    // A function that calculates the Manhattan distance between two points
    private float ManhattanDistance(Vector2Int a, Vector2Int b)
    {
        return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
    }

    // A functions that recreates the path that was present before using previous path nodes and current node
    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        var totalPath = new List<Vector2Int>{ current };    // Create a new list of just current position
        while(cameFrom.ContainsKey(current))                // If the previous nodes contain current position
        {
            current = cameFrom[current];                    // It is added to the list when it is a neighbour
            totalPath.Add(current);
        }
        totalPath.Reverse();                                // The path is reversed
        return totalPath;                                   // And is returned
    }

    // A function that returns all the positions in the Grid as an IEnumerable; No need for lists
    private IEnumerable<Vector2Int> GetAllPositions()
    {
        for(int y = 0; y < 10; y++)
        {
            for(int x = 0; x < 10; x++)
            {
                yield return new Vector2Int(x, y);
            }
        }
    }
}

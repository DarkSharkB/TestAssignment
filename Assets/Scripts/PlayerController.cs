using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 200f;
    public float gridCrossingTimeout = 0.01f;
    public float gridCrossingTime = 0f;
    public bool playerMoving = false;
    [SerializeField]
    private PathFinder pathFinder;
    private Queue<Vector2Int> travelPath = new();
    private Animator animator;

    // Finds the path to the position of Mouse Position on screen
    void FindPathToMousePosition()
    {
        if(Input.GetMouseButtonDown(0) && !playerMoving)
        {
            Vector2Int currentPosition = PathFinder.WorldToGridPosition(transform.position); // Gets the position of player in grid
            List<Vector2Int> path = pathFinder.FindPath(currentPosition, GridGenerator.GetHoveredTileInfo().Position); // Finds the path between the player position and mouse position
            foreach(var point in path)
            {
                travelPath.Enqueue(point);  // Puts all the found path values in the travelPath queue
            }
            playerMoving = true;            // The player is on move
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();   // Initialises a reference to Animator
    }

    void Update()
    {
        FindPathToMousePosition();                             // Calls to find the path through which the player character moves
        Vector3 movePoint = transform.position;         // Assigns the movePoint with current position
        if(gridCrossingTime <= 0)
        {
            if(travelPath.Count > 0)
            {
                movePoint = PathFinder.GridToWorldPosition(travelPath.Dequeue(), 0.5f); // Take a point from the calculated target path
                transform.LookAt(movePoint);           // To make player face the current movePoint
                animator.Play("Walking_A");            // Sets to walk animation
            }
            else
            {
                playerMoving = false;
                animator.Play("Idle");
            }
            gridCrossingTime = gridCrossingTimeout;    // Reset grid crossing timer; Used for better movement interpolation
        }
        transform.position = Vector3.Lerp(transform.position, movePoint, Time.deltaTime * moveSpeed); // Movement interpolation between current position and target position
        gridCrossingTime -= Time.deltaTime;            // Reduces timer value
    }
}

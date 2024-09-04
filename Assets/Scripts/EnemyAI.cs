using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IAI
{
    public float moveSpeed = 200f;
    public float gridCrossingTimeout = 0.01f;
    public float gridCrossingTimer = 0f;

    [SerializeField]
    private PathFinder pathFinder;
    [SerializeField]
    private Transform player;
    private Queue<Vector2Int> travelPath = new();
    private Animator animator;

    public bool ReachedPlayer { get; set; }
    public bool IsMoving { get; set; }

    // Finds a list of Vector2Int which is then enqueued into travelPath and is used later
    public void FindPathToPlayer()
    {
        if(!ReachedPlayer && !IsMoving)
        {
            Vector2Int currentPosition = PathFinder.WorldToGridPosition(transform.position); // Gets the current position of enemy on grid
            List<Vector2Int> path = pathFinder.FindPath(currentPosition, PathFinder.WorldToGridPosition(player.position)); // The path between enemy and player is calculated
            foreach(var point in path)
            {
                travelPath.Enqueue(point);  // Puts all the found path values in the travelPath queue
            }
            IsMoving = true;                // The enemy should move now
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();    // Initialises a reference to Animator
        IsMoving = false;                       // Initialises IsMoving
    }

    private void Update()
    {
        ReachedPlayer = Vector3.Distance(transform.position, player.position) < 2f; // Updates whether the Player is reached
        FindPathToPlayer();                             // Calls to find the path through which the enemy moves
        Vector3 movePoint = transform.position;         // Assigns the movePoint with current position
        if(gridCrossingTimer <= 0)
        {
            if(travelPath.Count > 1)                    // To avoid steping into Player's tile the count is checked whether it is greater than 1
            {
                movePoint = PathFinder.GridToWorldPosition(travelPath.Dequeue(), 0.5f); // Take a point from the calculated target path
                transform.LookAt(movePoint);            // To face enemy face the current movePoint
                animator.Play("Walking_A");             // Sets to walk animation
            }
            else
            {
                animator.Play("Idle");                  // Sets to idle animation
                IsMoving = false;                       // Set moving to false; Allows the calculation of next path
            }
            gridCrossingTimer = gridCrossingTimeout;    // Reset grid crossing timer; Used for better movement interpolation
        }
        transform.position = Vector3.Lerp(transform.position, movePoint, Time.deltaTime * moveSpeed); // Movement interpolation between current position and target position
        gridCrossingTimer -= Time.deltaTime;            // Reduces timer
    }
}

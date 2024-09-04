public interface IAI
{
    bool ReachedPlayer { get; set; }     // Property which is used to know whether the enemy has reached player
    bool IsMoving { get; set; }          // Property which is used to know whether the enemy is in move or not
    
    void FindPathToPlayer();            // Function to find the path to reach the player
}

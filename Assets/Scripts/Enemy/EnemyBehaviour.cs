using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    public enum EnemyState{
        Idle,
        Patrolling,
        Alert,
        Chase
    }

    //What is in the enemy behaviour?
    //STATES
    //Idle = It is standing still
    //Patrolling = It is moving along it's waypoints
    //Alert = IF it sees the player, but doesn't fill it's awareness, the enemy will move to where it last noticed the player
    //Chase =  It will move towards the player and follow them
    // Start is called before the first frame update

    //This is where the path of the enemy is stored when they patrol
    [SerializeField] private Waypoints waypoints;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private EnemyState currentState;

    [Tooltip("This is what determines how close the enemy should get to their waypoint before trying to move to the next one")]
    [Range(0.1f, 1f)]
    [SerializeField] private float distanceThreshold;

    private Transform currentWaypoint;
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //Initial position of first waypoint
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.position = currentWaypoint.position;

        //Set the next waypoint target
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case EnemyState.Patrolling:
            //When the enenmy is patrolling, they should move along their waypoints
            FollowWaypoint();
            break;
        }
        
    }

    void ChangeEnemyState(EnemyState newState)
    {

    }

    void FollowWaypoint()
    {
        rb.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, currentWaypoint.position) < distanceThreshold)
        {           
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        }
    }
}

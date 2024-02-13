using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

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
    public event Action PlayerSpotted;
    [SerializeField] private Waypoints waypoints;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private EnemyState currentState;
    public Transform headTransform;
    [SerializeField] public Player player;

    [Tooltip("This is what determines how close the enemy should get to their waypoint before trying to move to the next one")]
    [Range(0.1f, 1f)]
    [SerializeField] private float distanceThreshold;

    private Transform currentWaypoint;
    private Rigidbody rb;
    [SerializeField] private Detector detector;

    internal Vector3 playerLastSeen;
    [SerializeField] float currentAwareness = 0f;
    [SerializeField] float maxAwareness = 100f;

    internal float timeLastSawPlayer;
    [SerializeField] float searchTime = 5f;

    Vector3 lookTarget;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        detector = GetComponentInChildren<Detector>();
        //Initial position of first waypoint
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.position = currentWaypoint.position;

        //Set the next waypoint target
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        ChangeEnemyState(EnemyState.Patrolling);
    }

    // Update is called once per frame
    void Update()
    {
        if(detector.playerIsSeen)
        {
            lookTarget = player.transform.position;
        }
        lookTarget.y = transform.position.y;
        transform.LookAt(lookTarget);

        
        switch(currentState)
        {
            case EnemyState.Patrolling:
            //When the enenmy is patrolling, they should move along their waypoints
            FollowWaypoint();
            break;
            case EnemyState.Alert:
            if(detector.playerIsSeen && currentAwareness < maxAwareness)
            {
                ChangeAwareness(30);
            }
            if(detector.playerIsSeen == false && currentAwareness > 0)
            {
                //If the enemy cannot see the player, then lower the awareness over time until it reaches 0
                ChangeAwareness(-30);
            }
            MoveToPlayerLastSeen();
            break;
            case EnemyState.Chase:
            Debug.Log("Enemy is now chasing the player");
            if(detector.playerIsSeen)
            {
                ChasePlayer();
            }
            else if(Time.time > timeLastSawPlayer + searchTime)
            {
                Debug.Log("Enemy lost track of player");
                currentAwareness = 0;
                ChangeEnemyState(EnemyState.Patrolling);
            }
            break;
        } 
    }

    public void ChangeEnemyState(EnemyState newState)
    {
        switch(newState)
        {
            case EnemyState.Patrolling:
            break;
            case EnemyState.Alert:
            //Invoke the event when the player is spotted
            PlayerSpotted?.Invoke();

            break;
            case EnemyState.Chase:
            break;
        }
        currentState = newState;

    }

    void FollowWaypoint()
    {
        lookTarget = currentWaypoint.position;
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, currentWaypoint.position) < distanceThreshold)
        {           
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        }
    }

    void MoveToPlayerLastSeen()
    {

        transform.position = Vector3.MoveTowards(transform.position, playerLastSeen, moveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, playerLastSeen) <= 1)
        {           
            
            //Look around to see if they still see the player
            //Start a coroutine for waiting a few moments
            //If they don't see anything, they return to patrol
        }
    }
    void ChasePlayer()
    {
        lookTarget = player.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
    }

    void ChangeAwareness(float amount)
    {
        currentAwareness += amount * Time.deltaTime;
        Mathf.Clamp(currentAwareness, 0, maxAwareness);
        if(currentAwareness <= 0)
        {
            //If the awareness reaches 0, go back to patrolling
            ChangeEnemyState(EnemyState.Patrolling);
        }
        if(currentAwareness >= maxAwareness)
        {
            //When the awareness reaches the threshold, the enemy begins chasing the player
            ChangeEnemyState(EnemyState.Chase);
        }
    }

    IEnumerator LookAroundForPlayer()
    {
        //Look around for the player.
        //If they see the player, move to state as normal
        //If not, lower the awareness every frame the player is NOT in view of the enemy
        yield return null;
    }




}

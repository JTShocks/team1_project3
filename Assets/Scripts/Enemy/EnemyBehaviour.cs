using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


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
    public delegate void PlayerCaught();
    public static PlayerCaught OnPlayerCaught;
    [SerializeField] private Waypoints waypoints;
    [SerializeField] private float baseMoveSpeed = 5f;
    float currentMovespeed;
    [SerializeField] private EnemyState currentState;
    [SerializeField] public Player player;
    [SerializeField] LayerMask playerMask;

    [Tooltip("This is what determines how close the enemy should get to their waypoint before trying to move to the next one")]
    [Range(0.1f, 1f)]
    [SerializeField] private float distanceThreshold;

    private Transform currentWaypoint;
    private Rigidbody rb;

    Coroutine enemyStunned;

    [SerializeField] float currentAwareness = 0f;
    [SerializeField] float maxAwareness = 100f;

    internal float timeLastSawPlayer;
    [SerializeField] float searchTimer = 5f;

    [SerializeField] internal float enemyViewAngle = .3f;

    [SerializeField] bool playerIsSeen;
    [SerializeField] bool playerIsInRange;
    public bool isStunned;
    [SerializeField] internal float stunnedTimer;

    Collider enemyCollider;
    NavMeshAgent navMeshAgent;
    Vector3 lookTarget;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentMovespeed = baseMoveSpeed;
        
        

        enemyCollider = GetComponent<Collider>();
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
        navMeshAgent.speed = currentMovespeed;
        playerIsSeen = CheckLineOfSight();
        if(isStunned)
        {
            currentMovespeed = 0;
            stunnedTimer -= Time.deltaTime;
            if(stunnedTimer <= 0)
            {
                currentMovespeed = baseMoveSpeed;
                isStunned = false;
            }
        }

        switch(currentState)
        {
            case EnemyState.Patrolling:
            //When the enenmy is patrolling, they should move along their waypoints
                FollowWaypoint();
                if(playerIsSeen)
                {
                    ChangeEnemyState(EnemyState.Alert);
                }
            break;
            case EnemyState.Alert:
            if(!isStunned)
            {
                if(playerIsSeen && currentAwareness < maxAwareness)
                {
                    ChangeAwareness(30);
                }
            }

                if(!playerIsSeen && currentAwareness > 0)
                {
                    //If the enemy cannot see the player, then lower the awareness over time until it reaches 0
                    ChangeAwareness(-30);
                }

            break;
            case EnemyState.Chase:
                Debug.Log("Enemy is now chasing the player");
                
                if(playerIsSeen)
                {
                    searchTimer = 5f;
                    navMeshAgent.SetDestination(player.transform.position);
                }
                else 
                {
                    searchTimer -= Time.deltaTime;
                    if(searchTimer <= 0)
                    {
                        ChangeEnemyState(EnemyState.Patrolling);
                    }
                }
            break;
        } 
    }

    void ChangeEnemyState(EnemyState newState)
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
            searchTimer = 5f;
            break;
        }
        currentState = newState;

    }

    void FollowWaypoint()
    {
        navMeshAgent.SetDestination(currentWaypoint.position);
        //transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, currentWaypoint.position) < distanceThreshold)
        {           
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        }
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

    public bool CheckLineOfSight()
    {
        //Position of the player in relation to the enemy
        Vector3 targetDirection = player.transform.position - transform.position;
        targetDirection.Normalize();
        float dotProduct = Vector3.Dot(transform.forward, targetDirection);
        if(dotProduct > enemyViewAngle)
        {
            //Player is in view range
            playerIsInRange = true;
            Debug.Log("Player is in viewing range");
            RaycastHit hit;
            Ray ray = new Ray(transform.position, (player.transform.position - transform.position).normalized);
            if(Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out hit, Mathf.Infinity, playerMask))
            {
                Debug.Log(hit.collider.gameObject);
                if(hit.collider.CompareTag("Player"))
                {
                    //Confirming it can see the player, returns a position
                    return true;
                }
            }
        }
        playerIsInRange = false;
        return false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            OnPlayerCaught?.Invoke();
        }
    }
}

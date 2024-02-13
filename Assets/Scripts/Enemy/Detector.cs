using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    //This is the script holding the detection logic for the enemies
    //Basically, if they see the player (but not too long), they will move to the last known location they saw the player
    [SerializeField] private string tagToFilter;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField]EnemyBehaviour enemy;
    bool playerIsInRange;
    public bool playerIsSeen;

    internal Vector3 playerPosition;
    [SerializeField] internal Transform player;
    

    void Awake()
    {

    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, enemy.player.transform.position, out hit))
            {
                if(hit.transform == player)
                {
                    //If the player can be see from the enemy head position, save that as confirmation
                    enemy.playerLastSeen = playerPosition;
                    enemy.timeLastSawPlayer = Time.time;
                    Debug.Log(enemy.timeLastSawPlayer);
                    //The player can be seen from this position
                    if(!playerIsSeen)
                    {
                        playerIsSeen = true;
                        Debug.Log("Can be seen");
                        enemy.ChangeEnemyState(EnemyBehaviour.EnemyState.Alert);
                    }

                }
                else
                {
                    playerIsSeen = false;
                    Debug.Log("Cannot be seen");
                }

            }

    }


    void OnTriggerStay(Collider collider)
    {
        //If the tag matches the objects it is looking for
        if(collider.CompareTag(tagToFilter))
        {
            playerIsInRange = true;
            player = collider.transform;
            //Player is in the range of the enemy vision
            Debug.Log("Player is in range of enemy");
            playerPosition = collider.transform.position;
            //Store WHERE the player is when they are in range
            //Use a raycast to CONFIRM if they can see the player (so they don't track the player through walls)
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.CompareTag(tagToFilter))
        {
            playerIsInRange = false;
        }
    }
    void OnDrawGizmos()
    {
        if(player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }

    }

}

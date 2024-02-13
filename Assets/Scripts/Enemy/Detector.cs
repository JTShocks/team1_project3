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

    internal Vector3 playerPosition;
    Transform player;

    void Awake()
    {

    }

    void Update()
    {
        if(playerIsInRange)
        {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, player.position - transform.position, out hit))
            {
                if(hit.transform == player)
                {
                    //If the player can be see from the enemy head position, save that as confirmation
                    enemy.playerLastSeen = playerPosition;
                    //The player can be seen from this position
                    Debug.Log("Can be seen");
                }
                else
                {
                    Debug.Log("Cannot be seen");
                }

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
            Gizmos.DrawLine(enemy.headTransform.position, player.transform.position);
        }

    }

}

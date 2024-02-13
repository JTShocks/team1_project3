using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    //This is the script holding the detection logic for the enemies
    //Basically, if they see the player (but not too long), they will move to the last known location they saw the player
    [SerializeField] private string tagToFilter;
    [SerializeField] EnemyBehaviour enemy;
    [SerializeField] float sightRange;
    [SerializeField] LayerMask playerMask;

    internal Vector3 playerLastSeen;
    [SerializeField] internal Transform player;
    

    void Awake()
    {
        enemy = GetComponent<EnemyBehaviour>();
    }

    public bool CheckLineOfSight()
    {
        //Position of the player in relation to the enemy
        Vector3 targetDirection = player.position - transform.position;
        targetDirection.Normalize();
        float dotProduct = Vector3.Dot(transform.forward, targetDirection);
        if(dotProduct > enemy.enemyViewAngle)
        {
            //Player is in view range
            Debug.Log("Player is in viewing range");
            RaycastHit hit;
            if(Physics.Raycast(transform.position, player.transform.position, out hit, Mathf.Infinity, playerMask))
            {
                Debug.Log(hit.collider.gameObject);
                if(hit.collider.CompareTag(tagToFilter))
                {
                    //Confirming it can see the player, returns a position

                    playerLastSeen = player.transform.position;
                    return true;
                }
            }
        }
        return false;

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

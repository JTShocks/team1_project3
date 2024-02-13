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



    [SerializeField] internal Transform player;
    

    void Awake()
    {
        enemy = GetComponent<EnemyBehaviour>();
    }

}

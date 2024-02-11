using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Player))]
public class PlayerJumping : MonoBehaviour
{
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float jumpPressBufferTime = .05f;
    Player player;
    bool isTryingToJump;
    float lastJumpPressTime;

    void Awake()
    {
        player = GetComponent<Player>();
    }
    void OnEnable(){ player.OnBeforeMove += OnBeforeMove;}

    void OnDisable(){player.OnBeforeMove -= OnBeforeMove;}

    void OnJump()
    {
        isTryingToJump = true;
        lastJumpPressTime = Time.time;
    }

    void OnBeforeMove()
    {
        bool wasTryingToJump = Time.time - lastJumpPressTime < jumpPressBufferTime;

        bool isOrWasTryingToJump = isTryingToJump || wasTryingToJump;
        if(isOrWasTryingToJump && player.isGrounded)
        {
            player.velocity.y += jumpSpeed;
        }
        isTryingToJump = false;
    }
}

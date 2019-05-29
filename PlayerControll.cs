using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    public float gravity;
    public float runSpeed = 34F;
    public float walkSpeed = 2F;
    public float crouchSpeed = 1F;
    public float jumpPower = 1F;
    public float jumpDistance = 2.5F;
    public float rollDistance = 3F;
    public float rollSpeedMin = 10F;
    public float rollSpeedMax = 15F;
    public bool canRun;
    public bool isDogde;
    public bool isJump;

    //Realworld//
    public bool inRealworld;
    private float gravityRW = 9.81F;
    private float runSpeedRW = 24F;
    private float walkSpeedRW = 1F;
    private float crouchSpeedRW = 0.4F;
    private float jumpPowerRW = 140F;
    private float jumpDistanceRW = 1F;
    private float rollDistanceRW = 2F;
    private float rollSpeedMinRW = 5F;
    private float rollSpeedMaxRW = 10F;

    //Dreamworld//
    public bool inDreamworld;
    private float gravityDW = 6.8F;
    private float runSpeedDW = 34F;
    private float walkSpeedDW = 2F;
    private float crouchSpeedDW = 1F;
    private float jumpPowerDW = 260F;
    private float jumpDistanceDW = 2.5F;
    private float rollDistanceDW = 3F;
    private float rollSpeedMinDW = 10F;
    private float rollSpeedMaxDW = 15F;

    //Private
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    void Start()
    {
        //Connections
        controller = GetComponent<CharacterController>();
        //Initialisieren
        canRun = true;
        PlayerGoesRealWorld();
    }
    void Update()
    {
        if (controller.isGrounded)
        {
            // Walk
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), -1, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * walkSpeed;
            controller.Move(moveDirection * Time.deltaTime);
            //Dogde
            if (Input.GetButtonDown("Dogde"))
            {
                if (!isDogde)
                {
                    Debug.Log("hhs");
                    isDogde = true;
                }
                else
                {
                    isDogde = false;
                }
                UpdateCrouching();
            }
            //Jump
            if (Input.GetButtonDown("Jump"))
            {
                isJump = true;
                moveDirection.y = jumpPower;
                controller.Move(moveDirection * Time.deltaTime);
                //JumpAnimation
                moveDirection = new Vector3(0, -1, jumpDistance);
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection = moveDirection * walkSpeed;
                controller.Move(moveDirection * Time.deltaTime);
            }
            //Sprint
            if (canRun)
            {
                if (Input.GetButton("Run"))
                {
                    moveDirection = new Vector3(Input.GetAxis("Horizontal"), -1, Input.GetAxis("Vertical"));
                    moveDirection = transform.TransformDirection(moveDirection);
                    moveDirection = moveDirection * runSpeed;
                    controller.Move(moveDirection * Time.deltaTime);
                }
            }
            isJump = false;
        }
        else
        {
            //Fall
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }
    }
    //Player enters Dreamworld//
    public void PlayerGoesDreamWorld()
    {
        inDreamworld = true;
        inRealworld = false;
        gravity = gravityDW;
        runSpeed = runSpeedDW;
        walkSpeed = walkSpeedDW;
        crouchSpeed = crouchSpeedDW;
        jumpPower = jumpPowerDW;
        jumpDistance = jumpDistanceDW;
        rollDistance = rollDistanceDW;
        rollSpeedMin = rollSpeedMinDW;
        rollSpeedMax = rollSpeedMaxDW;
        isDogde = false;
        UpdateCrouching();
    }
    //Player enters Realworld//
    public void PlayerGoesRealWorld()
    {
        inRealworld = true;
        inDreamworld = false;
        gravity = gravityRW;
        runSpeed = runSpeedRW;
        walkSpeed = walkSpeedRW;
        crouchSpeed = crouchSpeedRW;
        jumpPower = jumpPowerRW;
        jumpDistance = jumpDistanceRW;
        rollDistance = rollDistanceRW;
        rollSpeedMin = rollSpeedMinRW;
        rollSpeedMax = rollSpeedMaxRW;
        isDogde = false;
        UpdateCrouching();
    }
    private void UpdateCrouching()
    {
        if(isDogde)
        {
            walkSpeed = crouchSpeed;
            //Change WalkAnim to CrouchAnim
        }
        else
        {
            if (inDreamworld)
            {
                walkSpeed = walkSpeedDW;
            }
            else
            {
                walkSpeed = walkSpeedRW;
            }
            //Change CrouchAnim to WalkAnim
        }
    }
}

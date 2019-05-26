using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHS.Control;
using CHS.Combat;


namespace CHS.Players
{
    [RequireComponent(typeof(MoveController))]
    public class Player : MonoBehaviour
    {
        [System.Serializable]
        public class MouseInput
        {
            public Vector2 Damping;
            public Vector2 Sensitivity;
            public bool LockMouse;
        }

        [SerializeField] float runSpeed;
        [SerializeField] float walkSpeed;
        [SerializeField] float crouchSpeed;
        [SerializeField] float sprintSpeed;

        public PlayerAim playerAim;

        [SerializeField] MouseInput MouseControl;

        MoveController moveController;
        InputController playerInput;
        Crosshair crosshair;

        Vector2 mouseInput;

        private void Awake()
        {
            if (MouseControl.LockMouse)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private void Start()
        {
            playerInput = GetComponent<InputController>();
            moveController = GetComponent<MoveController>();
            crosshair = GetComponentInChildren<Crosshair>();
        }

        private void Update()
        {
            Move();
            LookAround();
        }

        private void Move()
        {
            float moveSpeed = runSpeed;

            if (playerInput.IsWalking)
                moveSpeed = walkSpeed;
            if (playerInput.IsSprinting)
                moveSpeed = sprintSpeed;
            if (playerInput.IsCrouched)
                moveSpeed = crouchSpeed;

            Vector2 direction = new Vector2(playerInput.Vertical * moveSpeed, playerInput.Horizontal * moveSpeed);
            moveController.Move(direction);
        }


        private void LookAround()
        {
            mouseInput.x = Mathf.Lerp(mouseInput.x, playerInput.mouseInput.x, 1f / MouseControl.Damping.x);
            mouseInput.y = Mathf.Lerp(mouseInput.y, playerInput.mouseInput.y, 1f / MouseControl.Damping.y);
            transform.Rotate(Vector3.up * mouseInput.x * MouseControl.Sensitivity.x);
            crosshair.LookHeight(mouseInput.y * MouseControl.Sensitivity.y);
            playerAim.SetRotation(mouseInput.y * MouseControl.Sensitivity.y);
        }
    }
}
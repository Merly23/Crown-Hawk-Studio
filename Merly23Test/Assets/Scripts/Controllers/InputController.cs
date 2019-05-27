using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CHS.Control
{
    public class InputController : MonoBehaviour
    {
        public float Vertical;
        public float Horizontal;
        public Vector2 mouseInput;
        public bool Fire1;
        public bool Jump;
        public bool Reload;
        public bool IsWalking;
        public bool IsSprinting;
        public bool IsCrouched;

        private void Update()
        {
            Vertical = Input.GetAxis("Vertical");
            Horizontal = Input.GetAxis("Horizontal");
            mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            Fire1 = Input.GetButton("Fire1");
            Jump = Input.GetButton("Jump");
            Reload = Input.GetKey(KeyCode.R);
            IsWalking = Input.GetKey(KeyCode.LeftAlt);
            IsSprinting = Input.GetKey(KeyCode.LeftShift);
            IsCrouched = Input.GetKey(KeyCode.C);
        }
    }
}
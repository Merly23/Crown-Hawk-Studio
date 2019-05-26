using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHS.Control;

namespace CHS.Combat.Weapons
{
    public class AssaultRifle : Shooter
    {
        InputController input;

        private void Start()
        {
            input = GameObject.FindObjectOfType<InputController>();
        }

        public override void Fire()
        {
            base.Fire();

            if (canFire)
            {
                // we fire the gun;
            }
        }

        private void Update()
        {
            if (input.Reload)
            {
                Reload();
            }
        }
    }
}
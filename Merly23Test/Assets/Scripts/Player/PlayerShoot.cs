using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHS.Combat;
using CHS.Control;

namespace CHS.Players
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] Shooter assaultRifle;

        InputController inputController;

        private void Start()
        {
            inputController = GetComponent<InputController>();
        }

        private void Update()
        {
            if (inputController.Fire1)
            {
                assaultRifle.Fire();
            }
        }
    }
}
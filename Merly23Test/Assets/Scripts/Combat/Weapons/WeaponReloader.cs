using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHS.Core;


namespace CHS.Combat.Weapons
{
    public class WeaponReloader : MonoBehaviour
    {
        [SerializeField] int maxAmmo;
        [SerializeField] float reloadTime;
        [SerializeField] int clipSize;

        Timer timer;

        int ammo;
        public int shotsFiredInClip;
        bool isReloading;

        private void Start()
        {
            timer = GameObject.FindObjectOfType<Timer>();
        }

        public int RoundsRemainingInClip
        {
            get
            {
                return clipSize - shotsFiredInClip;
            }
        }

        public bool IsReloading
        {
            get
            {
                return isReloading;
            }
        }

        public void Reload()
        {
            if (isReloading)
                return;

            isReloading = true;
            print("Reload Started");
            timer.Add(ExecuteReload, reloadTime);
        }

        private void ExecuteReload()
        {
            print("Reload Executed");
            isReloading = false;
            ammo -= shotsFiredInClip;
            shotsFiredInClip = 0;

            if (ammo < 0)
            {
                ammo = 0;
                shotsFiredInClip += -ammo;
            }
        }

        public void TakeFromClip(int amount)
        {
            shotsFiredInClip += amount;
        }
    }
}
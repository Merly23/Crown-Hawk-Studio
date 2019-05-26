using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHS.Combat.Weapons;

namespace CHS.Combat
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] float rateOfFire;
        [SerializeField] Projectile projectile;

        [HideInInspector]
        public Transform muzzle;

        private WeaponReloader reloader;

        float nextFireAllowed;
        public bool canFire;

        private void Awake()
        {
            muzzle = transform.Find("Muzzle");
            reloader = GetComponent<WeaponReloader>();
        }

        public void Reload()
        {
            if (reloader == null)
                return;
            reloader.Reload();
        }

        public virtual void Fire()
        {
            canFire = false;

            if (Time.time < nextFireAllowed)
            {
                return;
            }

            if (reloader != null)
            {
                if (reloader.IsReloading)
                    return;
                if (reloader.RoundsRemainingInClip == 0)
                    return;
                reloader.TakeFromClip(1);
            }

            nextFireAllowed = Time.time + rateOfFire;

            print("Firing! :" + Time.deltaTime);

            // instantiate projectile
            Instantiate(projectile, muzzle.position, muzzle.rotation);

            canFire = true;
        }
    }
}

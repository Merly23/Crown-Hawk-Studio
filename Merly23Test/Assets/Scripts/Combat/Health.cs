using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHS.Combat
{
    public class Health : Destructable
    {
        public override void Die()
        {
            base.Die();

            print("We died.");
        }

        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);
            print("Remaining: " + HitPointsRemaining);
        }
    }
}
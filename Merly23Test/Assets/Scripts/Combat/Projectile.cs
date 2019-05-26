using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHS.Combat
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed;
        [SerializeField] float timeToLive;
        [SerializeField] float damage;

        private void Start()
        {
            Destroy(gameObject, timeToLive);
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            print("Hit : " + other.name);
            var destructable = other.transform.GetComponent<Destructable>();
            if (destructable == null)
                return;

            destructable.TakeDamage(damage);
        }
    }
}
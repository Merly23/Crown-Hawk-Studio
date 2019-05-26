using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHS.Players;

namespace CHS.Cameras
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        [SerializeField] Vector3 cameraOffset;
        [SerializeField] float damping;

        Transform cameraLookTarget;
        Player player;

        private void Start()
        {
            player = FindObjectOfType<Player>();
            cameraLookTarget = player.transform.Find("cameraLookTarget");

            if (cameraLookTarget == null)
            {
                cameraLookTarget = player.transform;
            }
        }

        private void Update()
        {
            Vector3 targetPosition = cameraLookTarget.position +
                player.transform.forward * cameraOffset.z +
                player.transform.up * cameraOffset.y +
                player.transform.right * cameraOffset.x;

            Quaternion targetRotation = Quaternion.LookRotation(cameraLookTarget.position - targetPosition, Vector3.up);

            transform.position = Vector3.Lerp(transform.position, targetPosition, damping * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, damping * Time.deltaTime);
        }
    }
}
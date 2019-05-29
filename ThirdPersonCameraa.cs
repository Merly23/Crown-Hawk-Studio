using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraa : MonoBehaviour
{
    //Public//
    public Transform cameraTransform;
    public GameObject Maincamera;
    // height of camera
    public float height = 4.0F;
    //

    //Private//
    private Transform _target;
    private Vector3 headOffset = Vector3.zero;
    private Vector3 centerOffset = Vector3.zero;
    private bool snap = false;
    private PlayerControll controller;
    //x,z distance
    private float distance = 7.0F;
    //
    private float angularSmoothLag = 0.3F;
    private float angularMaxSpeed = 15.0F;
    private float heightSmoothLag = 0.3F;
    private float snapSmoothLag = 0.2F;
    private float snapMaxSpeed = 720.0F;
    private float clampHeadPositionScreenSpace = 0.75F;
    private float lockCameraTimeout = 0.2F;
    private float heightVelocity = 0.0F;
    private float angleVelocity = 0.0F;
    private float targetHeight = 100000.0F;

    void Awake()
    {
        if (!cameraTransform && Camera.main)
            cameraTransform = Camera.main.transform;
        if (!cameraTransform)
        {
            Debug.Log("Assign Camera");
            enabled = false;
        }


        _target = transform;
        if (_target)
        {
            controller = _target.GetComponent<PlayerControll>();
        }

        if (controller)
        {
            CharacterController characterController = _target.GetComponent<CharacterController>();
            centerOffset = characterController.bounds.center - _target.position;
            headOffset = centerOffset;
            headOffset.y = characterController.bounds.max.y - _target.position.y;
        }
        Cut(_target, centerOffset);
    }
    public float AngleDistance(float a, float b)
    {
        a = Mathf.Repeat(a, 360);
        b = Mathf.Repeat(b, 360);
        return Mathf.Abs(b - a);
    }

    public void Apply(Transform dummyTarget, Vector3 dummyCenter)
    {
        // When no target
        if (!controller)
        {
            return;
        }
        var targetCenter = _target.position + centerOffset;
        var targetHead = _target.position + headOffset;
        // Calculate rotation angle
        var originalTargetAngle = _target.eulerAngles.y;
        var currentAngle = cameraTransform.eulerAngles.y;
        // Adjust angle
        var targetAngle = originalTargetAngle;
        // When pressing Fire2 (alt) the camera will snap to the target direction real quick.
        if (Input.GetButton("Fire2"))
        {
            snap = true;
        }
        if (snap)
        {
            if (AngleDistance(currentAngle, originalTargetAngle) < 3.0)
            {
                snap = false;
            }
            currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref angleVelocity, snapSmoothLag, snapMaxSpeed);
        }
        else
        {
            targetAngle = currentAngle;
            if (AngleDistance(currentAngle, targetAngle) > 160)
            {
                targetAngle += 180;
            }
            currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref angleVelocity, angularSmoothLag, angularMaxSpeed);
        }
        if (controller.isJump)
        {
            var newTargetHeight = targetCenter.y + height;
            if (newTargetHeight < targetHeight || newTargetHeight - targetHeight > 5)
                targetHeight = targetCenter.y + height;
        }
        else
        {
            targetHeight = targetCenter.y + height;
        }
        var currentHeight = cameraTransform.position.y;
        currentHeight = Mathf.SmoothDamp(currentHeight, targetHeight, ref heightVelocity, heightSmoothLag);
        var currentRotation = Quaternion.Euler(0, currentAngle, 0);
        cameraTransform.position = targetCenter;
        cameraTransform.position += currentRotation * Vector3.back * distance;
        SetUpRotation(targetCenter, targetHead);
    }
    void LateUpdate()
    {
        Apply(transform, Vector3.zero);
    }
    void Cut(Transform dummyTarget, Vector3 dummyCenter)
    {
        var oldHeightSmooth = heightSmoothLag;
        var oldSnapMaxSpeed = snapMaxSpeed;
        var oldSnapSmooth = snapSmoothLag;
        snapMaxSpeed = 10000F;
        snapSmoothLag = 0.001F;
        heightSmoothLag = 0.001F;
        snap = true;
        Apply(transform, Vector3.zero);
        heightSmoothLag = oldHeightSmooth;
        snapMaxSpeed = oldSnapMaxSpeed;
        snapSmoothLag = oldSnapSmooth;
    }
    void SetUpRotation(Vector3 centerPos, Vector3 headPos)
    {
        var cameraPos = cameraTransform.position;
        var offsetToCenter = centerPos - cameraPos;
        var yRotation = Quaternion.LookRotation(new Vector3(offsetToCenter.x, 0, offsetToCenter.z));
        var relativeOffset = Vector3.forward * distance + Vector3.down * height;
        cameraTransform.rotation = yRotation * Quaternion.LookRotation(relativeOffset);
        var centerRay = cameraTransform.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5F, 0.5F, 1));
        var topRay = cameraTransform.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5F, clampHeadPositionScreenSpace, 1F));
        var centerRayPos = centerRay.GetPoint(distance);
        var topRayPos = topRay.GetPoint(distance);
        var centerToTopAngle = Vector3.Angle(centerRay.direction, topRay.direction);
        var heightToAngle = centerToTopAngle / (centerRayPos.y - topRayPos.y);
        var extraLookAngle = heightToAngle * (centerRayPos.y - centerPos.y);
        if (extraLookAngle < centerToTopAngle)
        {
            extraLookAngle = 0;
        }
        else
        {
            extraLookAngle = extraLookAngle - centerToTopAngle;
            cameraTransform.rotation *= Quaternion.Euler(-extraLookAngle, 0, 0);
        }
    }
    Vector3 GetCenterOffset()
    {
        return centerOffset;
    }
}

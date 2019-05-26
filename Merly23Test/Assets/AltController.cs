using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltController : MonoBehaviour
{
    #region Variables

    [Header("Speed")]
    public float moveSpeed;
    public float axisSpeed;
    public float rotationSpeed;
    public float strafeSpeed;

    [Header("Physics")]
    public float gravity;
    public float jumpForce;
    public float jumpDecrease;

    [Header("Surface Control")]
    public Vector3 sensorLocal;
    public float surfaceSlideSpeed;
    public float slopeClimbSpeed;
    public float slopeDecendSpeed;

    [Header("External")]
    public Camera playerCamera;
    public LayerMask discludePlayer;

    private bool m_grounded;
    private float m_curGrav;
    #endregion

    private void Update()
    {
        Gravity();
        MouseLook();
        Jump();
        SimpleMove();
        FinalMovement();
    }

    private void MouseLook()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, discludePlayer))
        {
            if (hit.distance >= 2)
            {
                Vector3 rPos = hit.point;
                rPos.y = transform.position.y;
                transform.LookAt(rPos);
            }
        }
    }

    private float jumpHeight;
    private Vector3 moveVector;
    private void SimpleMove()
    {
        moveVector = collisionSlopeCheck(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))) * axisSpeed;
    }

    private void FinalMovement()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.red, 0.2f);
        Vector3 movement = new Vector3(moveVector.x, -m_curGrav + jumpHeight, moveVector.z) * moveSpeed;
        movement = transform.TransformDirection(movement);
        transform.position += movement;
    }

    private Vector3 collisionSlopeCheck(Vector3 dir)
    {
        Vector3 d = transform.TransformDirection(dir);
        Vector3 l = transform.TransformPoint(sensorLocal);

        Ray ray = new Ray(l, d);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2, discludePlayer))
        {
            if (hit.distance <= 0.7f)
            {
                Debug.DrawLine(transform.position, hit.point, Color.yellow, 0.2f);

                Vector3 temp = Vector3.Cross(hit.normal, d);
                Debug.DrawRay(hit.point, temp * 20, Color.green, 0.2f);

                Vector3 myDirection = Vector3.Cross(temp, hit.normal);
                Debug.DrawRay(hit.point, myDirection * 20, Color.red, 0.2f);

                Vector3 dir2 = myDirection * surfaceSlideSpeed * moveSpeed * axisSpeed;

                RaycastHit wCheck = wallCheckDetails(dir2);
                if (wCheck.transform != null)
                {
                    dir2 *= wCheck.distance * 0.5f;
                }
                transform.position += dir2;
                return Vector3.zero;
            }
            else
            {
                return dir;
            }
        }
        return dir;
    }

    private RaycastHit wallCheckDetails(Vector3 dir)
    {
        Vector3 l = transform.TransformPoint(sensorLocal);
        Ray ray = new Ray(l, dir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.2f, discludePlayer))
        {
            return hit;
        }
        return hit;
    }

    private void Jump()
    {
        if (m_grounded)
        {
            if (jumpHeight > 0)
            {
                jumpHeight = 0;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpHeight += jumpForce;
            }
        }
        else
        {
            if (jumpHeight > 0)
            {
                jumpHeight -= (jumpHeight * jumpDecrease * Time.deltaTime);
            }
            else
            {
                jumpHeight = 0;
            }
        }
    }

    private void Gravity()
    {
        Vector3 boxPos = new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2 - (Vector3.one / 2).y, transform.position.z);
        Vector3 boxSize = Vector3.one;
        m_grounded = Physics.CheckBox(boxPos, boxSize / 2);
        if (m_grounded)
        {
            m_curGrav = 0;
        }
        else
        {
            m_curGrav = -gravity;
        }

        if (m_grounded)
        {
            Ray ray = new Ray(transform.position, Vector3.down * 2);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, discludePlayer))
            {
                if (hit.distance <= 2)
                {
                    Debug.DrawRay(ray.origin, ray.direction * 20, Color.green, 0.2f);
                    Vector3 needed = new Vector3(transform.position.x, hit.point.y + transform.localScale.y, transform.position.z);
                    transform.position = needed;
                }
                else if (hit.distance > 2)
                {
                    m_grounded = true;
                    m_curGrav = -gravity;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 boxPos = new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2 - (Vector3.one / 2).y, transform.position.z);
        Vector3 boxSize = Vector3.one;
        if (!m_grounded)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawWireCube(boxPos, boxSize);

        Gizmos.color = Color.yellow;
        Vector3 l = transform.TransformPoint(sensorLocal);
        Gizmos.DrawWireSphere(l, 0.2f);
    }

    private void FixedUpdate()
    {
        Gravity();
    }
}

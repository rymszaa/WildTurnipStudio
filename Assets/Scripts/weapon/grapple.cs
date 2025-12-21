using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : MonoBehaviour
{

    private LineRenderer lr;
    private Vector3 pointToGrapple;
    public LayerMask whatIsGrappable;
    public InputActionReference shoot, climb;
    public Transform lineStart, camera, player;
    public int maxDistance;
    private SpringJoint joint;
    [SerializeField] public float climbSpeed = 5f;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void OnEnable()
    {
        shoot.action.started += StartGrapple;
        shoot.action.canceled += StopGrapple;
    }
    private void StartGrapple(InputAction.CallbackContext obj)
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappable))
        {
            pointToGrapple = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = pointToGrapple;
            
            float distanceFromPoint = Vector3.Distance(player.position, pointToGrapple);
            
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
            
            joint.spring = 1.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;
            
            lr.positionCount = 2;
        }
    }

    void DrawRope()
    {
        lr.SetPosition(0, lineStart.position);
        lr.SetPosition(1, pointToGrapple);
    }

    private void StopGrapple(InputAction.CallbackContext obj)
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    private void OnDisable()
    {
        shoot.action.started -= StartGrapple;
        shoot.action.canceled -= StopGrapple;
    }

    void LateUpdate()
    {
        if (!joint) return;

        DrawRope();
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return pointToGrapple;
    }

    void Update()
    {
        if (joint == null) return;

        // Read the Digital input value
        float climbHeld = climb.action.ReadValue<float>(); // 1 if held, 0 if not

        if (climbHeld > 0)
        {
            // Move player up the rope
            joint.maxDistance -= climbSpeed * Time.deltaTime;
            joint.minDistance -= climbSpeed * Time.deltaTime;

            // Clamp distances
            joint.maxDistance = Mathf.Max(joint.maxDistance, 0.5f);
            joint.minDistance = Mathf.Max(joint.minDistance, 0f);
        }
    }


}

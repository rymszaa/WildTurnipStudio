using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class grapple : MonoBehaviour
{

    private LineRenderer lr;
    private Vector3 pointToGrapple;
    public LayerMask whatIsGrappable;
    public InputActionReference shoot;
    public Transform lineStart, camera, player;
    public int maxDistance;
    private SpringJoint joint;

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
            
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;
        }
    }
    
    

    private void StopGrapple(InputAction.CallbackContext obj)
    {
        
    }

    // private void Shoot(InputAction.CallbackContext obj)
    // {
    //     Debug.Log("pew pew");
    // }

    private void OnDisable()
    {
        shoot.action.started -= StartGrapple;
        shoot.action.canceled -= StopGrapple;
    }

    void Update()
    {
        
    }

}

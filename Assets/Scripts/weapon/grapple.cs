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
    private CharacterController controller;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        controller = player.GetComponent<CharacterController>();
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
    
    void HandleGrappleMovement()
    {
        if (controller == null || pointToGrapple == null) return;

        // kierunek do punktu grappla
        Vector3 directionToPoint = pointToGrapple - player.position;
        float distance = directionToPoint.magnitude;

        if (distance > 0.1f)
        {
            // ustaw prędkość przyciągania
            float pullSpeed = 20f;

            // kinematyczne przesunięcie gracza
            Vector3 move = directionToPoint.normalized * pullSpeed * Time.deltaTime;

            // nie przesuwaj za daleko
            if (move.magnitude > distance)
                move = directionToPoint;

            controller.Move(move);
        }
    }

    void Update()
    {
        if (joint == null) return;

        // Odczyt wartości climb (1 jeśli przytrzymany, 0 jeśli nie)
        float climbHeld = climb.action.ReadValue<float>();

        // Przyciąganie tylko, gdy gracz przytrzymuje climb
        if (climbHeld > 0)
        {
            HandleGrappleMovement(); // przesunięcie CharacterController w kierunku grappla

            // Skracanie liny (wspinanie)
            joint.maxDistance -= climbSpeed * Time.deltaTime;
            joint.minDistance -= climbSpeed * Time.deltaTime;

            joint.maxDistance = Mathf.Max(joint.maxDistance, 0.5f);
            joint.minDistance = Mathf.Max(joint.minDistance, 0f);
        }
    }


}
//
// using UnityEngine;
// using UnityEngine.InputSystem;
//
// [RequireComponent(typeof(CharacterController))]
// [RequireComponent(typeof(LineRenderer))]
// public class Grapple : MonoBehaviour
// {
//     [Header("Input")]
//     public InputActionReference shoot; // przycisk strzału grappla
//     public InputActionReference climb; // przycisk wspinania
//
//     [Header("Setup")]
//     public Transform camera;    // kamera gracza
//     public Transform lineStart; // koniec pistoletu / punkt startowy liny
//     public LayerMask whatIsGrappable;
//
//     [Header("Settings")]
//     public float maxDistance = 50f;
//     public float grapplePullSpeed = 50f; // siła przyciągania
//     public float climbSpeed = 10f;
//
//     private CharacterController controller;
//     private LineRenderer lr;
//
//     private Vector3 grapplePoint;
//     private bool isGrappling = false;
//     private float currentRopeLength;
//
//     void Awake()
//     {
//         controller = GetComponent<CharacterController>();
//         lr = GetComponent<LineRenderer>();
//
//         lr.positionCount = 0;
//         lr.useWorldSpace = true; // ważne, żeby linia rysowała się w świecie
//     }
//
//     void OnEnable()
//     {
//         shoot.action.started += StartGrapple;
//         shoot.action.canceled += StopGrapple;
//     }
//
//     void OnDisable()
//     {
//         shoot.action.started -= StartGrapple;
//         shoot.action.canceled -= StopGrapple;
//     }
//
//     void StartGrapple(InputAction.CallbackContext ctx)
//     {
//         RaycastHit hit;
//         if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappable))
//         {
//             grapplePoint = hit.point;
//             currentRopeLength = Vector3.Distance(transform.position, grapplePoint);
//
//             isGrappling = true;
//             lr.positionCount = 2;
//         }
//     }
//
//     void StopGrapple(InputAction.CallbackContext ctx)
//     {
//         isGrappling = false;
//         lr.positionCount = 0;
//     }
//
//     void Update()
//     {
//         if (!isGrappling) return;
//
//         HandleGrappleMovement();
//         HandleClimb();
//         DrawRope();
//     }
//
//     void HandleGrappleMovement()
//     {
//         Vector3 directionToPoint = grapplePoint - transform.position;
//         float distance = directionToPoint.magnitude;
//
//         if (distance > 0.1f)
//         {
//             // przyciąganie gracza
//             Vector3 pull = directionToPoint.normalized * grapplePullSpeed * Time.deltaTime;
//
//             // nie przekraczaj punktu zaczepienia
//             if (pull.magnitude > distance)
//                 pull = directionToPoint;
//
//             controller.Move(pull);
//         }
//     }
//
//     void HandleClimb()
//     {
//         float climbHeld = climb.action.ReadValue<float>();
//
//         if (climbHeld > 0)
//         {
//             currentRopeLength -= climbSpeed * Time.deltaTime;
//             currentRopeLength = Mathf.Max(currentRopeLength, 1f);
//         }
//     }
//
//     void DrawRope()
//     {
//         lr.SetPosition(0, lineStart.position);
//         lr.SetPosition(1, grapplePoint);
//     }
//
//     public bool IsGrappling()
//     {
//         return isGrappling;
//     }
//
//     public Vector3 GetGrapplePoint()
//     {
//         return grapplePoint;
//     }
// }